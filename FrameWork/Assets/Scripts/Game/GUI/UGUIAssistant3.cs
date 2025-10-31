using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UGUIAssistant3 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        public Action OnEnterAct;
        public Action OnExitAct;
        public Action OnClickAct;
        public GameObject List2;
        /*public AnchorBoxRecordView AnchorBoxRecordView;*/
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            this.OnEnterAct?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.OnExitAct?.Invoke();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            this.List2.SetActive(true);
            
            this.OnClickAct?.Invoke();
        }
        
    }
}

