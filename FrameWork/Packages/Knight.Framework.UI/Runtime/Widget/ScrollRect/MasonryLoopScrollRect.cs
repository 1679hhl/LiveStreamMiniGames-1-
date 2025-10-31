using Knight.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("MasonryLoopScrollRect")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class MasonryLoopScrollRect : ScrollRect
    {
        [Tooltip("Masonry Prefab Source")]
        public LoopScrollPrefabSource PrefabSource;
        [Tooltip("Masonry Prefab Width")]
        public float ItemWidth = 50f;
        [Tooltip("Masonry Prefab Height")]
        public float ItemHeight = 50f;
        [Tooltip("Masonry Spacing")]
        public Vector2 Spacing = Vector2.zero;
        public List<float> ItemHeightOrWidthList { get; private set; } = new List<float>();

        public Func<IList, List<float>> GetItemHeightOrWidthListFunc;
        public Action<Transform, int> OnFillCellFunc;
        public Action<Transform, int> OnFillCellFunc_Other;

        public List<ValueTuple<int, float>> ColumnItemInfoList = new List<(int, float)>();
        public List<ValueTuple<int, float>> RowItemInfoList = new List<(int, float)>();
        public List<Rect> ItemRectList = new List<Rect>();


        protected override void Awake()
        {
            base.Awake();
            this.CheckSetting();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            this.CheckSetting();
        }
#endif

        private void CheckSetting()
        {
            // LogManager.Assert(!this.horizontal, "不可横向滚动");
            // LogManager.Assert(!this.vertical, "不可横向滚动");
            if (this.horizontal)
            {
                LogManager.Assert(this.ItemWidth > 0f, "元素的宽度必须大于0");
            }
            else if (this.vertical)
            {
                LogManager.Assert(this.ItemHeight > 0f, "元素的宽度必须大于0");
            }
            if (this.PrefabSource != null && this.PrefabSource.prefabObj)
            {
                var rPrefabRectTransform = this.PrefabSource.prefabObj.GetComponent<RectTransform>();
                if (rPrefabRectTransform)
                {
                    if (this.vertical)
                    {
                        var rAnchorMin = rPrefabRectTransform.anchorMin;
                        var rAnchorMax = rPrefabRectTransform.anchorMax;
                        var rPivot = rPrefabRectTransform.pivot;
                        LogManager.Assert(rAnchorMin.x == 0f && rAnchorMin.y == 1f, "预制体的AnchorMin必须为x=0;y=1;");
                        LogManager.Assert(rAnchorMax.x == 0f && rAnchorMax.y == 1f, "预制体的AnchorMax必须为x=0;y=1;");
                        LogManager.Assert(rPivot.x == 0f && rPivot.y == 1f, "预制体的Pivot必须为x=0;y=1;");
                    }
                    else
                    {
                        var rAnchorMin = rPrefabRectTransform.anchorMin;
                        var rAnchorMax = rPrefabRectTransform.anchorMax;
                        var rPivot = rPrefabRectTransform.pivot;
                        // LogManager.Assert(rAnchorMin.x == 0f && rAnchorMin.y == 1f, "预制体的AnchorMin必须为x=0;y=1;");
                        // LogManager.Assert(rAnchorMax.x == 0f && rAnchorMax.y == 1f, "预制体的AnchorMax必须为x=0;y=1;");
                        // LogManager.Assert(rPivot.x == 0f && rPivot.y == 1f, "预制体的Pivot必须为x=0;y=1;");
                    }
                }
            }
        }

        public void RefreshItemList(IList rIList)
        {
            this.ItemHeightOrWidthList.Clear();
            if (rIList != null)
            {
                if (this.GetItemHeightOrWidthListFunc != null)
                {
                    var rItemHeightOrWidthList = this.GetItemHeightOrWidthListFunc.Invoke(rIList);
                    this.ItemHeightOrWidthList.AddRange(rItemHeightOrWidthList);
                }
                else
                {
                    var nCount = rIList.Count;
                    for (int i = 0; i < nCount; i++)
                    {
                        this.ItemHeightOrWidthList.Add(0f);
                    }
                    LogManager.LogWarning("未配置获取Item高度函数，无法获取Item高度");
                }
            }

            if (this.vertical)
            {
                this.RefreshColumnHeight();
            }
            else if (this.horizontal)
            {
                this.RefreshRowWidth();
            }
            this.RefreshItemInfo();
        }

        private void RefreshColumnHeight()
        {
            this.ColumnItemInfoList.Clear();
            //列数
            var nColumnCount = Mathf.FloorToInt((this.content.rect.width + this.Spacing.y) / (this.ItemWidth + this.Spacing.y));
            nColumnCount = Mathf.Max(nColumnCount, 1);
            for (int i = 0; i < nColumnCount; i++)
            {
                this.ColumnItemInfoList.Add((-1, 0f));
            }
        }

        private void RefreshRowWidth()
        {
            //确认行数高度
            this.RowItemInfoList.Clear();
            //行数
            var nRowCount = Mathf.FloorToInt((this.content.rect.height + this.Spacing.x) / (this.ItemHeight + this.Spacing.x));
            nRowCount = Mathf.Max(nRowCount, 1);
            for (int i = 0; i < nRowCount; i++)
            {
                this.RowItemInfoList.Add((-1, 0f));
            }
        }

        private Rect CalcNextItemRect(int nItemIndex, float fItemHeightOrWidth)
        {
            Rect rRect;
            if (this.vertical)
            {
                var fMinHeight = 0f;
                var rLastIndex = -1;
                var nColumnIndex = -1;
                for (int i = 0; i < this.ColumnItemInfoList.Count; i++)
                {
                    var (rItemInfo_Index, rItemInfo_Height) = this.ColumnItemInfoList[i];
                    if (nColumnIndex == -1 || rItemInfo_Height < fMinHeight)
                    {
                        rLastIndex = rItemInfo_Index;
                        fMinHeight = rItemInfo_Height;
                        nColumnIndex = i;
                    }
                }
                var fStartHeight = rLastIndex != -1 ? fMinHeight + this.Spacing.y : fMinHeight;
                rRect = new Rect((this.ItemWidth + this.Spacing.x) * nColumnIndex, -fStartHeight, this.ItemWidth, fItemHeightOrWidth + this.Spacing.y);
                this.ColumnItemInfoList[nColumnIndex] = (nItemIndex, fStartHeight + fItemHeightOrWidth + this.Spacing.y);
            }
            else
            {
                var fMinWidth = 0f;
                var rLastIndex = -1;
                var nRowIndex = -1;
                for (int i = 0; i < this.RowItemInfoList.Count; i++)
                {
                    var (rItemInfo_Index, rItemInfo_Width) = this.RowItemInfoList[i];
                    if (nRowIndex == -1 || rItemInfo_Width < fMinWidth)
                    {
                        rLastIndex = rItemInfo_Index;
                        fMinWidth = rItemInfo_Width;
                        nRowIndex = i;
                    }
                }
                var fStartWidth = rLastIndex != -1 ? fMinWidth + this.Spacing.x : fMinWidth;
                rRect = new Rect(fStartWidth, (this.ItemHeight + this.Spacing.y) * nRowIndex, fItemHeightOrWidth + this.Spacing.x, this.ItemHeight);
                this.RowItemInfoList[nRowIndex] = (nItemIndex, fStartWidth + fItemHeightOrWidth + this.Spacing.x);
            }
            return rRect;
        }

        public float GetMaxHeight()
        {
            var fMaxHeight = 0f;
            for (int i = 0; i < this.ColumnItemInfoList.Count; i++)
            {
                var (_, rItemInfo_Height) = this.ColumnItemInfoList[i];
                if (fMaxHeight == 0f || rItemInfo_Height > fMaxHeight)
                {
                    fMaxHeight = rItemInfo_Height;
                }
            }

            return fMaxHeight;
        }

        public float GetMaxWidth()
        {
            float fMaxWidth = 0f;
            for (int i = 0; i < this.RowItemInfoList.Count; i++)
            {
                var (_, rItemInfo_Width) = this.RowItemInfoList[i];
                if (fMaxWidth == 0f || rItemInfo_Width > fMaxWidth)
                {
                    fMaxWidth = rItemInfo_Width;
                }
            }
            return fMaxWidth;
        }

        private void RefreshItemInfo()
        {
            this.ItemRectList.Clear();
            for (int i = 0; i < this.ItemHeightOrWidthList.Count; i++)
            {
                var fItemHeightOrWidth = this.ItemHeightOrWidthList[i];
                var rRect = this.CalcNextItemRect(i, fItemHeightOrWidth);
                this.ItemRectList.Add(rRect);
            }

            if (this.vertical)
            {
                var fMaxHeight = this.GetMaxHeight();
                this.content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, this.content.anchoredPosition.y, fMaxHeight);
            }
            else
            {
                var fMaxWidth = this.GetMaxWidth();
                this.content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, this.content.anchoredPosition.x, fMaxWidth);
            }
        }

        public void RefreshToHeight(float fHeight)
        {
            var rViewprotHeight = this.viewport.rect.height;
            var fTopHeight = fHeight;
            var fBottomHeight = fHeight - rViewprotHeight;
            var rContent = this.content;
            var nChildCount = rContent.childCount;
            var nChildIndex = 0;
            for (int i = 0; i < this.ItemRectList.Count; i++)
            {
                var rItemRect = this.ItemRectList[i];
                if (rItemRect.yMin - rItemRect.height <= fTopHeight && rItemRect.yMin >= fBottomHeight)
                {
                    RectTransform rItemRectTransform;
                    if (nChildIndex >= nChildCount)
                    {
                        var rItemGameObject = this.PrefabSource.GetObject();
                        var rItemTransform = rItemGameObject.transform;
                        rItemTransform.SetParent(rContent.transform, false);
                        rItemGameObject.SetActive(true);
                        rItemRectTransform = rItemGameObject.GetComponent<RectTransform>();
                    }
                    else
                    {
                        var rItemTransform = rContent.GetChild(nChildIndex);
                        rItemRectTransform = rItemTransform.GetComponent<RectTransform>();
                    }
                    if (rItemRectTransform)
                    {
                        rItemRectTransform.anchoredPosition3D = rItemRect.position;
                    }
                    rItemRectTransform.gameObject.name = "item" + i;
                    UtilTool.SafeExecute(this.OnFillCellFunc, rItemRectTransform, i);
                    UtilTool.SafeExecute(this.OnFillCellFunc_Other, rItemRectTransform, i);
                    nChildIndex++;
                }
            }
            for (int i = nChildCount - 1; i >= nChildIndex; i--)
            {
                var rItemTransform = rContent.GetChild(i);
                this.PrefabSource.Free(rItemTransform.gameObject);
            }
        }

        public void RefreshToWidth(float fWidth)
        {
            var rViewprotWidth = this.viewport.rect.width;
            var fLeftWidth = fWidth;
            var fRightWidth = rViewprotWidth - fWidth;
            var rContent = this.content;
            var nChildCount = rContent.childCount;
            var nChildIndex = 0;
            for (int i = 0; i < this.ItemRectList.Count; i++)
            {
                var rItemRect = this.ItemRectList[i];
                if (-rItemRect.xMin - rItemRect.width <= fLeftWidth && rItemRect.xMin <= fRightWidth)
                {
                    RectTransform rItemRectTransform;
                    if (nChildIndex >= nChildCount)
                    {
                        var rItemGameObject = this.PrefabSource.GetObject();
                        var rItemTransform = rItemGameObject.transform;
                        rItemTransform.SetParent(rContent.transform, false);
                        rItemGameObject.SetActive(true);
                        rItemRectTransform = rItemGameObject.GetComponent<RectTransform>();
                    }
                    else
                    {
                        var rItemTransform = rContent.GetChild(nChildIndex);
                        rItemRectTransform = rItemTransform.GetComponent<RectTransform>();
                    }
                    if (rItemRectTransform)
                    {
                        rItemRectTransform.anchoredPosition3D = rItemRect.position;
                    }
                    rItemRectTransform.gameObject.name = "item" + i;
                    UtilTool.SafeExecute(this.OnFillCellFunc, rItemRectTransform, i);
                    UtilTool.SafeExecute(this.OnFillCellFunc_Other, rItemRectTransform, i);
                    nChildIndex++;
                }
            }
            for (int i = nChildCount - 1; i >= nChildIndex; i--)
            {
                var rItemTransform = rContent.GetChild(i);
                this.PrefabSource.Free(rItemTransform.gameObject);
            }
        }

        public void RefreshToIndex(int nIndex)
        {
            if (nIndex < 0 || nIndex >= this.ItemRectList.Count - 1)
            {
                return;
            }
            var rRect = this.ItemRectList[nIndex];
            if (this.vertical)
            {
                this.RefreshToHeight(rRect.yMin);
            }
            else
            {
                this.RefreshToWidth(-rRect.xMin);
            }
        }

        public void Refresh()
        {
            if (this.vertical)
            {
                this.RefreshToHeight(-this.content.anchoredPosition.y);
            }
            else
            {
                this.RefreshToWidth(this.content.anchoredPosition.x);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.onValueChanged.RemoveListener(this.OnValueChanged);
            this.onValueChanged.AddListener(this.OnValueChanged);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.onValueChanged.RemoveListener(this.OnValueChanged);
        }

        private void OnValueChanged(Vector2 rValue)
        {
            this.Refresh();
        }
    }
}