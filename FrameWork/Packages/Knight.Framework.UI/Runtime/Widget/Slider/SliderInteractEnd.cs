using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace UnityEngine.UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderInteractEnd :MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public class OnInteractEndEvent : UnityEvent<float>
        {
        }


        public OnInteractEndEvent OnInteractEnd = new OnInteractEndEvent();
        public Slider Slider;
        private void Awake()
        {
            this.Slider = this.GetComponent<Slider>();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (this.Slider == null) return;
            this.OnInteractEnd?.Invoke(this.Slider.value);
        }
        public void OnPointerDown(PointerEventData eventData)
        {

        }
    }
}

