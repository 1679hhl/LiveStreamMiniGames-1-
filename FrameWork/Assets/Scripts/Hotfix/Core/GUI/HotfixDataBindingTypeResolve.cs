using Knight.Core;
using System;
using System.Linq;
using System.Reflection;

namespace UnityEngine.UI
{
    public class HotfixDataBindingTypeResolve
    {
        public static DataBindingProperty MakeViewModelDataBindingProperty(MemberBindingAbstract rMemberBinding)
        {
            var rViewModelKey = rMemberBinding.ViewModelKey;
            var rViewModelPropName = rMemberBinding.ViewModelPropName;
            var rViewModelClassName = rMemberBinding.ViewModelClassName;

            var rViewModelProperty = new DataBindingProperty(null, rViewModelKey, rViewModelPropName, rMemberBinding.GetViewModelPropValueType());
            Type rTempType = TypeResolveManager.Instance.GetType(rViewModelClassName);
            while (rTempType != null && rTempType != typeof(ViewModel))
            {
                rViewModelProperty.Property = rTempType.GetProperty(rViewModelPropName);
                if (rViewModelProperty.Property != null)
                {
                    break;
                }
                rTempType = rTempType.BaseType;
            }
            if (rViewModelProperty.Property == null)
            {
                Knight.Core.LogManager.LogError("ViewModelClass: " + rMemberBinding.ViewModelPath + " get property error.");
                return null;
            }
            return rViewModelProperty;
        }

        public static DataBindingProperty MakeViewModelDataBindingProperty(ViewModelDataSourceTemplate rViewModelDSTemplate)
        {
            var rViewModelKey = rViewModelDSTemplate.ViewModelKey;
            var rViewModelPropName = rViewModelDSTemplate.ViewModelPropName;
            var rViewModelClassName = rViewModelDSTemplate.ViewModelClassName;

            var rViewModelProperty = new DataBindingProperty(null, rViewModelKey, rViewModelPropName, "");
            Type rTempType = TypeResolveManager.Instance.GetType(rViewModelClassName);
            while (rTempType != null && rTempType != typeof(ViewModel))
            {
                rViewModelProperty.Property = rTempType.GetProperty(rViewModelPropName);
                if (rViewModelProperty.Property != null)
                {
                    break;
                }
                rTempType = rTempType.BaseType;
            }
            if (rViewModelProperty.Property == null)
            {
                Knight.Core.LogManager.LogError("ViewModelClass: " + rViewModelDSTemplate.ViewModelPath + " get property error.");
                return null;
            }
            return rViewModelProperty;
        }

        public static MethodInfo MakeViewModelDataBindingConvertMethod(MemberBindingAbstract rMemberBinding)
        {
            if (string.IsNullOrEmpty(rMemberBinding.ConvertMethodClassName) || string.IsNullOrEmpty(rMemberBinding.ConvertMethodName)) return null;
            return TypeResolveManager.Instance.GetType(rMemberBinding.ConvertMethodClassName)?.GetMethod(rMemberBinding.ConvertMethodName);
        }

        public static bool MakeViewModelDataBindingEvent(ViewController rViewController, EventBinding rEventBinding, Func<EventBinding, bool> rCheckAction = null)
        {
            var rViewModelEventName = rEventBinding.ViewModelEventName;
            if (string.IsNullOrEmpty(rViewModelEventName)) return false;

            MethodInfo rMethodInfo = null;
            var rAllMethodInfos = rViewController.GetType().GetMethods(ReflectionAssist.flags_method_inst);
            for (int i = 0; i < rAllMethodInfos.Length; i++)
            {
                if (rAllMethodInfos[i].Name.Equals(rViewModelEventName))
                {
                    rMethodInfo = rAllMethodInfos[i];
                    break;
                }
            }
            if (rMethodInfo != null)
            {
                Action<EventArg> rActionDelegate = (rEventArg) =>
                {
                    if (rCheckAction?.Invoke(rEventBinding) == false) 
                        return; 
                    rMethodInfo.Invoke(rViewController, new object[] { rEventArg });
                };
                rEventBinding.InitEventWatcher(rActionDelegate);
                rEventBinding.TargetMethod = rMethodInfo;
            }
            else
            {
                Knight.Core.LogManager.LogErrorFormat("Can not find Method: {0},{1} in ViewController.", rEventBinding, rEventBinding.ViewModelMethod);
                return false;
            }
            return true;
        }

        public static bool MakeListViewModelDataBindingEvent(ViewController rViewController, EventBinding rEventBinding, int nIndex,bool IsListTemplate, int nParentIndex, Func<EventBinding, bool> rCheckAction = null)
        {
            if (string.IsNullOrEmpty(rEventBinding.ViewModelMethod)) return false;

            var rViewModelEventClass = rEventBinding.ViewModelEventClass;
            var rViewModelEventName = rEventBinding.ViewModelEventName;

            MethodInfo rMethodInfo = null;
            var rAllMethodInfos = rViewController.GetType().GetMethods(ReflectionAssist.flags_method_inst);
            for (int i = 0; i < rAllMethodInfos.Length; i++)
            {
                if (rAllMethodInfos[i].Name.Equals(rViewModelEventName))
                {
                    rMethodInfo = rAllMethodInfos[i];
                    break;
                }
            }

            if (rMethodInfo != null)
            {
                Action<EventArg> rActionDelegate = (rEventArg) =>
                {
                    if (rCheckAction?.Invoke(rEventBinding) == false) return;
                    if (!IsListTemplate)
                    {
                        rMethodInfo.Invoke(rViewController, new object[] { nIndex, rEventArg });
                    }
                    else
                    {
                        rMethodInfo.Invoke(rViewController, new object[] { nIndex, nParentIndex, rEventArg });
                    }
                };
                rEventBinding.InitEventWatcher(rActionDelegate);
                rEventBinding.TargetMethod = rMethodInfo;
            }
            else
            {
                Knight.Core.LogManager.LogErrorFormat("Can not find Method: {0} in ViewController.", rEventBinding.ViewModelMethod);
                return false;
            }

            return true;
        }
    }
}
