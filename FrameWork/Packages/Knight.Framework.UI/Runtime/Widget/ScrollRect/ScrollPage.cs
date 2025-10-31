using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityEngine.UI
{
    public class ScrollPage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        public ScrollRect               RectScroll;
        [SerializeField]
        public GridLayoutGroup          GridLayout;
        [SerializeField]
        public int                      PageCellNum = 1;
        [SerializeField]
        public int                      CurIndex;
        [SerializeField]
        public float                    MoveOffsetMini = 200f;
        [SerializeField]
        public float                    MoveOffsetMax = 500f;
        [SerializeField]
        public float                    DragEndEventAnimTime = 0.5f;
        [SerializeField]
        public int                      TotalPageNum;

        private float                   mCellSizeX;
        private float                   mSpaceX;
        private int                     mPaddingLeft;
        private Vector2                 mStartPos;
        private Vector2                 mCurPos;
        private Vector3                 mContentStartPos;
        private Vector3                 mContentCurPos;
        private Vector3                 mEndContentPos;
        private bool                    mIsFirstIn;

        [Serializable]
        public class OnPageChangeEvent : UnityEvent<int> { }

        public OnPageChangeEvent OnPageChange = new OnPageChangeEvent();

        public Action OnDragBeginEvent;
        public Action OnDragEvent;
        public Action OnDragEndEvent;

        void Start()
        {
            this.GridLayout = this.RectScroll.content.GetComponent<GridLayoutGroup>();
            this.mCellSizeX = this.GridLayout.cellSize.x;
            this.mSpaceX = this.GridLayout.spacing.x;
            this.mPaddingLeft = this.GridLayout.padding.left;
            this.TotalPageNum = this.RectScroll.content.childCount / this.PageCellNum;
            if (this.RectScroll.content.childCount % this.PageCellNum != 0)
            {
                this.TotalPageNum += 1;
            }
            this.mEndContentPos = this.RectScroll.content.localPosition;
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            this.mStartPos = eventData.position;
            this.mContentStartPos = this.mEndContentPos;
            this.OnDragBeginEvent?.Invoke();
            this.mIsFirstIn = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.OnDragEvent?.Invoke();
            this.mCurPos = eventData.position;
            var fMoveOffset = this.mCurPos.x - this.mStartPos.x;
            if (Mathf.Abs(fMoveOffset) >= this.MoveOffsetMax)
            {
                if (!((fMoveOffset > 0 && this.CurIndex <= 0) ||
                    (fMoveOffset < 0 && this.CurIndex >= this.TotalPageNum - 1)))
                {
                    return;
                }

                if (this.mIsFirstIn)
                {
                    this.mIsFirstIn = false;
                    this.mContentCurPos = this.RectScroll.content.localPosition;
                }
                this.RectScroll.content.localPosition = this.mContentCurPos;
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            this.OnDragEndEvent?.Invoke();
            var fMoveOffset = this.mStartPos.x - this.mCurPos.x;
            if (Mathf.Abs(fMoveOffset) <= this.MoveOffsetMini)
            {
                DOTween.To(() => this.RectScroll.content.localPosition,
                    (fPos) => this.RectScroll.content.localPosition = fPos,
                    this.mContentStartPos, DragEndEventAnimTime).OnComplete(() => { });//.SetEase(Ease.InSine);
                return;
            }
            if (this.mCurPos.x - this.mStartPos.x > 0)
            {
                if (this.CurIndex <= 0)
                {
                    DOTween.To(() => this.RectScroll.content.localPosition,
                   (fPos) => this.RectScroll.content.localPosition = fPos,
                   this.mContentStartPos, DragEndEventAnimTime).OnComplete(() => { });//.SetEase(Ease.InSine);
                    return;
                }
                CurIndex--;
                this.OnPageChange?.Invoke(CurIndex);
                if (CurIndex == this.TotalPageNum)
                {
                    this.mEndContentPos.x += this.mPaddingLeft;
                }
                this.mEndContentPos.x += this.PageCellNum * (this.mCellSizeX + this.mSpaceX);
            }
            else
            {
                if (this.CurIndex >= this.TotalPageNum - 1)
                {
                    DOTween.To(() => this.RectScroll.content.localPosition,
                   (fPos) => this.RectScroll.content.localPosition = fPos,
                   this.mContentStartPos, DragEndEventAnimTime).OnComplete(() => { });//.SetEase(Ease.InSine);
                    return;
                }
                CurIndex++;
                this.OnPageChange?.Invoke(CurIndex);
                this.mEndContentPos.x -= this.PageCellNum * (this.mCellSizeX + this.mSpaceX);
            }
            DOTween.To(() => this.RectScroll.content.localPosition,
                   (fPos) => this.RectScroll.content.localPosition = fPos,
                   this.mEndContentPos, DragEndEventAnimTime);

        }
    }
}
