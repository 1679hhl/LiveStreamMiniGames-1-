using Knight.Core;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game
{
    public class LoopRectSortItem : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        public RectTransform LoopScrollRectContent; //content
        public GameObject FakeItem; //点击排序时需要显示的跟随鼠标物体
        public float ScreenUpY; //列表顶部在fakeitem的anchoredPositionY值
        public float ScreenDownY; //列表底部在fakeitem的anchoredPositionY值
        public float CellRange; //两个列表项之间的距离（单个宽度 + layout间距）
        public int BiasIndex; //列表中首个列表项在content的子物体索引
        public float FakeItemXBias; //弹起物体弹起位置X偏移值
        public float FakeItemYBias; //弹起物体弹起位置Y偏移值

        private Text FakeItemText;
        private RectTransform FakeItemRect;
        private Vector3 mLastMousePos;
        private int mOriginalItemIndex;
        private bool mIsNowClicking = false;
        private PointerEventData mEventData;

        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private UnityEvent m_OnClick = new UnityEvent();
        public UnityEvent onClick
        {
            get
            {
                return this.m_OnClick;
            }
            set
            {
                this.m_OnClick = value;
            }
        }

        public string ItemText { get; set; }
        public int ItemIndex { get; set; }
        public bool IsNull { get; set; }
        public bool IsSpecial { get; set; }

        protected void Start()
        {
            this.FakeItemText = this.FakeItem.GetComponentInChildren<Text>();
            this.FakeItemRect = this.FakeItem.GetComponent<RectTransform>();
        }
        private void Update()
        {
            if (!this.FakeItem.activeInHierarchy || !this.mIsNowClicking) return;
            int nTempIndex = this.GetTempIndex();
            if (this.FakeItemRect.anchoredPosition.y >= this.ScreenUpY)
            {
                this.LoopScrollRectContent.anchoredPosition = new Vector2(this.LoopScrollRectContent.anchoredPosition.x, this.LoopScrollRectContent.anchoredPosition.y - 18);
                //超出顶部，如果当前index不是第一个，就和上一个交换一次
                if (nTempIndex > 0) this.ExchangeItems(nTempIndex, nTempIndex - 1);
            }
            else if (this.FakeItemRect.anchoredPosition.y <= this.ScreenDownY)
            {
                this.LoopScrollRectContent.anchoredPosition = new Vector2(this.LoopScrollRectContent.anchoredPosition.x, this.LoopScrollRectContent.anchoredPosition.y + 18);
                if (nTempIndex != -1 && nTempIndex < this.LoopScrollRectContent.childCount - 1) this.ExchangeItems(nTempIndex, nTempIndex + 1);
            }
#if UNITY_EDITOR || UNITY_STANDALONE
            if (this.mIsNowClicking && !Input.GetMouseButton(0)) this.OnPointerUp(this.mEventData);
#else
            if (this.mIsNowClicking && Input.touchCount == 0) this.OnPointerUp(this.mEventData);
#endif
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!this.mIsNowClicking) return;
            float fDeltaY = eventData.position.y - this.mLastMousePos.y;
            if ((this.FakeItemRect.anchoredPosition.y < this.ScreenUpY || fDeltaY < 0) && (this.FakeItemRect.anchoredPosition.y > this.ScreenDownY || fDeltaY > 0))
            {
                if (this.FakeItemRect.anchoredPosition.y + fDeltaY >= this.ScreenUpY) 
                    this.FakeItemRect.anchoredPosition = new Vector2(this.FakeItemRect.anchoredPosition.x, this.ScreenUpY);
                else if (this.FakeItemRect.anchoredPosition.y + fDeltaY <= this.ScreenDownY) 
                    this.FakeItemRect.anchoredPosition = new Vector2(this.FakeItemRect.anchoredPosition.x, this.ScreenDownY);
                else 
                    this.FakeItemRect.anchoredPosition = new Vector2(this.FakeItemRect.anchoredPosition.x, this.FakeItemRect.anchoredPosition.y + fDeltaY);
                //获取相对Index
                int nTempIndex = this.GetTempIndex();
                for(int i = 0; i < nTempIndex; i++)
                {
                    if (nTempIndex > 0 && this.FakeItemRect.anchoredPosition.y < this.LoopScrollRectContent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y + this.CellRange + this.LoopScrollRectContent.anchoredPosition.y && this.FakeItemRect.anchoredPosition.y > this.LoopScrollRectContent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y + this.LoopScrollRectContent.anchoredPosition.y)
                    {
                        for (int j = nTempIndex; j > i; j--)
                        {
                            this.ExchangeItems(j, j - 1);
                        }
                    }
                }
                for (int i = this.LoopScrollRectContent.childCount - 1; i > nTempIndex; i--)
                {
                    if (nTempIndex != -1 && nTempIndex < this.LoopScrollRectContent.childCount - 1 && this.FakeItemRect.anchoredPosition.y < this.LoopScrollRectContent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y + this.CellRange + this.LoopScrollRectContent.anchoredPosition.y && this.FakeItemRect.anchoredPosition.y > this.LoopScrollRectContent.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y + this.LoopScrollRectContent.anchoredPosition.y)
                    {
                        for (int j = nTempIndex; j < i; j++)
                        {
                            this.ExchangeItems(j, j + 1);
                        }
                    }
                }
            }
            this.mLastMousePos = eventData.position;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            this.FakeItemText.text = this.ItemText;
            this.FakeItem.SetActive(true);
            EventManager.Instance.Distribute(GameEvent.kEventSortLoopRectButtonDown, this.GetComponentInParent<DataBindingRetrive>().DataIndex, true);
            for(int i = this.BiasIndex; i < this.LoopScrollRectContent.childCount; i++)
            {
                var rChild = this.LoopScrollRectContent.GetChild(i).GetComponent<RectTransform>();
                var rChildFirst = this.LoopScrollRectContent.GetChild(this.BiasIndex).GetComponent<RectTransform>();
                var rChildDragItem = rChild.GetComponentInChildren<LoopRectSortItem>();
                if (rChildDragItem != null && rChildDragItem.ItemIndex == this.ItemIndex)
                    this.FakeItemRect.anchoredPosition = new Vector2(rChild.anchoredPosition.x + this.FakeItemXBias, rChild.anchoredPosition.y - rChildFirst.anchoredPosition.y + this.LoopScrollRectContent.anchoredPosition.y + this.FakeItemYBias);
            }
            this.mLastMousePos = eventData.position;
            this.mOriginalItemIndex = this.ItemIndex;
            this.mIsNowClicking = true;
            this.mEventData = eventData;
            this.onClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.FakeItem.SetActive(false);
            for (int i = 0; i < this.LoopScrollRectContent.childCount; i++)
            {
                var rChild = this.LoopScrollRectContent.GetChild(i);
                var rChildDragItem = rChild.GetComponentInChildren<LoopRectSortItem>();
                if (rChildDragItem != null && rChildDragItem.ItemIndex == this.mOriginalItemIndex)
                {
                    EventManager.Instance.Distribute(GameEvent.kEventSortLoopRectButtonDown, rChild.GetComponentInParent<DataBindingRetrive>().DataIndex, false);
                }
            }
            this.mIsNowClicking = false;
        }
        private void ExchangeItems(int nThisIndex, int nOtherIndex)
        {
            if (nOtherIndex < 0 || nOtherIndex >= this.LoopScrollRectContent.childCount) return;
            var rThisTrans = this.LoopScrollRectContent.GetChild(nThisIndex);
            var rOtherTrans = this.LoopScrollRectContent.GetChild(nOtherIndex);
            var rOther = rOtherTrans.GetComponentInChildren<LoopRectSortItem>();
            if (rOther == null || rOther.IsNull || rOther.IsSpecial) return;
            EventManager.Instance.Distribute(GameEvent.kEventExchangeQuickBroadcastItem, rThisTrans.GetComponent<DataBindingRetrive>().DataIndex, rOtherTrans.GetComponent<DataBindingRetrive>().DataIndex);
        }
        private int GetTempIndex()
        {
            for(int i = 0; i < this.LoopScrollRectContent.childCount; i++)
            {
                var rChild = this.LoopScrollRectContent.GetChild(i);
                var rChildDragItem = rChild.GetComponentInChildren<LoopRectSortItem>();
                if (rChildDragItem != null && rChildDragItem.ItemIndex == this.mOriginalItemIndex)
                {
                    return rChild.GetComponent<DataBindingRetrive>().DataIndex - this.LoopScrollRectContent.GetChild(this.BiasIndex).GetComponent<DataBindingRetrive>().DataIndex + this.BiasIndex;
                }
            }
            return -1;
        }
    }
}