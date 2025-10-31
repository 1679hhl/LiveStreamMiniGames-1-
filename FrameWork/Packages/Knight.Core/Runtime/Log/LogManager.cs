//#define ENABLE_DEBUG 在csc.rsp文件中打开
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Knight.Core
{
    public class LogManager
    {

        [System.Diagnostics.Conditional("ENABLE_LOG_RELEASE")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogRelease(object text)
        {
#if UNITY_EDITOR
            LogSaver.AddLog(ELogSaveType.LogRelease, $"<color=#00EEEE>{text}</color>");
            Debug.Log($"<color=#00EEEE>{text}</color>");
#else
            LogSaver.AddLog(ELogSaveType.LogRelease, text);
            Debug.Log(text);
#endif
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ASSERT")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        //[System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void Assert(bool condition, string message)
        {
            Debug.Assert(condition, message);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_LOG")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        //[System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void Log(object text)
        {
            LogSaver.AddLog(ELogSaveType.Log, text);
            Debug.Log(text);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_LOG")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        //[System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void Log(object message, UnityEngine.Object context)
        {
            LogSaver.AddLog(ELogSaveType.Log, message);
            Debug.Log(message, context);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_LOG")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        //[System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogFormat(string format, params object[] args)
        {
            LogSaver.AddLog(ELogSaveType.Log, string.Format(format, args));
            Debug.LogFormat(format, args);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_WARNING")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogWarning(object text)
        {
            LogSaver.AddLog(ELogSaveType.Warning, text);
            Debug.LogWarning(text);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_WARNING")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogWarning(object text, UnityEngine.Object rAsset)
        {
            LogSaver.AddLog(ELogSaveType.Warning, text);
            Debug.LogWarning(text);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_WARNING")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogWarningFormat(string format, params object[] args)
        {
            LogSaver.AddLog(ELogSaveType.Warning, string.Format(format, args));
            Debug.LogWarningFormat(format, args);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ERROR")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogError(object text)
        {
            LogSaver.AddLog(ELogSaveType.Error, text);
            Debug.LogError(text);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ERROR")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogError(object message, UnityEngine.Object context)
        {
            LogSaver.AddLog(ELogSaveType.Error, message);
            Debug.LogError(message, context);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ERROR")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogErrorFormat(string text, params object[] args)
        {
            LogSaver.AddLog(ELogSaveType.Error, string.Format(text, args));
            Debug.LogErrorFormat(text, args);
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogOnlyEditor(object text)
        {
#if UNITY_EDITOR
            LogError(text);
#endif
        }
        public static void LogErrorOnEditor(object text)
        {
#if UNITY_EDITOR
            LogError(text);
#else
            LogWarning(text);
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogErrorOnEditor(object message, UnityEngine.Object context)
        {
#if UNITY_EDITOR
            LogError(message, context);
#else
            LogWarning(message, context);
#endif
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void LogErrorFormatOnEditor(string text, params object[] args)
        {
#if UNITY_EDITOR
            LogErrorFormat(text, args);
#else
            LogWarningFormat(text, args);
#endif
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ERROR")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogException(Exception e, UnityEngine.Object context)
        {
            Debug.LogException(e, context);
        }

        [System.Diagnostics.Conditional("ENABLE_DEBUG_ERROR")]
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("UNITY_STANDALONE")]
        public static void LogException(Exception e)
        {
            Debug.LogException(e);
        }
        /// <summary>
        /// 战斗重构调试日志 后续需要去掉调试日志时，直接打开条件编译宏
        /// </summary>
        /// <param name="rMessage"></param>
        [System.Diagnostics.Conditional("BATTLE_LOGTRACK")]
        public static void TrackLog(string rMessage)
        {
            Debug.Log($"<color=#F58F98>{rMessage}</color>");
        }
    }
}
