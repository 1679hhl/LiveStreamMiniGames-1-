using Knight.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public partial class ViewModelDataSourceMasonry : ViewModelDataSourceTemplate
    {
        public UnityEvent OnListChangedEvent = new UnityEvent();
        [DrawOrder(2)]
        public MasonryLoopScrollRect ScrollRect;

        protected override void Awake()
        {
            this.ScrollRect?.PrefabSource?.prefabObj?.SetActive(false);
        }
        public void OnListChanged(object rValue)
        {
            var rViewModel = rValue;//this.ViewModelProp.GetValue();
            if (null == rViewModel)
                return;
            Debug.Assert(this.ScrollRect, "MasonryLoopScrollRect can not null.");
            var rList = (IList)rValue;//this.ViewModelProp.GetValue();
            if (rList != null)
            {
                var rIList = (IList)rList;
                if (rIList != null)
                {
                    this.ScrollRect.RefreshItemList(rIList);
                }
            }
            var rObservableEvent = rViewModel as IObservableEvent;

            switch (rObservableEvent.RefreshType)
            {
                case ObservableRefreshType.RefreshToBegin:
                    this.ScrollRect.RefreshToIndex(0);
                    break;
                case ObservableRefreshType.Refresh:
                    this.ScrollRect.Refresh();
                    break;
                case ObservableRefreshType.RefreshToEnd:
                    this.ScrollRect.RefreshToIndex(this.ScrollRect.ItemHeightOrWidthList.Count - 1);
                    break;
                case ObservableRefreshType.RefreshStartIndex:
                    this.ScrollRect.RefreshToIndex(rObservableEvent.StartIndex);
                    break;
            }
            this.OnListChangedEvent?.Invoke();
        }

        public override void GetPaths()
        {
            base.GetPaths();

            this.ScrollRect = this.GetComponent<MasonryLoopScrollRect>();
            if (this.ScrollRect != null)
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