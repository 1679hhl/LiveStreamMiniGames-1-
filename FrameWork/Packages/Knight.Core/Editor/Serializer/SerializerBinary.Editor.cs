//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Knight.Core.Serializer.Editor
{
    public class SerializerBinaryEditor : TSingleton<SerializerBinaryEditor>
    {
        //[MenuItem("Tools/Serializer/0.生成技能节点相关", priority = 0)]
        public static void CodeGenerate_All()
        {
            CodeGenerate_Serializer(true);

            // 生成技能的编辑器脚本
            var rSkillScriptEditorType = System.AppDomain.CurrentDomain.GetAssemblies()
                     .Single(rAssembly => rAssembly.GetName().Name.Equals("Game.Editor"))?.GetTypes()?
                     .SingleOrDefault(rType => rType.FullName.Equals("Game.Editor.SkillScriptEditor"));
            if (rSkillScriptEditorType != null)
            {
                ReflectionAssist.MethodMember(rSkillScriptEditorType, "CodeGenerate", ReflectionAssist.flags_method_static);
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            UnityEngine.Debug.Log("Auto serializer cs generate complete..");
        }

        // 强制全部更新序列化代码
        //[MenuItem("Tools/Serializer/Dev/CS Serializer Force Generate")]
        public static void CodeGenerate_SerializerAll()
        {
            CodeGenerate_Serializer(false);
        }

        // 增量更新序列化代码
        [MenuItem("Tools/Serializer/1.生成序列化文件", priority = 1)]
        public static void CodeGenerate_Serializer()
        {
            CodeGenerate_Serializer(true);
        }

        static void CodeGenerate_Serializer(bool bIncrement)
        {
            var rStopwatch = Stopwatch.StartNew();
            string rGeneratePathRoot = "Packages/Knight-Lib/Knight.Framework.Assetbundle/Runtime/SerializerBinary/";
            CodeGenerate_Serializer(rGeneratePathRoot, "Framework.AssetBundle", "Knight.Framework.Assetbundles", "CommonSerializer", bIncrement);

            rGeneratePathRoot = "Assets/Game.Battle/Runtime/Generate/SerializerBinary/";
            CodeGenerate_Serializer(rGeneratePathRoot, "Game.Battle", "Game.Battle", "CommonSerializer", bIncrement);

            rGeneratePathRoot = "Assets/Game.Hotfix/Generate/SerializerBinary/";
            CodeGenerate_Serializer(rGeneratePathRoot, "Game.Hotfix", "Game", "CommonSerializerHotfix", bIncrement);

            rGeneratePathRoot = "Assets/Game/Script/Generate/SerializerBinary/";
            CodeGenerate_Serializer(rGeneratePathRoot, "Game", "Game", "CommonSerializer", bIncrement);

            var rCLRBindingType = System.AppDomain.CurrentDomain.GetAssemblies()
                     .Single(rAssembly => rAssembly.GetName().Name.Equals("Game.Editor"))?.GetTypes()?
                     .SingleOrDefault(rType => rType.FullName.Equals("Game.Editor.HotfixSerializerBinaryEditor"));
            if (rCLRBindingType != null)
            {
                ReflectionAssist.MethodMember(rCLRBindingType, "CodeGenerate", ReflectionAssist.flags_method_static);
            }

            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            UnityEngine.Debug.Log($"Auto serializer cs generate complete.. {rStopwatch.ElapsedMilliseconds}ms");
        }

        static void CodeGenerate_Serializer(string rGeneratePath, string rDLLModuleName, string rCommonNameSpace, string rCommonSerializerName, bool bIncrement)
        {
            string rCommonSerializerPath = rGeneratePath + "CommonSerializer.cs";
            List<CodeGenerator_ClassSerializer> rClassSerializers = new List<CodeGenerator_ClassSerializer>();
            CodeGenerator_CommonSerializer rCommonSerializer = new CodeGenerator_CommonSerializer(rCommonSerializerPath, rCommonNameSpace, rCommonSerializerName);

            EditorUtility.DisplayProgressBar("AutoCSGenerate", "Analyse Types", 0);
            var rSBTypes = SerializerBinaryTypes.Types;
            HashSet<string> rIncrementFileSet = new HashSet<string>();  // 增量更新的类型集合记录
            var rSerializerFileHashSet = new HashSet<string>
            {
                Path.GetFullPath(rCommonSerializer.FilePath).ReplaceSlash(),
            };
            for (int i = 0; i < rSBTypes.Count; i++)
            {
                if (!rSBTypes[i].Assembly.GetName().Name.Equals(rDLLModuleName)) continue;

                var rClassSerializer = CreateClassSerializer(rSBTypes[i], rGeneratePath, rCommonNameSpace, rCommonSerializerName);
                rSerializerFileHashSet.Add(Path.GetFullPath(rClassSerializer.FilePath).ReplaceSlash());

                rCommonSerializer.AnalyzeGenerateCommon(rSBTypes[i], false, rSBTypes[i]);
                // 增量更新判定
                bool bNeedGenIncrement = rClassSerializer.NeedGenIcrement();
                if (bNeedGenIncrement)
                    rIncrementFileSet.Add(rSBTypes[i].ToString());
                if (!bIncrement || bNeedGenIncrement)
                    rClassSerializers.Add(rClassSerializer);
                var rSerializeMemberInfo = SerializerAssists.FindSerializeMembers(rSBTypes[i]);
                foreach (var rMemberInfo in rSerializeMemberInfo)
                {
                    var bDynamic = rMemberInfo.IsDefined(typeof(SBDynamicAttribute), false);
                    if (rMemberInfo.MemberType == MemberTypes.Field)
                        rCommonSerializer.AnalyzeGenerateCommon((rMemberInfo as FieldInfo).FieldType, bDynamic, rSBTypes[i]);
                    else if (rMemberInfo.MemberType == MemberTypes.Property)
                        rCommonSerializer.AnalyzeGenerateCommon((rMemberInfo as PropertyInfo).PropertyType, bDynamic, rSBTypes[i]);
                }
            }

            for (int i = 0; i < rClassSerializers.Count; ++i)
            {
                rClassSerializers[i].WriteSaveWithCommonSerializer(rCommonSerializer);
                EditorUtility.DisplayProgressBar("AutoCSGenerate", $"Generate File: {rSBTypes[i].FullName}", (float)i / (float)rClassSerializers.Count);
            }

            bool CommonSerializerModified = rCommonSerializer.IsModified();

            // 增量更新时，检测公共序列化接口，影响到的非增量修改的文件
            if (bIncrement && CommonSerializerModified)
            {
                HashSet<string> rModifySet = rCommonSerializer.GetModifyList();
                foreach (string rStr in rIncrementFileSet)
                {
                    if (rModifySet.Contains(rStr))
                        rModifySet.Remove(rStr);
                }
                for (int i = 0; i < rSBTypes.Count; i++)
                {
                    string rName = rSBTypes[i].ToString();
                    if (rModifySet.Contains(rName))
                    {
                        var rClassSerializer = CreateClassSerializer(rSBTypes[i], rGeneratePath, rCommonNameSpace, rCommonSerializerName);
                        rClassSerializer.WriteSaveWithCommonSerializer(rCommonSerializer);
                    }
                }
            }

            // 公共序列化文件增量判定
            if (!bIncrement || CommonSerializerModified)
                rCommonSerializer.WriteAndSave();

            // 删除冗余序列化文件
            var rFiles = Directory.GetFiles(rGeneratePath, "*.cs", SearchOption.AllDirectories);
            for (int i = 0; i < rFiles.Length; i++)
            {
                var rFile = Path.GetFullPath(rFiles[i]).ReplaceSlash();
                if (!rSerializerFileHashSet.Contains(rFile))
                {
                    var rMetaFile = rFile + ".meta";
                    File.Delete(rFile);
                    File.Delete(rMetaFile);
                }
            }
        }

        static CodeGenerator_ClassSerializer CreateClassSerializer(Type rType, string rGeneratePath, string rCommonNameSpace, string rCommonSerializerName)
        {
            var rGroupName = string.Empty;
            var rAttributes = rType.GetCustomAttributes<SBGroupAttribute>(true);
            if (rAttributes.Length > 0)
                rGroupName = rAttributes[0].GroupName;
            string rPath = UtilTool.PathCombine(rGeneratePath, rGroupName, rType.FullName + ".Binary.cs");
            return new CodeGenerator_ClassSerializer(rPath, rCommonNameSpace, rType, rCommonSerializerName);
        }
    }
}
