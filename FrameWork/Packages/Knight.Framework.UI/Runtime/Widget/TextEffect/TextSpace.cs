using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    /// <summary>
    /// Text控件字间距工具
    /// </summary>
    public class TextSpace : BaseMeshEffect
    {
        public float Space = 1f;

        public override void ModifyMesh(VertexHelper rVH)
        {
            if (!this.IsActive() || rVH.currentVertCount == 0)
                return;

            List<UIVertex> vertexs = new List<UIVertex>();
            rVH.GetUIVertexStream(vertexs);
            if (vertexs.Count <= 6)
                return;

            float fWidth = vertexs[vertexs.Count - 1].position.x - vertexs[0].position.x + (vertexs.Count / 6) * this.Space;
            float fWidthSpace = fWidth + (vertexs.Count / 6 - 1) * this.Space;
            float fLeft = 0;
            Text rText = this.GetComponent<Text>();
            if (rText != null)
            {
                switch (rText.alignment)
                {
                    case TextAnchor.LowerCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.UpperCenter:
                        fLeft = (fWidth - fWidthSpace) / 2;
                        break;
                    case TextAnchor.LowerRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.UpperRight:
                        fLeft = fWidth - fWidthSpace;
                        break;
                }
            }

            int indexCount = rVH.currentIndexCount;
            UIVertex vt;
            for (int i = 0; i < indexCount; i++)
            {
                vt = vertexs[i];
                vt.position += new Vector3(fLeft + this.Space * (i / 6), 0, 0);
                vertexs[i] = vt;
                if (i % 6 <= 2)
                {
                    rVH.SetUIVertex(vt, (i / 6) * 4 + i % 6);
                }
                if (i % 6 == 4)
                {
                    rVH.SetUIVertex(vt, (i / 6) * 4 + i % 6 - 1);
                }
            }
        }
    }
}