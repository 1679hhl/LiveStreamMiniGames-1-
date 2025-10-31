using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;
using System;

namespace Knight.Framework.Hotfix
{
    /// <summary>
    /// 热更层Update更新管理器
    /// 热更层性能比C#层低100-5000倍，按需调用
    /// </summary>
    public class HotfixUpdateManager : TSingleton<HotfixUpdateManager>
    {
        // 更新对象
        public class UpdateInstance
        {
            public Action<float> LateUpdateAction;
            public Action<float> UpdateAction;

            internal void LateUpdate(float fDeltaTime)
            {
                if (this.LateUpdateAction != null)
                    this.LateUpdateAction(fDeltaTime);
            }

            internal void Update(float fDeltaTime)
            {
                if (this.UpdateAction != null)
                    this.UpdateAction(fDeltaTime);
            }
        }

        // 定时器
        private class Timer
        {
            public Action Action;
            public float CurrentTime;
            public float TotalTime;
        }

        // 定时更新
        private class TickUpdate
        {
            public Action UpdateAction;
            public float CurrentTime;
            public float TickTime;
        }

        private List<UpdateInstance> mUpdateInstance = new List<UpdateInstance>();

        private List<Timer> mTimers = new List<Timer>();

        private List<TickUpdate> mTickUpdates = new List<TickUpdate>();

        private Dictionary<Action, float> mDelayCallDict = new Dictionary<Action, float>();

        private HotfixUpdateManager() { }

        public void LateUpdate()
        {
            var fTime = Time.time;
            var fDeltaTime = Time.deltaTime;
            for (int i = 0; i < this.mUpdateInstance.Count; ++i)
                this.mUpdateInstance[i].LateUpdate(fDeltaTime);
            if (this.mDelayCallDict.Count > 0)
            {
                var rCalls = PoolList<Action>.Alloc();
                foreach (var rDict in this.mDelayCallDict)
                {
                    if (fTime >= rDict.Value)
                    {
                        rCalls.Add(rDict.Key);
                    }
                }
                for (int i = 0; i < rCalls.Count; i++)
                {
                    var rCall = rCalls[i];
                    this.mDelayCallDict.Remove(rCall);
                    rCall.Invoke();
                }
                rCalls.Free();
            }
        }

        public void Update()
        {
            var fDeltaTime = Time.deltaTime;
            for (int i = 0; i < this.mUpdateInstance.Count; ++i)
            {
                this.mUpdateInstance[i].Update(fDeltaTime);
            }

            for (int i = this.mTimers.Count - 1; i >= 0; --i)
            {
                Timer rTimer = this.mTimers[i];
                rTimer.CurrentTime += fDeltaTime;
                if (rTimer.CurrentTime > rTimer.TotalTime)
                {
                    rTimer.Action();
                    this.mTimers.RemoveAt(i);
                }
            }

            for (int i = 0; i < this.mTickUpdates.Count; ++i)
            {
                TickUpdate rTickUpdate = this.mTickUpdates[i];
                rTickUpdate.CurrentTime += fDeltaTime;
                if (rTickUpdate.CurrentTime > rTickUpdate.TickTime)
                {
                    rTickUpdate.UpdateAction();
                    rTickUpdate.CurrentTime = 0;
                }
            }
        }

        // 移除更新对象
        public void RemoveUpdateInstance(UpdateInstance rUpdateInstance)
        {
            this.mUpdateInstance.Remove(rUpdateInstance);
        }

        // 添加更新对象
        public void AddUpdateInstance(UpdateInstance rUpdateInstance)
        {
            this.mUpdateInstance.Add(rUpdateInstance);
        }

        // 开启定时器
        public void StartTimer(float fTime, Action rAction)
        {
            Timer rTimer = new Timer();
            rTimer.Action = rAction;
            rTimer.TotalTime = fTime;
            this.mTimers.Add(rTimer);
        }

        // 移除定时器
        public void RemoveTimer(Action rAction)
        {
            for (int i = this.mTimers.Count - 1; i >= 0; --i)
            {
                if (this.mTimers[i].Action == rAction)
                {
                    this.mTimers.RemoveAt(i);
                    break;
                }
            }
        }
        public void DelayCall(Action rAction, float fDelayTime = 0f)
        {
            if (!this.mDelayCallDict.ContainsKey(rAction))
            {
                this.mDelayCallDict.Add(rAction, Time.time + fDelayTime);
            }
        }
        public void RemoveDelayCall(Action rAction)
        {
            if(this.mDelayCallDict.ContainsKey(rAction))
                this.mDelayCallDict.Remove(rAction);
        }

        /// <summary>
        /// 添加计时Update循环
        /// 需每帧调用检测判断的地方使用计时>帧率的Update，可减少性能损耗
        /// 注：计时不精准
        /// </summary>
        /// <param name="fTickTime"></param>
        /// <param name="rUpdateAction"></param>
        public void AddTickUpdate(float fTickTime, Action rUpdateAction)
        {
            TickUpdate rTickUpdate = new TickUpdate();
            rTickUpdate.TickTime = fTickTime;
            rTickUpdate.UpdateAction = rUpdateAction;
            this.mTickUpdates.Add(rTickUpdate);
        }

        // 移除计时Upate循环
        public void RemoveTickUpdate(Action rUpdateAction)
        {
            for (int i = this.mTickUpdates.Count - 1; i >= 0; --i)
            {
                if (this.mTickUpdates[i].UpdateAction == rUpdateAction)
                {
                    this.mTickUpdates.RemoveAt(i);
                    break;
                }
            }
        }
    }
}