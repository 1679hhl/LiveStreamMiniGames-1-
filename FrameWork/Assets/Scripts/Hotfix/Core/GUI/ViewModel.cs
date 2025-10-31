using Game;
using Knight.Core;
using Knight.Hotfix.Core;
using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class ViewModelKeyAttribute : Attribute
    {
        public string Key;

        public ViewModelKeyAttribute(string rKey)
        {
            this.Key = rKey;
        }
    }

    public class ViewModelTypes : HotfixTypeSearchDefault<ViewModel>
    {
        public static Dictionary<string, ViewModelBindType> ViewModelTypeMap = new Dictionary<string, ViewModelBindType>()
        {
            { "Int32", ViewModelBindType.Int },
            { "Int64", ViewModelBindType.Long },
            { "Boolean", ViewModelBindType.Bool },
            { "Single", ViewModelBindType.Float },
            { "float", ViewModelBindType.Float },
            { "Color", ViewModelBindType.Color },
            { "Vector2", ViewModelBindType.Vector2 },
            { "Vector3", ViewModelBindType.Vector3 },
            { "String", ViewModelBindType.String },
        };
    }

    public class ViewModelPropMap
    {
        public Dictionary<string, List<RelatePropInfo>> PropMaps;

        public void Initialize(Type rViewModelType)
        {
            this.PropMaps = new Dictionary<string, List<RelatePropInfo>>();
            ViewModelTool.Initialize(rViewModelType, this.PropMaps);
        }
    }

    [TSIgnore]
    [TypeResolveCache]
    public class ViewModel
    {
        private Action<string, object> mPropChangedHandler;
        private Action<string, int> mPropChangedHandlerInt;
        private Action<string, long> mPropChangedHandlerInt64;
        private Action<string, bool> mPropChangedHandlerBool;
        private Action<string, float> mPropChangedHandlerFloat;
        private Action<string, Color> mPropChangedHandlerColor;
        private Action<string, Vector2> mPropChangedHandlerVector2;
        private Action<string, Vector3> mPropChangedHandlerVector3;
        private Action<string, string> mPropChangedHandlerString;

        protected Dictionary<string, Func<object>> mPropObjectGetters = new Dictionary<string, Func<object>>();
        protected Dictionary<string, Func<int>> mPropIntGetters = new Dictionary<string, Func<int>>();
        protected Dictionary<string, Func<long>> mPropInt64Getters = new Dictionary<string, Func<long>>();
        protected Dictionary<string, Func<bool>> mPropBoolGetters = new Dictionary<string, Func<bool>>();
        protected Dictionary<string, Func<float>> mPropFloatGetters = new Dictionary<string, Func<float>>();
        protected Dictionary<string, Func<Color>> mPropColorGetters = new Dictionary<string, Func<Color>>();
        protected Dictionary<string, Func<Vector2>> mPropVector2Getters = new Dictionary<string, Func<Vector2>>();
        protected Dictionary<string, Func<Vector3>> mPropVector3Getters = new Dictionary<string, Func<Vector3>>();
        protected Dictionary<string, Func<string>> mPropStringGetters = new Dictionary<string, Func<string>>();

        public bool IsGlobal { get; private set; } = false;

        public ViewModel()
        {
            this.AddPropGetterToDicts();
        }
        public void Initialize(bool bIsGlobal)
        {
            this.IsGlobal = bIsGlobal;
        }
        public virtual void AddPropGetterToDicts()
        {
        }
        public virtual void BindToProto()
        {
        }
        public virtual void UnbindFromProto()
        {
        }
        public virtual void ClearCacheData()
        {
        }

        public void AddPropGetterToDict_Object(string rPropName, Func<object> rPropGetter)
        {
            if (this.mPropObjectGetters.ContainsKey(rPropName))
                this.mPropObjectGetters[rPropName] = rPropGetter;
            else
                this.mPropObjectGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Int32(string rPropName, Func<int> rPropGetter)
        {
            if (this.mPropIntGetters.ContainsKey(rPropName))
                this.mPropIntGetters[rPropName] = rPropGetter;
            else
                this.mPropIntGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Int64(string rPropName, Func<long> rPropGetter)
        {
            if (this.mPropInt64Getters.ContainsKey(rPropName))
                this.mPropInt64Getters[rPropName] = rPropGetter;
            else
                this.mPropInt64Getters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_String(string rPropName, Func<string> rPropGetter)
        {
            if (this.mPropStringGetters.ContainsKey(rPropName))
                this.mPropStringGetters[rPropName] = rPropGetter;
            else
                this.mPropStringGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Single(string rPropName, Func<float> rPropGetter)
        {
            if (this.mPropFloatGetters.ContainsKey(rPropName))
                this.mPropFloatGetters[rPropName] = rPropGetter;
            else
                this.mPropFloatGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Boolean(string rPropName, Func<bool> rPropGetter)
        {
            if (this.mPropBoolGetters.ContainsKey(rPropName))
                this.mPropBoolGetters[rPropName] = rPropGetter;
            else
                this.mPropBoolGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Color(string rPropName, Func<Color> rPropGetter)
        {
            if (this.mPropColorGetters.ContainsKey(rPropName))
                this.mPropColorGetters[rPropName] = rPropGetter;
            else
                this.mPropColorGetters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Vector2(string rPropName, Func<Vector2> rPropGetter)
        {
            if (this.mPropVector2Getters.ContainsKey(rPropName))
                this.mPropVector2Getters[rPropName] = rPropGetter;
            else
                this.mPropVector2Getters.Add(rPropName, rPropGetter);
        }
        public void AddPropGetterToDict_Vector3(string rPropName, Func<Vector3> rPropGetter)
        {
            if (this.mPropVector3Getters.ContainsKey(rPropName))
                this.mPropVector3Getters[rPropName] = rPropGetter;
            else
                this.mPropVector3Getters.Add(rPropName, rPropGetter);
        }
        public virtual object GetPropValue_Object(string rPropName)
        {
            this.mPropObjectGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : null;
        }
        public virtual int GetPropValue_Int32(string rPropName)
        {
            this.mPropIntGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : 0;
        }
        public virtual long GetPropValue_Int64(string rPropName)
        {
            this.mPropInt64Getters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : 0;
        }
        public virtual string GetPropValue_String(string rPropName)
        {
            this.mPropStringGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : string.Empty;
        }
        public virtual float GetPropValue_Single(string rPropName)
        {
            this.mPropFloatGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : 0.0f;
        }
        public virtual bool GetPropValue_Boolean(string rPropName)
        {
            this.mPropBoolGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : false;
        }
        public virtual Color GetPropValue_Color(string rPropName)
        {
            this.mPropColorGetters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : Color.black;
        }
        public virtual Vector2 GetPropValue_Vector2(string rPropName)
        {
            this.mPropVector2Getters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : Vector2.zero;
        }
        public virtual Vector3 GetPropValue_Vector3(string rPropName)
        {
            this.mPropVector3Getters.TryGetValue(rPropName, out var rPropGetter);
            return rPropGetter != null ? rPropGetter.Invoke() : Vector3.zero;
        }

        public void PropChanged(string rPropName, object rValue = null)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null)
            {
                LogManager.LogError("rViewModelPropMap null");
                return;
            }
            if (rValue == null)
            {
                rValue = this.GetPropValue_Object(rPropName);
            }
            if(this.mPropChangedHandler==null)
                LogManager.LogRelease($"{rPropName} this.mPropChangedHandler null");

            this.mPropChangedHandler?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, string rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerString?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, int rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerInt?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, long rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerInt64?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, float rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerFloat?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, bool rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerBool?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, Color rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            this.mPropChangedHandlerColor?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, Vector2 rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            if (rValue == null)
            {
                rValue = this.GetPropValue_Vector2(rPropName);
            }
            this.mPropChangedHandlerVector2?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }
        public void PropChanged(string rPropName, Vector3 rValue)
        {
            var rViewModelPropMap = ViewModelManager.Instance.GetViewModelPropMap(this.GetType());
            if (rViewModelPropMap == null) return;
            if (rValue == null)
            {
                rValue = this.GetPropValue_Vector3(rPropName);
            }
            this.mPropChangedHandlerVector3?.Invoke(rPropName, rValue);
            this.PropchangedRelate(rPropName, rViewModelPropMap);
        }

        private void PropchangedRelate(string rPropName, ViewModelPropMap rViewModelPropMap)
        {
            if (rViewModelPropMap.PropMaps.TryGetValue(rPropName, out List<RelatePropInfo> rRelatedProps))
            {
                for (int i = 0; i < rRelatedProps.Count; i++)
                {
                    var rPropValueType = rRelatedProps[i].PropValueType;
                    var rRelatePropName = rRelatedProps[i].PropName;
                    switch (rPropValueType)
                    {
                        case ViewModelBindType.Int:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Int32(rRelatePropName));
                            break;
                        case ViewModelBindType.Long:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Int64(rRelatePropName));
                            break;
                        case ViewModelBindType.Float:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Single(rRelatePropName));
                            break;
                        case ViewModelBindType.String:
                            this.PropChanged(rRelatePropName, this.GetPropValue_String(rRelatePropName));
                            break;
                        case ViewModelBindType.Bool:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Boolean(rRelatePropName));
                            break;
                        case ViewModelBindType.Color:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Color(rRelatePropName));
                            break;
                        case ViewModelBindType.Vector2:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Vector2(rRelatePropName));
                            break;
                        case ViewModelBindType.Vector3:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Vector3(rRelatePropName));
                            break;
                        case ViewModelBindType.Object:
                        default:
                            this.PropChanged(rRelatePropName, this.GetPropValue_Object(rRelatePropName));
                            break;
                    }
                }
            }
        }

        public void BindPropChanged(MemberBindingAbstract rMemberBindingAbstract)
        {
            switch (rMemberBindingAbstract.ViewModelProp.ValueType)
            {
                case ViewModelBindType.Int:
                    this.mPropChangedHandlerInt += ((DataBindingPropertyWatcherInt)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Long:
                    this.mPropChangedHandlerInt64 += ((DataBindingPropertyWatcherInt64)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Float:
                    this.mPropChangedHandlerFloat += ((DataBindingPropertyWatcherFloat)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.String:
                    this.mPropChangedHandlerString += ((DataBindingPropertyWatcherString)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Bool:
                    this.mPropChangedHandlerBool += ((DataBindingPropertyWatcherBool)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Color:
                    this.mPropChangedHandlerColor += ((DataBindingPropertyWatcherColor)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Vector2:
                    this.mPropChangedHandlerVector2 += ((DataBindingPropertyWatcherVector2)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Vector3:
                    this.mPropChangedHandlerVector3 += ((DataBindingPropertyWatcherVector3)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Object:
                default:
                    this.mPropChangedHandler += ((DataBindingPropertyWatcherObject)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
            }
        }

        public void PropChanged_Manual(MemberBindingAbstract rMemberBindingAbstract)
        {
            switch (rMemberBindingAbstract.ViewModelProp.ValueType)
            {
                case ViewModelBindType.Int:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Int32(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Long:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Int64(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Float:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Single(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.String:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_String(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Bool:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Boolean(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Color:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Color(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Vector2:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Vector2(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Vector3:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Vector3(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
                case ViewModelBindType.Object:
                default:
                    this.PropChanged(rMemberBindingAbstract.ViewModelProp.PropertyName, this.GetPropValue_Object(rMemberBindingAbstract.ViewModelProp.PropertyName));
                    break;
            }
        }

        public void UnBindPropChanged(MemberBindingAbstract rMemberBindingAbstract)
        {
            switch (rMemberBindingAbstract.ViewModelProp.ValueType)
            {
                case ViewModelBindType.Int:
                    this.mPropChangedHandlerInt -= ((DataBindingPropertyWatcherInt)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Long:
                    this.mPropChangedHandlerInt64 -= ((DataBindingPropertyWatcherInt64)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Float:
                    this.mPropChangedHandlerFloat -= ((DataBindingPropertyWatcherFloat)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.String:
                    this.mPropChangedHandlerString -= ((DataBindingPropertyWatcherString)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Bool:
                    this.mPropChangedHandlerBool -= ((DataBindingPropertyWatcherBool)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Color:
                    this.mPropChangedHandlerColor -= ((DataBindingPropertyWatcherColor)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Vector2:
                    this.mPropChangedHandlerVector2 -= ((DataBindingPropertyWatcherVector2)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Vector3:
                    this.mPropChangedHandlerVector3 -= ((DataBindingPropertyWatcherVector3)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
                case ViewModelBindType.Object:
                default:
                    this.mPropChangedHandler -= ((DataBindingPropertyWatcherObject)rMemberBindingAbstract.ViewModelPropertyWatcher).PropertyChanged;
                    break;
            }
        }

        public void BindPropChanged(ViewModelDataSourceTemplate rViewModelDataSourceTemplate)
        {
            this.mPropChangedHandler += ((DataBindingPropertyWatcherObject)rViewModelDataSourceTemplate.DataBindingPropertyWatcherObject).PropertyChanged;
        }

        public void UnBindPropChanged(ViewModelDataSourceTemplate rViewModelDataSourceTemplate)
        {
            this.mPropChangedHandler -= ((DataBindingPropertyWatcherObject)rViewModelDataSourceTemplate.DataBindingPropertyWatcherObject).PropertyChanged;
        }

        public void ClearBindProp()
        {
            this.mPropChangedHandlerInt = null;
            this.mPropChangedHandlerInt64 = null;
            this.mPropChangedHandlerString = null;
            this.mPropChangedHandlerFloat = null;
            this.mPropChangedHandlerVector2 = null;
            this.mPropChangedHandlerVector3 = null;
            this.mPropChangedHandlerColor = null;
            this.mPropChangedHandlerBool = null;
            this.mPropChangedHandler = null;
        }
    }
}
