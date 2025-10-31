using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using Knight.Hotfix.Core;
using UnityFx.Async;
using Game;
using Knight.Framework.Hotfix;

namespace UnityEngine.UI
{
    public class FrameManager
    {
        // 逻辑渲染分离，不使用THotfixSingleton
        // 逻辑战斗时子类改写实例，重载部分函数，阻止UI弹出
        protected static FrameManager _instance;
        public static FrameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FrameManager();
                return _instance;
            }
        }

        public class BackCache
        {
            public string ViewName;
            public string ViewGUID;
            public View.State State;
            public System.Object[] GoToCache;
        }

        public enum BackCacheType
        {
            Normal,
            UIHUD,
            Extra
        }
        public class BtnBackCache
        {
            public string ViewName;
            public string ViewGUID;
            public View.State State;
            public BackCacheType BackCacheType;
            public bool IsLoading;
        }

        public int BackCacheCount => this.mBackCaches == null ? 0 : this.mBackCaches.Count;

        /// <summary>
        /// 回退的缓存
        /// </summary>
        private List<BackCache> mBackCaches;
        /// <summary>
        /// 回退缓存字典（key:view名字 value:view在上面那个列表里的索引）
        /// </summary>
        private Dictionary<string, int> mBackCacheDict;
        private bool mFirstBack;
        public Stack<BtnBackCache> AllBackCaches = new Stack<BtnBackCache>();

        /// <summary>
        /// 当前界面上一个底层界面（PageSwitch）
        /// </summary>
        private BackCache mLastPageView;
        protected FrameManager()
        {
            this.mBackCaches = new List<BackCache>();
            this.mBackCacheDict = new Dictionary<string, int>();
            this.mLastPageView = new BackCache();
        }

        #region 框架内部接口

        /// <summary>
        /// 清除当前界面的缓存
        /// </summary>
        private void ClearLastPageView()
        {
            if (string.IsNullOrEmpty(this.mLastPageView.ViewName)) return;
            this.mLastPageView.ViewGUID = string.Empty;
            this.mLastPageView.ViewName = string.Empty;
            LogManager.Log("ClearLastPageView");
        }
        /// <summary>
        /// 根据GUID获取打开的View
        /// </summary>
        /// <param name="rGUID"></param>
        /// <returns></returns>
        private View GetView(string rGUID)
        {
            if (ViewManager.Instance.CurFixedViews.TryGetValue(rGUID, out var rView))
            {
                return rView;
            }
            if (ViewManager.Instance.CurPageViews.TryGetValue(rGUID, out rView))
            {
                return rView;
            }

            if (ViewManager.Instance.CurPopupViews.TryGetValue(rGUID, out rView))
            {
                return rView;
            }
            if (ViewManager.Instance.CurPopUpFixedViews.TryGetValue(rGUID, out rView))
            {
                return rView;
            }
            if (ViewManager.Instance.CurMainCity3DViews.TryGetValue(rGUID, out rView))
            {
                return rView;
            }
            return null;
        }

        public View GetViewByName(string rViewName)
        {
            if (string.IsNullOrEmpty(rViewName)) return null;
            foreach (var rDict in ViewManager.Instance.CurFixedViews.Dict)
            {
                if (rDict.Value.ViewName.Equals(rViewName))
                {
                    return rDict.Value;
                }
            }
            foreach (var rDict in ViewManager.Instance.CurPageViews.Dict)
            {
                if (rDict.Value.ViewName.Equals(rViewName))
                {
                    return rDict.Value;
                }
            }
            foreach (var rDict in ViewManager.Instance.CurPopupViews.Dict)
            {
                if (rDict.Value.ViewName.Equals(rViewName))
                {
                    return rDict.Value;
                }
            }
            foreach (var rDict in ViewManager.Instance.CurPopUpFixedViews.Dict)
            {
                if (rDict.Value.ViewName.Equals(rViewName))
                {
                    return rDict.Value;
                }
            }
            return null;
        }

        public View IsUIOpen(string rViewName)
        {
            if (ViewPreLoaderManager.Instance.IsViewCanRepeat(rViewName)) return null;
            View rResView = null;
            rResView = this.GetViewByName(rViewName);
            return rResView;
        }


        /// <summary>
        /// 判断界面是否已打开（可多次打开的界面，默认为未打开）
        /// </summary>
        /// <param name="rViewName"></param>
        /// <param name="rViewState"></param>
        /// <returns></returns>
        public View IsUIOpen(string rViewName, View.State rViewState)
        {
            if (ViewPreLoaderManager.Instance.IsViewCanRepeat(rViewName)) return null;
            IndexedDict<string, View> rDict = null;
            switch (rViewState)
            {
                case View.State.Fixing:
                    rDict = ViewManager.Instance.CurFixedViews;
                    break;
                case View.State.Popup:
                    rDict = ViewManager.Instance.CurPopupViews;
                    break;
                case View.State.PageSwitch:
                    rDict = ViewManager.Instance.CurPageViews;
                    break;
                case View.State.PopupFixing:
                    rDict = ViewManager.Instance.CurPopUpFixedViews;
                    break;
                case View.State.SceneMainCity3D:
                    rDict = ViewManager.Instance.CurMainCity3DViews;
                    break;
                default:
                    break;
            }
            if (rDict == null)
                return null;
            var rGUIDList = rDict.Keys;
            for (int i = 0; i < rGUIDList?.Count; i++)
            {
                if (rDict[rGUIDList[i]].ViewName == rViewName)
                {
                    return rDict[rGUIDList[i]];
                }
            }

            return null;
        }

        protected virtual async Task<View> OpenPageUIAsync(string rViewName, View.State rState, Action<View> rOpenCompleted = null, bool IsBackView = false, System.Object[] rGotoParams = null)
        {
            if (rState == View.State.PageSwitch && !IsBackView)
            {
                this.CloseAllViewByState(View.State.Popup);
            }
            var rView = this.IsUIOpen(rViewName, rState);
            if (rView != null)
            {
                rView.GotoParam = rGotoParams;
                rOpenCompleted?.Invoke(rView);
                LogManager.LogRelease($"{rViewName}界面已打开");
                return rView;
            }
            if (rViewName == "UIMainFrame")
            {
                await this.SwitchToMainCity();

                if (!IsBackView)
                    this.ClearBackCache();
            }
            rView = ViewPreLoaderManager.Instance.GetViewCache(rViewName);
            if (rView == null)
            {
                rView = await ViewManager.Instance.PreLoadAsync(rViewName, rState);
            }

            rView.GotoParam = rGotoParams;

            System.Object[] rGotoCache = null;
            if (rView.IsBackCache && rView.CurState != View.State.Fixing && rView.CurState != View.State.PopupFixing)
            {
                if (!IsBackView && !string.IsNullOrEmpty(this.mLastPageView.ViewName))
                {
                    var rLastView = this.IsUIOpen(this.mLastPageView.ViewName);
                    rGotoCache = rLastView?.ViewController.CacheParamOnGoOut();
                }
            }

            await ViewManager.Instance.OpenAsync(rView, rOpenCompleted);

            if (rView.BtnBackCache != null) rView.BtnBackCache.IsLoading = false;

            if (rView.IsBackCache && rView.CurState != View.State.Fixing && rView.CurState != View.State.PopupFixing)
            {
                if (!IsBackView && !string.IsNullOrEmpty(this.mLastPageView.ViewName))
                {
                    this.mFirstBack = true;
                    if (this.mBackCacheDict.ContainsKey(this.mLastPageView.ViewName) && this.mBackCacheDict[this.mLastPageView.ViewName] < this.mBackCaches.Count)
                    {
                        var rViewCache = this.mBackCaches[this.mBackCacheDict[this.mLastPageView.ViewName]];
                        rViewCache.ViewName = "";
                        rViewCache.ViewGUID = "";
                    }
                    this.mBackCaches.Add(new BackCache()
                    {
                        ViewName = this.mLastPageView.ViewName,
                        ViewGUID = this.mLastPageView.ViewGUID,
                        State = this.mLastPageView.State,
                        GoToCache = rGotoCache,
                    });
                    if (this.mBackCacheDict.ContainsKey(this.mLastPageView.ViewName))
                        this.mBackCacheDict[this.mLastPageView.ViewName] = this.mBackCaches.Count - 1;
                    else
                        this.mBackCacheDict.Add(this.mLastPageView.ViewName, this.mBackCaches.Count - 1);
                    LogManager.Log($"BackCaches.Add: ViewName = {this.mLastPageView.ViewName}, State = {this.mLastPageView.State}");
                    this.ClearLastPageView();
                }

                this.mLastPageView.ViewName = rView.ViewName;
                this.mLastPageView.ViewGUID = rView.GUID;
                this.mLastPageView.State = rView.CurState;

                LogManager.Log($"this.mLastPageView:  ViewName = {this.mLastPageView.ViewName}, State = {this.mLastPageView.State}");
            }
            return rView;
        }

        public bool CloseUIByBackBtn()
        {
            //如果是全局非热更弹窗
            if (GlobalMessageBox.Instance.gameObject.activeSelf)
            {
                if (GlobalMessageBox.Instance.CancelButton.gameObject.activeSelf)
                {
                    GlobalMessageBox.Instance.CancelButton.onClick.Invoke();
                }
                else
                {
                    GlobalMessageBox.Instance.ConfirmButton.onClick.Invoke();
                }

                GlobalMessageBox.Instance.Close();
                return true;
            }
            if (this.AllBackCaches.Count == 0) return false;

            //取栈顶元素
            var rBackCache = this.AllBackCaches.Peek();

            if (rBackCache.IsLoading) return true;

            //特殊处理UIMessageBox
            // if (rBackCache.ViewName.Equals("UIMessageBox"))
            // {
            //     var rViewController = MessageBox.Instance.MessageBoxView.ViewController;
            //     if (rViewController?.GetViewModel("MessageBox") is MessageBoxViewModel rMesViewModel)
            //     {
            //         if (rViewController is MessageBoxView rMesViewController)
            //         {
            //             //取消存在
            //             if (rMesViewModel.mIsBtnCancelActive)
            //             {
            //                 rMesViewController.BackAction_Cancel();
            //                 this.AllBackCaches.Pop();
            //                 return true;
            //             }
            //             //关闭不存在  取消不存在
            //             if (!rMesViewModel.mIsBtnCancelActive && !rMesViewModel.mIsBtnCloseActive)
            //             {
            //                 rMesViewController.BackAction_Confirm();
            //                 this.AllBackCaches.Pop();
            //                 return true;
            //             }
            //         }
            //
            //     }
            // }

            //如果该界面已经被关闭  或者是 repeat界面
            if (rBackCache != null
                && rBackCache.BackCacheType == BackCacheType.Normal
                && !string.IsNullOrEmpty(rBackCache.ViewName)
                && this.IsUIOpen(rBackCache.ViewName, rBackCache.State) == null)
            {
                //可以repeat的界面
                if (ViewPreLoaderManager.Instance.IsViewCanRepeat(rBackCache.ViewName))//rBackCache.ViewName.Equals("UIMessageBox"))
                {
                    if (this.Close(rBackCache.ViewGUID))
                    {
                        if (this.AllBackCaches.Peek().ViewName.Equals(rBackCache.ViewName))
                            this.AllBackCaches.Pop();
                        return true;
                    }
                }
                //如果关闭失败 则进入递归
                if (this.AllBackCaches.Peek().ViewName.Equals(rBackCache.ViewName))
                    this.AllBackCaches.Pop();
                return this.CloseUIByBackBtn();
            }

           
            return false;
        }

        protected virtual void OpenPageUI(string rViewName, View.State rState, Action<View> rOpenCompleted = null, bool IsBackView = false)
        {
            if (rState == View.State.PageSwitch && !IsBackView)
            {
                this.CloseAllViewByState(View.State.Popup);
            }
            var rView = this.IsUIOpen(rViewName, rState);
            if (rView != null)
            {
                rOpenCompleted?.Invoke(rView);
                LogManager.Log($"{rViewName}界面已打开");
                return;
            }
            if (!IsBackView && rViewName == "UIMainFrame")
            {
                FrameManager.Instance.ClearBackCache();
                this.SwitchToMainCity().WrapErrors();
            }
            rView = ViewPreLoaderManager.Instance.GetViewCache(rViewName);
            if (rView == null)
            {
                rView = ViewManager.Instance.PreLoad(rViewName, rState);
            }
            if (rView == null)
            {
                LogManager.Log($"{rViewName}界面打开失败，找不到预制体");
                rOpenCompleted?.Invoke(rView);
                return;
            }

            ViewManager.Instance.Open(rView, (rViewResult) =>
            {
                if (rViewResult != null)
                {
                    if (rViewResult.IsBackCache && rViewResult.CurState != View.State.Fixing && rViewResult.CurState != View.State.PopupFixing)
                    {
                        if (!IsBackView && !string.IsNullOrEmpty(this.mLastPageView.ViewName))
                        {
                            var rLastView = this.IsUIOpen(this.mLastPageView.ViewName);
                            var rGotoCache = rLastView?.ViewController.CacheParamOnGoOut();

                            this.mFirstBack = true;
                            if (this.mBackCacheDict.ContainsKey(this.mLastPageView.ViewName) && this.mBackCacheDict[this.mLastPageView.ViewName] < this.mBackCaches.Count)
                            {
                                var rViewCache = this.mBackCaches[this.mBackCacheDict[this.mLastPageView.ViewName]];
                                rViewCache.ViewName = "";
                                rViewCache.ViewGUID = "";
                            }

                            this.mBackCaches.Add(new BackCache()
                            {
                                ViewName = this.mLastPageView.ViewName,
                                ViewGUID = this.mLastPageView.ViewGUID,
                                State = this.mLastPageView.State,
                                GoToCache = rGotoCache,
                            });
                            if (this.mBackCacheDict.ContainsKey(this.mLastPageView.ViewName))
                                this.mBackCacheDict[this.mLastPageView.ViewName] = this.mBackCaches.Count - 1;
                            else
                                this.mBackCacheDict.Add(this.mLastPageView.ViewName, this.mBackCaches.Count - 1);
                            LogManager.Log($"BackCaches.Add:  ViewName = {this.mLastPageView.ViewName}, State = {this.mLastPageView.State}");
                            this.ClearLastPageView();
                        }

                        this.mLastPageView.ViewName = rViewResult.ViewName;
                        this.mLastPageView.ViewGUID = rViewResult.GUID;
                        this.mLastPageView.State = rViewResult.CurState;
                        LogManager.Log($"Open---this.mLastPageView:  ViewName = {this.mLastPageView.ViewName}, State = {this.mLastPageView.State}");
                    }
                    rOpenCompleted?.Invoke(rViewResult);

                    if (rViewResult.BtnBackCache != null)
                    {
                        rViewResult.BtnBackCache.IsLoading = false;
                    }

                }
            });
        }

        public async Task CheckBeforeClose(string rGUID)
        {
            if (rGUID.IsNullOrEmpty()) return;

            var rView = ViewPreLoaderManager.Instance.GetViewCacheByGUID(rGUID);
            if (rView == null)
            {
                rView = this.GetView(rGUID);
            }
            if (rView != null)
            {
                if (rView.Animator != null && !string.IsNullOrEmpty(rView.AnimClipName))
                {
                    rView.Animator.Play(rView.AnimClipName);
                    await WaitAsync.WaitForSeconds(rView.AnimClipLength);
                }
            }
        }

        private async Task CloseView(string rGUID, bool bClearCache = false)
        {
            await this.CheckBeforeClose(rGUID);
            if (bClearCache)
            {
                var rBackCache = this.mBackCaches.Find((rItem) => { return rItem.ViewGUID.Equals(rGUID); });
                if (rBackCache != null)
                {
                    this.mBackCaches.Remove(rBackCache);
                    if (this.mBackCacheDict.ContainsKey(rBackCache.ViewName))
                        this.mBackCacheDict.Remove(rBackCache.ViewName);
                    LogManager.Log($"BackView: BackCaches Remove!!! ViewName = {rBackCache.ViewName}, ViewGUID = {rBackCache.ViewGUID}");
                }
            }
            var rView = ViewPreLoaderManager.Instance.GetViewCacheByGUID(rGUID);
            var rViewName = string.Empty;
            if (rView != null)
            {
                ViewManager.Instance.CloseViewWithOutDestroy(rGUID);
                rViewName = rView.ViewName;
            }
            else
            {
                rView = ViewManager.Instance.CloseView(rGUID);
                if (rView != null)
                {
                    rViewName = rView.ViewName;
                }
            }
            //if (rView.IsNeedCloseSound)
            //{
            //    SoundPlayer.Instance.SoundPlay("UI_SE_JieMian_S_001", EAudioMixerGroupType.UI, EAudioPlayType.Additive, 100);
            //}
            if (rView.CurState == View.State.Popup)
            {
                ViewManager.Instance.ReSetUIPopTopPanelLayer();
            }
            LogManager.Log($"内部接口关闭UI成功，VIewName = {rViewName}");
        }

        private async Task SwitchToMainCity()
        {
        }
        #endregion
        #region 外部调用接口

        //----------------------------打开UI-Start-----------------------------
        public virtual async Task<View> OpenPageUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            var rView = await this.OpenPageUIAsync(rViewName, View.State.PageSwitch, rOpenCompleted);
            return rView;
        }
        public virtual async Task<View> OpenPageUIAsyncBackView(string rViewName, Action<View> rOpenCompleted = null, bool IsBackView = false)
        {
            return await this.OpenPageUIAsync(rViewName, View.State.PageSwitch, rOpenCompleted, IsBackView);
        }
        public void OpenPageUI(string rViewName, Action<View> rOpenCompleted = null)
        {
            this.OpenPageUIAsync(rViewName, rOpenCompleted).WrapErrors();
        }

        /// <summary>
        /// 在主城中的某个挂点下打开UI
        /// </summary>
        /// <param name="rViewName">UI名字</param>
        /// <param name="rNode">场景挂点名称</param>
        /// <returns></returns>
        public async Task<View> OpenMainCityScene3DUIAsync(string rViewName, string rNodeID, string RolePoint)
        {
            // if (Game.World.Instance.WorldMgr.SceneMgr.SceneConfigMgr.GetPosition(rNodeID, out var rPosition, out var rQuaternion))
            // {
            //     var rView = await this.OpenPageUIAsync(rViewName, View.State.SceneMainCity3D, (rPlayerLevelRankView) =>
            //     {
            //         var rViewModel = rPlayerLevelRankView.ViewController.GetViewModel("PlayerLevelRank") as PlayerLevelRankViewModel;
            //         var rViewController = rPlayerLevelRankView.ViewController as PlayerLevelRankView;
            //
            //         rViewModel.RankPlaneObj = Game.World.Instance.WorldMgr.SceneMgr.SceneConfigMgr.GetPosProduct(rNodeID);
            //         rViewModel.RankPlaneObj.SetActive(true);
            //         rViewModel.RoleObj = Game.World.Instance.WorldMgr.SceneMgr.SceneConfigMgr.GetPosProduct(RolePoint);
            //         HotfixUpdateManager.Instance.AddTickUpdate(rViewController.mLoopSwitchTime, rViewController.RefreshNextRank);
            //
            //         rViewController.mRankTypeIndex = 0;
            //         rViewController.SetRankTypeList();
            //         // 测试一下热更新 
            //         rViewController.RequestRefrshRankList();
            //
            //         rViewController.OnEventRefreshRankListUI(null).WrapErrors();
            //     });
            //     if (rView == null || rView.GameObject == null)
            //         return null;
            //     rView.GameObject.transform.position = rPosition;
            //     rView.GameObject.transform.rotation = rQuaternion;
            //     rView.GameObject.transform.localScale = Vector3.one;
            //     return rView;
            // }
            // Knight.Core.LogManager.LogError($"MainCiyt3DUI Node {rNodeID} config not find");
            return null;
        }

        public virtual async Task<View> OpenPopUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            return await this.OpenPageUIAsync(rViewName, View.State.Popup, rOpenCompleted);
        }

        public void OpenPopupUI(string rViewName, Action<View> rOpenCompleted = null)
        {
            this.OpenPageUI(rViewName, View.State.Popup, rOpenCompleted);
        }

        public void OpenFixedUI(string rViewName, Action<View> rOpenCompleted = null)
        {
            this.OpenPageUI(rViewName, View.State.Fixing, rOpenCompleted, false);
        }
        public virtual async Task<View> OpenFixedAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            return await this.OpenPageUIAsync(rViewName, View.State.Fixing, rOpenCompleted, false);
        }

        public void OpenPopupFixedUI(string rViewName, Action<View> rOpenCompleted = null)
        {
            this.OpenPageUI(rViewName, View.State.PopupFixing, rOpenCompleted, false);
        }
        public virtual async Task<View> OpenPopupFixedUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            return await this.OpenPageUIAsync(rViewName, View.State.PopupFixing, rOpenCompleted, false);
        }
        /// <summary>
        /// 关闭不缓存的UI
        /// </summary>
        public bool Close(string rGUID)
        {
            if (rGUID.IsNullOrEmpty()) return false;
            var rView = ViewPreLoaderManager.Instance.GetViewCacheByGUID(rGUID);
            var rViewName = string.Empty;
            if (rView != null)
            {
                if (rView.CurState != View.State.Fixing && rView.CurState != View.State.PopupFixing && rView.IsBackCache)
                {
                    LogManager.LogRelease($"关闭UI失败，VIewName = {rView.ViewName}, 只有View.State = Fixing的或者不缓存的View可以调用此接口");
                    return false;
                }
                ViewManager.Instance.CloseViewWithOutDestroy(rGUID);
                rViewName = rView.ViewName;
            }
            else
            {
                rView = this.GetView(rGUID);
                if (rView == null)
                {
                    LogManager.Log($"关闭UI失败,界面未打开");
                    return false;
                }
                if (rView.CurState != View.State.Fixing && rView.CurState != View.State.PopupFixing && rView.IsBackCache)
                {
                    LogManager.LogError($"关闭UI失败,VIewName = {rView.ViewName}, 只有View.State = Fixing的或者不缓存的View可以调用此接口");
                    return false;
                }
                rView = ViewManager.Instance.CloseView(rGUID);
                if (rView != null)
                {
                    rViewName = rView.ViewName;
                }
            }
            if (rView.CurState == View.State.Popup)
            {
                ViewManager.Instance.ReSetUIPopTopPanelLayer();
            }

            LogManager.Log($"关闭UI成功, VIewName = {rViewName}");
            return true;
        }

        /// <summary>
        /// UI回退
        /// </summary>
        public void BackView(Action rOpenCompleted = null)
        {
            this.BackViewAsync(rOpenCompleted).WrapErrors();
        }

        public async Task CloseWithAnim(string rGUID)
        {
            await this.CheckBeforeClose(rGUID);
            this.Close(rGUID);
        }

        /// <summary>
        /// 界面回退
        /// 逻辑模式重载为空
        /// </summary>
        public virtual async Task BackViewAsync(Action rOpenCompleted = null)
        {
            //SoundPlayer.Instance.SoundPlay("UI_SE_JieMian_S_001", EAudioMixerGroupType.UI, EAudioPlayType.Additive, 100);
            //如果回退列表里面有当前打开的view，清掉回退列表里的
            if (this.mFirstBack && this.mBackCacheDict.ContainsKey(this.mLastPageView.ViewName) && this.mBackCacheDict[this.mLastPageView.ViewName] < this.mBackCaches.Count)
            {
                this.mFirstBack = false;
                var rViewCache = this.mBackCaches[this.mBackCacheDict[this.mLastPageView.ViewName]];
                rViewCache.ViewName = "";
                rViewCache.ViewGUID = "";
                this.mBackCacheDict.Remove(this.mLastPageView.ViewName);
            }

            if (this.mBackCaches == null || this.mBackCaches.Count == 0)
            {
                await this.OpenPageUIAsync("UILobby1", View.State.PageSwitch, null, true);
                LogManager.Log($"BackView: ViewName = UILobby1");
            }
            else
            {
                while(this.mBackCaches[this.mBackCaches.Count - 1].ViewName == "")
                {
                    this.mBackCaches.RemoveAt(this.mBackCaches.Count - 1);
                }

                var rBackcache = this.mBackCaches[this.mBackCaches.Count - 1];
                if (this.mLastPageView.State == View.State.Popup)
                {
                    await this.CloseView(this.mLastPageView.ViewGUID);

                    this.mLastPageView.ViewName = rBackcache.ViewName;
                    this.mLastPageView.ViewGUID = rBackcache.ViewGUID;
                    this.mLastPageView.State = rBackcache.State;
                    LogManager.Log($"BackView---this.mLastPageView:  ViewName = {this.mLastPageView.ViewName}, State = {this.mLastPageView.State}");
                }
                else
                {
                    var rList = new List<BackCache>();


                    for (int i = this.mBackCaches.Count - 1; i >= 0; i--)
                    {

                        rList.Add(this.mBackCaches[i]);

                        if (this.mBackCaches[i].State == View.State.PageSwitch)
                        {
                            break;
                        }
                    }

                    //如果找不到上一个PageSwicth的界面，就打开默认界面
                    if (rList[rList.Count - 1].State != View.State.PageSwitch)
                    {
                        await this.OpenPageUIAsync("UIMainFrame", View.State.PageSwitch, null, true);
                        LogManager.Log($"BackView: NoView.State == PageSwitch!!! ViewName = UIMainFrame");
                    }
                    //打开从上一个PageSwicth的到当前PageSwitch中间的界面
                    for (int i = rList.Count - 1; i >= 0; i--)
                    {
                        var rBackCache = rList[i];
                        await this.OpenPageUIAsync(rBackCache.ViewName, rBackCache.State, null, true, rBackcache.GoToCache);
                    }
                }
                if (this.mBackCacheDict.ContainsKey(rBackcache.ViewName))
                    this.mBackCacheDict.Remove(rBackcache.ViewName);
                this.mBackCaches.Remove(rBackcache);
                LogManager.Log($"BackView: BackCaches Remove!!! ViewName = {rBackcache.ViewName}");

            }
            rOpenCompleted?.Invoke();
        }

        /// <summary>
        /// 清空界面缓存数据
        /// </summary>
        public void ClearBackCache()
        {
            this.ClearLastPageView();
            this.mBackCaches.Clear();
            this.mBackCacheDict.Clear();
            this.mFirstBack = false;
        }

        /// <summary>
        /// 取消当前界面缓存
        /// </summary>
        public void CancelCurViewCache()
        {
            this.ClearLastPageView();
        }

        /// <summary>
        /// 按State关闭所有界面
        /// </summary>
        /// <param name="rState"></param>
        /// <param name="bIsClearCache"></param>
        public void CloseAllViewByState(View.State rState, bool bIsClearCache = false)
        {
            var rGUIDList = new List<string>();
            switch (rState)
            {
                case View.State.Fixing:
                    foreach (var rDict in ViewManager.Instance.CurFixedViews.Dict)
                    {
                        rGUIDList.Add(rDict.Key);
                    }
                    break;
                case View.State.Popup:
                    rGUIDList.AddRange(ViewManager.Instance.CurPopupViews?.Keys);
                    break;
                case View.State.PageSwitch:
                    rGUIDList.AddRange(ViewManager.Instance.CurPageViews?.Keys);
                    break;
                case View.State.PopupFixing:
                    rGUIDList.AddRange(ViewManager.Instance.CurPopUpFixedViews?.Keys);
                    break;
                case View.State.SceneMainCity3D:
                    rGUIDList.AddRange(ViewManager.Instance.CurMainCity3DViews?.Keys);
                    break;
                default:
                    break;
            }

            for (int i = 0; i < rGUIDList.Count; i++)
            {
                this.CloseView(rGUIDList[i], bIsClearCache).WrapErrors();
            }
        }

        /// <summary>
        /// 关闭所有界面和清除缓存
        /// </summary>
        public virtual void CloseAll()
        {
            var rGUIDList = new List<string>();
            rGUIDList.AddRange(ViewManager.Instance.CurPageViews?.Keys);
            rGUIDList.AddRange(ViewManager.Instance.CurPopupViews?.Keys);
            rGUIDList.AddRange(ViewManager.Instance.CurFixedViews?.Keys);
            rGUIDList.AddRange(ViewManager.Instance.CurPopUpFixedViews?.Keys);
            rGUIDList.AddRange(ViewManager.Instance.CurMainCity3DViews?.Keys);
            for (int i = 0; i < rGUIDList.Count; i++)
            {
                this.CloseView(rGUIDList[i]).WrapErrors();
            }
            ViewManager.Instance.CurPageViews?.Clear();
            ViewManager.Instance.CurPopupViews?.Clear();
            ViewManager.Instance.CurFixedViews?.Clear();
            ViewManager.Instance.CurPopUpFixedViews?.Clear();
            ViewManager.Instance.PanelNameDic?.Clear();
            ViewManager.Instance.CurMainCity3DViews?.Clear();

            this.ClearBackCache();
        }
        [System.Obsolete]
        public async Task OpenMainPopupFixedViews()
        {
            await FrameManager.Instance.OpenPopupFixedUIAsync("UILobby_TopTips");
        }

        #endregion
    }
}
