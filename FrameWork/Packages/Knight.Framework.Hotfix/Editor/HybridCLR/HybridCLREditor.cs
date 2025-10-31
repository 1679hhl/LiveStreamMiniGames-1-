// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using Knight.Core.Editor;
// using HybridCLR.Editor.Commands;
// using HybridCLR.Editor.Installer;
// using HybridCLR.Editor;
//
// namespace Knight.Framework.Hotfix.Editor
// {
//     public class HybridCLREditor
//     {
//         [MenuItem("Tools/HybridCLR/Compile DLL Active Build Target", priority = 100)]
//         public static void CompilerDLLActiveBuildTarget()
//         {
//             // 编译DLL
//             CompileDllCommand.CompileDllActiveBuildTarget();
//
//             var rHotfixDir = "Assets/GameAsset/Hotfix/";
//             var rHybridCLRDir = "HybridCLRData/HotUpdateDlls/" + EditorUserBuildSettings.activeBuildTarget.ToString() + "/";
//
//             // 静态注入ViewModel代码
//             ViewModelInjectEditor.Inject(rHybridCLRDir + "Game.Hotfix.dll");
//
//             // 复制路径到我们的热更新资源目录
//             File.Copy(rHybridCLRDir + "Game.Hotfix.dll", rHotfixDir + "Game.Hotfix.bytes", true);
//             File.Copy(rHybridCLRDir + "Game.Hotfix.pdb", rHotfixDir + "Game.Hotfix.PDB.bytes", true);
//             Debug.Log("compile dll copy finish!!!");
//         }
//
//         public static void InstallFromRepo()
//         {
//             var rInstallerController = new InstallerController();
//             rInstallerController.InstallDefaultHybridCLR();
//         }
//
//         [MenuItem("Tools/HybridCLR/HybridCLR Prebuild Project", priority = 100)]
//         public static void PreBuildProject()
//         {
//             // Generate All
//             PrebuildCommand.GenerateAll();
//             // 编译热更新DLL
//             HybridCLREditor.CompilerDLLActiveBuildTarget();
//             // 复制元数据dll到Resources中
//             CopyAOTAssembliesToResources();
//
//             AssetDatabase.Refresh();
//         }
//
//         [MenuItem("Tools/HybridCLR/Copy AOT Assemblies To Resources")]
//         public static void CopyAOTAssembliesToResources()
//         {
//             var target = EditorUserBuildSettings.activeBuildTarget;
//             string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
//             string aotAssembliesDstDir = Application.dataPath + "/GameAsset/AOTAsms";
//
//             foreach (var dll in SettingsUtil.AOTAssemblyNames)
//             {
//                 string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
//                 if (!File.Exists(srcDllPath))
//                 {
//                     Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
//                     continue;
//                 }
//                 string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
//                 File.Copy(srcDllPath, dllBytesPath, true);
//                 Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
//             }
//         }
//
//         public static void SetHybridCLREnabled(bool bIsEnabled)
//         {
//             HybridCLR.Editor.SettingsUtil.Enable = bIsEnabled;
//         }
//     }
// }
//
