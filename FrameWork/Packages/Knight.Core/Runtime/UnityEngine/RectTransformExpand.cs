namespace LittleBaby.Framework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using UnityEngine;
    public static class RectTransformExpand
    {
        public static void ResetAnchorAndPivot(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Boolean roundToInt = false)
        {
            if (!rectTransform)
            {
                return;
            }

            var pos = rectTransform.anchoredPosition;
            var offsetPos = rectTransform.sizeDelta * (rectTransform.pivot - pivot);
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.pivot = anchorMax;
            pos -= offsetPos;
            if (roundToInt)
            {
                pos.x = Mathf.RoundToInt(pos.x);
                pos.y = Mathf.RoundToInt(pos.y);
            }
            rectTransform.anchoredPosition = pos;
        }
    }
}
