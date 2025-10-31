#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

namespace Knight.Core
{
    public class UIAtlasCheck
    {
        public const string SelectUIAtlasCheckDebugModePath = "Tools/GUI/Atlas/Check Disable";


        public static void CheckAtlasRefUIPrefab(string rUIName, GameObject rGO)
        {
#if UNITY_EDITOR
            return;    //老规则图集检测屏蔽
            if (!AssetLoader.Instance.IsSumilateMode_GUI())
                return;

            if (Menu.GetChecked(SelectUIAtlasCheckDebugModePath))
                return;

            string rABSufix;
            if (rUIName.StartsWith("UI"))
                rABSufix = rUIName.Substring(2).ToLower();
            else
                rABSufix = rUIName.ToLower();

            if(!rGO)
            {
                Knight.Core.LogManager.LogError($"找不到rUIName={rUIName}的预制件，GameObject为空！");
            }
            CheckAtlasRefAB(rABSufix, rUIName, rGO.transform);
#endif
        }

        static void CheckAtlasRefAB(string rABSufix, string rUIName, Transform rTrans)
        {
#if UNITY_EDITOR
            for (int i = 0; i < rTrans.childCount; ++i)
                CheckAtlasRefAB(rABSufix, rUIName, rTrans.GetChild(i));

            Image rImage = rTrans.GetComponent<Image>();
            if (rImage != null && rImage.sprite != null)
            {
                string rAssetPath = AssetDatabase.GetAssetPath(rImage.sprite);
                if (!string.IsNullOrEmpty(rAssetPath))
                {
                    string rDirName = Path.GetDirectoryName(rAssetPath);
                    if (Path.GetFileNameWithoutExtension(Path.GetDirectoryName(rDirName)) != "Atlas")
                        return;
                    string rAtlasName = Path.GetFileNameWithoutExtension(rDirName).ToLower();
                    if (rAtlasName.StartsWith("common"))
                        return;
                    if (rAtlasName.StartsWith("mutilanguage"))
                        return;
                    if (rAtlasName.StartsWith("DynamicLoading"))
                        return;
                    if (rABSufix.StartsWith(rAtlasName))
                        return;
                    Knight.Core.LogManager.LogFormat("图集划分错误{0},精灵{1},图集{2},节点{3}", rUIName, rImage.sprite.name, rAtlasName, rTrans.name);
                }
            }
#endif
        }

        public static void CheckAtlasRefLoadSprite(string rSpriteName, Transform rTrans)
        {
#if UNITY_EDITOR
            if (!AssetLoader.Instance.IsSumilateMode_GUI())
                return;

            if (Menu.GetChecked(SelectUIAtlasCheckDebugModePath))
                return;

            string rUIName = CheckAtlasRef_FindUIName(rTrans);
            string rABSufix;
            if (rUIName.StartsWith("UI"))
                rABSufix = rUIName.Substring(2).ToLower();
            else
                rABSufix = rUIName.ToLower();
            CheckAtlasRefAB(rABSufix, rUIName, rTrans);
#endif
        }

        static string CheckAtlasRef_FindUIName(Transform rTrans)
        {
#if UNITY_EDITOR
            if (rTrans.parent != null)
            {
                string rParentName = rTrans.parent.name;
                if (rParentName == "__dynamicRoot" || rParentName == "__popupRoot" || rParentName == "__globalsRoot")
                    return rTrans.name;
                return CheckAtlasRef_FindUIName(rTrans.parent);
            }
            return "";
#else
            return "";
#endif
        }
    }

}
#endif