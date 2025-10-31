using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class Blocker : MonoBehaviour
    {
        public  RectTransform   LeftBlocker;
        public  RectTransform   RightBlocker;
        public  RectTransform   TopBlocker;
        public  RectTransform   BottomBlocker;

        public  RectTransform   Exclude;

        private Rect            mRect;

        public void HideAll()
        {
            this.Exclude = null;
            this.gameObject.SetActive(false);
        }

        public void Update()
        {
            if (this.Exclude == null) return;

            this.ShowAll(this.Exclude);
        }

        public Rect GetCenterRect()
        {
            return this.mRect;
        }

        public void ShowAll(RectTransform rExclude)
        {
            this.Exclude = rExclude;
            this.gameObject.SetActive(true);

            var screenWidth = Display.main.renderingWidth;
            var screenHeight = Display.main.renderingHeight;

            var worldCorners = new Vector3[4];
            rExclude.GetWorldCorners(worldCorners);

            for (int i = 0; i < 4; ++i)
                worldCorners[i] = UIRoot.Instance.UICamera.WorldToScreenPoint(worldCorners[i]);

            this.mRect.x = worldCorners[0].x;
            this.mRect.y = worldCorners[0].y;
            this.mRect.width = worldCorners[2].x - worldCorners[0].x;
            this.mRect.height = worldCorners[2].y - worldCorners[0].y;

            this.LeftBlocker.anchorMin = Vector2.zero;
            this.LeftBlocker.anchorMax = new Vector2(this.mRect.xMin / screenWidth, 1);
            this.LeftBlocker.offsetMax = Vector2.zero;
            this.LeftBlocker.offsetMin = Vector2.zero;

            this.RightBlocker.anchorMin = new Vector2(this.mRect.xMax / screenWidth, 0);
            this.RightBlocker.anchorMax = Vector2.one;
            this.RightBlocker.offsetMax = Vector2.zero;
            this.RightBlocker.offsetMin = Vector2.zero;

            this.TopBlocker.anchorMin = new Vector2(this.mRect.xMin / screenWidth, this.mRect.yMax / screenHeight);
            this.TopBlocker.anchorMax = new Vector2(this.mRect.xMax / screenWidth, 1);
            this.TopBlocker.offsetMax = Vector2.zero;
            this.TopBlocker.offsetMin = Vector2.zero;

            this.BottomBlocker.anchorMin = new Vector2(this.mRect.xMin / screenWidth, 0);
            this.BottomBlocker.anchorMax = new Vector2(this.mRect.xMax / screenWidth, this.mRect.yMin / screenHeight);
            this.BottomBlocker.offsetMax = Vector2.zero;
            this.BottomBlocker.offsetMin = Vector2.zero;

            this.gameObject.SetActive(true);
        }
    }
}
