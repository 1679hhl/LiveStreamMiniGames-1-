using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{
    public class GraphRectangle : GraphBase
    {

        public Rectangle Rectangle = new Rectangle();

        protected override void CreateVertices()
        {
            this.mVertexes.Clear();

            this.Rectangle.CalcPoints();
            this.mVertexes.AddRange(this.Rectangle.RrectanglePoints);

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
