//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.IO;
using Knight.Core.WindJson;

namespace Knight.Core.Editor
{
    public class EditorAssists
    {
        public static void RegisterUndo(string name, params Object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                UnityEditor.Undo.RecordObjects(objects, name);
                foreach (var obj in objects)
                {
                    if (obj == null) continue;
                    EditorUtility.SetDirty(obj);
                }
            }
        }

        public static string ToMemorySize(long byteNum)
        {
            if (byteNum < 0)
                byteNum = 0;

            if (byteNum < 1024)
            {
                return byteNum + "B";
            }
            else if (byteNum < 1048576 && byteNum >= 1024)
            {
                return (byteNum / 1024.0f).ToString("F2") + "KB";
            }
            else if (byteNum < 1073741824 && byteNum >= 1048576)
            {
                return (byteNum / 1048576.0f).ToString("F2") + "MB";
            }
            else
            {
                return (byteNum / 1073741824.0f).ToString("F2") + "GB";
            }
        }

        public static T ReceiveAsset<T>(string rAssetPath) where T : new()
        {
            if (!File.Exists(rAssetPath))
            {
                var rJsonNode = new JsonClass();
                var rJsonStr = rJsonNode.ToString();
                UtilTool.WriteAllText(rAssetPath, rJsonStr);
                return new T();
            }
            else
            {
                var rJsonStr = File.ReadAllText(rAssetPath);
                var rJsonNode = JsonParser.Parse(rJsonStr);
                return rJsonNode.ToObject<T>();
            }
        }

        public static List<string> GetAssetPaths(string rSearch, string[] rPaths)
        {
            var rResultPaths = new List<string>();
            var rGUIDS = AssetDatabase.FindAssets(rSearch, rPaths);
            for (int i = 0; i < rGUIDS.Length; i++)
            {
                var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDS[i]);
                rResultPaths.Add(rAssetPath);
            }
            return rResultPaths;
        }

        public static List<string> GetAssetNames(string rSearch, string[] rPaths)
        {
            var rResultPaths = new List<string>();
            var rGUIDS = AssetDatabase.FindAssets(rSearch, rPaths);
            for (int i = 0; i < rGUIDS.Length; i++)
            {
                var rAssetPath = AssetDatabase.GUIDToAssetPath(rGUIDS[i]);
                rResultPaths.Add(Path.GetFileNameWithoutExtension(rAssetPath));
            }
            return rResultPaths;
        }

        /// <summary>
        /// 加载Manifest
        /// </summary>
        public static void LoadManifest(string rManifestPath, Action<AssetBundleManifest> rLoadCompleted)
        {
            var assetBundle = AssetBundle.LoadFromFile(rManifestPath);

            if (assetBundle == null)
            {
                Debug.Log("加载Manifest出错: " + rManifestPath);
                UtilTool.SafeExecute(rLoadCompleted, null);
                return;
            }
            var asset = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (asset == null)
            {
                Debug.Log("加载Manifest出错：" + rManifestPath);
                return;
            }
            UtilTool.SafeExecute(rLoadCompleted, asset as AssetBundleManifest);
            assetBundle.Unload(false);
        }

        public static AssetBundleManifest LoadManifest(string rManifestPath)
        {
            var rAssetBundle = AssetBundle.LoadFromFile(rManifestPath);
            if (rAssetBundle == null) return null;

            var rABManifest = rAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest") as AssetBundleManifest;
            rAssetBundle.Unload(false);
            return rABManifest;
        }

        public static ulong MakeRegularObjectName(Object rObj)
        {
            var localFileID = GlobalObjectId.GetGlobalObjectIdSlow(rObj).targetObjectId;

            ulong oldLocalFileID = 0;
            var name = rObj.name;
            if (name.Length > 3)
            {
                if (name[0] == '[')
                {
                    var idStr = name.Substring(1);
                    var closeIdx = idStr.IndexOf(']');
                    if (closeIdx > 0)
                    {
                        if (!ulong.TryParse(idStr.Substring(0, closeIdx), out oldLocalFileID))
                        {
                            Debug.LogError("asset name format error:" + rObj.name);
                        }
                    }
                    name = idStr.Substring(closeIdx + 1);
                }
            }

            if (oldLocalFileID > 0 && oldLocalFileID != localFileID)
            {
                Debug.LogError("asset local file ID mismatch to previous ID:" + rObj.name);
            }

            rObj.name = "[" + localFileID + "]" + name;
            return localFileID;
        }
    }
}