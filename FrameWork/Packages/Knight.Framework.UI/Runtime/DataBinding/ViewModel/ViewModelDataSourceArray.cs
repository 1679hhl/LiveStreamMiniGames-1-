using Knight.Core;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public class ViewModelDataSourceArray : ViewModelDataSourceTemplate
    {
        [DrawOrder(2)]
        public GameObject ItemTemplateGo;
        [DrawOrder(2)]
        public UIArrayPool ArrayPool;

        public bool HasInitData;
        public bool IsChangeItemName = true;
        public List<RectTransform> ItemTransList = new List<RectTransform>();

        public UnityEvent OnFillArray = new UnityEvent();

        public Action<Transform, int> OnFillCellFunc_Other;

        public bool IsCustomSort;
        private int mLastSortChildCount;
        private List<ViewModelDataSourceArray_SortIndex> mSortIndexList = new List<ViewModelDataSourceArray_SortIndex>();
        protected override void Awake()
        {
            this.ItemTemplateGo?.SetActive(false);

            if (this.ArrayPool)
            {
                this.ArrayPool.RootPoolObj?.SetActive(false);
            }
        }

        public override void GetPaths()
        {
            base.GetPaths();

            var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                               DataBindingTypeResolve.GetListViewModelProperties(this.gameObject, this.IsListTemplate));
            this.ModelPaths = DataBindingTypeResolve.GetAllViewModelPaths(rViewModelProps).ToArray();

            if (!string.IsNullOrEmpty(this.ViewModelPath) && !this.ModelPaths.AnyOne(item => item == this.ViewModelPath))
            {
                this.ViewModelPath =string.Empty;
            }
            this.GetViewModelParams(this.ViewModelPath);
        }

        public void LateUpdate()
        {
            if (this.IsCustomSort)
            {
                this.RefreshSortIndex();
                this.CustomSort();
            }
        }
        public void CustomSort()
        {
            var bIsDirty = false;
            foreach (var rSortIndex in this.mSortIndexList)
            {
                if (rSortIndex.IsDirty)
                {
                    bIsDirty = true;
                    rSortIndex.IsDirty = false;
                }
            }
            if (!bIsDirty)
            {
                return;
            }
            this.mSortIndexList.Sort(this.SortIndexFunc);
            foreach (var rSortIndex in this.mSortIndexList)
            {
                rSortIndex.transform.SetAsLastSibling();
            }
        }
        private int SortIndexFunc(ViewModelDataSourceArray_SortIndex rLeft, ViewModelDataSourceArray_SortIndex rRight)
        {
            return rLeft.SortIndex.CompareTo(rRight.SortIndex);
        }
        public void RefreshSortIndex()
        {
            var nChildCount = this.transform.childCount;
            if (this.mLastSortChildCount != nChildCount)
            {
                this.mLastSortChildCount = nChildCount;
                this.mSortIndexList.Clear();
                this.transform.GetComponentsInChildren(this.mSortIndexList);
            }
        }
    }
}
