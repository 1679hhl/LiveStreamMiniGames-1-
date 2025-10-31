using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Knight.Core
{
    [ExecuteInEditMode]
    public class TransformFollow : MonoBehaviour
    {
        public Transform    Target;

        public bool         IsFollowPos;
        [ShowIf("IsFollowPos")]
        public Vector3      PositionOffset;

        public bool         IsFollowRotate;
        [ShowIf("IsFollowRotate")]
        public Vector3      RotateOffset;

        public bool         IsFollowScale;
        [ShowIf("IsFollowScale")]
        public float        ScaleOffset = 1.0f;

        public float        DynamicScale = 1.0f;

        public bool         IsIgnorePosY;
        public float        InitPosY;

        private Vector3     mTempPos;

        public bool IsUseDynamicScale;

        public bool IsUpdate;
        public void Init()
        {
            if (this.Target == null) return;
            var rOffset = this.PositionOffset * this.Target.lossyScale.x;
            this.transform.position = this.Target.transform.position + rOffset;
            this.mTempPos = this.transform.position;
            this.mTempPos.y = this.InitPosY + rOffset.y;
            this.transform.position = this.mTempPos;

            if (this.IsFollowRotate)
                this.transform.eulerAngles = this.Target.transform.eulerAngles + this.RotateOffset;
            if (this.IsFollowScale)
                this.transform.localScale = this.Target.transform.lossyScale * this.ScaleOffset; 
        }
        public void LateUpdate()
        {
            if (this.IsUpdate) return;

            if (this.Target == null) return;

            if (this.IsFollowPos)
            {
                var rOffset = this.PositionOffset * this.Target.lossyScale.x;
                this.transform.position = this.Target.transform.position + rOffset;
                if (this.IsIgnorePosY)
                {
                    this.mTempPos = this.transform.position;
                    this.mTempPos.y = this.InitPosY + rOffset.y;
                    this.transform.position = this.mTempPos;
                }
            }
            if (this.IsFollowRotate)
            {
                this.transform.eulerAngles = this.Target.transform.eulerAngles + this.RotateOffset;
            }
            if (this.IsFollowScale)
            {
                this.transform.localScale = this.Target.transform.lossyScale * this.ScaleOffset;
            }
            if (this.IsUseDynamicScale)
            {
                this.transform.localScale = this.Target.transform.lossyScale * this.DynamicScale; 
            }
        }
        public void Update()
        {
            if (!this.IsUpdate) return;

            if (this.Target == null) return;

            if (this.IsFollowPos)
            {
                var rOffset = this.PositionOffset * this.Target.lossyScale.x;
                this.transform.position = this.Target.transform.position + rOffset;
                if (this.IsIgnorePosY)
                {
                    this.mTempPos = this.transform.position;
                    this.mTempPos.y = this.InitPosY + rOffset.y;
                    this.transform.position = this.mTempPos;
                }
            }
            if (this.IsFollowRotate)
            {
                this.transform.eulerAngles = this.Target.transform.eulerAngles + this.RotateOffset;
            }
            if (this.IsFollowScale)
            {
                this.transform.localScale = this.Target.transform.lossyScale * this.ScaleOffset;
            }
            if (this.IsUseDynamicScale)
            {
                this.transform.localScale = this.Target.transform.lossyScale * this.DynamicScale;
            }
        }

        public void UpdatePosition(Vector3 rPos)
        {
            this.transform.position = rPos;
        }
    }
}
