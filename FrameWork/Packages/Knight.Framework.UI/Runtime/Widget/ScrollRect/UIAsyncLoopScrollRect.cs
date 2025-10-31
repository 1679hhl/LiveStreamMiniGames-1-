using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Knight.Core;

namespace UnityEngine.UI
{/// <summary>
 /// 同步两个LoopScorllRect
 /// </summary>
    public class UIAsyncLoopScrollRect : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public LoopScrollRect ParentScrollRect;
        public LoopScrollRect SubScrollRect;
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.SubScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.SubScrollRect?.OnDrag(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.SubScrollRect)
            {
                this.SubScrollRect.OnEndDrag(eventData);
            }
        }
    }
}