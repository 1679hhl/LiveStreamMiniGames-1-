using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{
    public class GraphCircle : GraphBase
    {
        public Circle Circle = new Circle();

        protected override void CreateVertices()
        {
            this.mVertexes.Clear();

            this.mVertexes.Add(this.Circle.Position);
            this.Circle.CalcPoints();
            this.mVertexes.AddRange(this.Circle.CirclePoints);

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
