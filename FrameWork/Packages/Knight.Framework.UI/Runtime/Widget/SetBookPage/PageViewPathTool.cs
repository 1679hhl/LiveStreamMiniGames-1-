
using Knight.Core;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace UnityEngine.UI
{
    public static class PageViewPathTool
    {
        public const string FileDir = "Assets/Game/GameAsset/Config/SetBookPageConfig";


        public static void ImportConfig(string rPrefabPath)
        {
#if UNITY_EDITOR
            var rPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rPrefabPath);
            var rPrefabConfigPath = GetPathConfigName(rPrefab.name);
            if (!File.Exists(rPrefabConfigPath)) return;

            var rPageViewPathComp = rPrefab.ReceiveComponent<PageViewPath>();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            rPageViewPathComp.ImportConfig();
#endif
        }

        public static string GetPathConfigName(string rPrefabName)
        {
            return string.Format($"{FileDir}/{rPrefabName}.Json");
        }


    }
}