using Knight.Core;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class NestScrollRectDrag : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        public ScrollRect ParentScrollRect;
        public ScrollRect SubScrollRect;
        public float StartDragDistance = 200f;

        private Vector2 mStartPos;
        private void OnEnable()
        {
            this.SubScrollRect = this.gameObject.GetComponent<ScrollRect>();
            if (this.ParentScrollRect == null)
                this.ParentScrollRect = UtilTool.GetComponentInParent<ScrollRect>(this.gameObject);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.mStartPos = eventData.delta;
            this.ParentScrollRect?.OnBeginDrag(eventData);
            this.SubScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.SubScrollRect.horizontal)
            {
                if (this.ParentScrollRect.horizontal)
                {
                    this.SubScrollRect?.OnDrag(eventData);
                }
                else if (this.mStartPos.x - eventData.delta.x > this.StartDragDistance)
                {
                    this.SubScrollRect?.OnDrag(eventData);
                }
                else
                {
                    this.ParentScrollRect?.OnDrag(eventData);
                }
            }
            else
            {
                if (this.ParentScrollRect.vertical)
                {
                    this.SubScrollRect?.OnDrag(eventData);
                }
                else if (this.mStartPos.y - eventData.delta.x > this.StartDragDistance)
                {
                    this.SubScrollRect?.OnDrag(eventData);
                }
                else
                {
                    this.ParentScrollRect?.OnDrag(eventData);
                }
            }

        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.ParentScrollRect?.OnEndDrag(eventData);
            this.SubScrollRect?.OnEndDrag(eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.ParentScrollRect?.OnInitializePotentialDrag(eventData);
            this.SubScrollRect?.OnInitializePotentialDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            this.ParentScrollRect?.OnScroll(eventData);
            this.SubScrollRect?.OnScroll(eventData);
        }
    }
}