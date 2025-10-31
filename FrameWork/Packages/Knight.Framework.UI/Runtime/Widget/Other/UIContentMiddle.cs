using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class UIContentMiddle : MonoBehaviour
    {
        public RectTransform ParentTrans;
        public RectTransform RectTrans;

        private GridLayoutGroup GridLayoutGroup;

        private LoopHorizontalScrollRect LoopHorizontalScrollRect;
        private float ContentWidth
        {
            get
            {
                float nWidth = this.RectTrans.rect.width;
                if (nWidth == 0)
                {
                    if (this.GridLayoutGroup)
                    {
                        //预计算ContentWidth
                        nWidth = this.GridLayoutGroup.padding.left + this.GridLayoutGroup.padding.right;
                        if (this.LoopHorizontalScrollRect)
                        {
                            int nTotalCellCount = this.LoopHorizontalScrollRect.totalCount;
                            if (nTotalCellCount != 0)
                                return nWidth + nTotalCellCount * (this.GridLayoutGroup.cellSize.x + this.GridLayoutGroup.spacing.x) - this.GridLayoutGroup.spacing.x;
                        }
                        return nWidth + this.GridLayoutGroup.cellSize.x;
                    }
                }

                return nWidth;
            }
        }

        [Header("Padding")]
        public float Left;
        public float Right;
        //public float Top;
        //public float Bottom;
        void Awake()
        {
            this.RectTrans = this.GetComponent<RectTransform>();
            if (this.RectTrans)
            {
                this.GridLayoutGroup = this.GetComponent<GridLayoutGroup>();

            }
            this.ParentTrans = this.transform.parent.GetComponent<RectTransform>();
            if (this.ParentTrans)
            {
                this.LoopHorizontalScrollRect = this.ParentTrans.parent.GetComponent<LoopHorizontalScrollRect>();
            }
        }
        void LateUpdate()
        {
            if (this.RectTrans == null || this.ParentTrans == null) return;

            if (this.ContentWidth + this.Left + this.Right <= this.ParentTrans.rect.width)
            {
                var fPosX = (this.ParentTrans.rect.width - this.ContentWidth - this.Left - this.Right) / 2.0f;
                this.RectTrans.anchoredPosition = new Vector2(fPosX, this.RectTrans.anchoredPosition.y);
            }
        }
    }
}
