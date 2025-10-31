using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Knight.Core;

namespace UnityEngine.UI
{
    public class ViewControllerContainer : MonoBehaviour
    {
        [InfoBox("ViewModelClass can not be null.", InfoBoxType.Error, "IsViewControllerClassNull")]
        [DropdownPro("ViewControllerClasses")]
        public string ViewControllerClass;

        [ReorderableKeyList]
        [InfoBox("Hei bro!!!!! Some ViewModel has same key.", InfoBoxType.Error, "IsViewModelKeyRepeated")]
        public List<ViewModelDataSource> ViewModels;

        [ReorderableList]
        public List<EventBinding> EventBindings;

        private string[] ViewControllerClasses = new string[0];

        [HideInInspector]
        public MemberBindingAbstract[] MemberBindingAbstracts;

        private bool IsViewModelDataSourceListShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceListShow")]
        public List<ViewModelDataSourceList> ViewModelDataSourceLists;

        private bool IsViewModelDataSourceTabShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceTabShow")]
        public List<ViewModelDataSourceTab> ViewModelDataSourceTabs;

        private bool IsViewModelDataSourceArrayShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceArrayShow")]
        public List<ViewModelDataSourceArray> ViewModelDataSourceArrays;

        private bool IsViewModelDataSourceFancyGridListShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceFancyGridListShow")]
        public List<ViewModelDataSourceFancyGridList> ViewModelDataSourceFancyGridLists;

        private bool IsViewModelDataSourceFancyListShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceFancyListShow")]
        public List<ViewModelDataSourceFancyList> ViewModelDataSourceFancyLists;

        private bool IsViewModelDataSourceFancyRectListShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceFancyRectListShow")]
        public List<ViewModelDataSourceFancyRectList> ViewModelDataSourceFancyRectLists;

        private bool IsViewModelDataSourceMasonryShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceMasonryShow")]
        public List<ViewModelDataSourceMasonry> ViewModelDataSourceMasonrys;

        private bool IsLoopScrollRectsShow;
        [ReorderableList]
        [ShowIf("IsViewModelDataSourceMasonryShow")]
        public List<LoopScrollRect> LoopScrollRects;

        public CanvasGroup CanvasGroup;
        public Canvas Canvas;
        public CommonRuleInfo CommonRuleInfo;
#if UNITY_EDITOR
        private void OnValidate()
        {
            this.GetAllViewModelDataSources();
            this.GetAllNeedCompnents();
        }
#endif

        public void GetAllViewModelDataSources()
        {
            this.ViewModels = new List<ViewModelDataSource>(this.GetComponentsInChildren<ViewModelDataSource>(true));
            this.EventBindings = new List<EventBinding>(this.GetComponentsInChildren<EventBinding>(true));
            this.MemberBindingAbstracts = this.GetComponentsInChildren<MemberBindingAbstract>(true);
            this.ViewControllerClasses = DataBindingTypeResolve.GetAllViews().ToArray();

            this.ViewModelDataSourceLists = new List<ViewModelDataSourceList>(this.GetComponentsInChildren<ViewModelDataSourceList>(true));
            this.IsViewModelDataSourceListShow = this.ViewModelDataSourceLists.Count != 0;

            this.ViewModelDataSourceTabs = new List<ViewModelDataSourceTab>(this.GetComponentsInChildren<ViewModelDataSourceTab>(true));
            this.IsViewModelDataSourceTabShow = this.ViewModelDataSourceTabs.Count != 0;

            this.ViewModelDataSourceArrays = new List<ViewModelDataSourceArray>(this.GetComponentsInChildren<ViewModelDataSourceArray>(true));
            this.IsViewModelDataSourceArrayShow = this.ViewModelDataSourceArrays.Count != 0;

            this.ViewModelDataSourceFancyGridLists = new List<ViewModelDataSourceFancyGridList>(this.GetComponentsInChildren<ViewModelDataSourceFancyGridList>(true));
            this.IsViewModelDataSourceFancyGridListShow = this.ViewModelDataSourceFancyGridLists.Count != 0;

            this.ViewModelDataSourceFancyLists = new List<ViewModelDataSourceFancyList>(this.GetComponentsInChildren<ViewModelDataSourceFancyList>(true));
            this.IsViewModelDataSourceFancyListShow = this.ViewModelDataSourceFancyLists.Count != 0;

            this.ViewModelDataSourceFancyRectLists = new List<ViewModelDataSourceFancyRectList>(this.GetComponentsInChildren<ViewModelDataSourceFancyRectList>(true));
            this.IsViewModelDataSourceFancyRectListShow = this.ViewModelDataSourceFancyRectLists.Count != 0;

            this.ViewModelDataSourceMasonrys = new List<ViewModelDataSourceMasonry>(this.GetComponentsInChildren<ViewModelDataSourceMasonry>(true));
            this.IsViewModelDataSourceArrayShow = this.ViewModelDataSourceMasonrys.Count != 0;
        }

        public void GetAllNeedCompnents()
        {
            this.LoopScrollRects = new List<LoopScrollRect>(this.GetComponentsInChildren<LoopScrollRect>(true));
            this.IsLoopScrollRectsShow = this.LoopScrollRects.Count != 0;

            this.CanvasGroup = this.gameObject.ReceiveComponent<CanvasGroup>();
            this.Canvas = this.gameObject.ReceiveComponent<Canvas>();
            this.Canvas.overrideSorting = true;
            this.gameObject.ReceiveComponent<GraphicRaycaster>();
            this.CommonRuleInfo = this.gameObject.GetComponentInChildren<CommonRuleInfo>(true);
        }

        public void Preload()
        {

            for (int i = 0; i < this.ViewModelDataSourceArrays.Count; i++)
            {
                var rViewModelDataSourceArray = this.ViewModelDataSourceArrays[i];

                if (rViewModelDataSourceArray.ArrayPool != null && !rViewModelDataSourceArray.HasInitData)
                {
                    rViewModelDataSourceArray.ArrayPool.Preload();
                }
            }

            for (int i = 0; i < this.ViewModelDataSourceLists.Count; i++)
            {
                var rViewModelDataSourceArray = this.ViewModelDataSourceLists[i];
                if (rViewModelDataSourceArray.ListView == null) continue;

                if (rViewModelDataSourceArray.ListView.prefabSource == null) continue;
                rViewModelDataSourceArray.ListView.prefabSource.Preload();
            }
        }

        private bool IsViewControllerClassNull()
        {
            return string.IsNullOrEmpty(this.ViewControllerClass);
        }

        private bool IsViewModelKeyRepeated()
        {
            if (this.ViewModels == null) return false;

            var rKeyList = new List<string>();
            for (int i = 0; i < this.ViewModels.Count; i++)
            {
                if (rKeyList.Contains(this.ViewModels[i].Key))
                {
                    return true;
                }
                else
                {
                    rKeyList.Add(this.ViewModels[i].Key);
                }
            }

            return false;
        }

        public void SetViewModelDataSource_Enable(bool bIsEnable)
        {
            for (int i = 0; i < this.ViewModelDataSourceLists.Count; i++)
            {
                this.ViewModelDataSourceLists[i].enabled = bIsEnable;
            }
        }
        public void SetViewCompnent_Enable(bool bIsEnable)
        {
            for (int i = 0; i < this.LoopScrollRects.Count; i++)
            {
                this.LoopScrollRects[i].enabled = bIsEnable;
            }
        }
    }
}
