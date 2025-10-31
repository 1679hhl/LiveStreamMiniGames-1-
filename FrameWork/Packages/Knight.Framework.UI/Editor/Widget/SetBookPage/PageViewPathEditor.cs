using Knight.Core;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    public class PageViewPathEditor : UnityEditor.Editor
    {


        [MenuItem("Assets/PagePath/Improt All")]
        public static void PagePathImprotAll()
        {
            var rFolder = AssetDatabase.GetAssetPath(Selection.activeObject);
            var rPrefabGuids = AssetDatabase.FindAssets("t:prefab", new string[] { rFolder });
            foreach (var rPrefabGuid in rPrefabGuids)
            {
                var rPrefabPath = AssetDatabase.GUIDToAssetPath(rPrefabGuid);
                PageViewPathTool.ImportConfig(rPrefabPath);
            }
        }

        

        [MenuItem("Assets/PagePath/Improt All", true)]
        public static bool CheckPagePathImprotAll()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path))
            {
                return true;
            }
            return false;
        }

        [MenuItem("Assets/PagePath/Improt")]
        public static void PagePathImprot()
        {
            var rPrefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            PageViewPathTool.ImportConfig(rPrefabPath);
        }



        [MenuItem("Assets/PagePath/Improt", true)]
        public static bool CheckPagePathImprot()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Contains(".prefab"))
            {
                return true;
            }
            return false;
        }


        [MenuItem("Assets/PagePath/Exprot All")]
        public static void PagePathExprotAll()
        {
            var rFolder = AssetDatabase.GetAssetPath(Selection.activeObject);
            var rPrefabGuids = AssetDatabase.FindAssets("t:prefab", new string[] { rFolder });
            foreach (var rPrefabGuid in rPrefabGuids)
            {
                var rPrefabPath = AssetDatabase.GUIDToAssetPath(rPrefabGuid);
                var rPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rPrefabPath);
                var rPageViewPathComp = rPrefab.GetComponent<PageViewPath>();
                if (!rPageViewPathComp) continue;
                AssetDatabase.Refresh();
                rPageViewPathComp.ExportConfig();
            }
        }

        [MenuItem("Assets/PagePath/Exprot All", true)]
        public static bool CheckPagePathExprotAll()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (AssetDatabase.IsValidFolder(path))
            {
                return true;
            }
            return false;
        }
        [MenuItem("Assets/PagePath/Exprot")]
        public static void PagePathExprot()
        {
            var rPrefabPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            var rPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rPrefabPath);
            var rPageViewPathComp = rPrefab.GetComponent<PageViewPath>();
            if (!rPageViewPathComp) return;
            AssetDatabase.Refresh();
            rPageViewPathComp.ExportConfig();
        }

        [MenuItem("Assets/PagePath/Exprot", true)]
        public static bool CheckPagePathExprot()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path.Contains(".prefab"))
            {
                return true;
            }
            return false;
        }
    }
}
