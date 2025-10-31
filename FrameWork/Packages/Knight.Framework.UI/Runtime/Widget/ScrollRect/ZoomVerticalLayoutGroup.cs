namespace UnityEngine.UI
{
    using System;

    [AddComponentMenu("UI/Zoom Out Vertical Layout", 53)]
    /// <summary>
    /// Layout child layout elements below each other.
    /// </summary>
    /// <remarks>
    /// The VerticalLayoutGroup component is used to layout child layout elements below each other.
    /// </remarks>
    public class ZoomVerticalLayoutGroup : ZoomHorizontalOrVerticalLayoutGroup
    {
        public Func<int> OnGetChildCount;
        public Func<int, RectTransform> OnGetChild;

        protected ZoomVerticalLayoutGroup()
        {
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, true);
        }

        public override void CalculateLayoutInputVertical()
        {
            CalcAlongAxis(1, true);
        }

        public override void SetLayoutHorizontal()
        {
            SetChildrenAlongAxis(0, true);
        }

        public override void SetLayoutVertical()
        {
            SetChildrenAlongAxis(1, true);
        }

        protected override int GetChildCount()
        {
            if (OnGetChildCount != null)
            {
                return OnGetChildCount();
            }
            return base.GetChildCount();
        }

        protected override RectTransform GetChild(int index)
        {
            if (OnGetChild != null)
            {
                return OnGetChild(index);
            }
            return base.GetChild(index);
        }

    }
}
