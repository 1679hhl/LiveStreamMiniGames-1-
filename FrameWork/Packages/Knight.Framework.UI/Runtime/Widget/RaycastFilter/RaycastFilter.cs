using Knight.Core;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class RaycastFilter : EmptyGraphics
    {
        private static readonly Dictionary<string, List<RectTransform>> mInvalidRects = new Dictionary<string, List<RectTransform>>();

        [SerializeField]
        private string mKey;
        public string Key { get => mKey; set => mKey = value; }

        public override bool Raycast(Vector2 screenPoint, Camera eventCamera)
        {
            if (string.IsNullOrWhiteSpace(mKey))
            {
                return true;
            }

            List<RectTransform> rTargets;
            string[] rKeys = mKey.Split(',');
            for (int i = 0; i < rKeys.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(rKeys[i])) continue;

                if (mInvalidRects.TryGetValue(rKeys[i], out rTargets))
                {
                    for (int j = 0; j < rTargets.Count; j++)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(rTargets[j], screenPoint, eventCamera))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public static void Add(string rKey, RectTransform rInvalidTarget)
        {
            if (string.IsNullOrWhiteSpace(rKey))
            {
                Knight.Core.LogManager.LogError($"{rInvalidTarget.name}忽略关键词无效！");
                return;
            }

            List<RectTransform> rTargets;
            if (mInvalidRects.TryGetValue(rKey, out rTargets) == false)
            {
                rTargets = new List<RectTransform>();
                mInvalidRects.Add(rKey, rTargets);
            }

            rTargets.Add(rInvalidTarget);
        }

        public static void Remove(string rKey, RectTransform rInvalidTarget)
        {
            if (string.IsNullOrWhiteSpace(rKey))
            {
                Knight.Core.LogManager.LogError($"{rInvalidTarget.name}忽略关键词无效！");
                return;
            }

            List<RectTransform> rTargets;
            if (mInvalidRects.TryGetValue(rKey, out rTargets))
            {
                rTargets.Remove(rInvalidTarget);
            }
        }
    }
}