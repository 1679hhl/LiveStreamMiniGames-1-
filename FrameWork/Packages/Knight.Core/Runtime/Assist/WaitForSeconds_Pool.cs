using System.Collections;
using UnityEngine;

namespace Knight.Core
{

    public class WaitForSeconds_Pool : TSingleton<WaitForSeconds_Pool>
    {
        public class WaitForSeconds : CustomYieldInstruction
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
            private bool m_Active = true;
            private float m_Time;
            private System.Action<WaitForSeconds> m_Free;
            private bool m_Done => Time.time < this.m_Time;
            public WaitForSeconds()
            {

            }
            public void SetFreeAction(System.Action<WaitForSeconds> rFree)
            {
                this.m_Free = rFree;
            }

            public void SetTime(float nTime)
            {
                this.m_Time = Time.time + nTime;
                this.m_Active = true;
            }
        }

        private TObjectPool<WaitForSeconds> m_Pool = null;
        private WaitForSeconds_Pool()
        {
            this.m_Pool = new TObjectPool<WaitForSeconds>(this.Create, null, null);
        }

        public WaitForSeconds AllocItem(float fTime)
        {
            var rItem = this.m_Pool.Alloc();
            rItem.SetTime(fTime);
            return rItem;
        }

        private void Free(WaitForSeconds rWaitForSeconds)
        {
            this.m_Pool.Free(rWaitForSeconds);
        }

        private WaitForSeconds Create()
        {
            var rInstance = new WaitForSeconds();
            rInstance.SetFreeAction(this.Free);
            return rInstance;
        }
    }

}
