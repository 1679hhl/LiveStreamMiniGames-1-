using Knight.Core;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragLoopScrollRect : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        public LoopScrollRect LoopScrollRect;
        public Button Button;
        private void OnEnable()
        {
            if (this.LoopScrollRect == null)
                this.LoopScrollRect = UtilTool.GetComponentInParent<LoopScrollRect>(this.gameObject);
            if (this.Button == null)
                this.Button = this.gameObject.GetComponent<Button>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.Button)
            {
                this.Button.enabled = false;
            }
            this.LoopScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.LoopScrollRect?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.Button)
            {
                this.Button.enabled = true;
            }
            this.LoopScrollRect?.OnEndDrag(eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.LoopScrollRect?.OnInitializePotentialDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            this.LoopScrollRect?.OnScroll(eventData);
        }
    }
}