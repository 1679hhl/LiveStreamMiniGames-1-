using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class RectTransformSizeFollow : MonoBehaviour
    {
        public RectTransform FollowTarget;
        public Rect PaddingRect;

        private RectTransform mSelfTrans;

        public bool LimitMaxWidth;
        public int MaxWidth;

        public bool LimitMaxHeight;
        public int MaxHeight;

        public bool FollowPosition = false;
        public bool FollowInUpdate = true;

        private static Vector3[] Corners = new Vector3[4];

        public void SetFollowTarget(RectTransform rTarget)
        {
            this.FollowTarget = rTarget;
            this.Follow();
        }

        private void OnEnable()
        {
            this.Follow();
        }

        private void OnDisable()
        {
            this.Follow();
        }

        private void LateUpdate()
        {
            //#if UNITY_EDITOR
            if (FollowInUpdate)
            {
                this.Follow();
            }
            //#endif
        }

        private void OnValidate()
        {
            if (FollowInUpdate)
            {
                this.Follow();
            }
        }

        public void Follow()
        {
            if (this.FollowTarget == null) return;
            if (this.mSelfTrans == null)
                this.mSelfTrans = this.gameObject.GetComponent<RectTransform>();
            if (this.mSelfTrans == null) return;

            float fWidth = this.FollowTarget.rect.width + this.PaddingRect.width;
            if (this.LimitMaxWidth && fWidth > MaxWidth)
            {
                fWidth = MaxWidth;
            }
            float fHeight = this.FollowTarget.rect.height + this.PaddingRect.height;
            if (this.LimitMaxHeight && fHeight > MaxHeight)
            {
                fHeight = MaxHeight;
            }
            this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fWidth);
            this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, fHeight);
            if (this.FollowPosition)
            {
                this.FollowTarget.GetWorldCorners(Corners);
                this.mSelfTrans.pivot = new Vector2(0.5f, 0.5f);
                this.mSelfTrans.position = (Corners[0] + Corners[2]) / 2;
            }
        }
    }
}
