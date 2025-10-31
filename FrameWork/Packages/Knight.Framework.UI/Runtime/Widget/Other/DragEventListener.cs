using Knight.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public enum DragType
    {
        Begin = 0,
        On = 1,
        End = 2,
        Clicked = 3
    }
    public class DragEventListener : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public class OnDragEvent : UnityEvent<int>
        {
        }


        public OnDragEvent OnDragEventFunc = new OnDragEvent();

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.OnDragEventFunc?.Invoke((int)DragType.Begin);
        }
        public void OnDrag(PointerEventData eventData)
        {
            this.OnDragEventFunc?.Invoke((int)DragType.On);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            this.OnDragEventFunc?.Invoke((int)DragType.End);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.OnDragEventFunc?.Invoke((int)DragType.Clicked);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }
    }
}
