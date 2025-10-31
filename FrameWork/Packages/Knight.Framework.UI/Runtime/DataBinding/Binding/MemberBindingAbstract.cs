using Knight.Core;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace UnityEngine.UI
{
    public partial class MemberBindingAbstract : MonoBehaviour
    {
        public bool                         IsListTemplate;
        [Dropdown("ViewPaths")]
        [HideIf("IsHideViewPath")]
        [InfoBox("ViewPath无效", InfoBoxType.Error, "IsViewPathInvaild")]
        public string                       ViewPath;

        [HideIf("IsDisplayConverter")]
        public bool                         IsDataConvert;
        [Dropdown("ModelConvertMethods")]
        [HideIf("IsNeedDataConverter")]
        [InfoBox("DataConverterMethodPath无效", InfoBoxType.Error, "IsDataConverterMethodPathInvalid")]
        public string                       DataConverterMethodPath;

        [Dropdown("ModelPaths")]
        [InfoBox("ViewModelPath无效", InfoBoxType.Error, "IsViewModelPathInvalid")]
        public string                       ViewModelPath;
        
        [SerializeField]
        [HideInInspector]
        public Component                    ViewComp;
        [SerializeField]
        [HideInInspector]
        public string                       ViewPropName;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelKey;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelClassName;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelPropName;
        [SerializeField]
        [HideInInspector]
        public string                       ConvertMethodClassName;
        [SerializeField]
        [HideInInspector]
        public string                       ConvertMethodName;

        public DataBindingProperty          ViewProp;
        public DataBindingProperty          ViewModelProp;


        public DataBindingPropertyWatcher ViewModelPropertyWatcher;
        public void BindingWatcher(object rPropOwner, string rPropName)
        {
            if (this.ViewModelProp.ValueTypeName == "Int32")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherInt(rPropOwner, rPropName, this.SyncFromViewModelInt);
            }
            else if (this.ViewModelProp.ValueTypeName == "Int64")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherInt64(rPropOwner, rPropName, this.SyncFromViewModelInt64);
            }
            else if (this.ViewModelProp.ValueTypeName == "String")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherString(rPropOwner, rPropName, this.SyncFromViewModelString);
            }
            else if (this.ViewModelProp.ValueTypeName == "Single")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherFloat(rPropOwner, rPropName, this.SyncFromViewModelFloat);
            }
            else if (this.ViewModelProp.ValueTypeName == "Vector2")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherVector2(rPropOwner, rPropName, this.SyncFromViewModelVector2);
            }
            else if (this.ViewModelProp.ValueTypeName == "Vector3")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherVector3(rPropOwner, rPropName, this.SyncFromViewModelVector3);
            }
            else if (this.ViewModelProp.ValueTypeName == "Color")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherColor(rPropOwner, rPropName, this.SyncFromViewModelColor);
            }
            else if (this.ViewModelProp.ValueTypeName == "Boolean")
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherBool(rPropOwner, rPropName, this.SyncFromViewModelBool);
            }
            else
            {
                this.ViewModelPropertyWatcher = new DataBindingPropertyWatcherObject(rPropOwner, rPropName, this.SyncFromViewModel);
            }
        }

        public void SyncFromViewModel(object rValue)
        {
            if (rValue == null)
            {
                rValue = this.ViewModelProp?.GetValue();
                if(rValue != null)
                    Debug.LogError($"SyncFromViewModel rValue == null, PropertyOwnerKey = {this.ViewProp.PropertyOwnerKey }; PropertyName = {this.ViewProp.PropertyName}");
            }
            else
            {
                if (this.ViewModelProp.ConvertMethod != null)
                {
                    rValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                }
            }
            this.ViewProp?.SetValue(rValue);
        }

        public void SyncFromViewModelInt(int rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
               var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }

        public void SyncFromViewModelInt64(long rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }
        public void SyncFromViewModelBool(bool rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }

        public void SyncFromViewModelColor(Color rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }

        public void SyncFromViewModelFloat(float rValue)
        {

            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }
        public void SyncFromViewModelVector2(Vector2 rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }
        public void SyncFromViewModelVector3(Vector3 rValue)
        {

            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }
        public void SyncFromViewModelString(string rValue)
        {
            if (this.ViewModelProp.ConvertMethod != null)
            {
                var rCovertValue = this.ViewModelProp.ConvertMethod.Invoke(null, new object[] { rValue });
                this.ViewProp?.SetValue(rCovertValue);
            }
            else
            {
                this.ViewProp?.SetValue(rValue);
            }
        }

        public void SyncFromView()
        {
            var rValue = this.ViewProp?.GetValue();
            this.ViewModelProp?.SetValue(rValue);
        }

        public virtual void OnDestroy()
        {
        }
    }

    public partial class MemberBindingAbstract
    {
        private string[] ViewPaths = new string[0];
        private string[] ModelPaths = new string[0];
        private string[] ModelConvertMethods = new string[0];

        public virtual void GetPaths()
        {
            this.ViewPaths = DataBindingTypeResolve.GetAllViewPaths(this.gameObject).ToArray();
            this.ViewProp = DataBindingTypeResolve.MakeViewDataBindingProperty(this.gameObject, this.ViewPath);

            if (this.ViewProp != null)
            {
                if (!this.IsDataConvert)
                {
                    var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                        DataBindingTypeResolve.GetViewModelProperties(this.gameObject, this.ViewProp.Property.PropertyType, this.IsListTemplate));
                    this.ModelPaths = DataBindingTypeResolve.GetAllViewModelPaths(rViewModelProps).ToArray();
                }
                else
                {
                    var rViewModelMethods = new List<BindableMember<MethodInfo>>(
                        DataBindingTypeResolve.GetViewModelConvertMethods(this.ViewProp.Property.PropertyType, this.IsListTemplate));
                    this.ModelConvertMethods = DataBindingTypeResolve.GetAllConvertMethodPaths(rViewModelMethods).ToArray();

                    if (!string.IsNullOrEmpty(this.DataConverterMethodPath))
                    {
                        var nIndex = new List<string>(this.ModelConvertMethods).IndexOf(this.DataConverterMethodPath);
                        if (nIndex >= 0)
                        {
                            var rViewModelType = rViewModelMethods[nIndex].ViewModelType;
                            var rParamType = rViewModelMethods[nIndex].Member.GetParameters()[0].ParameterType;
                            var rViewModelProps = new List<BindableMember<PropertyInfo>>(
                                DataBindingTypeResolve.GetViewModelProperties(this.gameObject, rParamType, rViewModelType, this.IsListTemplate));
                            this.ModelPaths = DataBindingTypeResolve.GetAllViewModelPaths(rViewModelProps).ToArray();
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(this.ViewPath))
            {
                this.ViewPath = this.ViewPaths.Length > 0 ? this.ViewPaths[0] : string.Empty;
            }
            if(!string.IsNullOrEmpty(this.ViewModelPath) && !this.ModelPaths.AnyOne(item => item == this.ViewModelPath))
            {
                this.ViewModelPath = string.Empty;
            }
            if (string.IsNullOrEmpty(this.DataConverterMethodPath))
            {
                this.DataConverterMethodPath = this.ModelConvertMethods.Length > 0 ? this.ModelConvertMethods[0] : string.Empty;
            }

            this.GetViewPathParams(this.ViewPath);
            this.GetViewModelParams(this.ViewModelPath);
            this.GetConvertMethodParams(this.DataConverterMethodPath);
        }

        private void GetViewPathParams(string rViewPath)
        {
            if (string.IsNullOrEmpty(rViewPath))
            {
                this.ViewPropName = string.Empty;
                this.ViewComp = null;
                return;
            }
            var rViewPathStrs = rViewPath.Split('/');
            if (rViewPathStrs.Length < 2)
            {
                this.ViewPropName = string.Empty;
                this.ViewComp = null;
                return;
            }
            var rViewClassName = rViewPathStrs[0].Trim();
            var rViewProp = rViewPathStrs[1].Trim();

            var rViewPropStrs = rViewProp.Split(':');
            if (rViewPropStrs.Length < 1)
            {
                this.ViewPropName = string.Empty;
                this.ViewComp = null;
                return;
            }

            this.ViewPropName = rViewPropStrs[0].Trim();
            this.ViewComp = this.GetComponents<Component>()
                .Where(comp => comp != null &&
                               comp.GetType().FullName.Equals(rViewClassName) &&
                               comp.GetType().GetProperty(this.ViewPropName) != null)
                .FirstOrDefault();

            //this.SetValueFuncName = $"Set_{this.ViewComp?.GetType().Name}_{this.ViewPropName}";

        }

        private void GetViewModelParams(string rViewModelPath)
        {
            if (string.IsNullOrEmpty(rViewModelPath)) return;

            var rViewModelPathStrs = rViewModelPath.Split('/');
            if (rViewModelPathStrs.Length < 2) return;

            var rViewModelClassName = rViewModelPathStrs[0].Trim();
            var rViewModelProp = rViewModelPathStrs[1].Trim();

            var rViewModelPropStrs = rViewModelProp.Split(':');
            if (rViewModelPropStrs.Length < 1) return;

            rViewModelClassName = rViewModelClassName.Replace("ListTemplate@", "");

            this.ViewModelPropName = rViewModelPropStrs[0].Trim();

            var rViewModelClassNameStrs = rViewModelClassName.Split('$');
            if (rViewModelClassNameStrs.Length == 2)
            {
                this.ViewModelKey = rViewModelClassNameStrs[0];
                this.ViewModelClassName = rViewModelClassNameStrs[1];
            }
            else
            {
                this.ViewModelClassName = rViewModelClassNameStrs[0];
            }

            //this.SetValueType = rViewModelPropStrs[1];
        }

        private void GetConvertMethodParams(string rDataConverterMethodPath)
        {
            var rMethodPathStrs = rDataConverterMethodPath.Split('/');
            if (rMethodPathStrs.Length < 2) return;

            this.ConvertMethodClassName = rMethodPathStrs[0].Trim();
            this.ConvertMethodName = rMethodPathStrs[1].Trim();
        }

        public string GetViewPropValueType()
        {
            var rViewPropStrs = this.ViewPath.Split(':');
            if (rViewPropStrs.Length < 1) return "";
            var rValueType = rViewPropStrs[1].Trim();
            return rValueType;
        }

        public string GetViewModelPropValueType()
        {
            if(string.IsNullOrEmpty(this.ViewModelPath))
            {
                return string.Empty;
            }
            var rViewModelPropStrs = this.ViewModelPath.Split(':');
            if (rViewModelPropStrs == null || rViewModelPropStrs.Length < 1) return string.Empty;
            var rValueType = rViewModelPropStrs[1].Trim();
            return rValueType;
        }

        protected virtual bool IsHideViewPath()
        {
            return false;
        }

        protected bool IsNeedDataConverter()
        {
            return !this.IsDataConvert;
        }

        protected bool IsDisplayConverter()
        {
            return this.GetType() != typeof(UnityEngine.UI.MemberBindingOneWay);
        }

        public virtual bool IsSelectionValid()
        {
            return this.IsViewPathInvaild() == false &&
                this.IsDataConverterMethodPathInvalid() == false &&
                this.IsViewModelPathInvalid() == false;
        }

        public virtual bool IsViewPathInvaild()
        {
            if (this.ViewPaths.Length == 0 ||
                this.ModelPaths.Length == 0 ||
                this.ModelConvertMethods.Length == 0)
            {
                GetPaths();
            }

            return !(this.IsHideViewPath() || this.ViewPaths.AnyOne(item => item == this.ViewPath));
        }

        public virtual bool IsDataConverterMethodPathInvalid()
        {
            if (this.ViewPaths.Length == 0 ||
              this.ModelPaths.Length == 0 ||
              this.ModelConvertMethods.Length == 0)
            {
                GetPaths();
            }

            return !(this.IsDataConvert == false || this.ModelConvertMethods.AnyOne(item => item == this.DataConverterMethodPath));
        }

        public virtual bool IsViewModelPathInvalid()
        {
            if (this.ViewPaths.Length == 0 ||
               this.ModelPaths.Length == 0 ||
               this.ModelConvertMethods.Length == 0)
            {
                GetPaths();
            }

            return !this.ModelPaths.AnyOne(item => item == this.ViewModelPath);
        }
    }
}
