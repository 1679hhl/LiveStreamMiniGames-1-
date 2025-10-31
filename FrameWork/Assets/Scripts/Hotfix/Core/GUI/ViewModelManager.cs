using System;
using System.Collections.Generic;
using Knight.Core;
using Knight.Hotfix.Core;

namespace UnityEngine.UI
{
    public class ViewModelManager : THotfixSingleton<ViewModelManager>
    {
        private Dictionary<string, ViewModel> mViewModels;                  // 全局的ViewModel
        private Dictionary<Type, ViewModelPropMap> mViewModelPropMaps;      // ViewModel的属性映射

        private ViewModelManager()
        {
        }

        public void Initialize()
        {
            this.mViewModels = new Dict<string, ViewModel>();
            this.mViewModelPropMaps = new Dictionary<Type, ViewModelPropMap>();

            var rViewModelTypes = ViewModelTypes.Types;
            for (int i = 0; i < rViewModelTypes.Count; i++)
            {
                var rViewModelType = rViewModelTypes[i];
                var rAttrObjs = rViewModelType.GetCustomAttributes(typeof(ViewModelInitializeAttribute), false);
                if (rAttrObjs == null || rAttrObjs.Length == 0) continue;

                this.ReceiveViewModel(rViewModelType);
            }
        }

        public void AddViewModelPropMap(Type rViewModelType)
        {
            if (!this.mViewModelPropMaps.TryGetValue(rViewModelType, out var rViewModelPropMap))
            {
                rViewModelPropMap = new ViewModelPropMap();
                rViewModelPropMap.Initialize(rViewModelType);
                this.mViewModelPropMaps.Add(rViewModelType, rViewModelPropMap);
            }
        }

        public ViewModelPropMap GetViewModelPropMap(Type rViewModelType)
        {
            this.mViewModelPropMaps.TryGetValue(rViewModelType, out var rViewModelPropMap);
            return rViewModelPropMap;
        }

        public ViewModel ReceiveViewModel(Type rViewModelType)
        {
            var rViewModelClassName = rViewModelType.FullName;
            return this.ReceiveViewModel(rViewModelClassName);
        }

        public ViewModel ReceiveViewModel(string rViewModelClassName)
        {
            ViewModel rViewModel = null;
            if (this.mViewModels == null)
            {
                this.mViewModels = new Dict<string, ViewModel>();
            }
            if (!this.mViewModels.TryGetValue(rViewModelClassName, out rViewModel))
            {
                var rViewModelType = TypeResolveManager.Instance.GetType(rViewModelClassName);
                if (rViewModelType == null) return null;
                rViewModel = HotfixReflectAssists.Construct(rViewModelType) as ViewModel;
                rViewModel.Initialize(true);
                rViewModel.BindToProto();
                this.AddViewModelPropMap(rViewModelType);

                this.mViewModels.Add(rViewModelClassName, rViewModel);
            }
            return rViewModel;
        }

        public T GetViewModel<T>() where T : ViewModel
        {
            var rViewModelType = typeof(T);
            return this.ReceiveViewModel(rViewModelType) as T;
        }
    }
}
