using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Game;
using Knight.Core;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;

namespace UnityEngine.UI
{
    public interface ICanChangeUILayer
    {
    }

    [TypeResolveCache]
    public class ViewController : HotfixKnightObject
    {
        public View View;
        protected Dict<string, ViewModel> ViewModels;//界面绑定数据所需要的所有ViewModel
        private HotfixUpdateManager.UpdateInstance mUpdateInstance = new HotfixUpdateManager.UpdateInstance();

        public ViewController()
        {
            this.ViewModels = new Dict<string, ViewModel>();
        }

        private List<ViewModelDataSourceTemplate> mSubViewModelDataSourceTemplates = new List<ViewModelDataSourceTemplate>();

        public string GUID
        {
            get
            {
                if (this.View == null) return "";
                return this.View.GUID;
            }
        }

        public void DataBindingConnect(ViewControllerContainer rViewControllerContainer)
        {
            if (rViewControllerContainer == null) return;

            // 把Event绑定到ViewController里面
            this.BindingEvents(rViewControllerContainer);

            // ViewModel和View之间的数据绑定
            this.BindingViewAndViewModels(rViewControllerContainer);

            // ListViewModel和View之间的数据绑定
            this.BindingListViewAndViewModels(rViewControllerContainer);

            // FancyViewModel和View之间的数据绑定
            this.BindingFancyListViewAndViewModels(rViewControllerContainer);

            //FancyRectViewModel和View之间的数据绑定
            this.BindingFancyRectListViewAndViewModels(rViewControllerContainer);

            //FancyGridViewModel和View之间的数据绑定
            this.BindingFancyGridListViewAndViewModels(rViewControllerContainer);

            // TabViewModel和View之间的数据绑定
            this.BindingTabViewAndViewModels(rViewControllerContainer);

            // ArrayViewModel和View之间的数据绑定
            this.BindingArrayViewAndViewModels(rViewControllerContainer);

            // MasonryViewModel和View之间的数据绑定
            this.BindingMasonryViewAndViewModels(rViewControllerContainer);
        }

        public void DataBindingDisconnect(ViewControllerContainer rViewControllerContainer)
        {
            if (!rViewControllerContainer) return;
            //解绑全局MemberBindingAbstract
            var rAllMemberBindings = rViewControllerContainer.MemberBindingAbstracts;
            for (int i = 0; i < rAllMemberBindings.Length; i++)
            {
                var rMemberBinding = rAllMemberBindings[i];
                if (rMemberBinding.ViewModelProp == null) continue;

                ViewModel rViewModel = rMemberBinding.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    if (rMemberBinding)
                        rViewModel.UnBindPropChanged(rMemberBinding);
                    //rViewModel.PropChanged -= rMemberBinding.ViewModelPropertyWatcher.PropertyChanged;
                }
                if (rMemberBinding)
                    rMemberBinding.OnDestroy();
            }

            //解绑全局EventBindings
            var rAllEventBindings = rViewControllerContainer.EventBindings;
            for (int i = 0; i < rAllEventBindings.Count; i++)
            {
                rAllEventBindings[i].OnDestroy();
            }

            //解绑动态生成的解绑全局MemberBindingAbstract
            var rMemberBindingAbstractRetrives = rViewControllerContainer.gameObject.GetComponentsInChildren<DataBindingRetrive>(true);
            for (int i = 0; i < rMemberBindingAbstractRetrives.Length; i++)
            {
                var rrAllMemberBindings = rMemberBindingAbstractRetrives[i];
                for (int j = 0; j < rrAllMemberBindings.MemberBindings.Count; j++)
                {
                    var rMemberBinding = rrAllMemberBindings.MemberBindings[j];
                    if (rMemberBinding == null || rMemberBinding.ViewModelProp == null) continue;

                    ViewModel rViewModel = rMemberBinding.ViewModelProp.PropertyOwner as ViewModel;
                    if (rViewModel != null)
                    {
                        if (rMemberBinding)
                            rViewModel.UnBindPropChanged(rMemberBinding);
                        //rViewModel.PropChangedHandler -= rMemberBinding.ViewModelPropertyWatcher.PropertyChanged;
                    }
                    if (rMemberBinding)
                        rMemberBinding.OnDestroy();
                }
                for (int j = 0; j < rrAllMemberBindings.EventBindings.Count; j++)
                {
                    rrAllMemberBindings.EventBindings[j]?.OnDestroy();
                }
            }

            #region 解绑List
            var rListViewModelDataSources = rViewControllerContainer.ViewModelDataSourceLists;
            for (int i = 0; i < rListViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rListViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                }
                var rListObservableObj = rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName) as IObservableEvent;
                if (rListObservableObj != null)
                {
                    rListObservableObj.ChangedHandler -= rViewModelDataSource.OnListChanged;
                }
            }

            var rTabViewModelDataSources = rViewControllerContainer.ViewModelDataSourceTabs;
            for (int i = 0; i < rTabViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rTabViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                }
            }

            var rArrayViewModelDataSources = rViewControllerContainer.ViewModelDataSourceArrays;
            for (int i = 0; i < rArrayViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rArrayViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    // rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                }
            }

            var rFancyGridViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyGridLists;
            for (int i = 0; i < rFancyGridViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rFancyGridViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                    var rListObservableObj = rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName) as IObservableEvent;
                    if (rListObservableObj != null)
                    {
                        rListObservableObj.ChangedHandler -= rViewModelDataSource.OnListChanged;
                    }
                }

            }

            var rFancyRectViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyRectLists;
            for (int i = 0; i < rFancyRectViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rFancyRectViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                    var rListObservableObj = rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName) as IObservableEvent;
                    if (rListObservableObj != null)
                    {
                        rListObservableObj.ChangedHandler -= rViewModelDataSource.OnListChanged;
                    }
                }

            }

            var rFancyViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyLists;
            for (int i = 0; i < rFancyViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rFancyViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate)
                {

                }
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                    var rListObservableObj = rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName) as IObservableEvent;
                    if (rListObservableObj != null)
                    {
                        rListObservableObj.ChangedHandler -= rViewModelDataSource.OnListChanged;
                    }
                }

            }

            var rViewModelDataSourceMasonrys = rViewControllerContainer.ViewModelDataSourceMasonrys;
            for (int i = 0; i < rViewModelDataSourceMasonrys.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSourceMasonrys[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                    var rListObservableObj = rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName) as IObservableEvent;
                    if (rListObservableObj != null)
                    {
                        rListObservableObj.ChangedHandler -= rViewModelDataSource.OnListChanged;
                    }
                }

            }
            #endregion

            //解绑二级List
            for (int i = 0; i < this.mSubViewModelDataSourceTemplates.Count; i++)
            {
                var rViewModelDataSource = this.mSubViewModelDataSourceTemplates[i];
                ViewModel rViewModel = rViewModelDataSource.ViewModelProp.PropertyOwner as ViewModel;
                if (rViewModel != null)
                {
                    rViewModel.UnBindPropChanged(rViewModelDataSource);
                    //rViewModel.PropChangedHandler -= rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;
                }
            }
            this.mSubViewModelDataSourceTemplates.Clear();

        }

        /// <summary>
        /// 把ViewModel绑定到ViewController里面
        /// </summary>
        public void BindingViewModels(ViewControllerContainer rViewControllerContainer)
        {
            for (int i = 0; i < rViewControllerContainer.ViewModels.Count; i++)
            {
                var rViewModelDataSource = rViewControllerContainer.ViewModels[i];
                ViewModel rViewModel = this.CreateViewModel(rViewModelDataSource.Key, rViewModelDataSource.ViewModelPath, rViewModelDataSource.IsGlobal);
                if (rViewModel != null)
                {
                    ViewModelManager.Instance.AddViewModelPropMap(rViewModel.GetType());
                    if (!rViewModel.IsGlobal)
                    {
                        rViewModel.BindToProto();
                    }
                    this.ViewModels.Add(rViewModelDataSource.Key, rViewModel);
                    var rViewModelFieldInfo = this.GetViewModelKeyFieldInfo(rViewModelDataSource.Key, rViewModel);
                    if (rViewModelFieldInfo != null)
                    {
                        rViewModelFieldInfo.SetValue(this, rViewModel);
                    }
                    else
                    {
                        Knight.Core.LogManager.LogErrorFormat("Not define ViewModel variable: {0}, {1}.", rViewModelDataSource.Key, rViewModelDataSource.ViewModelPath);
                    }
                }
                else
                {
                    Knight.Core.LogManager.LogErrorFormat("Can not find ViewModel {0}.", rViewModelDataSource.ViewModelPath);
                }
            }
        }


        private FieldInfo GetViewModelKeyFieldInfo(string rKey, ViewModel rViewModel)
        {
            var rAllFields = this.GetType().GetFields(HotfixReflectAssists.flags_all);
            for (int i = 0; i < rAllFields.Length; i++)
            {
                var rAttributes = rAllFields[i].GetCustomAttributes(typeof(ViewModelKeyAttribute), true);
                if (rAttributes.Length == 0) continue;
                var rViewModelKeyAttr = rAttributes[0] as ViewModelKeyAttribute;
                if (rViewModelKeyAttr == null) continue;

                if (rViewModelKeyAttr.Key.Equals(rKey))
                {
                    return rAllFields[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 把Event绑定到ViewController里面
        /// </summary>
        private void BindingEvents(ViewControllerContainer rViewControllerContainer)
        {
            for (int i = 0; i < rViewControllerContainer.EventBindings.Count; i++)
            {
                var rEventBinding = rViewControllerContainer.EventBindings[i];
                if (!rEventBinding || rEventBinding.IsListTemplate) continue;

                var bResult = HotfixDataBindingTypeResolve.MakeViewModelDataBindingEvent(this, rEventBinding, this.OnCheck);
                if (!bResult)
                {
                    Knight.Core.LogManager.LogErrorFormat("Make view model binding event {0} failed..", rEventBinding.ViewModelMethod);
                }
            }
        }

        /// <summary>
        /// 检查EventBinding前置条件是否满足
        /// </summary>
        /// <param name="rEventBinding"></param>
        /// <returns></returns>
        private bool OnCheck(EventBinding rEventBinding)
        {
            return true;
        }



        /// <summary>
        /// 处理入口
        /// </summary>
        private void JudgeEntryCondition()
        {
            
        }

        
        private bool IfCloseUIEntry(int nConfigId)
        {
            // if (!GameConfig.Instance.UISystem.Table.TryGetValue(nConfigId, out var rConfig))
            // {
            //     LogManager.LogError($"UISystem表中不存在{nConfigId}的数据");
            //     return false;
            // }
            //
            // bool IsPass = Model.Instance.UISystem.CheckSystemOpen(nConfigId, false, false);
            // bool IsShow = rConfig.IsShow == 1;
            // if (Model.Instance.UISystem.RestrictSystemDict.TryGetValue(rConfig.ID, out var rRestrictSystem))
            // {
            //     IsShow = rRestrictSystem.IsShow;
            // }
            //
            // return !IsPass && !IsShow;
            return true;
        }

        private void PlayerUpgrade_OnHandler(EventArg rEventArg)
        {
            // if (this.mEntryConditionList == null)
            //     return;
            //
            // // IList<EntryCondition> rCondtions = new List<EntryCondition>();
            // foreach (var item in this.mEntryConditionList)
            // {
            //     if (Model.Instance.UISystem.CheckSystemOpen(item.ConfigID,false,false))
            //     {
            //         foreach (var gameObj in item.HideRelateGameObjects)
            //         {
            //             gameObj.SetActiveSafe(true);
            //         }
            //         // rCondtions.Add(item);
            //     }
            //     else
            //     {
            //         if (this.IfCloseUIEntry(item.ConfigID))
            //         {
            //             foreach (var gameObj in item.HideRelateGameObjects)
            //             {
            //                 gameObj.SetActiveSafe(false);
            //             }
            //         }
            //     }
            // }
            // foreach (var item in rCondtions)
            // {
            //     this.mEntryConditionList.Remove(item);
            // }
        }


        /// <summary>
        /// ViewModel和View之间的数据绑定
        /// </summary>
        private void BindingViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rMemberBindingAbstracts = rViewControllerContainer.MemberBindingAbstracts;

            for (int i = 0; i < rMemberBindingAbstracts.Length; i++)
            {
                var rMemberBinding = rMemberBindingAbstracts[i];
                if (rMemberBinding.IsListTemplate) continue;    // 过滤掉ListTemplate标记得Binding Script

                rMemberBinding.ViewProp = DataBindingTypeResolve.MakeViewDataBindingProperty(rMemberBinding);
                if (rMemberBinding.ViewProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("ViewName: {2} GameObjectPath:{3} View Path: {0},{1} error..", rMemberBinding, rMemberBinding.ViewPath, this.View.ViewName, UtilTool.GetTransformPath(rMemberBinding.transform));
                    return;
                }

                rMemberBinding.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rMemberBinding);
                if (rMemberBinding.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("ViewName: {2} GameObjectPath:{3} View Model Path: {0},{1} error..", rMemberBinding, rMemberBinding.ViewModelPath, this.View.ViewName, UtilTool.GetTransformPath(rMemberBinding.transform));
                    return;
                }
                ViewModel rViewModel = this.GetViewModel(rMemberBinding.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0},{1} error..", rMemberBinding, rMemberBinding.ViewModelPath);
                    return;
                }

                // 设置类型转换器
                if (rMemberBinding.IsDataConvert)
                {
                    rMemberBinding.ViewModelProp.ConvertMethod = HotfixDataBindingTypeResolve.MakeViewModelDataBindingConvertMethod(rMemberBinding);
                    if (rMemberBinding.IsDataConvert && rMemberBinding.ViewModelProp.ConvertMethod == null)
                    {
                        Knight.Core.LogManager.LogError("Cannot find convert method: {0}.." + rMemberBinding.DataConverterMethodPath);
                        return;
                    }
                }
                else
                {
                    rMemberBinding.ViewModelProp.ConvertMethod = null;
                }

                rMemberBinding.ViewModelProp.PropertyOwner = rViewModel;
                //rMemberBinding.SyncFromViewModel(null);

                // ViewModel绑定View
                rMemberBinding.BindingWatcher(rViewModel, rMemberBinding.ViewModelProp.PropertyName);
                //rMemberBinding.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rMemberBinding.ViewModelProp.PropertyName, rMemberBinding.SyncFromViewModel);
                rViewModel.BindPropChanged(rMemberBinding);
                //rViewModel.PropChangedHandler += rMemberBinding.ViewModelPropertyWatcher.PropertyChanged;
                // View绑定ViewModel
                rViewModel.PropChanged_Manual(rMemberBinding);
                var rMemberBindingTwoWay = rMemberBinding as MemberBindingTwoWay;
                if (rMemberBindingTwoWay != null)
                {
                    rMemberBindingTwoWay.InitEventWatcher();
                }
            }
        }

        /// <summary>
        /// ListViewModel和View之间的数据绑定
        /// </summary>
        private void BindingListViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSourceList = rViewControllerContainer.ViewModelDataSourceLists;
            for (int i = 0; i < rViewModelDataSourceList.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSourceList[i];

                //过滤掉ListTemplate标记的ViewModelDataSourceList
                if (rViewModelDataSource.IsListTemplate) continue;
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }
                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定Watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    // 重新设置List数据时候，改变个数
                    this.BindingList(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    // 重新设置List数据时候，改变个数
                //    this.BindingList(rViewModelDataSource, rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                // 初始化list
                this.BindingList(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }

        private void BindingFancyListViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyLists;
            for (int i = 0; i < rViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSources[i];

                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }
                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定Watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    // 重新设置List数据时候，改变个数
                    this.BindingFancyList(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    // 重新设置List数据时候，改变个数
                //    this.BindingFancyList(rViewModelDataSource,rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                // 初始化list
                this.BindingFancyList(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }


        private void BindingFancyRectListViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyRectLists;
            for (int i = 0; i < rViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSources[i];
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }
                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定Watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    // 重新设置List数据时候，改变个数
                    this.BindingFancyRectList(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    // 重新设置List数据时候，改变个数
                //    this.BindingFancyRectList(rViewModelDataSource,rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                // 初始化list
                this.BindingFancyRectList(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }
        private void BindingFancyGridListViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSources = rViewControllerContainer.ViewModelDataSourceFancyGridLists;
            for (int i = 0; i < rViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSources[i];
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }
                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定Watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    // 重新设置List数据时候，改变个数
                    this.BindingFancyGridList(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    // 重新设置List数据时候，改变个数
                //    this.BindingFancyGridList(rViewModelDataSource,rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                // 初始化list
                this.BindingFancyGridList(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }

        private void BindingMasonryViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSourceList = rViewControllerContainer.ViewModelDataSourceMasonrys;
            for (int i = 0; i < rViewModelDataSourceList.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSourceList[i];

                //过滤掉ListTemplate标记的ViewModelDataSourceList
                if (rViewModelDataSource.IsListTemplate) continue;
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }
                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    // 重新设置List数据时候，改变个数
                    this.BindingMasonry(rViewModelDataSource, rValue);
                });
                //// 绑定Watcher
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, () =>
                //{
                //    // 重新设置List数据时候，改变个数
                //    this.BindingMasonry(rViewModelDataSource);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                // 初始化list
                this.BindingMasonry(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }

        public void BindingList(ViewModelDataSourceList rViewModelDataSource, object rValue)
        {
            var rViewModelObj = rValue;
            if (rViewModelObj != null)
            {
                var rListObservableObj = rViewModelObj as IObservableEvent;
                rListObservableObj.ChangedHandler += rViewModelDataSource.OnListChanged;

                var rListObj = rViewModelObj as IList;
                var nListCount = rListObj != null ? rListObj.Count : 0;

                rViewModelDataSource.ListView.OnFillCellFunc = (rTrans, nIndex) =>
                {
                    if (rListObservableObj.RefreshType != ObservableRefreshType.RefreshStartIndex)
                    {
                        rListObservableObj.StartIndex = rViewModelDataSource.ListView.ItemTypeStart;
                    }
                    rListObservableObj.EndIndex = rViewModelDataSource.ListView.ItemTypeEnd;

                    this.OnListViewFillCellFunc(rTrans, nIndex, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);

                };
                rViewModelDataSource.OnListChanged(rViewModelObj);
                rViewModelDataSource.ListView.totalCount = nListCount;
            }
            else
            {
                LogManager.Log($"ViewModel {rViewModelDataSource.name}, {rViewModelDataSource.ViewModelPath} getValue is null..");
            }
        }


        private void BindingFancyList(ViewModelDataSourceFancyList rViewModelDataSource, object rValue)
        {
            var rViewModelObj = rValue;
            if (rViewModelObj != null)
            {
                var rListObservableObj = rViewModelObj as IObservableEvent;
                rListObservableObj.ChangedHandler += rViewModelDataSource.OnListChanged;

                var rListObj = rViewModelObj as IList;

                rViewModelDataSource.ListView.OnUpdateContentAction = (rICell) =>
                {
                    var rTrans = rICell.GetTransform();
                    var nIndex = rICell.GetIndex();
                    this.OnListViewFillCellFunc(rTrans, nIndex, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);

                };

                rViewModelDataSource.OnListChanged(rViewModelObj);
            }
            else
            {
                LogManager.Log($"ViewModel {rViewModelDataSource.name}, {rViewModelDataSource.ViewModelPath} getValue is null..");
            }
        }
        private void BindingFancyRectList(ViewModelDataSourceFancyRectList rViewModelDataSource, object rValue)
        {
            var rViewModelObj = rValue;
            if (rViewModelObj != null)
            {
                var rListObservableObj = rViewModelObj as IObservableEvent;
                rListObservableObj.ChangedHandler += rViewModelDataSource.OnListChanged;

                var rListObj = rViewModelObj as IList;

                rViewModelDataSource.ListView.OnUpdateContentAction = (rICell) =>
                {
                    var rTrans = rICell.GetTransform();
                    var nIndex = rICell.GetIndex();
                    this.OnListViewFillCellFunc(rTrans, nIndex, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                };

                rViewModelDataSource.OnListChanged(rViewModelObj);
            }
            else
            {
                LogManager.Log($"ViewModel {rViewModelDataSource.name}, {rViewModelDataSource.ViewModelPath} getValue is null..");
            }
        }
        private void BindingFancyGridList(ViewModelDataSourceFancyGridList rViewModelDataSource, object rValue)
        {
            var rViewModelObj = rValue;
            if (rViewModelObj != null)
            {
                var rListObservableObj = rViewModelObj as IObservableEvent;
                rListObservableObj.ChangedHandler += rViewModelDataSource.OnListChanged;

                var rListObj = rViewModelObj as IList;

                rViewModelDataSource.ListView.OnUpdateContentAction = (rICell) =>
                {
                    var rTrans = rICell.GetTransform();
                    var nIndex = rICell.GetIndex();
                    this.OnListViewFillCellFunc(rTrans, nIndex, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                };

                rViewModelDataSource.OnListChanged(rViewModelObj);
            }
            else
            {
                LogManager.Log($"ViewModel {rViewModelDataSource.name}, {rViewModelDataSource.ViewModelPath} getValue is null..");
            }
        }
        private void BindingMasonry(ViewModelDataSourceMasonry rViewModelDataSource, object rValue)
        {
            var rViewModelObj = rValue;
            if (rViewModelObj != null)
            {
                var rListObservableObj = rViewModelObj as IObservableEvent;
                rListObservableObj.ChangedHandler += rViewModelDataSource.OnListChanged;

                var rListObj = rViewModelObj as IList;

                rViewModelDataSource.ScrollRect.GetItemHeightOrWidthListFunc = (rIList) =>
                {
                    var fItemHeightList = new List<float>();
                    if (rIList != null)
                    {
                        var nCount = rIList.Count;
                        for (int i = 0; i < nCount; i++)
                        {
                            var rItem = rIList[i];
                            if (rItem is IMasonryLoopScrollItem rMasonryItem)
                            {
                                fItemHeightList.Add(rMasonryItem.Height);
                            }
                            else
                            {
                                fItemHeightList.Add(0f);
                                LogManager.LogError("瀑布流数据类型错误");
                            }
                        }

                    }
                    return fItemHeightList;
                };

                rViewModelDataSource.ScrollRect.OnFillCellFunc = (rTrans, nIndex) =>
                {
                    this.OnListViewFillCellFunc(rTrans, nIndex, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                };

                rViewModelDataSource.OnListChanged(rViewModelObj);
            }
            else
            {
                LogManager.Log($"ViewModel {rViewModelDataSource.name}, {rViewModelDataSource.ViewModelPath} getValue is null..");
            }
        }
        private void OnListViewFillCellFunc(Transform rTrans, int nIndex, IList rListObj, bool IsListTemplate, int nParentIndex)
        {
            if (rListObj == null || nIndex >= rListObj.Count) return;

            var rListItem = rListObj[nIndex] as ViewModel;
            if (rListItem == null) return;
            rListItem.Initialize(false);

            var rBindingRetrive = rTrans.GetComponent<DataBindingRetrive>();
            if (rBindingRetrive == null)
            {
                string path = $"{rTrans.transform.name}";
                while (rTrans.transform.parent != null)
                {
                    path += $" < {rTrans.transform.name}";
                    rTrans = rTrans.parent;
                }
                LogManager.LogError($"DataBindingRetrive 为空 节点: {path}");
            }
            rBindingRetrive.DataIndex = nIndex;
            var rAllEventBindings = rBindingRetrive.EventBindings;
            for (int i = 0; i < rAllEventBindings.Count; i++)
            {
                var rEventBinding = rAllEventBindings[i];
                if (!rEventBinding.IsListTemplate) continue;

                if (rEventBinding.IsEventValid())
                {
                    Action<Knight.Core.EventArg> rActionDelegate = (rEventArg) =>
                    {
                        if (this.OnCheck(rEventBinding) == false) return;
                        if (!IsListTemplate)
                        {
                            rEventBinding.TargetMethod.Invoke(this, new object[] { nIndex, rEventArg });
                        }
                        else
                        {
                            rEventBinding.TargetMethod.Invoke(this, new object[] { nIndex, nParentIndex, rEventArg });
                        }
                    };
                    rEventBinding.SetEventAction(rActionDelegate);
                }
                else
                {
                    rEventBinding.OnDestroy();
                    var bResult = HotfixDataBindingTypeResolve.MakeListViewModelDataBindingEvent(this, rEventBinding, nIndex, IsListTemplate, nParentIndex, this.OnCheck);
                    if (!bResult)
                    {
                        Knight.Core.LogManager.LogErrorFormat("Make view model binding event {0} failed..", rEventBinding.ViewModelMethod);
                    }
                }
            }

            // 清除已有的事件监听
            rListItem.ClearBindProp();
            var rAllMemberBindings = rBindingRetrive.MemberBindings;
            for (int i = 0; i < rAllMemberBindings.Count; i++)
            {
                var rMemberBinding = rAllMemberBindings[i];
                if (!rMemberBinding.IsListTemplate) continue;    // 过滤掉非ListTemplate标记的Binding Script
                if (rMemberBinding.ViewProp == null)
                {
                    rMemberBinding.ViewProp = DataBindingTypeResolve.MakeViewDataBindingProperty(rMemberBinding);
                }
                if (rMemberBinding.ViewProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("List template View Path: {0} {1} error..", rMemberBinding, rMemberBinding.ViewPath);
                    return;
                }

                if (rMemberBinding.ViewModelProp == null)
                {
                    rMemberBinding.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rMemberBinding);
                    if (rMemberBinding.ViewModelProp == null)
                    {
                        Knight.Core.LogManager.LogErrorFormat("View Model {0} Path: {1} error..", rMemberBinding.name, rMemberBinding.ViewModelPath);
                        return;
                    }
                }

                // 设置类型转换器
                if (rMemberBinding.IsDataConvert)
                {
                    if (rMemberBinding.ViewModelProp.ConvertMethod == null)
                    {
                        rMemberBinding.ViewModelProp.ConvertMethod = HotfixDataBindingTypeResolve.MakeViewModelDataBindingConvertMethod(rMemberBinding);
                        if (rMemberBinding.IsDataConvert && rMemberBinding.ViewModelProp.ConvertMethod == null)
                        {
                            Knight.Core.LogManager.LogError("Cannot find convert method: {0}.." + rMemberBinding.DataConverterMethodPath);
                            return;
                        }
                    }
                }
                else
                {
                    rMemberBinding.ViewModelProp.ConvertMethod = null;
                }

                rMemberBinding.ViewModelProp.PropertyOwner = rListItem;
                //rMemberBinding.SyncFromViewModel(rListItem.GetPropValue_Object(rMemberBinding.ViewModelPropName));

                if (rListItem != null)
                {
                    // ViewModel绑定View
                    rMemberBinding.BindingWatcher(rListItem, rMemberBinding.ViewModelProp.PropertyName);
                    //rMemberBinding.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rListItem, rMemberBinding.ViewModelProp.PropertyName, (rValue) =>
                    //{
                    //    rMemberBinding.SyncFromViewModel(rValue);
                    //});
                    rListItem.BindPropChanged(rMemberBinding);
                    rListItem.PropChanged_Manual(rMemberBinding);
                    //rListItem.PropChangedHandler += rMemberBinding.ViewModelPropertyWatcher.PropertyChanged;
                }
            }
            this.OnListViewFillCellFunc_SubList(rTrans, nIndex, rListItem);

        }
        private void OnListViewFillCellFunc_SubList(Transform rTrans, int nIndex, ViewModel rListItem)
        {
            var rViewModelDataSourceTemplates = rTrans.GetComponentsInChildren<ViewModelDataSourceTemplate>(true);
            for (int i = 0; i < rViewModelDataSourceTemplates.Length; i++)
            {
                var rViewModelDataSourceTemplate = rViewModelDataSourceTemplates[i];
                //
                if (!rViewModelDataSourceTemplate.IsListTemplate) continue;
                rViewModelDataSourceTemplate.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSourceTemplate);
                if (rViewModelDataSourceTemplate.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSourceTemplate.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSourceTemplate.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"SubList View Model---{rTrans.gameObject.name}--- TemplatePath: {rViewModelDataSourceTemplate.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                rViewModelDataSourceTemplate.ViewModelProp.PropertyOwner = rListItem;
                rViewModelDataSourceTemplate.ParentIndex = nIndex;

                this.mSubViewModelDataSourceTemplates.Add(rViewModelDataSourceTemplate);
                //绑定子List
                var rViewModelDataSourceList = rViewModelDataSourceTemplates[i] as ViewModelDataSourceList;
                if (rViewModelDataSourceList != null)
                {
                    // 绑定Watcher
                    rViewModelDataSourceList.BindingWatcher(rListItem, rViewModelDataSourceList.ViewModelProp.PropertyName, (rValue) =>
                    {
                        // 重新设置List数据时候，改变个数
                        this.BindingList(rViewModelDataSourceList, rValue);
                    });
                    //rViewModelDataSourceList.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rListItem, rViewModelDataSourceList.ViewModelProp.PropertyName, (rValue) =>
                    //{
                    //    // 重新设置List数据时候，改变个数
                    //    this.BindingList(rViewModelDataSourceList,rValue);
                    //});
                    rListItem.BindPropChanged(rViewModelDataSourceList);
                    //rListItem.PropChangedHandler += rViewModelDataSourceList.ViewModelPropertyWatcher.PropertyChanged;

                    // 初始化list
                    this.BindingList(rViewModelDataSourceList, rListItem.GetPropValue_Object(rViewModelDataSourceList.ViewModelProp.PropertyName));
                    continue;
                }

                //绑定子Array
                var rViewModelDataSourceArray = rViewModelDataSourceTemplates[i] as ViewModelDataSourceArray;
                if (rViewModelDataSourceArray != null)
                {
                    // 绑定watcher
                    rViewModelDataSourceArray.BindingWatcher(rListItem, rViewModelDataSourceArray.ViewModelProp.PropertyName, (rValue) =>
                    {
                        this.FillArrayItems(rViewModelDataSourceArray, rValue);
                    });
                    //rViewModelDataSourceArray.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rListItem, rViewModelDataSourceArray.ViewModelProp.PropertyName, (rValue) =>
                    //{
                    //    this.FillArrayItems(rViewModelDataSourceArray, rValue);
                    //});
                    rListItem.BindPropChanged(rViewModelDataSourceArray);

                    this.FillArrayItems(rViewModelDataSourceArray, rListItem.GetPropValue_Object(rViewModelDataSourceArray.ViewModelProp.PropertyName));
                    continue;
                }

            }
        }

        private void BindingTabViewAndViewModels(ViewControllerContainer rViewControllerContainer)
        {
            var rViewModelDataSources = rViewControllerContainer.ViewModelDataSourceTabs;
            for (int i = 0; i < rViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSources[i];
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewControllerContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定Watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    this.FillTabItems(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    this.FillTabItems(rViewModelDataSource,rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                this.FillTabItems(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }

        private void BindingArrayViewAndViewModels(ViewControllerContainer rViewModelContainer)
        {
            var rViewModelDataSources = rViewModelContainer.ViewModelDataSourceArrays;
            for (int i = 0; i < rViewModelDataSources.Count; i++)
            {
                var rViewModelDataSource = rViewModelDataSources[i];
                if (rViewModelDataSource.IsListTemplate) continue;
                rViewModelDataSource.ViewModelProp = HotfixDataBindingTypeResolve.MakeViewModelDataBindingProperty(rViewModelDataSource);
                if (rViewModelDataSource.ViewModelProp == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model Path: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                var rViewModelDSTemplateItemType = TypeResolveManager.Instance.GetType(rViewModelDataSource.TemplatePath);
                if (rViewModelDSTemplateItemType == null)
                {
                    Knight.Core.LogManager.LogErrorFormat($"View Model---{rViewModelContainer.gameObject.name}--- TemplatePath: {rViewModelDataSource.TemplatePath} error..");
                    return;
                }
                ViewModelManager.Instance.AddViewModelPropMap(rViewModelDSTemplateItemType);

                ViewModel rViewModel = this.GetViewModel(rViewModelDataSource.ViewModelProp.PropertyOwnerKey);
                if (rViewModel == null)
                {
                    Knight.Core.LogManager.LogErrorFormat("View Model: {0} error..", rViewModelDataSource.ViewModelPath);
                    return;
                }

                rViewModelDataSource.ViewModelProp.PropertyOwner = rViewModel;

                // 绑定watcher
                rViewModelDataSource.BindingWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                {
                    this.FillArrayItems(rViewModelDataSource, rValue);
                });
                //rViewModelDataSource.ViewModelPropertyWatcher = new DataBindingPropertyWatcher(rViewModel, rViewModelDataSource.ViewModelProp.PropertyName, (rValue) =>
                //{
                //    this.FillArrayItems(rViewModelDataSource, rValue);
                //});
                rViewModel.BindPropChanged(rViewModelDataSource);
                //rViewModel.PropChangedHandler += rViewModelDataSource.ViewModelPropertyWatcher.PropertyChanged;

                this.FillArrayItems(rViewModelDataSource, rViewModel.GetPropValue_Object(rViewModelDataSource.ViewModelProp.PropertyName));
            }
        }

        private List<Transform> mTempNeedFreeTransformList = new List<Transform>();
        private void FillArrayItems(ViewModelDataSourceArray rViewModelDataSource, object rValue)
        {
            var rListObj = (IList)rValue;
            var nListCount = rListObj != null ? rListObj.Count : 0;

            if (!rViewModelDataSource.HasInitData)
            {
                if (rViewModelDataSource.ArrayPool != null)
                {
                    this.mTempNeedFreeTransformList.Clear();
                    //rViewModelDataSource.ItemTransList.Clear();
                    var nChildCount = rViewModelDataSource.transform.childCount;
                    int k = 0;
                    for (int i = 0; i < nChildCount; i++)
                    {
                        var rChildTrans = rViewModelDataSource.transform.GetChild(i);
                        if (!rChildTrans.gameObject.activeSelf || rChildTrans == rViewModelDataSource.ArrayPool.transform) continue;
                        if (k >= nListCount)
                        {
                            this.mTempNeedFreeTransformList.Add(rChildTrans);
                        }
                        this.OnListViewFillCellFunc(rChildTrans, k, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                        UtilTool.SafeExecute(rViewModelDataSource.OnFillCellFunc_Other, rChildTrans, k);
                        k++;
                    }
                    for (int i = 0; i < this.mTempNeedFreeTransformList.Count; i++)
                    {
                        rViewModelDataSource.ArrayPool.Free(this.mTempNeedFreeTransformList[i].gameObject);
                    }
                    //rViewModelDataSource.ItemTransList = new List<RectTransform>();
                    for (int i = k; i < nListCount; i++)
                    {
                        if (!rViewModelDataSource.ItemTemplateGo) continue;

                        GameObject rItemInstGo = rViewModelDataSource.ArrayPool.GetObject();
                        rItemInstGo.SetActive(true);
                        var rRectTrans = rItemInstGo.GetComponent<RectTransform>();
                        rViewModelDataSource.ItemTransList.Add(rRectTrans);
                        if (rViewModelDataSource.IsChangeItemName)
                            rItemInstGo.name = "Item" + (i + 1);
                        rItemInstGo.transform.localPosition = Vector3.zero;
                        rItemInstGo.transform.SetParent(rViewModelDataSource.transform, false);

                        this.OnListViewFillCellFunc(rItemInstGo.transform, i, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                        UtilTool.SafeExecute(rViewModelDataSource.OnFillCellFunc_Other, rItemInstGo.transform, i);
                    }
                }
                else
                {
                    // 删除节点下的所有数据
                    rViewModelDataSource.transform.DeleteChildren(true);
                    rViewModelDataSource.ItemTransList = new List<RectTransform>();
                    for (int i = 0; i < nListCount; i++)
                    {
                        if (!rViewModelDataSource.ItemTemplateGo) continue;

                        GameObject rItemInstGo = GameObject.Instantiate(rViewModelDataSource.ItemTemplateGo);
                        rItemInstGo.SetActive(true);
                        var rRectTrans = rItemInstGo.GetComponent<RectTransform>();
                        rViewModelDataSource.ItemTransList.Add(rRectTrans);
                        if (rViewModelDataSource.IsChangeItemName)
                            rItemInstGo.name = "Item" + (i + 1);
                        rItemInstGo.transform.localPosition = Vector3.zero;
                        rItemInstGo.transform.SetParent(rViewModelDataSource.transform, false);

                        this.OnListViewFillCellFunc(rItemInstGo.transform, i, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                        UtilTool.SafeExecute(rViewModelDataSource.OnFillCellFunc_Other, rItemInstGo.transform, i);
                    }
                }
            }
            else
            {
                int k = 0;
                for (int i = 0; i < rViewModelDataSource.ItemTransList.Count; i++)
                {
                    var rTrans = rViewModelDataSource.ItemTransList[i];
                    if (rTrans.gameObject.activeSelf)
                    {
                        if (rViewModelDataSource.IsChangeItemName)
                            rTrans.name = "Item" + (k + 1);
                        this.OnListViewFillCellFunc(rTrans, k, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);
                        UtilTool.SafeExecute(rViewModelDataSource.OnFillCellFunc_Other, rTrans, k);
                        k++;
                    }
                }
            }

            rViewModelDataSource.OnFillArray?.Invoke();
        }

        private void FillTabItems(ViewModelDataSourceTab rViewModelDataSource, object rValue)
        {
            // 重新设置Tab数据时候，改变个数
            var rListObj = (IList)rValue;
            var nListCount = rListObj != null ? rListObj.Count : 0;

            rViewModelDataSource.TabView.transform.DeleteChildren(true);
            rViewModelDataSource.TabView.TabButtons = new List<TabButton>();

            for (int k = 0; k < nListCount; k++)
            {
                GameObject rTabInstGo = GameObject.Instantiate(rViewModelDataSource.TabView.TabTemplateGo);
                rTabInstGo.SetActive(true);
                rTabInstGo.name = "tab_" + k;
                rTabInstGo.transform.SetParent(rViewModelDataSource.TabView.transform, false);
                this.OnListViewFillCellFunc(rTabInstGo.transform, k, rListObj, rViewModelDataSource.IsListTemplate, rViewModelDataSource.ParentIndex);

                var rTabButton = rTabInstGo.ReceiveComponent<TabButton>();
                rTabButton.group = rViewModelDataSource.TabView;
                rTabButton.TabIndex = k;
                rTabButton.isOn = k == 0;
                rViewModelDataSource.TabView.TabButtons.Add(rTabButton);
            }
        }

        private ViewModel CreateViewModel(string rKey, string rViewModelClassName, bool bIsGlobal)
        {
            if (!bIsGlobal)
            {
                var rViewModelType = TypeResolveManager.Instance.GetType(rViewModelClassName);
                Type parentType = rViewModelType.BaseType;
                var Obj = HotfixReflectAssists.Construct(rViewModelType);
                var rViewModel = Obj as ViewModel;
                if (rViewModel == null)
                    LogManager.LogError($"rViewModel==null!!!{rKey} {rViewModelClassName} {bIsGlobal}");
                rViewModel.Initialize(false);
                return rViewModel;
            }
            else
            {
                return ViewModelManager.Instance.ReceiveViewModel(rViewModelClassName);
            }
        }

        public ViewModel GetViewModel(string rKey)
        {
            ViewModel rViewModel = null;
            if (!this.ViewModels.TryGetValue(rKey, out rViewModel))
            {
                LogManager.LogError($"获取ViewModel失败(GetViewModel): rKey = {rKey}");
            }

            return rViewModel;
        }

        public async Task Open()
        {
            if (this.View != null && this.View.ViewModelContainer != null && this.View.ViewModelContainer.CanvasGroup != null)
                this.View.ViewModelContainer.CanvasGroup.interactable = false;
            await base.Initialize();
            await this.OnOpen();
            if (this.View != null && this.View.ViewModelContainer != null && this.View.ViewModelContainer.CanvasGroup != null)
                this.View.ViewModelContainer.CanvasGroup.interactable = true;
        }

        public void Show()
        {
            this.OnShow();
        }

        public void Hide()
        {
            this.OnHide();
        }

        public void Closing()
        {
            this.Close();
        }

        #region Virtual Function

        protected override void OnDispose()
        {
            foreach (var rDict in this.ViewModels)
            {
                if (!rDict.Value.IsGlobal)
                {
                    rDict.Value.UnbindFromProto();
                }
            }
            //this.ViewModels.Clear();
        }

        /// <summary>
        /// 数据绑定之后
        /// </summary>
#pragma warning disable 1998
        protected virtual async Task OnOpen()
#pragma warning restore 1998
        {
            // this.JudgeEntryCondition();
            HotfixUpdateManager.Instance.AddUpdateInstance(this.mUpdateInstance);
        }


        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected override void OnClose()
        {
            HotfixUpdateManager.Instance.RemoveUpdateInstance(this.mUpdateInstance);
        }

#pragma warning disable 1998
        public virtual async Task UIPreloading()
        {
        }
#pragma warning restore 1998
        #endregion

        protected void SetLateUpdateAction(Action<float> rAction)
        {
            this.mUpdateInstance.LateUpdateAction = rAction;
        }

        protected void SetUpdateAction(Action<float> rAction)
        {
            this.mUpdateInstance.UpdateAction = rAction;
        }

        // public virtual BackActionState OnBackActionClose()
        // {
        //     return BackActionState.None;
        // }

        public virtual System.Object[] CacheParamOnGoOut()
        {
            return null;
        }
    }
}
