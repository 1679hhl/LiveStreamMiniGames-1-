using System.Collections;
using UnityEngine;

namespace Knight.Core
{
    public class WaitForSecondsRealTime_Pool : TSingleton<WaitForSecondsRealTime_Pool>
    {
        public class WaitForSecondsRealTime : CustomYieldInstruction
        {
            public override bool keepWaiting
            {
                get
                {
                    var bWaiting = this.m_Done;
                    if (!bWaiting && this.m_Active)
                    {
                        this.m_Active = false;
                        this.m_Free.Invoke(this);
                    }
                    return bWaiting;
                }
            }
            private float m_Time;
            private System.Action<WaitForSecondsRealTime> m_Free;
            private bool m_Active = true;
            private bool m_Done => Time.realtimeSinceStartup < this.m_Time;
            public WaitForSecondsRealTime()
            {

            }
            public void SetFreeAction(System.Action<WaitForSecondsRealTime> rFree)
            {
                this.m_Free = rFree;
            }

            public void SetTime(float fTime)
            {
                this.m_Time = Time.realtimeSinceStartup + fTime;
                this.m_Active = true;
            }
        }

        private TObjectPool<WaitForSecondsRealTime> m_Pool = null;
        private WaitForSecondsRealTime_Pool()
        {
            this.m_Pool = new TObjectPool<WaitForSecondsRealTime>(this.Create, null, null);
        }

        public WaitForSecondsRealTime AllocItem(float fTime)
        {
            var rItem = this.m_Pool.Alloc();
            rItem.SetTime(fTime);
            return rItem;
        }

        private void Free(WaitForSecondsRealTime rWaitForSeconds)
        {
            this.m_Pool.Free(rWaitForSeconds);
        }

        private WaitForSecondsRealTime Create()
        {
            var rInstance = new WaitForSecondsRealTime();
            rInstance.SetFreeAction(this.Free);
            return rInstance;
        }
    }

}
