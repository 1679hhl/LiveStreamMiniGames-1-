using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Core
{
    public interface IKnightObject : IDisposable
    {
        Task Initialize();
        void Update(float fDeltaTime);
        void LateUpdate(float fDeltaTime);
    }

    public class KnightEvent
    {
        public ulong EventCode;
        public Action<EventArg> EventHandler;
    }

    public class ProtoMsgEvent
    {
        public int MsgCmd;
        public Action<ProtoMsgEventArg> EventHandler;
    }

    public static class KnightObjectTool
    {
        public static void BindEvent(object rTarget, out List<KnightEvent> rEvents, out List<ProtoMsgEvent> rProtoMsgEvents)
        {
            // 解析事件Attribute            
            rEvents = new List<KnightEvent>();
            rProtoMsgEvents = new List<ProtoMsgEvent>();

            if (rTarget == null) return;
            var rType = rTarget.GetType();
            if (rType == null) return;

            var rBindingFlags = BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            var rMethodInfos = rType.GetMethods(rBindingFlags);
            for (int i = 0; i < rMethodInfos.Length; i++)
            {
                var rMethodInfo = rMethodInfos[i];

                // 普通消息
                var rAttrObjs = rMethodInfo.GetCustomAttributes(typeof(EventAttribute), false);
                if (rAttrObjs != null && rAttrObjs.Length > 0)
                {
                    var rEventAttr = rAttrObjs[0] as EventAttribute;
                    if (rEventAttr != null)
                    {
                        Action<EventArg> rActionDelegate = (rEventArgs) => { rMethodInfo.Invoke(rTarget, new object[] { rEventArgs }); };
                        var rKnightEvent = new KnightEvent()
                        {
                            EventCode = rEventAttr.MsgCode,
                            EventHandler = rActionDelegate
                        };
                        rEvents.Add(rKnightEvent);
                        // 绑定事件
                        EventManager.Instance.Binding(rKnightEvent.EventCode, rKnightEvent.EventHandler);
                    }
                }
                // 网络消息
                rAttrObjs = rMethodInfo.GetCustomAttributes(typeof(ProtoMsgEventAttribute), false);
                if (rAttrObjs != null && rAttrObjs.Length > 0)
                {
                    var rNetEventAttr = rAttrObjs[0] as ProtoMsgEventAttribute;
                    if (rNetEventAttr != null)
                    {
                        Action<ProtoMsgEventArg> rActionDelegate = (rEventArg) => { rMethodInfo.Invoke(rTarget, new object[] { rEventArg }); };

                        var rProtoMsgEvent = new ProtoMsgEvent()
                        {
                            MsgCmd = rNetEventAttr.MsgCmd,
                            EventHandler = rActionDelegate,
                        };
                        rProtoMsgEvents.Add(rProtoMsgEvent);
                        // 绑定网络消息
                        ProtoMsgEventManager.Instance.Binding(rProtoMsgEvent.MsgCmd, rProtoMsgEvent.EventHandler);
                    }
                }
            }
        }
        public static void UnbindEvent(List<KnightEvent> rEvents, List<ProtoMsgEvent> rProtoMsgEvents)
        {
            if (rEvents != null)
            {
                for (int i = 0; i < rEvents.Count; i++)
                {
                    // 解绑事件
                    EventManager.Instance.Unbinding(rEvents[i].EventCode, rEvents[i].EventHandler);
                    rEvents[i].EventHandler = null;
                }
                rEvents.Clear();
            }
            if (rProtoMsgEvents != null)
            {
                for (int i = 0; i < rProtoMsgEvents.Count; i++)
                {
                    ProtoMsgEventManager.Instance.Unbinding(rProtoMsgEvents[i].MsgCmd, rProtoMsgEvents[i].EventHandler);
                }
                rProtoMsgEvents.Clear();
            }
        }
    }

    public class TSKnightObject<T> where T : TSKnightObject<T>
    {
        static object SyncRoot = new object();
        static T instance;

        public static readonly Type[] EmptyTypes = new Type[0];

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyTypes, null);
                            if (ci == null) { throw new InvalidOperationException("class must contain a private constructor"); }
                            instance = (T)ci.Invoke(null);
                            instance.BindEvent();
                        }
                    }
                }
                return instance;
            }
        }

        protected List<KnightEvent> mEvents;
        protected List<ProtoMsgEvent> mProtoMsgEvents;
        private void BindEvent()
        {
            KnightObjectTool.BindEvent(this, out this.mEvents, out this.mProtoMsgEvents);
        }
        public void UnbindEvent()
        {
            KnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }
    }

    public class TKnightObject<T> : IKnightObject where T : IKnightObject
    {
        protected List<KnightEvent> mEvents;
        protected List<ProtoMsgEvent> mProtoMsgEvents;

#pragma warning disable 1998
        public virtual async Task Initialize()
#pragma warning restore 1998
        {
            KnightObjectTool.BindEvent(this, out this.mEvents, out this.mProtoMsgEvents);
        }

        public virtual void Update(float fDeltaTime)
        {
        }

        public virtual void LateUpdate(float fDeltaTime)
        {
        }

        public virtual void Dispose()
        {
            KnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }
    }

    public class KnightObject : TKnightObject<KnightObject>
    {
        public sealed override async Task Initialize()
        {
            await base.Initialize();
            await this.OnInitialize();
        }

        public sealed override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            this.OnUpdate(fDeltaTime);
        }

        public sealed override void LateUpdate(float fDeltaTime)
        {
            base.LateUpdate(fDeltaTime);
            this.OnLateUpdate(fDeltaTime);
        }

        public sealed override void Dispose()
        {
            this.OnDispose();
            base.Dispose();
        }

#pragma warning disable 1998
        protected virtual async Task OnInitialize()
#pragma warning restore 1998
        {
        }

        protected virtual void OnUpdate(float fDeltaTime)
        {
        }

        protected virtual void OnLateUpdate(float fDeltaTime)
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}
