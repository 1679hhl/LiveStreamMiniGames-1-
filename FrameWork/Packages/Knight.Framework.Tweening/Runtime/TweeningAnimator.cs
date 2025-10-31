using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Framework.Tweening
{
    public enum TweeningActionType
    {
        None,
        Position,
        LocalPosition,
        Rotate,
        LocalRotate,
        LocalScale,
        Color,
        CanvasAlpha,
        Delay,
        ImageAmount,
        RectTransPos,
        RectTransPosX,
        RectTransPosY,
    }

    [System.Serializable]
    public class TweeningAction
    {
        public bool                 IsEnable = false;
        public bool                 IsFold = false;

        public bool                 IsLoop = false;
        public LoopType             LoopType;
        public int                  LoopCount = -1;

        public float                Duration = 0.35f;
        public AnimationCurve       TimeCurve = AnimationCurve.EaseInOut(0 , 0 , 1 , 1);

        public TweeningActionType   Type;

        public float                StartF = 0.0f;
        public float                EndF = 0.0f;

        public Vector3              StartV3 = Vector3.zero;
        public Vector3              EndV3 = Vector3.zero;

        public Color                StartCol = Color.white;
        public Color                EndCol = Color.white;

        public Vector3              DefultVector3;
        public float                DefultFloat;
        public Color                DefultColor;
        
        public Action               SetDefultValue;
        public Action               SetPlayStartValue;
        public Tweener              Tweener;
    }
    
    [AddComponentMenu("UI Animation/TweeningAnimator")]
    public class TweeningAnimator : MonoBehaviour
    {
        public bool                 IsIgnoreTimeScale;
        public bool                 IsUseFixedUpdate;
        public bool                 IsAutoExecute = true;
        public bool                 IsLoopAnimator;

        public List<TweeningAction> Actions;

        private int                 mAnimationCount;

        private bool                mIsPlay;

        public bool                 IsPlay
        {
            get { return this.mIsPlay; }
            set {
                this.mIsPlay = value;
                if(this.mIsPlay)
                {
                    this.Play();
                }
                else
                {
                    this.Pause();
                }
            }
        }

        private void Awake()
        {
            this.Create();
        }

        private void OnDestroy()
        {
            this.Stop();
        }

        private void OnEnable()
        {
            this.Play();
        }

        private void OnDisable()
        {
            this.Pause();
        }

        public void Create()
        {
            this.mAnimationCount = 0;
            for (int i = 0; i < this.Actions.Count; i++)
            {
                this.Actions[i].SetDefultValue?.Invoke();
                TweeningAnimationFactory.CreateTweenBehaviour(this.Actions[i] , this.gameObject);
                this.SetUpTweener(this.Actions[i]);
                if (this.Actions[i].Tweener != null)
                {
                    this.Actions[i].Tweener.onComplete = this.nextAnimation;
                }
                this.Actions[i].SetDefultValue?.Invoke();
            }
            if (this.IsAutoExecute)
            {
                 this.Play();
            }
        }

        public void Stop()
        {
            if (this.Actions == null) return;
            for (int i = 0; i < this.Actions.Count; i++)
            {
                if (this.Actions[i].Tweener == null) return;
                this.Actions[i].Tweener.Kill(false);
            }
            this.Actions.Clear();
            this.mAnimationCount = 0;
        }

        public void Play()
        {
            if (this.Actions == null || this.Actions.Count == 0) return;
            for (int i = 0; i < this.Actions.Count; i++)
            {
                this.Actions[i].Tweener.Restart();
            }
            this.Actions[this.mAnimationCount].Tweener.Play();
        }

        public void Pause()
        {
            if (this.Actions == null) return;
            for (int i = 0; i < this.Actions.Count; i++)
            {
                if (this.Actions[i].Tweener == null) return;
                this.Actions[i].Tweener.Pause();
            }
            this.mAnimationCount = 0;
        }

        private void SetUpTweener(TweeningAction rTweenAction)
        {
            if (rTweenAction.Tweener == null) return;

            // 先暂停
            rTweenAction.Tweener.Pause();
            rTweenAction.Tweener.SetUpdate(this.IsUseFixedUpdate ? UpdateType.Fixed : UpdateType.Normal, !this.IsIgnoreTimeScale);
            rTweenAction.Tweener.timeScale = this.IsIgnoreTimeScale ? Time.timeScale : 1;
            rTweenAction.Tweener.SetEase(rTweenAction.TimeCurve);
            // 是否循环
            if (rTweenAction.IsLoop)
            {
                rTweenAction.Tweener.SetLoops(rTweenAction.LoopCount, rTweenAction.LoopType);
            }
        }

        private void nextAnimation()
        {
            this.mAnimationCount++;
            if (this.mAnimationCount == this.Actions.Count)
            {
                this.mAnimationCount = 0;
                if (this.IsLoopAnimator)
                {
                    this.Play();
                }
            }
            else if (this.Actions[this.mAnimationCount].Tweener != null)
            {
                this.Actions[this.mAnimationCount].SetPlayStartValue?.Invoke();
                this.Actions[this.mAnimationCount].Tweener.Play();
            }
            else
            {
                this.nextAnimation();
            }
        }
    }
}
