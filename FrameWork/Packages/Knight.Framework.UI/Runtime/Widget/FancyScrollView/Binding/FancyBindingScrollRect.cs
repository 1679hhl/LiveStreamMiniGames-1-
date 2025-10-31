using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;
using System;
using EasingCore;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class FancyBindingScrollRect : FancyScrollRect<FancyBindingItemData>
    {
        public GameObject CellPrefabGo;

        [Space(5)]
        public float CellSizeValue;

        public FancyIndexChangeEvent OnSelectionChanged = new FancyIndexChangeEvent();

        public int CurrentSelectIndex
        {
            get
            {
                return this.mCurSelIndex;
            }
            set
            {
                var nValue = Mathf.Min(value, this.ItemsSource.Count - 1);
                nValue = Mathf.Max(0, nValue);

                if (nValue != this.mCurSelIndex)
                {
                    this.mCurSelIndex = nValue;
                    this.ScrollToCell(nValue, this.mScrollDuration);
                }
            }
        }

        public float ScrollDuration
        {
            get
            {
                return this.mScrollDuration;
            }
            set
            {
                this.mScrollDuration = Mathf.Max(float.Epsilon, value);
            }
        }
        private int mCurSelIndex { get; set; } = 0;
        private float mScrollDuration { get; set; } = 0.35f;
        public float PaddingTop
        {
            get => paddingHead;
            set
            {
                paddingHead = value;
                Relayout();
            }
        }

        public float PaddingBottom
        {
            get => paddingTail;
            set
            {
                paddingTail = value;
                Relayout();
            }
        }

        public float Spacing
        {
            get => spacing;
            set
            {
                spacing = value;
                Relayout();
            }
        }
        protected override float CellSize => this.CellSizeValue;
        protected override GameObject CellPrefab => this.CellPrefabGo;


        protected override void Initialize()
        {
            base.Initialize();
        }
        public void RefreshCells(int nCount)
        {
            this.UpdateContents(nCount);
            this.Scroller.SetTotalCount(nCount);
        }

        /// <summary>
        /// 选择当前
        /// </summary>
        /// <param name="nIndex"></param>
        public virtual void Select(int nIndex, Alignment alignment = Alignment.Middle)
        {
            if (nIndex < 0 || nIndex >= this.ItemsSource.Count)
            {
                return;
            }

            this.UpdateSelection(nIndex);
            this.JumpTo(nIndex, this.GetAlignment(alignment));
        }

        /// <summary>
        /// 选择最后
        /// </summary>
        public virtual void SelectEnd()
        {
            if (this.ItemsSource.Count == 0) return;

            this.Select(this.ItemsSource.Count - 1);
        }

        public virtual void ScrollToCell(int index, float duration, Ease easing = Ease.InOutQuint, Alignment alignment = Alignment.Middle, Action onComplete = null)
        {
            this.UpdateSelection(index);
            this.ScrollTo(index, duration, easing, this.GetAlignment(alignment), onComplete);
        }

        /// <summary>
        /// 主动拖拽滑动选择后的回调
        /// </summary>
        /// <param name="nIndex"></param>
        protected virtual void UpdateSelection(int nIndex)
        {
            this.Refresh();
            this.OnSelectionChanged?.Invoke(nIndex);
        }
        float GetAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Upper: return 0.0f;
                case Alignment.Middle: return 0.5f;
                case Alignment.Lower: return 1.0f;
                default: return GetAlignment(Alignment.Middle);
            }
        }
    }
    public enum Alignment
    {
        Upper,
        Middle,
        Lower,
    }
}
