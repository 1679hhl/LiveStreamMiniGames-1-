//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using Knight.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityFx.Async;
using static UnityEngine.UI.UIAssetLoader;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class UIRoot : MonoBehaviour
    {
        private static UIRoot __instance;
        public static UIRoot Instance { get { return __instance; } }

        public GameObject DynamicRoot;
        public RectTransform DynamicRootRectTransform;
        public GameObject GlobalsRoot;
        public GameObject PopupRoot;
        public GameObject Render2DRoot;
        public RectTransform BattleNumberRoot;
        public GameObject BGNoneGo;

        public Camera UICamera;
        // public UnityEngine.Rendering.Universal.UniversalAdditionalCameraData UICameraAdditionalData;

        public CanvasScaler CanvasScaler;
        public EventSystem UIEventSystem;

        public GameObject UIGlobalVolume;

        public Func<int> GetPopupViewCount;
        public List<string> GlobalViewNames;
        public List<string> IgnoreNames;

        public HashSet<string> UIViewNames;

        public int MutiTouchDisableCount = 0;

        public RectTransform HeadinfoTransform;

        public RectTransform AttributeTransform;
        
        public RectTransform GiftTipsTransform;

        public Camera MainCamera;
        public Transform Zero;

        void Awake()
        {
            if (__instance == null)
                __instance = this;

            this.DynamicRootRectTransform = this.DynamicRoot.GetComponent<RectTransform>();
            // this.UICameraAdditionalData =
            //     this.UICamera.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        }

        private void OnEnable()
        {
            __instance = this;
        }

        private void OnDisable()
        {
            __instance = this;
        }

        public void Initialize()
        {
            this.MutiTouchDisableCount = 0;

            // 屏蔽掉Navigation
            this.UIEventSystem.sendNavigationEvents = false;

            this.DynamicRoot.SetActive(true);
            this.GlobalsRoot.SetActive(true);
            this.PopupRoot.SetActive(true);

            this.GlobalViewNames = new List<string>() { "KNLoading", "GlobalMessageBox", "Toast",};
            this.IgnoreNames = new List<string>() {"KNLoading","Toast","GlobalMessageBox" };
        }

        public async Task PreLoadGlobalMessage_Async()
        {
            var rRequest = new LoaderRequest("GlobalMessageBox");
            var rAssetRequest = await UIAssetLoader.Instance.LoadUIAsync(rRequest.ViewName);
            if (rAssetRequest.ViewPrefabGo != null)
            {
                rRequest.ViewPrefabGo = rAssetRequest.ViewPrefabGo;
                rRequest.ViewPrefabGo.SetActive_Effective(true);
                this.GlobalsRoot.transform.AddChildNoScale(rRequest.ViewPrefabGo, "UITop");
            }
        }

        public async Task LoadLoadingView_Async()
        {
            var rSplashRequest = await UIAssetLoader.Instance.LoadUIAsync("KNLoading");
            GameObject rLoadingPrefabGo = null;
            if (rSplashRequest.ViewPrefabGo != null)
            {
                rLoadingPrefabGo = rSplashRequest.ViewPrefabGo;
                rLoadingPrefabGo.SetActive_Effective(true);
                this.GlobalsRoot.transform.AddChildNoScale(rLoadingPrefabGo, "UITop");
            }
        }

        public async Task LoadEssentialView_Async(bool bIsContainLoading)
        {
            foreach (var rViewName in this.GlobalViewNames)
            {
                if (!bIsContainLoading && rViewName.Equals("KNLoading")) continue;
                if (!bIsContainLoading && rViewName.Equals("GlobalMessageBox")) continue;

                var rRequest = new LoaderRequest(rViewName);
                var rAssetRequest = await UIAssetLoader.Instance.LoadUIAsync(rRequest.ViewName);
                if (rAssetRequest.ViewPrefabGo != null)
                {
                    rRequest.ViewPrefabGo = rAssetRequest.ViewPrefabGo;
                    rRequest.ViewPrefabGo.SetActive_Effective(true);
                    this.GlobalsRoot.transform.AddChildNoScale(rRequest.ViewPrefabGo, "UITop");
                }
            }
        }

        public void RemoveGlobalInitViews(List<string> rRemovedUIAssets)
        {
            for (int i = 0; i < rRemovedUIAssets.Count; i++)
            {
                var rUITrans = this.GlobalsRoot.transform.Find(rRemovedUIAssets[i]);
                if (rUITrans != null)
                {
                    UtilTool.SafeDestroy(rUITrans.gameObject);
                }
            }
        }
        
        public void RemoveGlobalInitViewsWithoutKNLGloMsg()
        {
            for (int i = 0; i < this.GlobalViewNames.Count; i++)
            {
                if(this.GlobalViewNames[i].Equals("KNLoading")) continue;
                if(this.GlobalViewNames[i].Equals("GlobalMessageBox")) continue;

                var rUITrans = this.GlobalsRoot.transform.Find(this.GlobalViewNames[i]);
                if (rUITrans != null)
                {
                    UtilTool.SafeDestroy(rUITrans.gameObject);
                }
            }
        }

        // 将2D根节点的缩放同步到canvas的大小，使坐标系相同
        public void Sync2DRootScaleToUICanvas()
        {
            this.Render2DRoot.transform.localScale = this.PopupRoot.transform.localScale;
        }

        public async Task ReloadGlobalUI()
        {
            this.RemoveGlobalInitViews(this.GlobalViewNames);
            await this.LoadEssentialView_Async(true);
        }

        public void DisableMultiTouch()
        {
            this.MutiTouchDisableCount++;
            if (this.MutiTouchDisableCount > 0)
            {
                Input.multiTouchEnabled = false;    //禁止多点触摸
            }
        }

        public void EnableMultiTouch()
        {
            this.MutiTouchDisableCount--;
            if (this.MutiTouchDisableCount <= 0)
                this.MutiTouchDisableCount = 0;
            if (this.MutiTouchDisableCount == 0)
            {
                Input.multiTouchEnabled = true;     //启用多点触摸
            }
        }
    }
}
