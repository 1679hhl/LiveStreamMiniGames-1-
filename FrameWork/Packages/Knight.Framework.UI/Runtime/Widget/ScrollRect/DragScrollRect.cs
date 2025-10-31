using Knight.Core;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragScrollRect : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        public ScrollRect ScrollRect;
        public Button Button;
        public DoubleClickButton DoubleClickButton;
        private void OnEnable()
        {
            if (this.ScrollRect == null)
                this.ScrollRect = UtilTool.GetComponentInParent<ScrollRect>(this.gameObject);
            if (this.Button == null)
                this.Button = this.gameObject.GetComponent<Button>();
            if (this.DoubleClickButton == null)
                this.DoubleClickButton = this.gameObject.GetComponent<DoubleClickButton>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.Button)
            {
                this.Button.enabled = false;
            }
            if (this.DoubleClickButton)
            {
                this.DoubleClickButton.enabled = false;
            }
            this.ScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.ScrollRect?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.Button)
            {
                this.Button.enabled = true;
            }
            if (this.DoubleClickButton)
            {
                this.DoubleClickButton.enabled = true;
            }
            this.ScrollRect?.OnEndDrag(eventData);
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            this.ScrollRect?.OnInitializePotentialDrag(eventData);
        }

        public void OnScroll(PointerEventData eventData)
        {
            this.ScrollRect?.OnScroll(eventData);
        }
    }
}