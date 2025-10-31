using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.UI
{
    public enum LargeSizeLimitType
    {
        Hight,
        Width,
    }
    [DisallowMultipleComponent]
    public class LargeSizeLimit : MonoBehaviour
    {
        public ScrollRect ScrollRect;
        public LayoutElement LayoutElement;
        public Vector2 LargeSize;
        public LargeSizeLimitType LargeSizeLimitType;
        private void LateUpdate()
        {
            if (!this.ScrollRect) return;
            if (!this.LayoutElement) return;
            if (this.LargeSizeLimitType == LargeSizeLimitType.Width)
            {
                if (this.ScrollRect.content.sizeDelta.x < this.LargeSize.x)
                {
                    this.LayoutElement.minWidth = this.ScrollRect.content.sizeDelta.x;
                    this.ScrollRect.enabled = false;
                }
                else
                {
                    this.LayoutElement.minWidth = this.LargeSize.x;
                    this.ScrollRect.enabled = true;
                }
            }
            else if(this.LargeSizeLimitType == LargeSizeLimitType.Hight)
            {
                if (this.ScrollRect.content.sizeDelta.y < this.LargeSize.y)
                {
                    this.LayoutElement.minHeight = this.ScrollRect.content.sizeDelta.y;
                    this.ScrollRect.enabled = false;
                }
                else
                {
                    this.LayoutElement.minHeight = this.LargeSize.y;
                    this.ScrollRect.enabled = true;
                }
            }

        }
    }
}

