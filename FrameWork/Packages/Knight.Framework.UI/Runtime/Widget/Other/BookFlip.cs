using Knight.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityEngine.UI
{
    public enum SetBookFilpType
    {
        LeftPage,
        RightPage
    }

    public class BookFlip : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICancelHandler
    {
        public SetBookFilpType SetBookFilpType;
        public float DragLimit;//超过范围翻书

        private float mPageWidth;
        public float PageWidth
        {
            get
            {
                if (this.mPageWidth == 0)
                {
                    this.mPageWidth = this.GetComponent<RectTransform>().rect.width;
                }
                return this.mPageWidth;
            }
        }

        private Vector2 mStartPos;

        private Vector2 mEmptyPos;

        private float MoveDistance = 0;


        private float MaxSpeed = 20f;

        private float PerDistance;
        public class BookFlipEndEvent : UnityEvent<bool>
        {

        }

        public class BookFlipingEvent : UnityEvent<float>
        {
        }
        public BookFlipEndEvent OnBookFlipFunc = new BookFlipEndEvent();

        public BookFlipingEvent OnBookFlipingFunc = new BookFlipingEvent();

        public UnityEvent OnBookFlipStartFunc;
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.MoveDistance = 0;
            this.mStartPos = eventData.position;
            this.mEmptyPos = this.mStartPos;

            this.OnBookFlipStartFunc?.Invoke();

        }

        public void OnDrag(PointerEventData eventData)
        {
            this.PerDistance = eventData.position.x - this.mEmptyPos.x;
            this.mEmptyPos = eventData.position;
            var fMoveDistance = eventData.position.x - this.mStartPos.x;
            if (Mathf.Abs(this.PerDistance) > this.MaxSpeed)
            {
                if (this.PerDistance > 0)
                {
                    this.PerDistance = this.MaxSpeed;
                }
                else
                {
                    this.PerDistance = -this.MaxSpeed;
                }
            }
            this.MoveDistance += this.PerDistance;

            if (this.SetBookFilpType == SetBookFilpType.LeftPage && this.MoveDistance > 0)
            {
                this.OnBookFlipingFunc?.Invoke(Mathf.Abs(this.MoveDistance));
            }
            else if (this.SetBookFilpType == SetBookFilpType.RightPage && this.MoveDistance < 0)
            {
                this.OnBookFlipingFunc?.Invoke(Mathf.Abs(this.MoveDistance));
            }

        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.SetBookFilpType == SetBookFilpType.LeftPage)
            {

                if (this.PerDistance > 0)
                {
                    LogManager.Log("左翻--正向");
                    this.OnBookFlipFunc?.Invoke(false);
                }
                else
                {
                    LogManager.Log("左翻--反向");
                    this.OnBookFlipFunc?.Invoke(true);
                }

            }
            else
            {
                if (this.PerDistance < 0)
                {
                    LogManager.Log("右翻--正向");
                    this.OnBookFlipFunc?.Invoke(false);
                }
                else
                {
                    LogManager.Log("右翻--反向");
                    this.OnBookFlipFunc?.Invoke(true);
                }

            }
        }
        public void OnCancel(BaseEventData eventData)
        {
            if (this.SetBookFilpType == SetBookFilpType.LeftPage)
            {
                LogManager.LogError("取消左翻--反向");
                this.OnBookFlipFunc?.Invoke(true);

            }
            else
            {
                LogManager.LogError("取消右翻--反向");
                this.OnBookFlipFunc?.Invoke(true);

            }
        }
    }
}
