using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Knight.Core;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Zoom Out List Vertical", 52)]
    [DisallowMultipleComponent]
    public class ZoomOutListVertical : LoopVerticalScrollRect
    {
        public int ZoomOutDistance;
        public int ScrollHeightPerSecond;

        //parameters: (selected control, item index)
        //as same as OnFillCellFunc
        public Action<Transform, int> OnSelectChangedFunc;

        struct ZoomItemInfo
        {
            public RectTransform rect;
            public AnimationState state;
            public Animation animation;
            public int itemIdx;
        }

        private List<ZoomItemInfo> m_ZoomItems = new List<ZoomItemInfo>();
        private Vector3[] m_WorldCorners = new Vector3[4];
        private int m_CurrentSel = -1;
        private ZoomVerticalLayoutGroup m_ContentLayoutGroup;

        public override void ClearCells()
        {
            base.ClearCells();
            this.m_CurrentSel = -1;
        }

        protected override Vector2 GetVector(float value)
        {
            if (m_ContentLayoutGroup != null)
            {
                value += m_ContentLayoutGroup.spacing;
            }
            return base.GetVector(value);
        }

        protected override void SetAsFirstSibling(Transform t)
        {
            //sibling control is in LateUpdate
        }

        protected override void SetAsLastSibling(Transform t)
        {
            //sibling control is in LateUpdate
        }

        protected override int GetContentChildCount()
        {
            return m_ZoomItems.Count;
        }

        protected override RectTransform GetContentChild(int index)
        {
            return m_ZoomItems[index].rect;
        }

        protected override Vector2 GetContentMaxMargin()
        {
            return new Vector2(0, this.viewRect.rect.height / 2);
        }

        protected override void OnNewItem(Transform item, int itemIdx)
        {
            base.OnNewItem(item, itemIdx);

            var anim = item.GetComponent<Animation>();
            if (anim != null && anim.clip != null)
            {
                //setup animation.
                anim.Play();
                anim.clip.wrapMode = WrapMode.ClampForever;
                var state = anim[anim.clip.name];
                state.normalizedTime = 0;

                //initialize new item.
                var zoomAnim = new ZoomItemInfo();
                zoomAnim.rect = item.GetComponent<RectTransform>();
                zoomAnim.state = state;
                zoomAnim.animation = anim;
                zoomAnim.itemIdx = itemIdx;

                int insertPos = 0;
                for (int i = 0; i < this.m_ZoomItems.Count; i++)
                {
                    if (this.m_ZoomItems[i].itemIdx > itemIdx)
                    {
                        break;
                    }
                    insertPos++;
                }
                this.m_ZoomItems.Insert(insertPos, zoomAnim);
            }

            var handler = item.gameObject.ReceiveComponent<ListItemClickHandler>();
            handler.itemIndex = itemIdx;
            handler.OnListItemClicked = this.OnListItemClicked;
        }

        protected override void OnDeleteItem(Transform item)
        {
            var rect = item.GetComponent<RectTransform>();
            this.m_ZoomItems.RemoveAll((pair) => { return pair.rect == rect; });

            base.OnDeleteItem(item);
        }

        protected override void Awake()
        {
            base.Awake();

            if (content != null)
            {
                this.m_ContentLayoutGroup = content.GetComponent<ZoomVerticalLayoutGroup>();
                if (this.m_ContentLayoutGroup)
                {
                    this.m_ContentLayoutGroup.OnGetChildCount = this.GetContentChildCount;
                    this.m_ContentLayoutGroup.OnGetChild = this.GetContentChild;
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            this.RefillCells();
        }

        protected override void Update()
        {
            base.Update();

            this.viewRect.GetWorldCorners(this.m_WorldCorners);
            var centerY = (this.m_WorldCorners[1].y + this.m_WorldCorners[0].y) / 2;
            var halfHeight = (this.m_WorldCorners[1].y - this.m_WorldCorners[0].y) / 2;
            var zoomReferPoint = centerY - this.ZoomOutDistance;

            float minDistance = float.MaxValue;
            int minDistanceIdx = -1;

            for (int idx = 0; idx < this.m_ZoomItems.Count; idx++)
            {
                var item = this.m_ZoomItems[idx];

                //determine animation point according to the distance from zoom-out point.
                item.rect.GetWorldCorners(this.m_WorldCorners);
                var itemPoint = (this.m_WorldCorners[1].y + this.m_WorldCorners[0].y) / 2;
                var distance = Mathf.Abs(itemPoint - zoomReferPoint);
                item.state.normalizedTime = Mathf.Lerp(0, 1, distance / halfHeight);
                item.animation.Sample();

                //find the item of minimum distance.
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minDistanceIdx = idx;
                }
            }

            if (minDistanceIdx != -1)
            {
                var zoomOutItem = this.m_ZoomItems[minDistanceIdx];

                //trigger selection changed event.
                int newSel = zoomOutItem.itemIdx;
                if (newSel != this.m_CurrentSel)
                {
                    this.m_CurrentSel = newSel;

                    var rect = zoomOutItem.rect;
                    UtilTool.SafeExecute(this.OnSelectChangedFunc, rect, newSel);
                }

                //set draw-order, selected item should be upper-most.
                if (this.m_ContentLayoutGroup != null)
                {
                    int siblingIdx = 0;
                    for (int idx = 0; idx < minDistanceIdx; idx++)
                    {
                        var item = this.m_ZoomItems[idx];
                        this.SetNewSiblingIndex(item, siblingIdx);
                        siblingIdx++;
                    }

                    for (int idx = this.m_ZoomItems.Count - 1; idx > minDistanceIdx; idx--)
                    {
                        var item = this.m_ZoomItems[idx];
                        this.SetNewSiblingIndex(item, siblingIdx);
                        siblingIdx++;
                    }

                    this.SetNewSiblingIndex(zoomOutItem, siblingIdx);
                }
            }
        }

        private void SetNewSiblingIndex(ZoomItemInfo item, int newIdx)
        {
            var currentIdx = item.rect.GetSiblingIndex();
            if (currentIdx != newIdx)
            {
                item.rect.SetSiblingIndex(newIdx);
            }
        }

        private void OnListItemClicked(int itemIndex)
        {
            ScrollToCell(itemIndex, this.ScrollHeightPerSecond);
        }
    }

    class ListItemClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public int itemIndex { get; set; }
        public Action<int> OnListItemClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.OnListItemClicked != null)
            {
                this.OnListItemClicked(this.itemIndex);
            }
        }
    }
}