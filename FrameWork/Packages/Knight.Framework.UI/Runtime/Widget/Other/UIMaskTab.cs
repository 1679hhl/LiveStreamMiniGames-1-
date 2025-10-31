using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
	public class UIMaskTab : MonoBehaviour
    {
        public enum Align
        {
            VerticalTop,
            VerticelBottom,
            HorizontalLeft,
            HorizontalRight,
        }

        public GameObject MaskObj;
        public GameObject BackgroundObj;
        public Align ChildAlign;
        public int Index
        {
            get
            {
                return this.mIndex;
            }
            set
            {
                if (value == this.mIndex) return;
                this.mIndex = value;
                this.UpdateMaskPistion();
            }
        }

        private RectTransform mMaskRect;
        private RectTransform mBackgroundRect;
        private Vector2 mBeginMaskPosition;
        private Vector2 mBeginBgPosition;
        private Vector2 mCellSize;
        private int mIndex;
        private void Awake()
        {
            this.mMaskRect = this.MaskObj?.GetComponent<RectTransform>();
            this.mBackgroundRect = this.BackgroundObj?.GetComponent<RectTransform>();
            this.mBeginMaskPosition = this.mMaskRect.anchoredPosition;
            this.mBeginBgPosition = this.mBackgroundRect.anchoredPosition;
            this.mCellSize = this.mMaskRect.sizeDelta;
        }

        private void UpdateMaskPistion()
        {
            var rMaskPosition = this.mBeginMaskPosition;
            var rBgPosition = this.mBeginBgPosition;
            var rDiffPosition = Vector2.zero;
            switch (this.ChildAlign)
            {
                case Align.HorizontalLeft:
                    rDiffPosition = new Vector2(this.mCellSize.x * this.mIndex, 0);
                    rMaskPosition += rDiffPosition;
                    rBgPosition -= rDiffPosition;
                    break;
                case Align.HorizontalRight:
                    rDiffPosition = new Vector2(this.mCellSize.x * this.mIndex, 0);
                    rMaskPosition -= rDiffPosition;
                    rBgPosition += rDiffPosition;
                    break;
                case Align.VerticalTop:
                    rDiffPosition = new Vector2(0, this.mCellSize.y * this.mIndex);
                    rMaskPosition -= rDiffPosition;
                    rBgPosition += rDiffPosition;
                    break;
                case Align.VerticelBottom:
                    rDiffPosition = new Vector2(0, this.mCellSize.y * this.mIndex);
                    rMaskPosition += rDiffPosition;
                    rBgPosition -= rDiffPosition;
                    break;
                default:
                    break;
            }
            this.mMaskRect.anchoredPosition = rMaskPosition;
            this.mBackgroundRect.anchoredPosition = rBgPosition;
        }
    }
}