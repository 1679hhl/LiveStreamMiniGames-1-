using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    /// <summary>
    /// 8方向描边
    /// </summary>
    public class TextOutline8 : Shadow
    {
        public List<UIVertex> mVertexList = new List<UIVertex>();
        public override void ModifyMesh(VertexHelper rVH)
        {
            if (!this.IsActive())
                return;

            this.mVertexList.Clear();
            rVH.GetUIVertexStream(mVertexList);
            this.ModifyVertices(mVertexList);
            rVH.Clear();
            rVH.AddUIVertexTriangleStream(mVertexList);
        }

        public void ModifyVertices(List<UIVertex> rVert)
        {
            if (!this.IsActive())
                return;

            var neededCapacity = rVert.Count * 9;
            if (rVert.Capacity < neededCapacity)
                rVert.Capacity = neededCapacity;

            var original = rVert.Count;
            var count = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (!(x == 0 && y == 0))
                    {
                        var next = count + original;
                        this.ApplyShadow(rVert, this.effectColor, count, next, this.effectDistance.x * x, this.effectDistance.y * y);
                        count = next;
                    }
                }
            }
        }
    }
}