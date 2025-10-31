//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using UnityEngine;
using UnityFx.Async;

namespace Knight.Framework.Hotfix
{
    public interface IGameMainLogic
    {
        void Initialize();
        void OnDestroy();
    }

    public class HotfixGameMainLogic : MonoBehaviour
    {
        private static HotfixGameMainLogic __instance;
        public static HotfixGameMainLogic Instance { get { return __instance; } }

        public string MainLogicScript;

        public HotfixObject MainLogicHotfixObj;
        public IHotfixMethod MainLogicHotfixUpdateMethod;
        public IHotfixMethod MainLogicHotfixLateUpdateMethod;
        public IHotfixMethod MainLogicHotfixFixedUpdateMethod;

        public IGameMainLogic mGameMainLogic;

        void Awake()
        {
            if (__instance == null)
                __instance = this;
        }

        public void Initialize()
        {
            #if UNITY_EDITOR
            this.mGameMainLogic = HotfixManager.Instance.Instantiate(HotfixManager.Instance.HofixApp,this.MainLogicScript).Object as IGameMainLogic;
            this.mGameMainLogic.Initialize();
            return;
            #endif
            LogManager.LogRelease("Initialize2");
            this.mGameMainLogic = HotfixManager.Instance.Instantiate(HotfixManager.Instance.HofixApp,this.MainLogicScript).Object as IGameMainLogic;
            this.mGameMainLogic.Initialize();
            return;
            // if (!HotfixManager.Instance.IsHotfixDLLMode())
            // {
            //     LogManager.LogRelease("Initialize1");
            //     //加载Hotfix端的代码
            //     this.MainLogicHotfixObj = HotfixManager.Instance.Instantiate(HotfixManager.Instance.HofixApp,this.MainLogicScript);
            //     this.MainLogicHotfixUpdateMethod = HotfixManager.Instance.GetMethod(HotfixManager.Instance.HofixApp,this.MainLogicScript, "Update", 0);
            //     this.MainLogicHotfixLateUpdateMethod = HotfixManager.Instance.GetMethod(HotfixManager.Instance.HofixApp,this.MainLogicScript, "LateUpdate", 0);
            //     this.MainLogicHotfixFixedUpdateMethod = HotfixManager.Instance.GetMethod(HotfixManager.Instance.HofixApp,this.MainLogicScript, "FixedUpdate", 0);
            //     //加载Hotfix端的代码
            //     HotfixManager.Instance.Invoke(HotfixManager.Instance.HofixApp,this.MainLogicHotfixObj, "Initialize");
            // }
            // else
            // {
            //     LogManager.LogRelease("Initialize2");
            //     this.mGameMainLogic = HotfixManager.Instance.Instantiate(HotfixManager.Instance.HofixApp,this.MainLogicScript).Object as IGameMainLogic;
            //     this.mGameMainLogic.Initialize();
            // }
        }

        void Update()
        {
            HotfixUpdateManager.Instance.Update();
        }

        void LateUpdate()
        {
            HotfixUpdateManager.Instance.LateUpdate();
        }

        public void OnDestroy()
        {
            if (!HotfixManager.Instance.IsHotfixDLLMode() && !HotfixManager.Instance.IsHotfixHybridCLRMode())
            {
                if (this.MainLogicHotfixObj == null) return;
                HotfixManager.Instance.Invoke(HotfixManager.Instance.HofixApp,this.MainLogicHotfixObj, "OnDestroy");
            }
            else
            {
                this.mGameMainLogic?.OnDestroy();
            }
        }

#if UNITY_EDITOR
        void OnApplicationQuit()
        {
            this.OnDestroy();
        }
#endif
    }
}
