using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;
using UnityEditor;

namespace UnityEngine.UI
{
    public class UIFollow3DObject : MonoBehaviour
    {
        public RectTransform ParentTrans;
        public RectTransform SelfTrans;

        public Camera MainCamera;
        public Transform TargetTrans;
        public Vector3 TargetOffset;
        public Vector2 UIOffset;

        // 屏幕范围
        private Vector2 mScreenMin;
        private Vector2 mScreenMax;

        // 移出屏幕标记
        private bool mIsOutOfScreen;

        // 更新开启标记
        private bool mUpdateEnable;

        private void Awake()
        {
            this.mUpdateEnable = (this.ParentTrans != null && this.MainCamera != null && this.SelfTrans != null);

            if (this.mUpdateEnable)
            {
                Canvas rCanvas = UtilTool.GetComponentInParent<Canvas>(this.transform);
                Rect rRect = rCanvas.pixelRect;

                // 设定范围比屏幕大1/2以覆盖血条范围
                this.mScreenMin = new Vector2(-rRect.width / 2, -rRect.height / 2);
                this.mScreenMax = new Vector2(rRect.width * 3 / 2, rRect.height * 3 / 2);
            }
        }

        private void OnEnable()
        {
            this.mIsOutOfScreen = false;
            this.UpdateFollow();
        }
        private void LateUpdate()
        {
            this.UpdateFollow();
        }

        private void UpdateFollow()
        {
            if (!this.mUpdateEnable)
                return;
            if (!this.TargetTrans) // 有可能被销毁，需判断
                return;

            // 移出屏幕判定，移出后再更新一帧，避免快速移动、闪现出问题
            var rScreenPos = this.MainCamera.WorldToScreenPoint(this.TargetTrans.position + this.TargetOffset);
            bool bOutOfScreen = (rScreenPos.x < this.mScreenMin.x || rScreenPos.x > this.mScreenMax.x || rScreenPos.y < this.mScreenMin.y || rScreenPos.y > this.mScreenMax.y);
            if (bOutOfScreen && this.mIsOutOfScreen)
                return;

            if (!this.ParentTrans) // 有可能被销毁，需判断
                return;
            var rAnchorPos = UtilTool.ScreenPointToLocalPointInRectangle(this.ParentTrans, rScreenPos, UIRoot.Instance.UICamera);
            this.SelfTrans.anchoredPosition = rAnchorPos + this.UIOffset;
            this.mIsOutOfScreen = bOutOfScreen;
        }
    }
}
