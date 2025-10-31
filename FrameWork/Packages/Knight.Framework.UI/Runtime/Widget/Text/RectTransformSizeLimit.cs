using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    [ExecuteAlways]
    public class RectTransformSizeLimit : MonoBehaviour
    {
        public bool IsLimitWidth = false;
        [ShowIf("IsLimitWidth")]
        public int MaxWidth;
        public bool IsLimitHeight = false;
        [ShowIf("IsLimitHeight")]
        public int MaxHeight;

        public bool IsPrefferedWidth = false;
        public bool IsPrefferedHeight = false;

        private RectTransform mSelfTrans;

        private void Start()
        {
            this.mSelfTrans = this.GetComponent<RectTransform>();
        }

        private void Update()
        {
            this.UpdateRectTransform();
        }

        private void OnValidate()
        {
            this.UpdateRectTransform();
        }

        public void UpdateRectTransform()
        {
            if (this.mSelfTrans == null) return;

            if (this.IsPrefferedWidth)
                this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredSize(this.mSelfTrans, (int)RectTransform.Axis.Horizontal));
            if (this.IsLimitWidth && this.mSelfTrans.rect.width > this.MaxWidth)
                this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.MaxWidth);


            if (this.IsPrefferedHeight)
                this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredSize(this.mSelfTrans, (int)RectTransform.Axis.Vertical));
            if (this.IsLimitHeight && this.mSelfTrans.rect.height > this.MaxHeight)
                this.mSelfTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.MaxHeight);

            LayoutRebuilder.MarkLayoutForRebuild(this.mSelfTrans);
        }
    }
}
