using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public enum Orientation
    {
        LeftDown = 0,
        LeftUp,
        LeftCenter,
        RightUp,
        RigthDown,
        RightCenter,
        CenterUp,
        CenterDown,
        Center
    }

    public class ScreenLimit : MonoBehaviour
    {
        [Tooltip("物体方位")]
        public Orientation OriDirect;
        [Tooltip("限制在屏幕内的物体")]
        public RectTransform TargetRect;
        [Tooltip("出现的位置相对于手指点击位置的偏移量")]
        public float m_posOffset = 0;

        private CanvasScaler mCanvasScaler;
        private Camera mUICamera;

        private void Awake()
        {
            this.mCanvasScaler = UIRoot.Instance.CanvasScaler;
            this.mUICamera = UIRoot.Instance.UICamera;
        }

        private void Start()
        {
            this.SetUIPos();
        }

        /// <summary>
        /// 根据位置信息，设置不同方位的偏移量
        /// </summary>
        public void SetUIPos()
        {
            Vector2 curInputPos = Input.mousePosition;
            float x = curInputPos.x;
            float y = curInputPos.y;
            float w = Mathf.Abs(this.TargetRect.rect.x) * this.GetScaleFactor();
            float h = Mathf.Abs(this.TargetRect.rect.y) * this.GetScaleFactor();
            Knight.Core.LogManager.Log(w);
            Knight.Core.LogManager.Log(h);
            float posOffset = this.m_posOffset * this.GetScaleFactor();
            switch (this.OriDirect)
            {
                case Orientation.LeftDown:
                    curInputPos = new Vector2(x + w, y + h);
                    break;
                case Orientation.LeftUp:
                    curInputPos = new Vector2(x + w, y - h);
                    break;
                case Orientation.LeftCenter:
                    curInputPos = new Vector2(x + w + posOffset, y);
                    break;
                case Orientation.RightUp:
                    curInputPos = new Vector2(x - w, y - h);
                    break;
                case Orientation.RigthDown:
                    curInputPos = new Vector2(x - w, y + h);
                    break;
                case Orientation.RightCenter:
                    curInputPos = new Vector2(x - w - posOffset, y);
                    break;
                case Orientation.CenterUp:
                    curInputPos = new Vector2(x, y - h - posOffset);
                    break;
                case Orientation.CenterDown:
                    curInputPos = new Vector2(x, y + h + posOffset);
                    break;
                case Orientation.Center:
                    curInputPos = new Vector2(x, y);
                    break;
            }
            this.SetRightPos(curInputPos);
        }

        /// <summary>
        /// ui的位置如果超出屏幕了，要把ui移到屏幕里面
        /// </summary>
        /// <param name="curInputPos"></param>
        public void SetRightPos(Vector2 curInputPos)
        {
            Transform parent = this.TargetRect.transform.parent;
            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(parent, this.TargetRect);
            var boundsMin = parent.localToWorldMatrix.MultiplyPoint3x4(bounds.min);
            var boundsMax = parent.localToWorldMatrix.MultiplyPoint3x4(bounds.max);
            var scrMin = RectTransformUtility.WorldToScreenPoint(UIRoot.Instance.UICamera, boundsMin);
            var scrMax = RectTransformUtility.WorldToScreenPoint(UIRoot.Instance.UICamera, boundsMax);
            var w = Mathf.Abs(scrMax.x - scrMin.x);
            var h = Mathf.Abs(scrMax.y - scrMin.y);
            scrMin = new Vector2(curInputPos.x - w / 2, curInputPos.y - h / 2);
            scrMax = new Vector2(curInputPos.x + w / 2, curInputPos.y + h / 2);
            var scrPos = curInputPos;
            float xResive = 0, yResive = 0;
            float xMin = Mathf.Min(scrMax.x, scrMin.x);
            float xMax = Mathf.Max(scrMax.x, scrMin.x);
            float yMin = Mathf.Min(scrMax.y, scrMin.y);
            float yMax = Mathf.Max(scrMax.y, scrMin.y);

            if (xMin < 0)
            {
                xResive = -xMin;
            }
            else if (xMax > Screen.width)
            {
                xResive = Screen.width - xMax;
            }

            if (yMin < 0)
            {
                yResive = -yMin;
            }
            else if (yMax > Screen.height)
            {
                yResive = Screen.height - yMax;
            }
            var screenPos = new Vector2(scrPos.x + xResive, scrPos.y + yResive);
            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent.GetComponent<RectTransform>(), screenPos, this.mUICamera, out localPos);
            this.TargetRect.anchoredPosition = localPos;
        }

        /// <summary>
        /// 获取画布的缩放
        /// </summary>
        /// <returns></returns>
        public float GetScaleFactor()
        {
            float scaleFactor = 1f;
            float kLogBase = 2;
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            if (this.mCanvasScaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
            {
                switch (this.mCanvasScaler.screenMatchMode)
                {
                    case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                        float logWidth = Mathf.Log(screenSize.x / this.mCanvasScaler.referenceResolution.x, kLogBase);
                        float logHeight = Mathf.Log(screenSize.y / this.mCanvasScaler.referenceResolution.y, kLogBase);
                        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, this.mCanvasScaler.matchWidthOrHeight);
                        scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
                        break;
                    case CanvasScaler.ScreenMatchMode.Expand:
                        scaleFactor = Mathf.Min(Screen.width / this.mCanvasScaler.referenceResolution.x, Screen.height / this.mCanvasScaler.referenceResolution.y);
                        break;
                    case CanvasScaler.ScreenMatchMode.Shrink:
                        scaleFactor = Mathf.Max(Screen.width / this.mCanvasScaler.referenceResolution.x, Screen.height / this.mCanvasScaler.referenceResolution.y);
                        break;
                }

            }
            return scaleFactor;
        }
    }
}

