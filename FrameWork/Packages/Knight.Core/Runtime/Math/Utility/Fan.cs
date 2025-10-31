using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class Fan
    {
        public Vector3          Position = Vector3.zero;
        public Vector3          Forward  = Vector3.forward;
        public Vector3          Up       = Vector3.up;
        public float            Radius   = 1.0f;
        [Range(0, 360)]
        public float            Angle    = 60.0f;

        [HideInInspector]
        public List<Vector3>    FanPoints     = new List<Vector3>();
        [HideInInspector]
        public float            CosHalfAngle  = Mathf.Cos(30.0f * Mathf.Deg2Rad);
        [HideInInspector]
        public Vector3          WorldPosition = Vector3.zero;

        public void CalcFanPoints()
        {
            this.FanPoints.Clear();
            var rCurAngle = -this.Angle / 2.0f;
            while (rCurAngle < this.Angle / 2.0f)
            {
                var rDir = Quaternion.AngleAxis(rCurAngle, this.Up) * this.Forward;
                rDir.Normalize();
                this.FanPoints.Add(this.Position + rDir * this.Radius);
                rCurAngle += 10.0f;
            }
            rCurAngle = this.Angle / 2.0f;
            var rFinalDir = Quaternion.AngleAxis(rCurAngle, this.Up) * this.Forward;
            rFinalDir.Normalize();
            this.FanPoints.Add(this.Position + rFinalDir * this.Radius);

            this.CosHalfAngle = Mathf.Cos(this.Angle / 2.0f * Mathf.Deg2Rad);
        }
        
        /// <summary>
        /// 这里只是2D的版本 y = 0
        /// </summary>
        public bool IntersectWithPoint(Vector3 rPos)
        {
            float dx = rPos.x - this.Position.x;
            float dy = rPos.z - this.Position.z;

            float fSquaredLength = dx * dx + dy * dy;
            if (fSquaredLength > this.Radius * this.Radius)
                return false;

            float fDdotU = dx * this.Forward.x + dy * this.Forward.z;

            if (fDdotU >= 0 && this.CosHalfAngle >= 0)
            {
                return fDdotU * fDdotU > fSquaredLength * this.CosHalfAngle * this.CosHalfAngle;
            }
            else if (fDdotU < 0 && this.CosHalfAngle < 0)
            {
                return fDdotU * fDdotU < fSquaredLength * this.CosHalfAngle * this.CosHalfAngle;
            }
            else
            {
                return fDdotU >= 0;
            }
        }
        
        private Vector3 temp1 = Vector3.zero;

        /// <summary>
        /// 这里只是2D的版本 z = 0
        /// 如果两个点其中一个在扇形内的话，说明相交
        /// 否则 判断 线段和扇形的两条边判断是否相交 并且判断圆心到线段的投投影点是否在扇形范围内
        /// </summary>
        public bool IntersectWithLine(Vector3 rPos1, Vector3 rPos2)
        {
            if (this.IntersectWithPoint(rPos1) || this.IntersectWithPoint(rPos2))
            {
                return true;
            }

            // 判断线段和边界相交
            if (Line.LineIntersectLine(rPos1, rPos2, this.Position, this.FanPoints[0])) return true;
            if (Line.LineIntersectLine(rPos1, rPos2, this.Position, this.FanPoints[this.FanPoints.Count - 1])) return true;

            // 计算点到线段的垂足
            // 求直线方程
            float a, b, c;  // ax+by+c=0;
            if (rPos1.x == rPos2.x)
            {
                a = 1; b = 0; c = -rPos1.x;     // 特殊情况判断，分母不能为零
            }
            else if (rPos1.z == rPos2.z)
            {
                a = 0; b = 1; c = -rPos1.z;     // 特殊情况判断，分母不能为零
            }
            else
            {
                a = rPos1.z - rPos2.z;
                b = rPos2.x - rPos1.x;
                c = rPos1.x * rPos2.z - rPos1.z * rPos2.x;
            }

            if (a * a + b * b < 1e-5f) return false; // 说明是一个点，并且这个点没在扇形内

            if (Mathf.Abs(a * this.Position.x + b * this.Position.z + c) < 1e-13f)    // 点在直线上
            {
                return false;
            }
            else
            {
                float newX = (b * b * this.Position.x - a * b * this.Position.z - a * c) / (a * a + b * b);
                float newY = (-a * b * this.Position.x + a * a * this.Position.z - b * c) / (a * a + b * b);
                temp1.x = newX; temp1.y = 0.1f; temp1.z = newY;

                float maxX = Mathf.Max(rPos1.x, rPos2.x);
                float minX = Mathf.Min(rPos1.x, rPos2.x);
                float maxY = Mathf.Max(rPos1.z, rPos2.z);
                float minY = Mathf.Min(rPos1.z, rPos2.z);
                // 判断点有没有在线段内
                if ((newX < minX || newX > maxX) || (newY < minY || newY > maxY))
                {
                    return false;
                }

                // 判断垂足是否在扇形内
                return this.IntersectWithPoint(temp1);
            }
        }
    }
}
