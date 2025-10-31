//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knight.Core;
using System.IO;
using UnityEngine;

namespace Knight.Framework.Hotfix
{
    public class HotfixManager : TSingleton<HotfixManager>
    {
        public static string IsHotfixDebugModeKey = "HofixManager_IsHofixDebugMode";
        public static string HotfixDebugDllDir = "Library/ScriptAssemblies/";
        public static string HotfixDllDir = "Assets/Game/GameAsset/Hotfix/Libs/";

        private HotfixApp mHofixApp;
        public HotfixApp HofixApp => this.mHofixApp;
        
        private HotfixManager()
        {
        }

        public void Initialize()
        {
            this.mHofixApp = new HotfixApp_DLL();
        }

        public void Dispose()
        {
            this.mHofixApp?.Dispose();
        }

        // public void InitApp(byte[] rDLLBytes, byte[] rPDBBytes, HotfixApp rApp)
        // {
        //     if (rApp == null) return;
        //     rApp.InitApp(rDLLBytes, rPDBBytes);
        // }

        public void InitApp(string rModuleName, HotfixApp rApp)
        {
            if (rApp == null) return;
            rApp.InitApp(rModuleName);
        }

        public async Task Load(string rHotfixModuleName, HotfixApp rApp,byte[] dllBytes, byte[] pdbByes)
        {
            if (rApp == null) return;
            // await rApp.Load(rHotfixModuleName);
            await ((HotfixApp_DLL)rApp).Load(rHotfixModuleName, dllBytes, pdbByes);
        }

        public HotfixObject Instantiate(HotfixApp rApp, string rTypeName, params object[] rArgs)
        {
            if (rApp == null) return null;
            return rApp.Instantiate(rTypeName, rArgs);
        }

        public T Instantiate<T>( HotfixApp rApp, string rTypeName, params object[] rArgs)
        {
            if (rApp == null) return default(T);
            return rApp.Instantiate<T>(rTypeName, rArgs);
        }

        public IHotfixMethod GetMethod(HotfixApp rApp, string rClassName, string rMethodName, int nParamCount)
        {
            if (this.mHofixApp == null) return null;
            return this.mHofixApp.GetMethod(rClassName, rMethodName, nParamCount);
        }

        public object Invoke(HotfixApp rApp,object rObj, string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.mHofixApp == null) return null;
            return this.mHofixApp.Invoke(rObj, rTypeName, rMethodName, rArgs);
        }

        public object Invoke(HotfixApp rApp,HotfixObject rHotfixObj, string rMethodName, params object[] rArgs)
        {
            if (this.mHofixApp == null) return null;
            return this.mHofixApp.Invoke(rHotfixObj, rMethodName, rArgs);
        }

        public object InvokeParent(HotfixApp rApp,HotfixObject rHotfixObj, string rParentType, string rMethodName, params object[] rArgs)
        {
            if (this.mHofixApp == null) return null;
            return this.mHofixApp.InvokeParent(rHotfixObj, rParentType, rMethodName, rArgs);
        }

        public object InvokeStatic(HotfixApp rApp,string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.mHofixApp == null) return null;
            return this.mHofixApp.InvokeStatic(rTypeName, rMethodName, rArgs);
        }

        public bool IsHotfixDebugMode()
        {
#if UNITY_EDITOR
            bool bIsHotfixDebugMode = false;
            bIsHotfixDebugMode = UnityEditor.EditorPrefs.GetBool(HotfixManager.IsHotfixDebugModeKey);
            return bIsHotfixDebugMode;
#else
#if HOTFIX_REFLECT_USE
            return true;
#else
            return false;
#endif
#endif
        }

        public bool IsHotfixDLLMode()
        {
#if HOTFIX_DLL_USE
            return true;
#else
            return false;
#endif
        }

        public bool IsHotfixHybridCLRMode()
        {
#if HOTFIX_HYBRIDCLR_USE
            return true;
#else
            return false;
#endif
        }

        public Type[] GetTypes()
        {
            if (this.HofixApp == null)
            {
                LogManager.LogRelease("GetTypes Null");
                return null;
            }
            return this.HofixApp.GetTypes();
        }
    }
}
