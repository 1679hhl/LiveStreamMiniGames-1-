using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class Circle
    {
        public Vector3          Position = Vector3.zero;
        public Vector3          Forward  = Vector3.forward;
        public Vector3          Up       = Vector3.up;
        public float            Radius   = 1.0f;

        [HideInInspector]
        public List<Vector3>    CirclePoints = new List<Vector3>();

        public void CalcPoints()
        {
            this.CirclePoints.Clear();
            var rCurAngle = 0.0f;
            while (rCurAngle < 360.0f)
            {
                var rDir = Quaternion.AngleAxis(rCurAngle, this.Up) * Vector3.forward;
                rDir.Normalize();
                this.CirclePoints.Add(this.Position + rDir * this.Radius);
                rCurAngle += 10.0f;
            }
            rCurAngle = 360.0f;
            var rFinalDir = Quaternion.AngleAxis(rCurAngle, this.Up) * Vector3.forward;
            rFinalDir.Normalize();
            this.CirclePoints.Add(this.Position + rFinalDir * this.Radius);
        }
    }
}
