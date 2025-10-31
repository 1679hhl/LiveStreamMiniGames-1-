using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine.UI
{
    public class DataBindingProperty
    {
        public string PropertyOwnerKey;
        public object PropertyOwner;

        public string PropertyName;

        private PropertyInfo mProperty;
        public PropertyInfo Property
        {
            set
            {
                this.mProperty = value;
            }
            get
            {
                if (this.mProperty == null)
                {
                    this.mProperty = this.PropertyOwner?.GetType()?.GetProperty(this.PropertyName);
                }
                return this.mProperty;
            }
        }

        public string SetValueFuncName;
        public string ValueTypeName;
        public ViewModelBindType ValueType;

        public string ConvertMethodName;
        public MethodInfo ConvertMethod;

        public DataBindingProperty(object rPropOwner, string rPropName, string rValueTypeName)
            : this(rPropOwner, "", rPropName, rValueTypeName)
        {
        }
        public DataBindingProperty(object rPropOwner, string rPropOwnerKey, string rPropName, string rValueTypeName)
        {
            this.PropertyOwnerKey = rPropOwnerKey;
            this.PropertyOwner = rPropOwner;
            this.PropertyName = rPropName;
            if (rPropOwner != null)
                this.SetValueFuncName = $"Set_{rPropOwner.GetType()?.Name}_{rPropName}";
            this.ValueTypeName = rValueTypeName;
            this.ValueType = ViewModelBindType.Object;
            if (ViewModelTypes.ViewModelTypeMap.ContainsKey(rValueTypeName))
                this.ValueType = ViewModelTypes.ViewModelTypeMap[rValueTypeName];
        }
        public object GetValue()
        {
            var rValue = this.Property?.GetValue(this.PropertyOwner);

            if (this.ConvertMethod != null)
            {
                rValue = this.ConvertMethod.Invoke(null, new object[] { rValue });
            }
            return rValue;
        }
        public void SetValue(object rValue)
        {
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(string rValue)
        {
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(int rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(bool rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(Color rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(Vector2 rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(Vector3 rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
        public void SetValue(float rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }

        public void SetValue(long rValue)
        {
            if (ViewBindTool.SetValue(this.SetValueFuncName, this.PropertyOwner, rValue))
            {
                return;
            }
            this.Property?.SetValue(this.PropertyOwner, rValue);
        }
    }
}
