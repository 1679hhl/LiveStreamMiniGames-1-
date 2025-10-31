using Knight.Core;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class ViewModelDataSourceFancyRectList : ViewModelDataSourceTemplate
    {
        public UnityEvent OnListChangedEvent = new UnityEvent();

        [DrawOrder(2)]
        public FancyBindingScrollRect ListView;

        public void OnListChanged(object rValue)
        {
            var rViewModelObj = rValue;//this.ViewModelProp.GetValue();
            if (null == rViewModelObj)
                return;

            var rListObservableObj = rViewModelObj as IObservableEvent;

            var rListObj2 = (IList)rValue;//this.ViewModelProp.GetValue();
            var nListCount2 = rListObj2 != null ? rListObj2.Count : 0;

            this.ListView.RefreshCells(nListCount2);

            if(rListObservableObj.RefreshType == ObservableRefreshType.RefreshStartIndex)
            {
                this.ListView.Select(rListObservableObj.StartIndex);
            }
            else if(rListObservableObj.RefreshType == ObservableRefreshType.RefreshToEnd)
            {
                this.ListView.SelectEnd();
            }

            this.OnListChangedEvent?.Invoke();
        }

        public override void GetPaths()
        {
            base.GetPaths();

            this.ListView = this.GetComponent<FancyBindingScrollRect>();
            if (this.ListView != null)
            {
                var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                    DataBindingTypeResolve.GetListViewModelProperties(this.gameObject,this.IsListTemplate));
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

