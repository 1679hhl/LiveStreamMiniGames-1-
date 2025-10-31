using System;
using System.Collections.Generic;
using Knight.Core;
using NaughtyAttributes;
using System.Linq;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    [DefaultExecutionOrder(100)]
    public partial class MemberBindingTwoWay : MemberBindingAbstract
    {
        [Dropdown("mEventPaths")]
        [DrawOrder(2)]
        public  string              EventPath;

        [SerializeField]
        [HideInInspector]
        public  Component           ViewEventComp;
        [SerializeField]
        [HideInInspector]
        public  string              ViewEventName;

        private UnityEventWatcher   mUnityEventWatcher;

        private void Awake()
        {
            this.IsDataConvert = false;
        }

        public void InitEventWatcher()
        {
            var rBoundEvent = DataBindingTypeResolve.MakeViewDataBindingEvent(this);
            if (rBoundEvent != null)
            {
                this.mUnityEventWatcher = new UnityEventWatcher(rBoundEvent.Component, rBoundEvent.Name, this.SyncFromView);
            }
            else
            {
                Knight.Core.LogManager.LogErrorFormat("Can not parse bound event: {0}.", this.EventPath);
            }
        }

        public override void OnDestroy()
        {
            this.mUnityEventWatcher?.Dispose();
        }

        private void SyncFromView(EventArg rEventArg)
        {
            this.SyncFromView();
        }
    }

    public partial class MemberBindingTwoWay
    {
        [HideInInspector]
        private string[]             mEventPaths = new string[0];

        public void GetEventPaths()
        {
            this.mEventPaths = DataBindingTypeResolve.GetBindableEventPaths(this.gameObject);
            if (string.IsNullOrEmpty(this.EventPath))
            {
                this.EventPath = this.mEventPaths.Length > 0 ? this.mEventPaths[0] : "";
            }
            this.GetViewEventParams(this.EventPath);
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
    }
}
