using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    public class ButtonFeedback : InteractionFeedback,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            this.Switch(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this.Switch(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.Switch(false);
        }
    }
}
