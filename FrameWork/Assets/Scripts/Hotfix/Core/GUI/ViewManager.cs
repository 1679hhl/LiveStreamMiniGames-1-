//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using Knight.Core;
using System;
using System.Threading.Tasks;
using Knight.Framework.Tweening;
using System.Collections.Generic;
using Game;
using Knight.Hotfix.Core;
using UnityFx.Async;

namespace UnityEngine.UI
{
    public class ViewManager : THotfixSingleton<ViewManager>
    {
        public List<string> mIgnoreUIList = new List<string>()
        {
            "UINewUserGuide",
            "UIStageGuide",
        };
        /// <summary>
        /// 存放各种动态节点的地方
        /// </summary>
        public GameObject DynamicRoot;
        /// <summary>
        /// 存放各种弹出框节点的地方
        /// </summary>
        public GameObject PopupRoot;
        /// <summary>
        /// 存放场景3DUI的地方
        /// </summary>
        public GameObject Scene3DRoot;
        /// <summary>
        /// 是否在缓存中
        /// </summary>
        public Func<string, bool> FuncIsInCache;

        /// <summary>
        /// 当前的UI中的Views，每个View是用GUID来作唯一标识
        /// 底层-->顶层 { 0 --> list.count }
        /// </summary>
        private IndexedDict<string, View> mCurPageViews;
        public IndexedDict<string, View> CurPageViews => this.mCurPageViews;

        private IndexedDict<string, View> mCurPopupViews;
        public IndexedDict<string, View> CurPopupViews => this.mCurPopupViews;
        private IndexedDict<string, View> mCurFixedViews;
        public IndexedDict<string, View> CurFixedViews => this.mCurFixedViews;
        private IndexedDict<string, View> mCurMainCity3DViews;
        public IndexedDict<string, View> CurMainCity3DViews => this.mCurMainCity3DViews;

        private IndexedDict<string, View> mCurPopUpFixedViews;
        public IndexedDict<string, View> CurPopUpFixedViews => this.mCurPopUpFixedViews;

        /// <summary>
        /// 当前打开的所有UI面板的(GUID, controller类名)
        /// </summary>
        public Dict<string, string> PanelNameDic;

        public bool NeedGlobalBlur = false;

        private ViewManager()
        {
        }

        public void Initialize()
        {
            this.DynamicRoot = UIRoot.Instance.DynamicRoot;
            this.PopupRoot = UIRoot.Instance.PopupRoot;
            this.Scene3DRoot = new GameObject("Scene3DRoot");
            this.Scene3DRoot.transform.SetParent(UIRoot.Instance.transform);

            this.mCurPageViews = new IndexedDict<string, View>();
            this.mCurPopupViews = new IndexedDict<string, View>();
            this.mCurFixedViews = new IndexedDict<string, View>();
            this.mCurPopUpFixedViews = new IndexedDict<string, View>();
            this.mCurMainCity3DViews = new IndexedDict<string, View>();
            this.PanelNameDic = new Dict<string, string>();
            UIRoot.Instance.UIViewNames = new HashSet<string>();

            this.CheckNeedBlurInUITop();
        }

        public View PreLoad(string rViewName, View.State rViewState)
        {
            var rLoaderRequest = UIAssetLoader.Instance.LoadUI(rViewName);
            return this.PreLoadView(rViewName, rLoaderRequest.ViewPrefabGo, rViewState);
        }

        public async Task<View> PreLoadAsync(string rViewName, View.State rViewState)
        {
            return await this.PreLoadViewAsync(rViewName, rViewState);
        }

        private async Task<View> PreLoadViewAsync(string rViewName, View.State rViewState)
        {
            var rLoaderRequest = await UIAssetLoader.Instance.LoadUIAsync(rViewName);
            return this.PreLoadView(rViewName, rLoaderRequest.ViewPrefabGo, rViewState);
        }

        /// <summary>
        /// 框架内部使用，不要调用这个接口  打开一个View
        /// </summary>
        public void Open(View rView, Action<View> rOpenCompleted = null)
        {
            this.OpenAsync(rView, rOpenCompleted).WrapErrors();
        }

        /// <summary>
        /// 框架内部使用，不要调用这个接口  打开一个View
        /// </summary>
        public async Task<View> OpenAsync(View rView, Action<View> rOpenCompleted = null)
        {
            // 企图关闭当前的View
            await this.MaybeCloseTopView(rView.CurState);
            var rCtrlName = rView.ViewController.GetType().Name;
            if (rCtrlName.IndexOf("GM") == -1 && rCtrlName.IndexOf("GlobalTouch") == -1 && !this.PanelNameDic.ContainsKey(rView.GUID))//GM与touch不放入此记录开启的map
            {
                this.PanelNameDic.Add(rView.GUID, rCtrlName);
            }
            return await this.OpenViewAsync(rView, rOpenCompleted);
        }

        /// <summary>
        /// 框架内部使用，不要调用这个接口
        /// </summary>
        private async Task<View> OpenViewAsync(View rView, Action<View> rOpenCompleted = null)
        {
            return await this.OpenView(rView, rOpenCompleted);
        }

        public View PreLoadView(string rViewName, GameObject rViewPrefab, View.State rViewState)
        {
            if (rViewPrefab == null)
            {
                LogManager.LogErrorFormat("ViewPrefab {0} is not exist.", rViewName);
                return null;
            }

            this.SetViewGoEnable(rViewPrefab, false);

            //把View的GameObject结点加到rootCanvas下
            GameObject rViewGo = null;
            switch (rViewState)
            {
                case View.State.PageSwitch:
                case View.State.Fixing:
                    rViewGo = this.DynamicRoot.transform.AddChildNoScale(rViewPrefab, "UI");
                    break;
                case View.State.Popup:
                case View.State.PopupFixing:
                    rViewGo = this.PopupRoot.transform.AddChild(rViewPrefab, "UITop");
                    break;
                case View.State.SceneMainCity3D:
                    rViewGo = this.Scene3DRoot.transform.AddChildNoScale(rViewPrefab, "Default");



                    break;
            }
            this.SetViewSortOrder(rViewName, rViewState, rViewGo);
            this.SetViewGoEnable(rViewGo, false);

            // 为了提高效率
            if (AssetLoader.Instance.IsSumilateMode_GUI())
            {
                this.SetViewGoEnable(rViewPrefab, true);
            }

            var rView = View.CreateView(rViewGo);
            string rViewGUID = Guid.NewGuid().ToString();           //生成GUID
            if (rView == null)
            {
                LogManager.LogErrorFormat("GameObject {0} is null.", rViewName);
                return null;
            }
            // bool bIsCache = true;
            // if (GameConfig.Instance.GameUI.Table.TryGetValue(rViewName, out var rGameUIRow))
            // {
            //     bIsCache = rGameUIRow.IsBackCache;
            // }
            rView.Initialize(rViewName, rViewGUID, rViewState);     //为View的初始化设置
            return rView;
        }
        /// <summary>
        ///PageSwitch:1000以下
        ///PopUp(弹窗)：1000以上1500以下
        ///全局：1500以上3000以下
        /// </summary>
        /// <param name="rViewName"></param>
        /// <param name="rViewState"></param>
        /// <param name="rViewGo"></param>
        private void SetViewSortOrder(string rViewName, View.State rViewState, GameObject rViewGo)
        {
            if (this.mIgnoreUIList.Contains(rViewName))
            {
                return;
            }
            var rCanvas = rViewGo.GetComponent<Canvas>();
            if (rCanvas != null)
            {
                // if (GameConfig.Instance.GameUI.Table.TryGetValue(rViewName, out var rConfig))
                // {
                //     rCanvas.overrideSorting = true;
                //     rCanvas.sortingOrder = rConfig.SortingOrder;
                //
                // }
                // else
                // {
                //     if (rViewState == View.State.Popup || rViewState == View.State.PopupFixing)
                //
                //     {
                //         rCanvas.overrideSorting = true;
                //         rCanvas.sortingOrder = 1000;
                //     }
                //
                // }

                //容错处理
                if (rViewState == View.State.Popup || rViewState == View.State.PopupFixing)
                {
                    if (rCanvas.sortingOrder < 1000)
                    {
                        LogManager.LogWarning($"弹窗层级错误！！！，VieName = {rViewName},SortingOrder = {rCanvas.sortingOrder}");
                        rCanvas.sortingOrder = rCanvas.sortingOrder + 1000;
                    }

                }
            }

        }

        private void SetViewGoEnable(GameObject rViewGo, bool bIsEnabled)
        {
            if (rViewGo == null) return;

            var rGraphicRaycasters = rViewGo.GetComponentsInChildren<GraphicRaycaster>(true);
            if (rGraphicRaycasters != null)
            {
                for (int i = 0; i < rGraphicRaycasters.Length; ++i)
                    rGraphicRaycasters[i].enabled = bIsEnabled;
            }
            var rTweenAnimators = rViewGo.GetComponentsInChildren<TweeningAnimator>(true);
            for (int i = 0; i < rTweenAnimators.Length; i++)
            {
                rTweenAnimators[i].enabled = bIsEnabled;
            }
            var rAnimators = rViewGo.GetComponentsInChildren<Animator>(true);
            for (int i = 0; i < rAnimators.Length; i++)
            {
                rAnimators[i].Rebind();
                rAnimators[i].enabled = bIsEnabled;
            }
            var rPrefabName = rViewGo.name;
            var rImageReplaces = rViewGo.GetComponentsInChildren<ImageReplace>(true);
            for (int i = 0; i < rImageReplaces.Length; i++)
            {
                rImageReplaces[i].PrefabName = rPrefabName;
            }
            var rLoopScrollRects = rViewGo.GetComponentsInChildren<UnityEngine.UI.ScrollRect>(true);
            for (int i = 0; i < rLoopScrollRects.Length; i++)
            {
                rLoopScrollRects[i].enabled = bIsEnabled;
            }
        }

        /// <summary>
        /// 初始化View，如果是Dispatch类型的话，只对curViews顶层View进行交换
        /// </summary>
        public async Task<View> OpenView(View rView, Action<View> rOpenCompleted)
        {
            rView.GameObject.transform.localScale = Vector3.one;
            rView.GameObject.SetActive_Effective(true);
            this.SetViewGoEnable(rView.GameObject, true);
            //新的View的存储逻辑
            switch (rView.CurState)
            {
                case View.State.Fixing:
                    this.mCurFixedViews.Add(rView.GUID, rView);
                    break;
                case View.State.PageSwitch:
                    this.mCurPageViews.Add(rView.GUID, rView);
                    break;
                case View.State.Popup:
                    this.mCurPopupViews.Add(rView.GUID, rView);
                    break;
                case View.State.PopupFixing:
                    this.mCurPopUpFixedViews.Add(rView.GUID, rView);
                    break;
                case View.State.SceneMainCity3D:
                    this.mCurMainCity3DViews.Add(rView.GUID, rView);
                    break;
                default:
                    break;
            }
            UIRoot.Instance.UIViewNames.Add(rView.ViewName);
            await rView.Open();
            this.ChangeNeedBlurTopUI(rView);
            this.CheckNeedBlurInUITop();
            this.OpenRefreshHUDTopUI(rView);
            UtilTool.SafeExecute(rOpenCompleted, rView);
            rView.IsStartOpen = false;
            return rView;
        }

        /// <summary>
        /// 多个PopUpUI同时存在时，除最上层UI，其他PopUp需要被毛玻璃化
        /// </summary>
        /// <param name="rCurView"></param>
        private void ChangeNeedBlurTopUI(View rCurView)
        {
            if (rCurView.CurState == View.State.Popup && rCurView.IsNeedSeparableBlur)
            {
                for (int i = 0; i < this.mCurPopupViews.Count; i++)
                {
                    var rPreView = this.mCurPopupViews[this.mCurPopupViews.Keys[i]];
                    //当有PopUpFix NeedBlur 所有的界面都模糊
                    if (!this.NeedGlobalBlur)
                    {
                        if (rCurView.GUID == rPreView.GUID) return;
                    }
                    if (rPreView.CurViewLayer == View.ViewLayer.UI) continue;
                    if (!rPreView.CanChangeUILayer || rPreView.ViewController is ICanChangeUILayer) continue;
                    rPreView.SetViewLayer(View.ViewLayer.UI);
                }
            }
        }

        public void CheckNeedBlurInUITop()
        {
            // if (UIRoot.Instance == null || UIRoot.Instance.UICamera == null) return;
            //
            // var rUICameraData = UIRoot.Instance.UICamera.GetComponent<UniversalAdditionalCameraData>();
            // var nCurPopupCount = 0;
            // for (int i = 0; i < this.mCurPopupViews.Count; i++)
            // {
            //     if (this.mCurPopupViews[this.mCurPopupViews.Keys[i]].IsNeedSeparableBlur)
            //     {
            //         nCurPopupCount++;
            //     }
            // }
            //
            // var bNeedBlur = (nCurPopupCount > 0 || this.NeedGlobalBlur) && !LoadingView_Knight.Instance.IsInLoading;
            //
            // RenderAssetSetting.SetAfterPostProcessEnable(bNeedBlur,UIRoot.Instance.UICameraAdditionalData);
            // rUICameraData.renderPostProcessing = bNeedBlur;
            // UIRoot.Instance.UIGlobalVolume.SetActive(bNeedBlur);
            // EventManager.Instance.Distribute(GameEvent.kEventCameraPostProcessChange);
        }

        /// <summary>
        /// 检查是否需要Top条
        /// </summary>
        /// <param name="rCurView"></param>
        private void OpenRefreshHUDTopUI(View rCurView)
        {
            // if (rCurView != null)
            // {
            //     if (GameConfig.Instance.GameUI.Table.TryGetValue(rCurView.ViewName, out var rGameUICfg))
            //     {
            //         if (rGameUICfg.IsHUDShow)
            //         {
            //             //UIHUDManager.Instance.Open(rCurView);
            //         }
            //     }
            // }
        }

        private void CloseRefreshHUDTopUI(View rCurView)
        {
            // if (rCurView != null)
            // {
            //     if (GameConfig.Instance.GameUI.Table.TryGetValue(rCurView.ViewName, out var rGameUICfg))
            //     {
            //         if (rGameUICfg.IsHUDShow)
            //         {
            //             //UIHUDManager.Instance.Close(rCurView);
            //             UIHUDManager.Instance.BackView(rCurView.ViewName);
            //         }
            //     }
            // }
        }

        /// <summary>
        /// 检查弹窗数量是否为零
        /// </summary>
        /// <returns></returns>
        public bool CheckPopupCountIsZero()
        {
            var nCurPopupCount = 0;
            for (int i = 0; i < this.mCurPopupViews.Count; i++)
            {
                nCurPopupCount++;
            }
            return nCurPopupCount == 0;
        }

        /// <summary>
        /// 设置是否需要全局虚化背景
        /// </summary>
        /// <param name="bNeedGlobalBlur"></param>
        public void SetGlobalBlur(bool bNeedGlobalBlur)
        {
            this.NeedGlobalBlur = bNeedGlobalBlur;
            if (!bNeedGlobalBlur)
            {
                this.ReSetUIPopTopPanelLayer();
            }
            this.CheckNeedBlurInUITop();
        }

        /// <summary>
        /// 企图关闭一个当前的View，当存在当前View时候，并且传入的View是需要Dispatch的。
        /// </summary>
        private async Task MaybeCloseTopView(View.State rViewState)
        {
            // 得到顶层结点
            KeyValuePair<string, View> rTopNode = new KeyValuePair<string, View>();
            if (this.mCurPageViews.Count > 0)
                rTopNode = this.mCurPageViews.Last();
            if (rTopNode.Value == null) return;

            string rViewGUID = rTopNode.Key;
            View rView = rTopNode.Value;
            if (rView == null) return;

            if (rViewState == View.State.PageSwitch)
            {
                // 播放退出动画
                if (rView.Animator != null && !string.IsNullOrEmpty(rView.AnimClipName))
                {
                    rView.Animator.Play(rView.AnimClipName);
                    if (rView.IsNeedWaitCloseAnim)
                        await WaitAsync.WaitForSeconds(rView.AnimClipLength);
                }
                // 移除顶层结点
                this.mCurPageViews.Remove(rViewGUID);
                UIRoot.Instance.UIViewNames.Remove(rView.ViewName);

                rView.Close();
                rView.GameObject.SetActive_Effective(false);
                this.SetViewGoEnable(rView.GameObject, false);

                // 判断是否删除这个View
                if (this.FuncIsInCache == null || (this.FuncIsInCache != null && !this.FuncIsInCache(rView.GUID)))
                {
                    this.DestroyView(rView);
                }
                //从打开的panelName列表里面移除当前UI面板
                this.PanelNameDic.Remove(rView.GUID);
            }
        }

        /// <summary>
        /// 移除顶层View
        /// </summary>
        public void Pop(Action rCloseCompleted = null)
        {
            // 得到顶层结点
            KeyValuePair<string, View> rTopNode = this.mCurPageViews.Last();
            if (rTopNode.Value == null)
            {
                UtilTool.SafeExecute(rCloseCompleted);
                return;
            }

            string rViewGUID = rTopNode.Key;
            View rView = rTopNode.Value;

            if (rView == null)
            {
                UtilTool.SafeExecute(rCloseCompleted);
                return;
            }

            // 移除顶层结点
            this.mCurPageViews.Remove(rViewGUID);
            UIRoot.Instance.UIViewNames.Remove(rView.ViewName);

            rView.Close();
            this.DestroyView(rView);
            UtilTool.SafeExecute(rCloseCompleted);
        }

        /// <summary>
        /// 框架内部使用，不要调用这个接口 根据GUID来关闭指定的View
        /// </summary>
        public View CloseView(string rViewGUID)
        {
            var rView = this.CloseViewWithOutDestroy(rViewGUID);
            if (rView != null)
            {
                this.DestroyView(rView);
            }
            return rView;
        }


        /// <summary>
        /// 重置PopUp最上层的UI的层级为UITop
        /// </summary>
        public void ReSetUIPopTopPanelLayer()
        {
            if (this.mCurPopupViews != null && this.mCurPopupViews.Count != 0)
            {
                for (int i = this.mCurPopupViews.Count - 1; i >= 0; i--)
                {
                    var rTempTopNode = this.mCurPopupViews[this.mCurPopupViews.Keys[i]];
                    if (rTempTopNode == null) return;
                    LogManager.Log($"[重置PopUp最上层UI的层级] TopNodeViewName = {rTempTopNode.ViewName}");
                    if (!this.NeedGlobalBlur)
                    {
                        if (rTempTopNode.CurViewLayer == View.ViewLayer.UITop) return;
                        LogManager.Log($"[重置PopUp最上层UI的层级成功] TopNodeViewName = {rTempTopNode.ViewName}");
                        rTempTopNode.SetViewLayer(View.ViewLayer.UITop);
                    }
                    //部分Tips打开时，下层UI不需要模糊，返回时需要恢复
                    if (rTempTopNode.IsNeedSeparableBlur)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 框架内部使用，不要调用这个接口 根据GUID来关闭指定的View
        /// </summary>
        public View CloseViewWithOutDestroy(string rViewGUID)
        {
            View rView = null;

            // 找到View
            if (!this.mCurFixedViews.TryGetValue(rViewGUID, out rView) &&
                !this.mCurPageViews.TryGetValue(rViewGUID, out rView) &&
                !this.mCurPopupViews.TryGetValue(rViewGUID, out rView) &&
                !this.mCurPopUpFixedViews.TryGetValue(rViewGUID, out rView) &&
                !this.mCurMainCity3DViews.TryGetValue(rViewGUID, out rView))
            {
                return null;
            }

            // View存在
            if (rView.CurState == View.State.PageSwitch)
            {
                this.mCurPageViews.Remove(rViewGUID);
            }
            else if (rView.CurState == View.State.Popup)
            {
                this.mCurPopupViews.Remove(rViewGUID);
            }
            else if (rView.CurState == View.State.Fixing)
            {
                this.mCurFixedViews.Remove(rViewGUID);
            }
            else if (rView.CurState == View.State.PopupFixing)
            {
                this.mCurPopUpFixedViews.Remove(rViewGUID);
            }
            else if (rView.CurState == View.State.SceneMainCity3D)
            {
                this.mCurMainCity3DViews.Remove(rViewGUID);
            }
            UIRoot.Instance.UIViewNames.Remove(rView.ViewName);

            // 移除顶层结点
            rView.Close();
            rView.GameObject.SetActive_Effective(false);
            this.SetViewGoEnable(rView.GameObject, false);

            this.CheckNeedBlurInUITop();
            this.CloseRefreshHUDTopUI(rView);
            //从打开的panelName列表里面移除当前UI面板
            this.PanelNameDic.Remove(rView.GUID);

            return rView;
        }

        /// <summary>
        /// 等待View关闭动画播放完后开始删除一个View
        /// </summary>
        public void DestroyView(View rView)
        {
            rView.Dispose();
            if (rView.IsImmediateClose)
            {
                UtilTool.SafeDestroy(rView.GameObject);
            }
            else
            {
                GameObject.Destroy(rView.GameObject);
            }
            UIAssetLoader.Instance.UnloadUI(rView.ViewName);
            // 战斗内不做卸载操作
            // if (!GameBattle.Instance.IsInBattle && RenderLevelManager.Instance.IsLowMemeryDevice)
            // {
            //     Resources.UnloadUnusedAssets();
            // }
            rView = null;
        }

        /// <summary>
        /// 删除现存资源
        /// </summary>
        public void UnloadUIAssetsWithout(List<string> rWithoutViews)
        {
            this.UnloadUIAssetWithout(this.mCurPageViews, rWithoutViews);
            this.UnloadUIAssetWithout(this.mCurPopupViews, rWithoutViews);
            this.UnloadUIAssetWithout(this.mCurFixedViews, rWithoutViews);
            this.UnloadUIAssetWithout(this.mCurPopUpFixedViews, rWithoutViews);
            this.UnloadUIAssetWithout(this.mCurMainCity3DViews, rWithoutViews);
        }

        private void UnloadUIAssetWithout(IndexedDict<string, View> rCurViews, List<string> rWithoutViews)
        {
            var rNeedRemoveViews = new HashSet<View>();
            for (int i = 0; i < rCurViews.Count; i++)
            {
                var rKey = rCurViews.Keys[i];
                if (rWithoutViews.Contains(rCurViews[rKey].ViewName))
                    continue;
                rNeedRemoveViews.Add(rCurViews[rKey]);
            }
            foreach (var rView in rNeedRemoveViews)
            {
                this.CloseView(rView.GUID);
                UIAssetLoader.Instance.UnloadUI(rView.ViewName);
                ViewPreLoaderManager.Instance.ViewCaches.Remove(rView.ViewName);
            }
            // 移除未在ViewManager中管理的资源
            UIAssetLoader.Instance.UnloadWithoutUIAssets(rWithoutViews);
        }

        /// <summary>
        /// 移除指定的UI资源
        /// </summary>
        public void UnloadAssignedAUIAssets(List<string> rAssignedUIAssets)
        {
            for (int i = 0; i < rAssignedUIAssets.Count; i++)
            {
                UIAssetLoader.Instance.UnloadUI(rAssignedUIAssets[i]);
            }
        }

        public void Destroy()
        {
            this.DynamicRoot = null;
            this.PopupRoot = null;
            if (this.Scene3DRoot)
                GameObject.Destroy(this.Scene3DRoot);
            this.Scene3DRoot = null;
            // this.mCurPageViews?.Clear();
            // this.mCurPageViews = null;
            // this.mCurPopupViews?.Clear();
            // this.mCurPopupViews = null;
            // this.mCurFixedViews?.Clear();
            // this.mCurFixedViews = null;
            // this.mCurPopUpFixedViews?.Clear();
            // this.mCurPopUpFixedViews = null;
            // this.mCurMainCity3DViews?.Clear();
            // this.mCurMainCity3DViews = null;
            // this.PanelNameDic?.Clear();
            // this.PanelNameDic = null;
            // UIRoot.Instance.UIViewNames?.Clear();
            // UIRoot.Instance.UIViewNames = null;
        }
    }
}
