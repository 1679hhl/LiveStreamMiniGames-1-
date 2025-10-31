using Knight.Core;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class DragNestScrollRect : MonoBehaviour, IInitializePotentialDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollHandler
    {
        public UINestedLoopScrollRect NestedScrollRect;
        public PressButton PressButton;
        private void OnEnable()
        {
            if (this.NestedScrollRect == null)
                this.NestedScrollRect = UtilTool.GetComponentInParent<UINestedLoopScrollRect>(this.gameObject);
            if (this.PressButton == null)
                this.PressButton = this.gameObject.GetComponent<PressButton>();
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (this.PressButton && this.PressButton.enabled)
            {
                this.PressButton.onClick?.Invoke(false, false);
                this.PressButton.enabled = false;
            }
            this.NestedScrollRect?.OnBeginDrag(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            this.NestedScrollRect?.OnDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.NestedScrollRect?.OnEndDrag(eventData);
            if (this.PressButton)
            {
                this.PressButton.enabled = true;
            }
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {

        }

        public void OnScroll(PointerEventData eventData)
        {

        }
    }
}