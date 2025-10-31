using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [ExecuteInEditMode]
    public class GraphLine : GraphBase
    {
        public float            Width = 1;
        public Line             Line  = new Line();

        private float           mReadWidth;

        protected override void CreateVertices()
        {
            this.mVertexes.Clear();
            this.mReadWidth = this.Width / 8000.0f;

            var rForward = this.Line.Pos2 - this.Line.Pos1;
            var rRight = Vector3.Cross(rForward, this.Line.Up);

            this.mVertexes.Add(this.Line.Pos1 + rRight * this.mReadWidth / 2.0f);     // A
            this.mVertexes.Add(this.Line.Pos2 + rRight * this.mReadWidth / 2.0f);     // B
            this.mVertexes.Add(this.Line.Pos2 - rRight * this.mReadWidth / 2.0f);     // C
            this.mVertexes.Add(this.Line.Pos1 - rRight * this.mReadWidth / 2.0f);     // D

            this.mIndices.Clear();
            this.mIndices.Add(0);
            this.mIndices.Add(1);
            this.mIndices.Add(3);
            this.mIndices.Add(1);
            this.mIndices.Add(2);
            this.mIndices.Add(3);
        }
    }
}
