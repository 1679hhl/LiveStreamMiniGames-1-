using Knight.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public interface IFadeMask
    {
        void FadeOutHide(Action rCallback = null);
    }
	public class FadeMask : MonoBehaviour, IFadeMask
	{
        protected static IFadeMask         __instance;
        public  static IFadeMask           Instance { get { return __instance; } }
        private CoroutineHandler mFadeCoroutineHandler;
        public Image mMaskImage;
        private float mFadeDuration = 0.4f;
        private Color mStartMaskColor = new Color(0, 0, 0, 0);
        private Color mEndMaskColor = new Color(0, 0, 0, 1);
        private Action mFadeOutHideCallback;
        private void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
            }
            this.gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            __instance = null;
            this.StopFadeOut();
        }

        public void FadeOutHide(Action rCallback = null)
        {
            this.gameObject.SetActive(true);
            this.mFadeOutHideCallback = rCallback;
            this.StartFadeOut();
        }

        private void StartFadeOut()
        {
            if (this.mFadeCoroutineHandler == null)
            {
                this.mFadeCoroutineHandler = CoroutineManager.Instance.StartHandler(this.FadeOutCoroutine());
            }
        }

        private IEnumerator FadeOutCoroutine()
        {
            float rElapsedTime = 0f;
            this.mMaskImage.gameObject.SetActive(true);
            Color rStartColor = this.mStartMaskColor;
            Color rEndColor = this.mEndMaskColor;
            while (rElapsedTime < this.mFadeDuration)
            {
                float rNormalizedTime = rElapsedTime / this.mFadeDuration;
                this.mMaskImage.color = Color.Lerp(rStartColor, rEndColor, rNormalizedTime);
                rElapsedTime += Time.deltaTime;
                yield return null;
            }
            this.mMaskImage.color = this.mEndMaskColor;
            this.gameObject.SetActive(false);
            if (this.mFadeOutHideCallback != null)
            {
                this.mFadeOutHideCallback();
                this.mFadeOutHideCallback = null;
            }
            this.StopFadeOut();
        }

        private void StopFadeOut()
        {
            if (this.mFadeCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mFadeCoroutineHandler);
                this.mFadeCoroutineHandler = null;
            }
        }
    }
}

