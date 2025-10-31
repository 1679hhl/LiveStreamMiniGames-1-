using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler,IEndDragHandler
    {
        public class OnDropEvent : UnityEvent
        {
        }

        public Action       OnDragBeginEvent;
        public Action       OnDragEvent;
        public Action       OnDragEndEvent;

        public OnDropEvent  OnDropHandler = new OnDropEvent();

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.OnDragBeginEvent?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.OnDragEvent?.Invoke();
        }

        public void OnDrop(PointerEventData eventData)
        {
            this.OnDropHandler?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.OnDragEndEvent?.Invoke();
        }
    }
}
