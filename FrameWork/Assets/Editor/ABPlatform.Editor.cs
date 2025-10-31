//
// using System;
// using System.Collections.Generic;
// using Game;
// using UnityEditor;
//
//
//     public class ABPlatformEditor
//     {
//         private const string    mSelectDevelopeModeMenuPath         = "Tools/Develope Mode";
//
//         private const string    mSelectSimulateModeMenuPath_Scene   = "Tools/Simulate Mode/Scene";
//         private const string    mSelectSimulateModeMenuPath_Avatar  = "Tools/Simulate Mode/Avatar";
//         private const string    mSelectSimulateModeMenuPath_Config  = "Tools/Simulate Mode/Config";
//         private const string    mSelectSimulateModeMenuPath_GUI     = "Tools/Simulate Mode/GUI";
//         private const string    mSelectSimulateModeMenuPath_Script  = "Tools/Simulate Mode/Script";
//         private const string    mSelectSimulateModeMenuPath_Effect  = "Tools/Simulate Mode/Effect";
//         private const string    mSelectSimulateModeMenuPath_Sound   = "Tools/Simulate Mode/Sound";
//         private const string    mSelectSimulateModeMenuPath_VideoAndSubtitle = "Tools/Simulate Mode/VideoAndSubtitles";
//
//         
//         [MenuItem(mSelectDevelopeModeMenuPath, priority = 1000)]
//         public static void SelectDevelopeMode_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectDevelopeModeMenuPath);
//             EditorPrefs.SetBool(ABPlatform.IsDevelopeModeKey, !bSelected);
//             Menu.SetChecked(mSelectDevelopeModeMenuPath, !bSelected);
//         }
//
//         [MenuItem(mSelectDevelopeModeMenuPath, true)]
//         public static bool SelectDevelopeMode_Check_Menu()
//         {
//             Menu.SetChecked(mSelectDevelopeModeMenuPath, EditorPrefs.GetBool(ABPlatform.IsDevelopeModeKey, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Scene, priority = 1050)]
//         public static void SelectSimulateMode_Scene_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Scene);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Scene, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Scene, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Scene, true)]
//         public static bool SelectSimulateMode_Check_Scene_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Scene, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Scene, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Avatar, priority = 1050)]
//         public static void SelectSimulateMode_Avatar_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Avatar);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Avatar, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Avatar, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Avatar, true)]
//         public static bool SelectSimulateMode_Check_Avatar_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Avatar, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Avatar, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Config, priority = 1050)]
//         public static void SelectSimulateMode_Config_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Config);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Config, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Config, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Config, true)]
//         public static bool SelectSimulateMode_Check_Config_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Config, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Config, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_GUI, priority = 1050)]
//         public static void SelectSimulateMode_GUI_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_GUI);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_GUI, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_GUI, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_GUI, true)]
//         public static bool SelectSimulateMode_Check_GUI_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_GUI, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_GUI, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Script, priority = 1050)]
//         public static void SelectSimulateMode_Script_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Script);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Script, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Script, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Script, true)]
//         public static bool SelectSimulateMode_Check_Script_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Script, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Script, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Effect, priority = 1050)]
//         public static void SelectSimulateMode_Effect_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Effect);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Effect, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Effect, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Effect, true)]
//         public static bool SelectSimulateMode_Check_Effect_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Effect, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Effect, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Sound, priority = 1050)]
//         public static void SelectSimulateMode_Sound_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_Sound);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_Sound, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Sound, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_Sound, true)]
//         public static bool SelectSimulateMode_Check_Sound_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_Sound, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_Sound, false));
//             return true;
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_VideoAndSubtitle, priority = 1050)]
//         public static void SelectSimulateMode_VideoAndSubtitle_Menu()
//         {
//             bool bSelected = Menu.GetChecked(mSelectSimulateModeMenuPath_VideoAndSubtitle);
//             EditorPrefs.SetBool(ABPlatform.IsSimulateModeKey_VideoAndSubtitle, !bSelected);
//             Menu.SetChecked(mSelectSimulateModeMenuPath_VideoAndSubtitle, !bSelected);
//         }
//
//         [MenuItem(mSelectSimulateModeMenuPath_VideoAndSubtitle, true)]
//         public static bool SelectSimulateMode_Check_VideoAndSubtitle_Menu()
//         {
//             Menu.SetChecked(mSelectSimulateModeMenuPath_VideoAndSubtitle, EditorPrefs.GetBool(ABPlatform.IsSimulateModeKey_VideoAndSubtitle, false));
//             return true;
//         }
//     }
