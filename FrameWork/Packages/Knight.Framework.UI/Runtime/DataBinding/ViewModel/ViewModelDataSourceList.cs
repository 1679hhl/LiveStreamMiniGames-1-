using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Knight.Core;
using NaughtyAttributes;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public partial class ViewModelDataSourceList : ViewModelDataSourceTemplate
    {
        public UnityEvent OnListChangedEvent = new UnityEvent();
        [DrawOrder(2)]
        public LoopScrollRect ListView;

        private bool mIsListViewFilled = false;

        protected override void Awake()
        {
            this.ListView?.prefabSource?.prefabObj?.SetActive(false);
        }
        public void OnListChanged(object rValue)
        {
            var rViewModelObj = rValue;//this.ViewModelProp.GetValue();
            if (null == rViewModelObj)
                return;

            var rListObservableObj = rViewModelObj as IObservableEvent;

            var rListObj2 = (IList)rValue;//this.ViewModelProp.GetValue();
            var nListCount2 = rListObj2 != null ? rListObj2.Count : 0;

            var nOldCount = this.ListView.totalCount;
            this.ListView.totalCount = nListCount2;

            if (nListCount2 == nOldCount && rListObservableObj.RefreshType != ObservableRefreshType.RefreshStartIndex && rListObservableObj.RefreshType != ObservableRefreshType.RefreshToEnd)
            {
                if (!this.mIsListViewFilled)
                {
                    this.mIsListViewFilled = true;
                    this.ListView.RefillCells(0);
                }
                else
                {
                    this.ListView.RefreshCells();
                }
            }
            else
            {
                if (rListObservableObj.RefreshType == ObservableRefreshType.RefreshToEnd)
                    this.ListView.RefillCellsFromEnd(0);
                else if (rListObservableObj.RefreshType == ObservableRefreshType.RefreshToBegin)
                    this.ListView.RefillCells(0);
                else if (rListObservableObj.RefreshType == ObservableRefreshType.RefreshStartIndex)
                    this.ListView.RefillCells(rListObservableObj.StartIndex);
                else if (rListObservableObj.RefreshType == ObservableRefreshType.Refresh)
                    this.ListView.RefreshCells();
            }
            this.OnListChangedEvent?.Invoke();
        }

        public override void GetPaths()
        {
            base.GetPaths();

            this.ListView = this.GetComponent<LoopScrollRect>();
            if (this.ListView != null)
            {
                var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                                       DataBindingTypeResolve.GetListViewModelProperties(this.gameObject, this.IsListTemplate));
                this.ModelPaths = DataBindingTypeResolve.GetAllViewModelPaths(rViewModelProps).ToArray();

                if (!string.IsNullOrEmpty(this.ViewModelPath) && !this.ModelPaths.AnyOne(item => item == this.ViewModelPath))
                {
                    this.ViewModelPath = string.Empty;
                }
                this.GetViewModelParams(this.ViewModelPath);
            }
        }
    }
}
