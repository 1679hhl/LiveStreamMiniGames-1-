using FancyScrollView;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class FancyBindingCell : FancyCell<FancyBindingItemData>
    {
        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        protected float mCurrentPosition = 0;

        public Animator Animator;
        public bool ChangeCanvasLayer;
        [ShowIf("ChangeCanvasLayer")]
        public int BeginCanvasLayer;
        [ShowIf("ChangeCanvasLayer")]
        public int MidCanvasLayer;
        [ShowIf("ChangeCanvasLayer")]
        public int EndCanvaslayer;
        [ShowIf("ChangeCanvasLayer")]
        public Canvas TargetCanvas;

        void OnEnable()
        {
            this.UpdatePosition(this.mCurrentPosition);
        }
        public override void UpdateContent(FancyBindingItemData rItemData)
        {
        }

        public override void UpdatePosition(float fPosition)
        {
            this.mCurrentPosition = fPosition;
            if (this.Animator.isActiveAndEnabled)
            {
                this.Animator.Play(AnimatorHash.Scroll, -1, fPosition);
            }

            if(this.ChangeCanvasLayer && this.TargetCanvas != null)
            {
                var nLayer = this.MidCanvasLayer;
                if(fPosition <= 0.5f)
                {
                    nLayer = this.BeginCanvasLayer + Mathf.FloorToInt((this.MidCanvasLayer - this.BeginCanvasLayer) * fPosition / 0.5f);
                }
                else
                {
                    nLayer = this.MidCanvasLayer + Mathf.FloorToInt((this.EndCanvaslayer - this.MidCanvasLayer) * (fPosition - 0.5f) / 0.5f);
                }
                this.TargetCanvas.sortingOrder = nLayer;
            }
            this.Animator.speed = 0;
        }
    }
}
