using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class ImageSlider : MonoBehaviour
    {
        public Image Slider;
        public float SliderAnimTime = 2.0f;
        // Start is called before the first frame update
        private float mStartValue;
        private float mCurValue;
        private float mEndValue;

        private int mStartCount;
        private int mCurCount;
        private int mEndCount;
        private bool mIsEffectEnd;
        [SerializeField]
        public float StartValue
        {
            get { return mStartValue; }
            set
            {
                mStartValue = value;
                this.Slider.fillAmount = StartValue;
            }
        }
        [SerializeField]
        public int StartCount
        {
            get { return mStartCount; }
            set
            {
                mStartCount = value;
            }
        }
        [SerializeField]
        public int EndCount
        {
            get { return mEndCount; }
            set
            {
                mEndCount = value;
            }
        }
        [SerializeField]
        public int CurCount
        {
            get { return mCurCount; }
            set
            {
                mCurCount = value;
            }
        }
        [SerializeField]
        public bool IsEffectEnd
        {
            get { return mIsEffectEnd; }
            set
            {
                mIsEffectEnd = value;
               
            }
        }
        [SerializeField]
        public float EndValue
        {
            get { return mEndValue; }
            set
            {
                mEndValue = value + this.mEndCount - this.mStartCount;
                this.IsEffectEnd = false;
                DOTween.To(() => this.Slider.fillAmount,
                    (fCurValue) => {
                        this.mCurValue = fCurValue;
                    }, this.mEndValue, this.SliderAnimTime).OnComplete(() => { this.IsEffectEnd = true; }).SetEase(Ease.InSine);
            }
        }
        private void Awake()
        {
            if (!this.Slider)
            {
                this.Slider = this.GetComponent<Image>();
            }
            this.StartCount = 0;
        }
        private void Update()
        {
            if (!this.Slider)return;
            if (this.IsEffectEnd == true)return;

            this.Slider.fillAmount = this.mCurValue % 1.0f;
            var nCurCount = this.StartCount + (int)(this.mCurValue / 1.0f);
            if (nCurCount <= this.CurCount)
            {
                return;
            }
            this.CurCount = nCurCount;
        }
    }
}
