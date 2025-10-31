using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Loop Vertical Scroll Rect", 51)]
    [DisallowMultipleComponent]
    public class LoopVerticalScrollRect : LoopScrollRect
    {
        public Action<float> OnDragEvent;
        public Action<int, RectTransform> OnUpdateEvent;
        public Action<RectTransform, float> OnOutOfView;
        public bool IsNotifyOutOfView;

        private Dictionary<RectTransform, float> m_PositionCheckList = new Dictionary<RectTransform, float>();

        protected VerticalLayoutGroup m_VerticalLayout = null;

        public Action<Transform, int> OnNewItemEvent;

        public Action OnBeginDragEvent;

        public Action OnEndDragEvent;

        public Vector3 ExpandViewBounds;
        protected override float GetSize(RectTransform item)
        {
            float size = contentSpacing;
            if(this.EnableCellSizeCustom)
            {
                size += this.CellSize.y;
            }
            else if (m_GridLayout != null)
            {
                size += m_GridLayout.cellSize.y;
            }
            else
            {
                size += item.rect.height;
            }
            return size;
        }

        protected override float GetDimension(Vector2 vector)
        {
            m_VerticalLayout = content.GetComponent<VerticalLayoutGroup>();
            if (m_VerticalLayout != null)
            {
                return m_VerticalLayout.spacing;
            }
            return vector.y;
        }

        protected override void OnNewItem(Transform item, int itemIdx)
        {
            base.OnNewItem(item, itemIdx);
            this.OnNewItemEvent?.Invoke(item, itemIdx);
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(0, value);
        }

        protected override void Awake()
        {
            base.Awake();
            directionSign = -1;

            GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
            if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
            {
                Knight.Core.LogManager.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            if (ExpandViewBounds != Vector3.zero)
            {
                viewBounds.Expand(ExpandViewBounds);
            }
            bool changed = false;
            if (viewBounds.min.y < contentBounds.min.y)
            {
                float size = NewItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.min.y < contentBounds.min.y - totalSize)
                {
                    size = NewItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.min.y > contentBounds.min.y + threshold)
            {
                float size = DeleteItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.min.y > contentBounds.min.y + threshold + totalSize)
                {
                    size = DeleteItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.max.y > contentBounds.max.y)
            {
                float size = NewItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y > contentBounds.max.y + totalSize)
                {
                    size = NewItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.max.y < contentBounds.max.y - threshold)
            {
                float size = DeleteItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y < contentBounds.max.y - threshold - totalSize)
                {
                    size = DeleteItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            return changed;
        }

        private Bounds GetItemBounds(RectTransform item)
        {
            Vector3[] corners = new Vector3[4];
            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = viewRect.worldToLocalMatrix;
            item.GetWorldCorners(corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        protected override void Update()
        {
            if (!this.mUpdateEnable)
                return;
            base.Update();

            if (this.IsNotifyOutOfView)
            {
                this.NotifyItemsPosition(this.m_ViewBounds);
            }
            this.OnDragEvent?.Invoke(content.anchoredPosition.y);
            this.OnUpdateEvent?.Invoke(this.ItemTypeEnd, this.GetContentChild(this.GetContentChildCount() - 1));
        }

        private void NotifyItemsPosition(Bounds viewBounds)
        {
            if (this.OnOutOfView == null)
            {
                return;
            }

            for (int idx = 0; idx < this.GetContentChildCount(); idx++)
            {
                var item = this.GetContentChild(idx);

                bool isPosChanged = true;
                if (!m_PositionCheckList.ContainsKey(item))
                {
                    this.m_PositionCheckList.Add(item, item.position.y);
                }
                else
                {
                    float lastY = m_PositionCheckList[item];
                    if (Mathf.Abs(lastY - item.position.y) > Mathf.Epsilon)
                    {
                        m_PositionCheckList[item] = item.position.y;
                    }
                    else
                    {
                        isPosChanged = false;
                    }
                }

                if (!isPosChanged)
                {
                    continue;
                }

                var bounds = this.GetItemBounds(item);

                float rate = 0.0f;
                if (bounds.max.y > viewBounds.max.y)
                {
                    rate = Mathf.Min(1.0f, (bounds.max.y - viewBounds.max.y) / bounds.size.y);
                }
                else if (bounds.min.y < viewBounds.min.y)
                {
                    rate = Mathf.Min(1.0f, (viewBounds.min.y - bounds.min.y) / bounds.size.y);
                }

                this.OnOutOfView(item, rate);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            this.OnBeginDragEvent?.Invoke();
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            this.OnEndDragEvent?.Invoke();
        }
    }
}