using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;
using System.IO;
using System.Text;

namespace Knight.Core
{
    public enum ELogSaveType
    {
        Invaild,
        Log,
        LogRelease,
        Warning,
        Error,
        Exception,
    }
    public partial struct KeyValue<TKey, TValue>
    {
        private TKey key;
        private TValue value;

        public KeyValue(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key => this.key;

        public TValue Value => this.value;

        public override string ToString() => this.PairToString((object) this.Key, (object) this.Value);

        public void Deconstruct(out TKey key, out TValue value)
        {
            key = this.Key;
            value = this.Value;
        }
        
        string PairToString(object key, object value)
        {
            StringBuilder sb = StringBuilderCache.Acquire();
            sb.Append('[');
            if (key != null)
                sb.Append(key);
            sb.Append(", ");
            if (value != null)
                sb.Append(value);
            sb.Append(']');
            return StringBuilderCache.GetStringAndRelease(sb);
        }
    }
    public static class StringBuilderCache
    {
        [ThreadStatic]
        private static StringBuilder _cache = new StringBuilder(256);

        private const int MAX_BUILDER_SIZE = 512;

        public static StringBuilder Acquire(int capacity = 256)
        {
            StringBuilder cache = _cache;
            if (cache != null && cache.Capacity >= capacity)
            {
                _cache = null;
                cache.Clear();
                return cache;
            }
            return new StringBuilder(capacity);
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            string result = sb.ToString();
            Release(sb);
            return result;
        }

        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity <= 512)
            {
                _cache = sb;
            }
        }
    }
    public class LogSaver : MonoBehaviour
    {
        [HideInInspector]
        public List<string> LogList;
        [HideInInspector]
        public int LogSaveCount;
        public const int MaxLogCount = 2500; //�������������һ��log
        public int PlayerID;

        private static List<KeyValue<string, ELogSaveType>> mLogCacheList = new List<KeyValue<string, ELogSaveType>>();
        private static bool mInitialized = false;
        private string mPersistentDataPath = "";
        private void Start()
        {
#if GAME_RELEASE
            this.LogList = new List<string>();
            Application.logMessageReceivedThreaded += this.OnReceiveLogMessage;
            mInitialized = true;
            this.mPersistentDataPath = Application.persistentDataPath;
#endif
        }
        public static void AddLog(ELogSaveType rType, object rLog)
        {
#if GAME_RELEASE
            if (!mInitialized) return;
            var rLogRes = rLog is StringBuilder ? rLog.ToString() : rLog;
            if (rLogRes is not string) return;
            mLogCacheList.Add(new KeyValue<string, ELogSaveType>((string)rLogRes, rType));
#endif
        }
        public void OnReceiveLogMessage(string condition, string stackTrace, LogType type)
        {
            var bFind = false;
            var rSaveType = ELogSaveType.Invaild;
            for (int i = mLogCacheList.Count - 1; i >= 0; i--)
            {
                var rKV = mLogCacheList[i];
                if(rKV.Key == condition)
                {
                    bFind = true;
                    rSaveType = rKV.Value;
                    mLogCacheList.RemoveAt(i);
                    break;
                }
            }
            if (!bFind && type != LogType.Exception) return;
            if (type == LogType.Exception) rSaveType = ELogSaveType.Exception;
            lock (this.LogList)
            {
                if (this.LogList.Count >= MaxLogCount)
                    this.Clear();
                switch (rSaveType)
                {
                    case ELogSaveType.Log:
#if ENABLE_DEBUG_LOG_STACKTRACE
                        this.LogList.Add($"[{rSaveType}] {condition}\n{stackTrace}");
#else
                        this.LogList.Add($"[{rSaveType}] {condition}");
#endif
                        break;
                    case ELogSaveType.LogRelease:
#if ENABLE_LOG_RELEASE_STACKTRACE
                        this.LogList.Add($"[{rSaveType}] {condition}\n{stackTrace}");
#else
                        this.LogList.Add($"[{rSaveType}] {condition}");
#endif
                        break;
                    case ELogSaveType.Warning:
                        this.LogList.Add($"[{rSaveType}] {condition}");
                        break;
                    case ELogSaveType.Error:
                        this.LogList.Add($"[{rSaveType}] {condition}\n{stackTrace}");
                        break;
                    case ELogSaveType.Exception:
                        this.LogList.Add($"[{rSaveType}] {condition}\n{stackTrace}");
                        this.SaveLogToFile();
                        break;
                }
            }
        }
        public string GetLogString()
        {
            return string.Join('\n', this.LogList);
        }
        public void SaveLogToFile()
        {
            lock (this.LogList)
            {
                var rNow = DateTime.Now;
                this.LogSaveCount++;
                var rFileDirName = this.mPersistentDataPath + $@"\{rNow.Year}_{rNow.Month}_{rNow.Day}_{rNow.Hour}_{rNow.Minute}_{rNow.Second}_File{this.LogSaveCount}_PlayerID{this.PlayerID}_[LOG].txt";
                FileStream rFile;
                rFile = File.Create(rFileDirName);
                var rBytes = Encoding.UTF8.GetBytes(this.GetLogString());
                rFile.Write(rBytes, 0, rBytes.Length);
                rFile.Close();
                this.Clear();
            }
        }
        public void Clear()
        {
            this.LogList.Clear();
            GC.Collect();
        }
    }

}