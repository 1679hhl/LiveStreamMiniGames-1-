using Knight.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Knight.Hotfix.Core
{
    public interface IHotfixKnightObject : IDisposable
    {
        Task Initialize();
    }

    public class HotfixKnightEvent
    {
        public ulong EventCode;
        public Action<EventArg> EventHandler;
    }

    public class HotfixProtoMsgEvent
    {
        public int MsgCmd;
        public Action<ProtoMsgEventArg> EventHandler;
    }

    public static class HotfixKnightObjectTool
    {
        public static void BindEvent(object rTarget, out List<HotfixKnightEvent> rEvents, out List<HotfixProtoMsgEvent> rProtoMsgEvents)
        {
            // 解析事件Attribute            
            rEvents = new List<HotfixKnightEvent>();
            rProtoMsgEvents = new List<HotfixProtoMsgEvent>();

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
                        var rKnightEvent = new HotfixKnightEvent()
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

                        var rProtoMsgEvent = new HotfixProtoMsgEvent()
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
        public static void UnbindEvent(List<HotfixKnightEvent> rEvents, List<HotfixProtoMsgEvent> rProtoMsgEvents)
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

    public class TSHotfixKnightObject<T> where T : TSHotfixKnightObject<T>
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

        protected List<HotfixKnightEvent> mEvents;
        protected List<HotfixProtoMsgEvent> mProtoMsgEvents;
        private void BindEvent()
        {
            HotfixKnightObjectTool.BindEvent(this, out this.mEvents, out this.mProtoMsgEvents);
        }
        public void UnbindEvent()
        {
            HotfixKnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }
        
        /// <summary>
        /// 谨慎调用,确保该单例对象需要释放的时候才能调用
        /// </summary>
        public static void DestroyInstance()
        {
            instance = null;
        }
    }

    public class THotfixKnightObject<T> : IHotfixKnightObject where T : IHotfixKnightObject
    {
        protected List<HotfixKnightEvent> mEvents;
        protected List<HotfixProtoMsgEvent> mProtoMsgEvents;

#pragma warning disable 1998
        public virtual async Task Initialize()
#pragma warning restore 1998
        {
            HotfixKnightObjectTool.BindEvent(this, out this.mEvents, out this.mProtoMsgEvents);
        }

        public virtual void Close()
        {
            HotfixKnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }

        public virtual void Dispose()
        {
            HotfixKnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }
    }

    public class HotfixKnightObject : THotfixKnightObject<HotfixKnightObject>
    {

        public sealed override void Close()
        {
            this.OnClose();
            base.Close();
        }

        public sealed override void Dispose()
        {
            this.OnDispose();
            base.Dispose();
        }


        protected virtual void OnClose()
        {
        }

        protected virtual void OnDispose()
        {
        }
    }
}
