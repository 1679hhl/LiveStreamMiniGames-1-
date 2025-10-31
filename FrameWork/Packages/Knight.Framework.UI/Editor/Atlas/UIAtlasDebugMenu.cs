using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Knight.Core.Editor
{
    // 编辑器禁用UI图集检测菜单
    public class UIAtlasDebugMenu
    {
        public const string IsUIAtlasCheckDebugModeKey = "UIAtlas_IsAtlasCheckDebugMode";

        [MenuItem(UIAtlasCheck.SelectUIAtlasCheckDebugModePath, priority = 1000)]
        public static void SelectDevelopeMode_Menu()
        {
            bool bSelected = Menu.GetChecked(UIAtlasCheck.SelectUIAtlasCheckDebugModePath);
            EditorPrefs.SetBool(IsUIAtlasCheckDebugModeKey, !bSelected);
            Menu.SetChecked(UIAtlasCheck.SelectUIAtlasCheckDebugModePath, !bSelected);
        }
    }

}