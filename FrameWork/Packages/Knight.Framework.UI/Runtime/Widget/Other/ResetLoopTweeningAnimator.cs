using Knight.Core;
using Knight.Framework.Tweening;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(TweeningAnimator))]
    public class ResetLoopTweeningAnimator : MonoBehaviour
    {
        public enum MoveDirect
        {
            None,
            Left,
            Right,
        }
        [SerializeField]
        private float scrollSpeed = 12.125f; // 滚动速度
        private TweeningAnimator mTweeningAnimator;
        private float OffsetX = 0;
        public bool IsMoveLeft = true;
        private MoveDirect moveDirect = MoveDirect.None;
        [ContextMenu("Reset")]
        private void OnReset()
        {
            this.mTweeningAnimator = this.GetComponent<TweeningAnimator>();
            if(this.mTweeningAnimator == null)
                return;
            this.OffsetX = this.transform.localPosition.x;
            //获取激活的子节点个数
            int nChildActiveCount = 0;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.activeSelf)
                {
                    nChildActiveCount++;
                }
            }

            var rHorizontalLayoutComponent = this.GetComponent<HorizontalLayoutGroup>();
            float rSpacing = rHorizontalLayoutComponent.spacing;
            float moveDistance = this.transform.GetComponent<RectTransform>().rect.width / nChildActiveCount;
            this.moveDirect = this.IsMoveLeft ? MoveDirect.Left : MoveDirect.Right;
            float endValue = this.moveDirect == MoveDirect.Left ? (-moveDistance - (rSpacing / 2)) : (moveDistance + rSpacing / 2 + this.OffsetX);
            endValue = this.moveDirect == MoveDirect.Left ? this.OffsetX - Mathf.Abs(endValue) : this.OffsetX + Mathf.Abs(endValue);
            LogManager.Log($"获取移动距离：{endValue}");
            float durationValue = Mathf.Abs(endValue) / this.scrollSpeed;
            LogManager.Log($"计算移动速度：{durationValue}");
            if (this.mTweeningAnimator.Actions.Count == 1)
            {
                this.mTweeningAnimator.Actions[0].Duration = durationValue;
                this.mTweeningAnimator.Actions[0].StartF = this.OffsetX;
                this.mTweeningAnimator.Actions[0].DefultFloat = this.OffsetX;
                this.mTweeningAnimator.Actions[0].EndF = endValue;
            }
        }
    }
}
