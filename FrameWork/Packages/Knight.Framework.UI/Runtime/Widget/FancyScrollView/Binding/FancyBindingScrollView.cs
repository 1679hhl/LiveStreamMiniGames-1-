using EasingCore;
using FancyScrollView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    [Serializable]
    public class FancyIndexChangeEvent : UnityEvent<int>
    {
    }
    public class FancyBindingScrollView : FancyScrollView<FancyBindingItemData>
    {

        public Scroller Scroller;
        public GameObject CellPrefabGo;
        protected override GameObject CellPrefab => this.CellPrefabGo;

        public FancyIndexChangeEvent OnSelectionChanged = new FancyIndexChangeEvent();
        public int CurrentSelectIndex
        {
            get
            {
                return this.mCurSelectIndex;
            }
            set
            {
                var nValue = Mathf.Min(value, this.ItemsSource.Count - 1);
                nValue = Mathf.Max(0, nValue);

                if (nValue != this.mCurSelectIndex)
                {
                    this.mCurSelectIndex = nValue;
                    this.ScrollTo(nValue);
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
                this.mScrollDuration = Mathf.Max(float.Epsilon,value);
            }
        }

        protected int mCurSelectIndex { get; set; } = 0;
        protected float mScrollDuration { get; set; } = 0.35f;
        protected override void Initialize()
        {
            base.Initialize();
            this.Scroller.OnValueChanged(this.UpdatePosition);
            this.Scroller.OnSelectionChanged(this.UpdateSelection);
        }


        /// <summary>
        /// 按数量刷新
        /// </summary>
        /// <param name="nListCount"></param>
        public void RefreshCells(int nListCount)
        {
            this.UpdateContents(nListCount);
            this.Scroller.SetTotalCount(nListCount);
        }

        /// <summary>
        /// 选择当前
        /// </summary>
        /// <param name="nIndex"></param>
        public virtual void Select(int nIndex)
        {
            if (nIndex < 0 || nIndex >= this.ItemsSource.Count)
            {
                return;
            }

            this.Scroller.JumpTo(nIndex);
        }

        /// <summary>
        /// 选择最后
        /// </summary>
        public virtual void SelectEnd()
        {
            if (this.ItemsSource.Count == 0) return;

            this.Select(this.ItemsSource.Count - 1);
        }

        /// <summary>
        /// 点击滑动
        /// </summary>
        /// <param name="nIndex"></param>
        /// <param name="rScrollTime"></param>
        public virtual void ScrollTo(int nIndex)
        {
            if (nIndex < 0 || nIndex >= this.ItemsSource.Count)
            {
                return;
            }

            this.Scroller.ScrollTo(nIndex, this.mScrollDuration);
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
    }
}
