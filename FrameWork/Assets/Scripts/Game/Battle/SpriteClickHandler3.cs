using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpriteClickHandler3 : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Action OnEnterAct;
    public Action OnExitAct;
    public Action OnClickAct;
    
    public void OnPointerClick(PointerEventData eventData)
    {
       this.OnClickAct?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       this.OnEnterAct?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      this.OnExitAct?.Invoke();
    }
}
