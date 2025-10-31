using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/PressClickButton")]
    public class PressClickButton : Selectable
    {
        [Serializable]
        public class ButtonClickedEvent : UnityEvent<bool, bool>
        //public class ButtonClickedEvent : UnityEvent<bool>
        {
        }
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        private Coroutine mTriggerCoroutine;
        private bool mIsPointerDown = false;
        private bool mIsTriggerLongPress = false;
        public float LongPressTime = 0;
        public ButtonClickedEvent onClick
        {
            get
            {
                return this.m_OnClick;
            }
            set
            {
                this.m_OnClick = value;
            }
        }
        private void StopTriggerCoroutine()
        {
            if (this.mTriggerCoroutine != null)
            {
                this.StopCoroutine(this.mTriggerCoroutine);
                this.mTriggerCoroutine = null;
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                this.mIsPointerDown = true;
                this.mIsTriggerLongPress = false;
                if (this.LongPressTime > 0)
                {
                    this.StopTriggerCoroutine();
                    this.mTriggerCoroutine = this.StartCoroutine(this.TriggerPress(this.LongPressTime));
                }
                else
                {
                    this.Press(true);
                }
            }
        }
        private IEnumerator TriggerPress(float fTime)
        {
            yield return new WaitForSeconds(fTime);
            this.mIsTriggerLongPress = true;
            this.Press(false);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                this.mIsPointerDown = false;
                this.StopTriggerCoroutine();
                this.Press(true);
            }
        }
        private void Press(bool bIsPress)
        {
            if (this.IsActive() && this.IsInteractable())
            {
                this.m_OnClick?.Invoke(bIsPress, this.mIsTriggerLongPress);
                //this.m_OnClick?.Invoke(bIsPress);
            }
        }
#if UNITY_EDITOR || UNITY_STANDALONE
        private void LateUpdate()
        {
            if (this.mIsPointerDown && !Input.GetMouseButton(0))
            {
                this.mIsPointerDown = false;
                this.StopTriggerCoroutine();
                this.Press(false);
            }
        }
#endif
    }
}
