using Knight.Core;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public partial class ViewModelDataSourceTemplate : MonoBehaviour
    {
        [InfoBox("ViewModelPath无效", InfoBoxType.Error, "IsViewModelPathInvalid")]
        [Dropdown("ModelPaths")]
        public string                       ViewModelPath;
        [InfoBox("TemplatePath无效", InfoBoxType.Error, "IsTemplatePathInvalid")]
        [DropdownPro("TemplateViewModels")]
        public string                       TemplatePath;

        public DataBindingProperty          ViewModelProp;
        public DataBindingPropertyWatcherObject DataBindingPropertyWatcherObject;
        
        [NonSerialized]
        protected string[]                  ModelPaths = new string[0];
        [NonSerialized]
        protected string[]                  TemplateViewModels = new string[0];

        public bool                         IsListTemplate = false;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelKey;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelClassName;
        [SerializeField]
        [HideInInspector]
        public string                       ViewModelPropName;

        [HideInInspector]
        public int                          ParentIndex { get;  set; } = -1;//父列表Index,用作点击事件参数传递
        protected virtual void Awake()
        {
        }
        public virtual void GetPaths()
        {
            this.TemplateViewModels = DataBindingTypeResolve.GetAllViewModels().ToArray();
        }

        public void BindingWatcher(object rPropOwner, string rPropName,Action<object> rAction)
        {
            this.DataBindingPropertyWatcherObject = new DataBindingPropertyWatcherObject(rPropOwner, rPropName, rAction);

        }

        public virtual Type GetViewModelType()
        {
            return typeof(ViewModelDataSourceTemplate);
        }
        public virtual bool IsSelectionValid()
        {
            return IsViewModelPathInvalid() == false &&
                 IsTemplatePathInvalid() == false;
        }

        public virtual bool IsViewModelPathInvalid()
        {
            if (ModelPaths.Length == 0 || TemplateViewModels.Length == 0)
            {
                this.GetPaths();
            }

            return !this.ModelPaths.AnyOne(item => item == ViewModelPath);
        }

        public virtual bool IsTemplatePathInvalid()
        {
            if (ModelPaths.Length == 0 || TemplateViewModels.Length == 0)
            {
                this.GetPaths();
            }

            return !this.TemplateViewModels.AnyOne(item => item == TemplatePath);
        }

        protected void GetViewModelParams(string rViewModelPath)
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
        }
    }
}