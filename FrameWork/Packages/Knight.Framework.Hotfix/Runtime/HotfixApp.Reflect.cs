//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Knight.Core;

namespace Knight.Framework.Hotfix
{
    // public class HotfixApp_Reflect : HotfixApp
    // {
    //     private Assembly mApp;
    //
    //     public override void Dispose()
    //     {
    //         this.mApp = null;
    //         this.App = null;
    //     }
    //
    //     public override async Task Load(string rABPath, string rHotfixModuleName)
    //     {
    //         var rRequest = await HotfixAssetLoader.Instance.Load(rABPath, rHotfixModuleName);
    //         this.InitApp(rRequest.DLLPath, rRequest.PDBPath,rHotfixModuleName);
    //     }
    //     
    //     public override void InitApp(byte[] rDLLBytes, byte[] rPDBBytes)
    //     {
    //         this.mApp = Assembly.Load(rDLLBytes, rPDBBytes);
    //         this.App = this.mApp;
    //     }
    //
    //     public override void InitApp(string rDLLPath, string rPDBPath, string rModuleName)
    //     {
    //         this.mApp = Assembly.LoadFile("F:\\NewFrame\\GFrame2\\GFrame\\"+rDLLPath);
    //         this.App = this.mApp;
    //     }
    //
    //     public override HotfixObject Instantiate(string rTypeName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return null;
    //         var rObject = new HotfixObject(this, rTypeName);
    //         rObject.Object = Activator.CreateInstance(this.mApp.GetType(rTypeName), rArgs);
    //         return rObject;
    //     }
    //
    //     public override T Instantiate<T>(string rTypeName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return default(T);
    //         return (T)Activator.CreateInstance(this.mApp.GetType(rTypeName), rArgs);
    //     }
    //
    //     public override IHotfixMethod GetMethod(string rClassName, string rMethodName, int nParamCount)
    //     {
    //         if (this.mApp == null) return null;
    //         var rType = TypeResolveManager.Instance.GetType(rClassName);
    //         var rMethod = new HotfixMethod_Reflect();
    //         rMethod.Initialize(nParamCount);
    //         rMethod.Method = rType.GetMethod(rMethodName);
    //         return rMethod;
    //     }
    //
    //     public override object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return null;
    //         Type rObjType = rObj.GetType();
    //         return ReflectionAssist.MethodMember(rObj, rMethodName, ReflectionAssist.flags_method_inst, rArgs);
    //     }
    //
    //     public override object Invoke(HotfixObject rHotfixObj, string rMethodName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return null;
    //         Type rObjType = this.mApp.GetType(rHotfixObj.TypeName);
    //         return ReflectionAssist.MethodMember(rHotfixObj.Object, rMethodName, ReflectionAssist.flags_method_inst, rArgs);
    //     }
    //
    //     public override object Invoke(HotfixObject rHotfixObj, IHotfixMethod rHotfixMethod)
    //     {
    //         if (this.mApp == null) return null;
    //         var rMethodReflect = rHotfixMethod as HotfixMethod_Reflect;
    //         return rMethodReflect.Method.Invoke(rHotfixObj.Object, rHotfixMethod.Params);
    //     }
    //
    //     public override object InvokeParent(HotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return null;
    //         Type rObjType = this.mApp.GetType(rParentType);
    //         return ReflectionAssist.MethodMember(rHotfixObj.Object, rMethodName, ReflectionAssist.flags_method_inst, rArgs);
    //     }
    //
    //     public override object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
    //     {
    //         if (this.mApp == null) return null;
    //         Type rObjType = this.mApp.GetType(rTypeName);
    //         return rObjType.InvokeMember(rMethodName, ReflectionAssist.flags_method | BindingFlags.Static, null, null, rArgs);
    //     }
    //
    //     public override Type[] GetTypes()
    //     {
    //         if (this.mApp == null) return null;
    //         return this.mApp.GetTypes();
    //     }
    // }
}
