using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    public class GraphFan : GraphBase
    {
        public Fan Fan = new Fan();
        
        protected override void CreateVertices()
        {
            this.mVertexes.Clear();

            this.mVertexes.Add(this.Fan.Position);
            this.Fan.CalcFanPoints();
            this.mVertexes.AddRange(this.Fan.FanPoints);

            this.mIndices.Clear();
            for (int i = 2; i < this.mVertexes.Count; i++)
            {
                this.mIndices.Add(0);
                this.mIndices.Add(i - 1);
                this.mIndices.Add(i);
            }
        }
    }
}
