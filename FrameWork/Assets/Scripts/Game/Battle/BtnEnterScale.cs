using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class BtnEnterScale : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
    {
        private Tween mScaleTween;
        void Start()
        {
            this.gameObject.transform.localScale = Vector3.one;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            UIRoot.Instance.MainCamera.GetComponent<Raycast2DClick>().enabled = false;
            this.mScaleTween?.Kill();
            this.mScaleTween = this.gameObject.transform.DOScale(Vector3.one * 1.3f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIRoot.Instance.MainCamera.GetComponent<Raycast2DClick>().enabled = true;
            this.mScaleTween?.Kill();
            this.mScaleTween = this.gameObject.transform.DOScale(Vector3.one, 0.3f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.mScaleTween?.Kill();
            this.mScaleTween = this.gameObject.transform.DOScale(Vector3.one, 0.3f);
        }

        private void OnDestroy()
        {
            this.mScaleTween?.Kill();
        }
    }
}

