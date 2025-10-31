using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnityEngine.UI
{
    public class ViewContainer
    {
        public enum EViewType
        {
            Page,
            Pop,
            Fixed,
            PopupFixed,
        }
        public class ViewInfo
        {
            public string ViewName;
            public EViewType ViewType;
        }
        public List<ViewInfo> ViewInfoList;
        public Dictionary<string, View> ViewDict;
        public ViewContainer()
        {
            this.ViewInfoList = new List<ViewInfo>();
            this.ViewDict = new Dictionary<string, View>();
        }
        public async Task PreloadFixedUIAsync(string rViewName)
        {
            var bIsNeedOpenSound = true;
            var bIsNeedCloseSound = true;

            var rView = FrameManager.Instance.IsUIOpen(rViewName, View.State.Fixing);
            if (rView != null)
            {
                bIsNeedOpenSound = rView.IsNeedOpenSound;
                bIsNeedCloseSound = rView.IsNeedCloseSound;
            }

            // 预加载不播放音效
            rView = await FrameManager.Instance.OpenFixedAsync(rViewName, (rViewInfo) =>
            {
                rViewInfo.IsNeedOpenSound = false;
                rViewInfo.IsNeedCloseSound = false;
            });
            FrameManager.Instance.Close(rView.GUID);

            // 还原音效状态
            if (rView != null)
            {
                rView.IsNeedOpenSound = bIsNeedOpenSound;
                rView.IsNeedCloseSound = bIsNeedCloseSound;
            }
        }
        public async Task PreloadPopUIAsync(string rViewName)
        {
            var bIsNeedOpenSound = true;
            var bIsNeedCloseSound = true;

            var rView = FrameManager.Instance.IsUIOpen(rViewName , View.State.Popup);
            if (rView != null) 
            {
                bIsNeedOpenSound = rView.IsNeedOpenSound;
                bIsNeedCloseSound = rView.IsNeedCloseSound;
            }

            // 预加载不播放音效
            rView = await FrameManager.Instance.OpenPopUIAsync(rViewName, (rViewInfo) =>
            {
                rViewInfo.IsNeedOpenSound = false;
                rViewInfo.IsNeedCloseSound = false;
            });
            FrameManager.Instance.Close(rView.GUID);

            // 还原音效状态
            if (rView != null) 
            {
                rView.IsNeedOpenSound = bIsNeedOpenSound;
                rView.IsNeedCloseSound = bIsNeedCloseSound;
            }
        }
        public async Task<View> OpenPageUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            // Page方式打开的UI不需要缓存
            //if (this.ViewDict.ContainsKey(rViewName)) return null;
            var rView = await FrameManager.Instance.OpenPageUIAsync(rViewName, rOpenCompleted);
            //this.ViewDict.Add(rViewName, rView);
            return rView;
        }
        public async Task<View> OpenPopUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            if (this.ViewDict.ContainsKey(rViewName)) return null;
            this.ViewInfoList.Add(new ViewInfo()
            {
                ViewName = rViewName,
                ViewType = EViewType.Pop,
            });
            var rView = await FrameManager.Instance.OpenPopUIAsync(rViewName, rOpenCompleted);
            this.ViewDict.Add(rViewName, rView);
            return rView;
        }
        public async Task<View> OpenFixedAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            if (this.ViewDict.ContainsKey(rViewName)) return null;
            this.ViewInfoList.Add(new ViewInfo()
            {
                ViewName = rViewName,
                ViewType = EViewType.Fixed,
            });
            var rView = await FrameManager.Instance.OpenFixedAsync(rViewName, rOpenCompleted);
            this.ViewDict.Add(rViewName, rView);
            return rView;
        }
        public async Task<View> OpenPopupFixedUIAsync(string rViewName, Action<View> rOpenCompleted = null)
        {
            if (this.ViewDict.ContainsKey(rViewName)) return null;
            this.ViewInfoList.Add(new ViewInfo()
            {
                ViewName = rViewName,
                ViewType = EViewType.PopupFixed,
            });
            var rView = await FrameManager.Instance.OpenPopupFixedUIAsync(rViewName, rOpenCompleted);
            this.ViewDict.Add(rViewName, rView);
            return rView;
        }
        public void CloseAll()
        {
            this.ViewInfoList.Clear();
            foreach (var rDict in this.ViewDict)
            {
                FrameManager.Instance.Close(rDict.Value.GUID);
            }
            this.ViewDict.Clear();
        }
        public bool TryGetView(string rViewName, out View rView)
        {
            return this.ViewDict.TryGetValue(rViewName, out rView);
        }
        public void HideAll(params string[] rFilters)
        {
            foreach (var rDict in this.ViewDict)
            {
                if (!rFilters.Contains(rDict.Key))
                {
                    rDict.Value.Hide();
                }
            }
        }
        public void ShowAll(params string[] rFilters)
        {
            foreach (var rDict in this.ViewDict)
            {
                if (!rFilters.Contains(rDict.Key))
                {
                    rDict.Value.Show();
                }
            }
        }
        /// <summary>
        /// 重新打开UI
        /// </summary>
        public async Task ReOpenAsync()
        {
            var rReOpens = new string[]
            {
                "UIBattlePadTouchInfo",
            };
            var rRemoveKeyList = new List<string>();
            foreach (var rDict in this.ViewDict)
            {
                if (rReOpens.Contains(rDict.Key))
                {
                    FrameManager.Instance.Close(rDict.Value.GUID);
                    rRemoveKeyList.Add(rDict.Key);
                }
            }
            for (int i = 0; i < rRemoveKeyList.Count; i++)
            {
                string rRemoveKey = rRemoveKeyList[i];
                this.ViewDict.Remove(rRemoveKey);
            }
            for (int i = 0; i < this.ViewInfoList.Count; i++)
            {
                var rViewInfo = this.ViewInfoList[i];
                if (!rReOpens.Contains(rViewInfo.ViewName))
                {
                    continue;
                }
                switch (rViewInfo.ViewType)
                {
                    case EViewType.Pop:
                        await this.OpenPopUIAsync(rViewInfo.ViewName);
                        break;
                    case EViewType.Fixed:
                        await this.OpenFixedAsync(rViewInfo.ViewName);
                        break;
                    case EViewType.PopupFixed:
                        await this.OpenPopupFixedUIAsync(rViewInfo.ViewName);
                        break;
                }
            }
        }
    }
}
