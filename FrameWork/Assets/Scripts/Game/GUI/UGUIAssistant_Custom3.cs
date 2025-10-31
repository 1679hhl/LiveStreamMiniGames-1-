using System;
using Knight.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UGUIAssistant_Custom3 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        public GameObject TGameObject;
        public ulong EventID;
        public int nIndex { get; set; }
        public void OnPointerEnter(PointerEventData eventData)
        {
            /*if(this.TGameObject!=null)
                this.TGameObject.SetActive(true);*/
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            /*if(this.TGameObject!=null)
                this.TGameObject.SetActive(false);*/
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.Instance.Distribute(EventID,this.nIndex);
        }
    }
}