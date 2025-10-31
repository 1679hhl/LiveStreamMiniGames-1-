using System;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class UGUIAssistant_Custom2 : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        public GameObject TGameObject;
        public ulong EventID;
        public int nIndex { get; set; }
        private bool IsTouch;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // 监听鼠标左键点击
            {
                if (TGameObject != null && !IsClickOnUI(TGameObject))
                {
                    TGameObject.SetActive(false);
                }
            }
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (this.TGameObject != null && this.TGameObject.name != "BoxContent" && this.TGameObject.name != "storexiangqing")
            {
                this.TGameObject.SetActive(true);
                if ((this.TGameObject.name=="special" ||this.TGameObject.name=="FishBowl")  && !IsTouch)
                {
                    IsTouch = true;
                    // 获取鼠标在屏幕上的位置
                    Vector2 screenPoint = eventData.position;
                    var targetRectTransform = TGameObject.GetComponent<RectTransform>();
                    // 转换为 UI 的 anchoredPosition
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        UIRoot.Instance.PopupRoot.GetComponent<RectTransform>(),  // 转换目标 UI 元素
                        screenPoint,          // 鼠标屏幕坐标
                        UIRoot.Instance.UICamera, // 适用于 Screen Space - Camera 或 World
                        out localPoint        // 输出本地坐标
                    );
                    targetRectTransform.anchoredPosition = localPoint;
                    EventManager.Instance.Distribute(EventID,this.nIndex);
                }
                
            }

            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(this.TGameObject!=null && this.TGameObject.name!="BoxContent" && this.TGameObject.name!="storexiangqing")
                this.TGameObject.SetActive(false);
            IsTouch = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.Instance.Distribute(EventID,this.nIndex);
            if (this.TGameObject.name=="BoxContent")
            {
                this.TGameObject.SetActive(true);
            }
            if (this.TGameObject.name=="storexiangqing")
            {
                this.TGameObject.SetActive(true);
            }
        }
        

        // 判断鼠标是否点击到了指定 UI 及其子对象
        private bool IsClickOnUI(GameObject target)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
                {
                    return true; // 点击在指定 UI 上
                }
            }
            return false; // 没有点击在指定 UI 上
        }
    }
}