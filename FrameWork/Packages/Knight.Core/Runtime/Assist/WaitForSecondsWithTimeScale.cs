using System.Collections;

namespace Knight.Core
{
    public class WaitForSecondsWithTimeScale
    {
        public static object Wait(float fWaitForSecond, bool bIsIgnoreTimeScale)
        {
            if (bIsIgnoreTimeScale)
            {
                return WaitForSecondsRealTime_Pool.Instance.AllocItem(fWaitForSecond);
            }
            else
            {
                return WaitForSeconds_Pool.Instance.AllocItem(fWaitForSecond);
            }
        }
    }
}
