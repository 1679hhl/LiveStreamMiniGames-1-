using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class WidthExtend : MonoBehaviour
    {

        public RectTransform RectTransform;

        public int MaxHight;

        public CanvasGroup CanvasGroup;

        private float mTimer;

        

        private float fLocalPosY;
        private bool IsReset;
        // Start is called before the first frame update
        void Start()
        {
            this.RectTransform = this.GetComponent<RectTransform>();
            this.CanvasGroup = this.GetComponent<CanvasGroup>();

        }

        private void OnEnable()
        {
            this.IsReset = false;
            if(this.CanvasGroup)
                this.CanvasGroup.alpha = 0;
            this.mTimer = 0;
        }

        void Update()
        {
            if (this.IsReset) return;
            if (this.RectTransform.rect.height >= this.MaxHight)
            {
                this.IsReset = true;
                if (this.CanvasGroup)
                    this.CanvasGroup.alpha = 1;
                var fOffset = this.RectTransform.rect.height - this.MaxHight;
                if (this.RectTransform.pivot.y  == 0)
                {
                    this.fLocalPosY = this.RectTransform.anchoredPosition.y - fOffset;
                }
                else
                {
                    this.fLocalPosY = this.RectTransform.anchoredPosition.y + fOffset;
                }
                this.RectTransform.anchoredPosition = new Vector2(this.RectTransform.anchoredPosition.x, this.fLocalPosY);
            }

            this.mTimer+= Time.deltaTime;
            if(this.mTimer >= 0.1f)
            {
                this.IsReset = true;
                this.CanvasGroup.alpha = 1.0f;
                this.mTimer = 0;

            }
        }
    }


}
