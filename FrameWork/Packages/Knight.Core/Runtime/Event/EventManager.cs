//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;

namespace Knight.Core
{
    public class EventArg : IPoolObject
    {
        public List<object> Args;

        public bool Use { get; set; }

        public EventArg()
        {
            this.Args = new List<object>(0);
        }
        public EventArg(object rArg0)
        {
            this.Args = new List<object>(1);
            this.Args.Add(rArg0);
        }
        public EventArg(object rArg0, object rArg1)
        {
            this.Args = new List<object>(2);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
        }
        public EventArg(object rArg0, object rArg1, object rArg2)
        {
            this.Args = new List<object>(3);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
        }
        public EventArg(object rArg0, object rArg1, object rArg2, object rArg3)
        {
            this.Args = new List<object>(4);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
        }
        public EventArg(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4)
        {
            this.Args = new List<object>(5);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
        }
        public EventArg(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5)
        {
            this.Args = new List<object>(6);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
        }
        public EventArg(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5, object rArg6)
        {
            this.Args = new List<object>(7);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
            this.Args.Add(rArg6);
        }
        public EventArg(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5, object rArg6, object rArg7)
        {
            this.Args = new List<object>(8);
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
            this.Args.Add(rArg6);
            this.Args.Add(rArg7);
        }
        public void SetParams(object rArg0)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
        }
        public void SetParams(object rArg0, object rArg1)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2, object rArg3)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5, object rArg6)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
            this.Args.Add(rArg6);
        }
        public void SetParams(object rArg0, object rArg1, object rArg2, object rArg3, object rArg4, object rArg5, object rArg6, object rArg7)
        {
            this.Args.Clear();
            this.Args.Add(rArg0);
            this.Args.Add(rArg1);
            this.Args.Add(rArg2);
            this.Args.Add(rArg3);
            this.Args.Add(rArg4);
            this.Args.Add(rArg5);
            this.Args.Add(rArg6);
            this.Args.Add(rArg7);
        }

        public T Get<T>(int nIndex)
        {
            if (this.Args == null) return default(T);
            if (nIndex < 0 || nIndex >= this.Args.Count) return default(T);
            return (T)this.Args[nIndex];
        }
        public void Add(object rArg)
        {
            this.Args.Add(rArg);
        }

        public void Clear()
        {
            this.Args.Clear();
        }
    }

    public class EventArgBehave : EventArg
    {
    }

    public class EventManager : TSingleton<EventManager>
    {
        public class Event
        {
            public ulong MsgCode;
            public EventList<Action<EventArg>> Callbacks;
        }

        public Dict<ulong, Event> mEvents;

        private EventManager()
        {
        }

        public void Initialize()
        {
            this.mEvents = new Dict<ulong, Event>();
        }

        public bool Contains(ulong nMsgCode)
        {
            return this.mEvents.ContainsKey(nMsgCode);
        }

        public void Binding(ulong nMsgCode, Action<EventArg> rEventCallback)
        {
            if (this.mEvents == null) return;

            Event rEvent = null;
            if (this.mEvents.TryGetValue(nMsgCode, out rEvent))
            {
                if (rEvent.Callbacks == null)
                {
                    rEvent.Callbacks = new EventList<Action<EventArg>>();
                }
                else
                {
                    if (!rEvent.Callbacks.Contains(rEventCallback))
                    {
                        rEvent.Callbacks.Add(rEventCallback);
                    }
                    if (rEvent.Callbacks.Count > 1000)
                    {
                        Knight.Core.LogManager.LogError($"Event 数量超标!!! MsgCode:{nMsgCode}");
                    }
                }
            }
            else
            {
                rEvent = new Event() { MsgCode = nMsgCode, Callbacks = new EventList<Action<EventArg>>() { rEventCallback } };
                this.mEvents.Add(nMsgCode, rEvent);
            }
        }

        public void Unbinding(ulong nMsgCode, Action<EventArg> rEventCallback)
        {
            if (this.mEvents == null) return;

            Event rEvent = null;
            if (this.mEvents.TryGetValue(nMsgCode, out rEvent))
            {
                if (rEvent.Callbacks != null)
                    rEvent.Callbacks.Remove(rEventCallback);
            }
        }

        public void Unbinding(ulong nMsgCode)
        {
            if (this.mEvents == null) return;

            if (this.mEvents.TryGetValue(nMsgCode, out var rEvent))
            {
                if (rEvent.Callbacks != null)
                    rEvent.Callbacks.Clear();
            }
        }

        public void Distribute(ulong nMsgCode)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2, object rEventArg3)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2, rEventArg3);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2, object rEventArg3, object rEventArg4)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2, rEventArg3, rEventArg4);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2, object rEventArg3, object rEventArg4, object rEventArg5)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2, rEventArg3, rEventArg4, rEventArg5);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2, object rEventArg3, object rEventArg4, object rEventArg5, object rEventArg6)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2, rEventArg3, rEventArg4, rEventArg5, rEventArg6);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }
        public void Distribute(ulong nMsgCode, object rEventArg0, object rEventArg1, object rEventArg2, object rEventArg3, object rEventArg4, object rEventArg5, object rEventArg6, object rEventArg7)
        {
            var rEventArg = TSPoolObject<EventArg>.Instance.Alloc();
            rEventArg.SetParams(rEventArg0, rEventArg1, rEventArg2, rEventArg3, rEventArg4, rEventArg5, rEventArg6, rEventArg7);
            this.DistributeArg(nMsgCode, rEventArg);
            TSPoolObject<EventArg>.Instance.Free(rEventArg);
        }

        public void DistributeArg(ulong nMsgCode, EventArg rEventArg)
        {
            if (this.mEvents == null) return;

            Event rEvent = null;
            if (this.mEvents.TryGetValue(nMsgCode, out rEvent))
            {
                if (rEvent.Callbacks != null)
                {
                    var rCallbacks = PoolList<Action<EventArg>>.Alloc();
                    rCallbacks.AddRange(rEvent.Callbacks);
                    for (int i = 0; i < rCallbacks.Count; i++)
                    {
                        UtilTool.SafeExecute(rCallbacks[i], rEventArg);
                    }
                    rCallbacks.Free();
                }
            }
        }
    }
}
