//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knight.Core
{
    public enum LoadingViewStyle
    {
        Mutil,//登录背景图
        Single,//单图片loading条
    }
    public interface ILoadingView
    {
        bool IsInLoading { get; set; }
        LoadingViewStyle LoadingViewStyle { get; set; }
        void ShowLoading(float fTargetProgress, string rTextTips, LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil,bool bIsShowTips = true);
        void ShowLoading(float rIntervalTime, float fTargetProgress, string rTextTips, LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil, bool bIsReset = true,bool bIsShowTips = true);
        void SetLoadingProgress(float fProgressValue);
        void HideLoading();
        void HideLoadingImmediate();
        void HideBottomInfo();
        void SetTips(string rTextTips, string rTextTips2);
        void SetVersionTextVisible();
        void HideBackground();
        void ShowBackground();
        void SetBackgroundLayer();
        void ShowHideLoadingInfo(bool bActive);
    }

    public class GameLoading : MonoBehaviour
    {
        private static GameLoading __instance;
        public static GameLoading Instance { get { return __instance; } }

        /// <summary>
        /// 加载界面
        /// </summary>
        public ILoadingView LoadingView;
        void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
            }
        }

        /// <summary>
        /// 开始加载
        /// </summary>
        public void StartLoading(float rIntervalTime, float fTargetProgress, string rTextTips = "", LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil, bool bIsReset = true,bool bIsShowTips = true)
        {
            LogManager.Log("[GameLoading] StartLoading");
            if (this.LoadingView != null)
            {
                this.LoadingView.ShowLoading(rIntervalTime, fTargetProgress, rTextTips, rLoadingViewStyle, bIsReset,bIsShowTips);
            }
        }

        public void StartLoading(float fTargetProgress, string rTextTips = "", LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil,bool bIsShowTips = true)
        {
            LogManager.Log("[GameLoading] StartLoading");
            if (this.LoadingView != null)
            {
                this.LoadingView.ShowLoading(fTargetProgress, rTextTips, rLoadingViewStyle);
            }
        }
        public void SetVersionTextVisible()
        {
            this.LoadingView.SetVersionTextVisible();
        }
        public void SetTips(string rTextTips, string rTextTips2 = "")
        {
            if (this.LoadingView != null)
                this.LoadingView.SetTips(rTextTips, rTextTips2);
        }

        public void SetLoadingProgress(float fProgressValue)
        {
            if (this.LoadingView != null)
                this.LoadingView.SetLoadingProgress(fProgressValue);
        }

        public void Hide(bool bIsHideBackGround = true)
        {
            if (this.LoadingView != null)
            {
                this.LoadingView.HideLoading();
                if (bIsHideBackGround)
                {
                    this.LoadingView.HideBackground();
                }
            }
            LogManager.Log($"[GameLoading] Hide bIsHideBackGround = {bIsHideBackGround}");
        }

        public void HideImmediate(bool bIsHideBackGround)
        {
            if (this.LoadingView != null)
            {
                this.LoadingView.HideLoadingImmediate();
                if (bIsHideBackGround)
                {
                    this.LoadingView.HideBackground();
                }
            }
            LogManager.Log($"[GameLoading] HideImmediate bIsHideBackGround = {bIsHideBackGround}");
        }

        public void HideBottomInfo()
        {
            if (this.LoadingView != null)
                this.LoadingView.HideBottomInfo();
        }

        public void ShowHideLoadingInfo(bool bActive)
        {
            if (this.LoadingView != null)
                this.LoadingView.ShowHideLoadingInfo(bActive);
        }

        public void HideBackground()
        {
            if (this.LoadingView != null)
                this.LoadingView.HideBackground();
            LogManager.Log("[GameLoading] HideBackground");
        }
        public void ShowBackground()
        {
            if (this.LoadingView != null)
                this.LoadingView.ShowBackground();
            LogManager.Log("[GameLoading] ShowBackground");
        }

        public void SetBackgroundLayer()
        {
            if (this.LoadingView != null)
                this.LoadingView.SetBackgroundLayer();
        }

        public void SetHeroCardInfo()
        {
            if (this.LoadingView != null)
                this.LoadingView.SetBackgroundLayer();
        }
    }
}