using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Mono.Cecil;
using System.Linq;
using System;
using Mono.Cecil.Cil;
using System.Reflection;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Reflection.Emit;
using OpCodes = Mono.Cecil.Cil.OpCodes;
using System.Threading;
using System.IO;

namespace Knight.Core.Editor
{
    class ViewModelIbject_CustomBuildProcessor : IPostBuildPlayerScriptDLLs
    {
        public int callbackOrder { get { return 0; } }
        public void OnPostBuildPlayerScriptDLLs(BuildReport report)
        {
#if HOTFIX_DLL_USE
            UnityEngine.Debug.Log("MyCustomBuildProcessor.OnPostBuildPlayerScriptDLLs for target " + report.summary.platform + " at path " + report.summary.outputPath);
            ViewModelInjectEditor.Inject_Path("Temp/StagingArea/Data/Managed/Game.Hotfix.dll", true);
#endif
        }
    }

    public class ViewModelInjectEditor
    {
        private static string mDLLPath = "Library/ScriptAssemblies/Game.Hotfix.dll";
        private static string mUIDLLPath = "Library/ScriptAssemblies/Framework.UI.dll";
        private static string mTestDLLPath = "F:\\GFrameworks\\PC\\GameAssembly.dll";

        private static List<string> mNeeeInjectPropType = new List<string>()
        {
            "System.Int32",
            "System.Int64",
            "System.Single",
            "System.Boolean",
            "UnityEngine.Color",
            "UnityEngine.Vector2",
            "UnityEngine.Vector3",
            "System.String",
            "System.Object",
        };

        public static void Inject()
        {
            Inject_Path(mDLLPath, true);
        }

        public static void Inject(string rDLLPath)
        {
            Inject_Path(rDLLPath, true);
        }

        [MenuItem("Tools/Other/ViewModel Injector 1")]
        public static void InjectMenu()
        {
            Inject_Path(mDLLPath, true);
        }
        
        [MenuItem("Tools/Other/ViewModel Injector 2")]
        public static void InjectMenu2()
        {
            Inject_Path(mTestDLLPath, true);
        }

        public static void Inject_Path(string rDLLPath, bool bIsSymbols)
        {
            AssemblyDefinition rAssembly = null;
            AssemblyDefinition rUIAssembly = null;
            try
            {
                // 取Assetmbly
                var rDefaultAssemblyResolver = new DefaultAssemblyResolver();
                rDefaultAssemblyResolver.AddSearchDirectory(Path.GetDirectoryName(typeof(Func<>).Assembly.Location));
                var readerParameters = new ReaderParameters { ReadSymbols = bIsSymbols, ReadWrite = true, AssemblyResolver = rDefaultAssemblyResolver };

                rAssembly = AssemblyDefinition.ReadAssembly(rDLLPath, readerParameters);
                rUIAssembly = AssemblyDefinition.ReadAssembly(mUIDLLPath, readerParameters);

                rAssembly.MainModule.ImportReference(typeof(Func<>));
                rAssembly.MainModule.ImportReference(typeof(System.Object));
                rAssembly.MainModule.ImportReference(typeof(IntPtr));
                rAssembly.MainModule.ImportReference(typeof(void));

                var rHotfixTemplateType = rAssembly.MainModule.GetType("Game.HotfixTemplate");
                var rField = rHotfixTemplateType.Fields.SingleOrDefault(f => f.Name.Equals("__Is_ViewModel_Injected__"));
                if (rField != null) return;
                rField = new FieldDefinition("__Is_ViewModel_Injected__", FieldAttributes.Static | FieldAttributes.Public, GetBoolValueTypeReference(rAssembly));
                rHotfixTemplateType.Fields.Add(rField);

                var rViewModelDataBindingTypes = rAssembly.MainModule.GetTypes().Where(
                                                    rType => rType != null &&
                                                    rType.BaseType != null &&
                                                    IsBaseTypeEquals(rUIAssembly, rType, "UnityEngine.UI.ViewModel"));

                var rViewModelTypeList = new List<TypeDefinition>(rViewModelDataBindingTypes);
                foreach (var rType in rViewModelTypeList)
                {
                    var rNeedInjectProps = rType.Properties.Where(rProp => rProp != null &&
                                                                  rProp.CustomAttributes.Any(rAttr => rAttr.AttributeType.FullName.Equals("UnityEngine.UI.DataBindingAttribute")));

                    var rNeedInjectPropList = new List<PropertyDefinition>(rNeedInjectProps);
                    foreach (var rProp in rNeedInjectPropList)
                    {
                        InjectType(rAssembly, rUIAssembly, rType, rProp.Name);
                    }
                    foreach (var rPropTypeFullName in mNeeeInjectPropType)
                    {
                        var rPropertyTypeType = GetType(rPropTypeFullName, rAssembly);
                        InjectViewModelGetPropValueMethod(rPropertyTypeType, rAssembly, rType);
                    }
                }

                var rNeedInjectPropDict = new Dict<string, List<PropertyDefinition>>();
                foreach (var rViewModelType in rViewModelTypeList)
                {
                    var rNeedInjectProps = rViewModelType.Properties.Where(rProp => rProp != null &&
                                                                           rProp.CustomAttributes.Any(rAttr => rAttr.AttributeType.FullName.Equals("UnityEngine.UI.DataBindingAttribute") ||
                                                                           rAttr.AttributeType.FullName.Equals("UnityEngine.UI.DataBindingRelatedAttribute")));

                    rNeedInjectPropDict.Clear();
                    foreach (var rPropTypeFullName in mNeeeInjectPropType)
                    {
                        var rNeedInjectPropList = new List<PropertyDefinition>();
                        rNeedInjectPropDict.Add(rPropTypeFullName, rNeedInjectPropList);
                    }
                    foreach (var rPropDef in rNeedInjectProps)
                    {
                        var rPropertyTypeType = GetType(rPropDef.PropertyType.FullName, rAssembly);
                        rNeedInjectPropDict[rPropertyTypeType.FullName].Add(rPropDef);

                        InjectTypeGetPropValueMethod_OneProp(rPropertyTypeType, rAssembly, rUIAssembly, rViewModelType, rPropDef);
                    }
                    InjectType_AddPropGetterToDicts(rNeedInjectPropDict, rAssembly, rUIAssembly, rViewModelType);
                }

                var rWriteParameters = new WriterParameters { WriteSymbols = bIsSymbols };
                rAssembly.Write(rWriteParameters);
                rAssembly.Dispose();
                rUIAssembly.Dispose();
            }
            catch (Exception e)
            {
                LogManager.LogException(e);
            }
            finally
            {
                rAssembly?.Dispose();
                rUIAssembly?.Dispose();
            }
            UnityEngine.Debug.Log("ViewModel inject success!!!");
        }

        private static TypeReference GetType(string rFullName, AssemblyDefinition rAssembly)
        {
            if (rFullName == "System.Int32")
            {
                return rAssembly.MainModule.ImportReference((typeof(System.Int32)));
            }
            else if (rFullName == "System.Int64")
            {
                return rAssembly.MainModule.ImportReference((typeof(System.Int64)));
            }
            else if (rFullName == "System.Single")
            {
                return rAssembly.MainModule.ImportReference((typeof(System.Single)));
            }
            else if (rFullName == "System.Boolean")
            {
                return rAssembly.MainModule.ImportReference((typeof(System.Boolean)));
            }
            else if (rFullName == "UnityEngine.Color")
            {
                return rAssembly.MainModule.ImportReference((typeof(UnityEngine.Color)));
            }
            else if (rFullName == "UnityEngine.Vector2")
            {
                return rAssembly.MainModule.ImportReference((typeof(UnityEngine.Vector2)));
            }
            else if (rFullName == "UnityEngine.Vector3")
            {
                return rAssembly.MainModule.ImportReference((typeof(UnityEngine.Vector3)));
            }
            else if (rFullName == "System.String")
            {
                return rAssembly.MainModule.ImportReference((typeof(System.String)));
            }
            else
            {
                //LogManager.LogWarning($"ViewModel没有类型{rFullName}对应的PropChanged方法");
                return rAssembly.MainModule.ImportReference(typeof(System.Object));
            }
        }

        /// <summary>
        /// 检测类型是否有类型对应的GetPropVale方法
        /// </summary>
        private static bool CheckTypeInViewModel(TypeReference rTypeReference)
        {
            if (rTypeReference.FullName == "System.Int32")
            {
                return true;
            }
            else if (rTypeReference.FullName == "System.Int64")
            {
                return true;
            }
            else if (rTypeReference.FullName == "System.Single")
            {
                return true;
            }
            else if (rTypeReference.FullName == "System.Boolean")
            {
                return true;
            }
            else if (rTypeReference.FullName == "UnityEngine.Color")
            {
                return true;
            }
            else if (rTypeReference.FullName == "UnityEngine.Vector2")
            {
                return true;
            }
            else if (rTypeReference.FullName == "UnityEngine.Vector3")
            {
                return true;
            }
            else if (rTypeReference.FullName == "System.String")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Type GetReturnFuncType(TypeReference rTypeReference)
        {
            if (rTypeReference.FullName == "System.Int32")
            {
                return typeof(Func<System.Int32>);
            }
            else if (rTypeReference.FullName == "System.Int64")
            {
                return typeof(Func<System.Int64>);
            }
            else if (rTypeReference.FullName == "System.Single")
            {
                return typeof(Func<System.Single>);
            }
            else if (rTypeReference.FullName == "System.Boolean")
            {
                return typeof(Func<System.Boolean>);
            }
            else if (rTypeReference.FullName == "UnityEngine.Color")
            {
                return typeof(Func<UnityEngine.Color>);
            }
            else if (rTypeReference.FullName == "UnityEngine.Vector2")
            {
                return typeof(Func<UnityEngine.Vector2>);
            }
            else if (rTypeReference.FullName == "UnityEngine.Vector3")
            {
                return typeof(Func<UnityEngine.Vector3>);
            }
            else if (rTypeReference.FullName == "System.String")
            {
                return typeof(Func<System.String>);
            }
            else
            {
                return typeof(Func<System.Object>);
            }
        }

        #region HotfixTemplate获取基础类型引用
        private static TypeReference GetBoolValueTypeReference(AssemblyDefinition rAssembly)
        {
            var rHotfixTemplateType = rAssembly.MainModule.Types.SingleOrDefault(
                                        rType => rType != null &&
                                        rType.BaseType != null &&
                                        rType.FullName.Equals("Game.HotfixTemplate"));

            if (rHotfixTemplateType != null)
            {
                var rBoolValueField = rHotfixTemplateType.Fields.SingleOrDefault(
                                        rField => rField.Name.Equals("__Bool_Value_Template__"));
                return rBoolValueField.FieldType;
            }
            return null;
        }

        private static TypeReference GetValueTypeReference(AssemblyDefinition rAssembly, string rTypeName)
        {
            var rHotfixTemplateType = rAssembly.MainModule.Types.SingleOrDefault(
                                        rType => rType != null &&
                                        rType.BaseType != null &&
                                        rType.FullName.Equals("Game.HotfixTemplate"));

            if (rHotfixTemplateType != null)
            {
                var rRefreshMethod = rHotfixTemplateType.Methods.SingleOrDefault(
                                        rMethod => rMethod.Name.Equals(rTypeName));
                return rRefreshMethod.ReturnType;
            }
            return null;
        }
        #endregion

        private static bool IsBaseTypeEquals(AssemblyDefinition rUIAssembly, TypeDefinition rType, string rTypeName)
        {
            var rTempType = GetBaseType(rUIAssembly, rType, rTypeName);
            return rTempType != null;
        }

        private static TypeDefinition GetBaseType(AssemblyDefinition rUIAssembly, TypeDefinition rType, string rTypeName)
        {
            var rTempType = rType.BaseType as TypeDefinition;
            if (rTempType == null && rType.BaseType.FullName.Equals(rTypeName))
            {
                rTempType = rUIAssembly.MainModule.Types.SingleOrDefault(t => t.FullName.Equals(rTypeName));
            }
            while (rTempType != null)
            {
                if (rTempType.FullName.Equals(rTypeName))
                {
                    break;
                }
                var rSrcTempType = rTempType;
                rTempType = rTempType.BaseType as TypeDefinition;
                if (rTempType == null && rSrcTempType.BaseType.FullName.Equals(rTypeName))
                {
                    rTempType = rUIAssembly.MainModule.Types.SingleOrDefault(t => t.FullName.Equals(rTypeName));
                }
            }
            return rTempType;
        }

        #region 注入属性PropChanged方法
        private static void InjectType(AssemblyDefinition rAssembly, AssemblyDefinition rUIAssembly, TypeDefinition rNeedInjectType, string rPropertyName)
        {
            if (rNeedInjectType == null) return;

            PropertyDefinition rNeedInjectProperty = rNeedInjectType.Properties.Single(t => t.Name == rPropertyName);
            if (rNeedInjectProperty == null)
            {
                Console.WriteLine("Can not find property " + rPropertyName);
                return;
            }

            var rBaseType = GetBaseType(rUIAssembly, rNeedInjectType, "UnityEngine.UI.ViewModel");

            var rPropChangedMethod = rBaseType.Methods.SingleOrDefault(t => t.Name == "PropChanged" && t.Parameters.Count == 2 && t.Parameters[1].ParameterType == rNeedInjectProperty.PropertyType);
            if (rPropChangedMethod == null)
            {
                LogManager.LogWarning($"没有PropertyType对应的PropChanged方法：rNeedInjectProperty.PropertyType = {rNeedInjectProperty.PropertyType}");
                rPropChangedMethod = rBaseType.Methods.SingleOrDefault(t => t.Name == "PropChanged" && t.Parameters.Count == 2 && t.Parameters[1].ParameterType.Name == "Object");
            }
            var rPropChangedMethodRef = rAssembly.MainModule.ImportReference(rPropChangedMethod);

            // 通过Inject Property的名字找到对应的Set方法
            var rNeedInjectPropertySetMethod = rNeedInjectType.Methods.SingleOrDefault(t => t.Name == "set_" + rPropertyName);
            InjectProperty(rAssembly, rNeedInjectType, rPropChangedMethodRef, rNeedInjectPropertySetMethod, rNeedInjectProperty, rPropertyName);
        }

        private static void InjectProperty(AssemblyDefinition rAssembly, TypeDefinition rNeedInjectType, MethodReference rPropChangedMethod,
                                           MethodDefinition rNeedInjectPropertySetMethod, PropertyDefinition rNeedInjectProperty, string rPropertyName)
        {
            if (rPropChangedMethod == null) return;
            if (rNeedInjectPropertySetMethod == null) return;

            var rPropertyDefinition = rNeedInjectType.Properties.SingleOrDefault((x) => { return x.Name == rPropertyName; });

            var nInstructCount = rNeedInjectPropertySetMethod.Body.Instructions.Count;
            var rStartInsertPoint = rNeedInjectPropertySetMethod.Body.Instructions[0];
            var rEndInsertPoint = rNeedInjectPropertySetMethod.Body.Instructions[nInstructCount - 1];
            var rProcessor = rNeedInjectPropertySetMethod.Body.GetILProcessor();

            var bNeedInjectCheck = false;
            // 目前只做基础类型与String类型的检测
            if (rNeedInjectProperty != null && (
                rNeedInjectProperty.PropertyType.IsPrimitive ||
                rNeedInjectProperty.PropertyType.FullName == "System.String"))
            {
                bNeedInjectCheck = true;
            }

            if (bNeedInjectCheck)
            {
                var rFieldDefinition = rNeedInjectType.Fields.SingleOrDefault(t => t.Name == $"<{rPropertyName}>k__BackingField");
                if (rFieldDefinition == null)
                {
                    bNeedInjectCheck = false;
                }
                else
                {
                    // 将参数入推送到计算堆栈
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
                    // 将字段值推送到计算堆栈
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ldfld, rFieldDefinition));
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ldarg_1));
                    // 比较两个值。 如果这两个值相等，则将整数值 1 (int32) 推送到计算堆栈上；否则，将 0 (int32) 推送到计算堆栈上。
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ceq));
                    // 将 0 (int32)推送到计算堆栈
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ldc_I4_0));
                    // 比较两个值。 如果这两个值相等，则将整数值 1 (int32) 推送到计算堆栈上；否则，将 0 (int32) 推送到计算堆栈上。
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Ceq));
                    // 如果 value 为 false、空引用或零，则将控制转移到目标指令。
                    rProcessor.InsertBefore(rStartInsertPoint, rProcessor.Create(OpCodes.Brfalse_S, rEndInsertPoint));
                }
            }
            rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
            rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Ldstr, rPropertyName));

            rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Ldarg_1));
            if (!CheckTypeInViewModel(rPropertyDefinition.PropertyType))
            {
                rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Box, rPropertyDefinition.PropertyType));
            }
            rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Call, rPropChangedMethod));
            rProcessor.InsertBefore(rEndInsertPoint, rProcessor.Create(OpCodes.Nop));
        }
        #endregion

        #region 注入获取属性值代码
        private static void InjectViewModelGetPropValueMethod(TypeReference rType, AssemblyDefinition rAssembly, TypeDefinition rNeedInjectType)
        {
            var rAddPropGetterToDictsMethod = rNeedInjectType.Methods.SingleOrDefault(t => t.Name == "AddPropGetterToDicts");
            if (rAddPropGetterToDictsMethod == null)
            {
                var rVoidTypeRef = rAssembly.MainModule.ImportReference(typeof(void));
                rAddPropGetterToDictsMethod = new MethodDefinition("AddPropGetterToDicts", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Virtual, rVoidTypeRef);
                var rProcessor = rAddPropGetterToDictsMethod.Body.GetILProcessor();
                rAddPropGetterToDictsMethod.Body.Instructions.Add(rProcessor.Create(OpCodes.Nop));
                rAddPropGetterToDictsMethod.Body.Instructions.Add(rProcessor.Create(OpCodes.Ret));
                rNeedInjectType.Methods.Add(rAddPropGetterToDictsMethod);
            }
        }

        private static void InjectTypeGetPropValueMethod_OneProp(TypeReference rPropType, AssemblyDefinition rAssembly, AssemblyDefinition rUIAssembly,
                                                                 TypeDefinition rNeedInjectClassType, PropertyDefinition rNeedInjectProp)
        {
            var rNeedInjectGetValueMethod = rNeedInjectClassType.Methods.SingleOrDefault(t => t.Name == $"GetProp_{rNeedInjectProp.Name}");
            if (rNeedInjectGetValueMethod != null) return;

            var rNeedInjectPropertyGetMethod = rNeedInjectClassType.Methods.SingleOrDefault(t => t.Name == "get_" + rNeedInjectProp.Name);
            var rPropertyDefinition = rNeedInjectClassType.Properties.SingleOrDefault(x => x.Name == rNeedInjectProp.Name);
            if (rNeedInjectPropertyGetMethod == null || rPropertyDefinition == null) return;

            rNeedInjectGetValueMethod = new MethodDefinition($"GetProp_{rNeedInjectProp.Name}", Mono.Cecil.MethodAttributes.Public, rPropType);
            var rProcessor = rNeedInjectGetValueMethod.Body.GetILProcessor();
            rProcessor.Body.Variables.Add(new VariableDefinition(rAssembly.MainModule.ImportReference(rPropType)));

            rNeedInjectGetValueMethod.Body.Instructions.Add(rProcessor.Create(OpCodes.Ret));

            var nInstructCount = rNeedInjectGetValueMethod.Body.Instructions.Count;
            var rInsertPoint = rNeedInjectGetValueMethod.Body.Instructions[0];

            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Nop));
            Instruction rLdLoc1Instruction = rProcessor.Create(OpCodes.Ldloc_0);
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Call, rNeedInjectPropertyGetMethod));
            if (!CheckTypeInViewModel(rPropertyDefinition.PropertyType))
            {
                rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Box, rPropertyDefinition.PropertyType));
            }
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Stloc_0));
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Br_S, rLdLoc1Instruction));
            rProcessor.InsertBefore(rInsertPoint, rLdLoc1Instruction);

            rNeedInjectClassType.Methods.Add(rNeedInjectGetValueMethod);
        }

        private static void InjectType_AddPropGetterToDicts(Dict<string, List<PropertyDefinition>> rNeedInjectPropDict, AssemblyDefinition rAssembly,
                                                            AssemblyDefinition rUIAssembly, TypeDefinition rNeedInjectClassType)
        {
            var rBaseType = GetBaseType(rUIAssembly, rNeedInjectClassType, rNeedInjectClassType.BaseType.FullName);
            var rAddPropGetterToDictsMethodBase = rBaseType.Methods.SingleOrDefault(t => t.Name == "AddPropGetterToDicts");
            if (rAddPropGetterToDictsMethodBase == null)
            {
                LogManager.LogError($"rNeedInjectType = {rNeedInjectClassType.FullName}, rBaseType = {rBaseType.FullName} , AddPropGetterToDicts");
                rBaseType = GetBaseType(rUIAssembly, rNeedInjectClassType, "UnityEngine.UI.ViewModel");
                rAddPropGetterToDictsMethodBase = rBaseType.Methods.SingleOrDefault(t => t.Name == "AddPropGetterToDicts");
            }
            var rAddPropGetterToDictsMethod = rNeedInjectClassType.Methods.SingleOrDefault(t => t.Name == "AddPropGetterToDicts");
            if (rAddPropGetterToDictsMethod == null)
            {
                return;
            }

            var nInstructCount = rAddPropGetterToDictsMethod.Body.Instructions.Count;
            var rInsertPoint = rAddPropGetterToDictsMethod.Body.Instructions[nInstructCount - 1];
            var rProcessor = rAddPropGetterToDictsMethod.Body.GetILProcessor();
            //最后一个调用Base
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Call, rAddPropGetterToDictsMethodBase));
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Nop));
            foreach (var rNeedInjectTypePropPair in rNeedInjectPropDict)
            {
                var rPropertyType = GetType(rNeedInjectTypePropPair.Key, rAssembly);

                var rFuncMethodConstructInfo = GetReturnFuncType(rPropertyType).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
                var rFuncMethodConstructType = rAssembly.MainModule.ImportReference(rFuncMethodConstructInfo);
                var rViewModelType = rAssembly.MainModule.Types.SingleOrDefault(t => t.FullName.Equals("UnityEngine.UI.ViewModel"));
                var rNeedInjectTypePropGetterTypeMethod = rViewModelType.Methods.SingleOrDefault(t => t.Name == $"AddPropGetterToDict_{rPropertyType.Name}");
                foreach (var rNeedInjectTypeProp in rNeedInjectTypePropPair.Value)
                {
                    var rNeedInjectTypePropGetterMethod = rNeedInjectClassType.Methods.SingleOrDefault(t => t.Name == $"GetProp_{rNeedInjectTypeProp.Name}");
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldstr, rNeedInjectTypeProp.Name));
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldarg_0));
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Ldftn, rNeedInjectTypePropGetterMethod));
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Newobj, rFuncMethodConstructType));
                    rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Call, rNeedInjectTypePropGetterTypeMethod));
                }
            }
            rProcessor.InsertBefore(rInsertPoint, rProcessor.Create(OpCodes.Nop));
        }
        #endregion
    }
}