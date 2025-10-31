using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public class TextOutlineCircle : Shadow
    {
        public enum EEffectLevel { Low, Medium, High }
        public EEffectLevel EffectLevel;
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

        public void ModifyVertices(List<UIVertex> verts)
        {
            if (!this.IsActive())
                return;

            int nCircleCount;
            int nFirstSample;
            int nSampleIncrement;
            switch (this.EffectLevel)
            {
                case EEffectLevel.Medium:
                    nCircleCount = 7;
                    nFirstSample = 2;
                    nSampleIncrement = 3;
                    break;
                case EEffectLevel.High:
                    nCircleCount = 9;
                    nFirstSample = 2;
                    nSampleIncrement = 4;
                    break;
                default:// low
                    nCircleCount = 4;
                    nFirstSample = 2;
                    nSampleIncrement = 3;
                    break;
            }


            var total = (nFirstSample * 2 + nSampleIncrement * (nCircleCount - 1)) * nCircleCount / 2;
            var neededCapacity = verts.Count * (total + 1);
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;
            var original = verts.Count;
            var count = 0;
            var sampleCount = nFirstSample;
            var dx = this.effectDistance.x / nCircleCount;
            var dy = this.effectDistance.y / nCircleCount;
            for (int i = 1; i <= nCircleCount; i++)
            {
                var rx = dx * i;
                var ry = dy * i;
                var radStep = 2 * Mathf.PI / sampleCount;
                var rad = (i % 2) * radStep * 0.5f;
                for (int j = 0; j < sampleCount; j++)
                {
                    var next = count + original;
                    this.ApplyShadow(verts, this.effectColor, count, next, rx * Mathf.Cos(rad), ry * Mathf.Sin(rad));
                    count = next;
                    rad += radStep;
                }
                sampleCount += nSampleIncrement;
            }
        }
    }
}