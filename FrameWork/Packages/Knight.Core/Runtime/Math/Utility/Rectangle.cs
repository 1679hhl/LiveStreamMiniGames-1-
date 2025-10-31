using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class Rectangle
    {
        public Vector3 Position = Vector3.zero;
        public Vector3 Forward = Vector3.forward;
        public Vector3 Up = Vector3.up;
        public float Height = 2f;
        public float Width = 1f;

        [HideInInspector]
        public List<Vector3> RrectanglePoints = new List<Vector3>();

        public void CalcPoints()
        {

            this.RrectanglePoints.Clear();
            if (Mathf.Abs(this.Forward.x) <= 0.01f)
            {
                this.Forward.x = this.Forward.x > 0 ? 0.01f : -0.01f;
            }
            var rDir = Quaternion.FromToRotation(Vector3.forward, this.Forward);
            rDir.Normalize();

            this.RrectanglePoints.Add(this.Position + rDir * new Vector3(this.Width / 2, 0, 0));
            this.RrectanglePoints.Add(this.Position + rDir * new Vector3(-this.Width / 2, 0, 0));
            this.RrectanglePoints.Add(this.Position + rDir * new Vector3(-this.Width / 2, 0, this.Height));
            this.RrectanglePoints.Add(this.Position + rDir * new Vector3(this.Width / 2, 0, this.Height));
        }
    }
}
