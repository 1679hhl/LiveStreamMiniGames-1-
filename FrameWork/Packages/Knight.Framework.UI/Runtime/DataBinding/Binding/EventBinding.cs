using System;
using System.Collections.Generic;
using Knight.Core;
using NaughtyAttributes;
using System.Reflection;
using System.Linq;

namespace UnityEngine.UI
{   
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public partial class EventBinding : MonoBehaviour
    {
        public bool                 IsListTemplate;
        [InfoBox("ViewEvent无效", InfoBoxType.Error, "IsEventInvalid")]
        [Dropdown("ViewEvents")]
        public string               ViewEvent;
        [InfoBox("ViewModelMethod无效", InfoBoxType.Error, "IsViewModelMethodInvalid")]
        [Dropdown("ViewModelMethods")]

        public string               ViewModelMethod;
        [SerializeField]
        [HideInInspector]
        public string               ViewModelEventClass;
        [SerializeField]
        [HideInInspector]
        public string               ViewModelEventName;
        [SerializeField]
        [HideInInspector]
        public Component            ViewEventComp;
        [SerializeField]
        [HideInInspector]
        public string               ViewEventName;

        public MethodInfo           TargetMethod { get; set; }

        private UnityEventWatcher   mUnityEventWatcher;
        
        public void InitEventWatcher(Action<EventArg> rAction)
        {
            var rBoundEvent = DataBindingTypeResolve.MakeViewDataBindingEvent(this);
            if (rBoundEvent != null)
            {
                this.mUnityEventWatcher = new UnityEventWatcher(rBoundEvent.Component, rBoundEvent.Name, rAction);
            }
            else
            {
                Knight.Core.LogManager.LogErrorFormat("Can not parse bound event: {0}.", this.ViewEvent);
            }
        }

        public void OnDestroy()
        {
            this.mUnityEventWatcher?.Dispose();
        }

        public bool IsEventValid()
        {
            return this.mUnityEventWatcher != null;
        }

        public void SetEventAction(Action<EventArg> rAction)
        {
            this.mUnityEventWatcher.SetEventAction(rAction);
        }

        public override string ToString()
        {
            return this.ViewModelMethod;
        }
    }

    public partial class EventBinding
    {
        private string[]     ViewEvents          = new string[0];
        private string[]     ViewModelMethods    = new string[0];

        public bool IsSelectionValid()
        {
            return IsEventInvalid() == false &&
                IsViewModelMethodInvalid() == false;
        }

        public bool IsEventInvalid()
        {
            if (this.ViewEvents == null || this.ViewEvents.Length == 0 )
            {
                this.GetPaths();
            }

            return !this.ViewEvents.AnyOne(item => item == this.ViewEvent);
        }

        public bool IsViewModelMethodInvalid()
        {
            if (this.ViewModelMethods == null || this.ViewModelMethods.Length == 0)
            {
                this.GetPaths();
            }

            return !this.ViewModelMethods.AnyOne(item => item == this.ViewModelMethod);
        }

        public void GetPaths()
        {
            this.ViewEvents = DataBindingTypeResolve.GetBindableEventPaths(this.gameObject);
            this.ViewModelMethods = DataBindingTypeResolve.GetViewModelBindingEvents(this.gameObject);
            if (string.IsNullOrEmpty(this.ViewEvent))
            {
                this.ViewEvent = this.ViewEvents.Length > 0 ? this.ViewEvents[0] : "";
            }
            if (!string.IsNullOrEmpty(this.ViewModelMethod) && !this.ViewModelMethods.AnyOne((item) => item == this.ViewModelMethod))
            {
                this.ViewModelMethod = string.Empty;
            }
            this.GetViewEventParams(this.ViewEvent);

            this.GetViewModelEventParams(this.ViewModelMethod);
        }

        private void GetViewEventParams(string rViewEvent)
        {
            if (string.IsNullOrEmpty(rViewEvent))
            {
                this.ViewEventComp = null;
                this.ViewEventName = string.Empty;
                return;
            }
            var rEventPathStrs = rViewEvent.Split('/');
            if (rEventPathStrs.Length < 2)
            {
                this.ViewEventComp = null;
                this.ViewEventName = string.Empty;
                return;
            }
            var rEventClassName = rEventPathStrs[0].Trim();
            this.ViewEventName = rEventPathStrs[1].Trim();
            this.ViewEventComp = this.GetComponents<Component>()
                .Where(comp => comp != null && comp.GetType().FullName.Equals(rEventClassName)).FirstOrDefault();
        }

        private void GetViewModelEventParams(string rViewModelMethod)
        {
            if(string.IsNullOrEmpty(rViewModelMethod))
            {
                this.ViewModelEventClass = string.Empty;
                this.ViewModelEventName = string.Empty;
                return;
            }
            var rViewModelMethodStrs = rViewModelMethod.Split('/');
            if (rViewModelMethodStrs.Length < 2)
            {
                this.ViewModelEventClass = string.Empty;
                this.ViewModelEventName = string.Empty;
                return;
            }
            this.ViewModelEventClass = rViewModelMethodStrs[0];
            this.ViewModelEventName = rViewModelMethodStrs[1];
        }
    }
}
