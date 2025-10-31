using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    public class ReuleauxTriangle
    {
        public Vector3 vertex1;
        public Vector3 vertex2;
        public Vector3 vertex3;
        public Vector3 CirCleCenter = Vector3.zero;
        public float radius;
        private int tempVertex;

        public ReuleauxTriangle()
        {
        }

        public void CalVertex()
        {
            this.vertex1 = new Vector3(this.CirCleCenter.x, this.CirCleCenter.y, this.CirCleCenter.z - (Mathf.Cos(30 * Mathf.Deg2Rad) * 2 / 3 * this.radius));
            this.vertex2 = new Vector3(this.CirCleCenter.x - (Mathf.Sin(30 * Mathf.Deg2Rad) * this.radius), this.CirCleCenter.y, this.CirCleCenter.z + (Mathf.Cos(30 * Mathf.Deg2Rad) * 1 / 3 * this.radius));
            this.vertex3 = new Vector3(this.CirCleCenter.x + (Mathf.Sin(30 * Mathf.Deg2Rad) * this.radius), this.CirCleCenter.y, this.CirCleCenter.z + (Mathf.Cos(30 * Mathf.Deg2Rad) * 1 / 3 * this.radius));
        }

        public Vector3 GetPoint(Vector3 point, out bool bIsVaild)
        {
            float angle = Vector3.SignedAngle(Vector3.forward, (point - this.CirCleCenter).normalized, Vector3.up);//通过夹角判断目标点在哪个范围内 0-120,120-240,240-360  
            if (angle < 0 ) 
            {
                angle += 360;
            }
            bIsVaild = this.CheckPointInArea(point, angle);
            if (bIsVaild)
            {
                return point;
            }
            else
            {
                var rPos = this.GetRecentDistancePos(point, angle);
                return rPos;
            }
        }
        public bool CheckPointInArea(Vector3 point, float angle)
        {
            return this.CheckAngle(point, angle);
        }

        public void SetVertex(float angle)
        {
            if (angle <= 300 && angle >= 180)
            {
                this.tempVertex = 3;
            }
            else if (angle <= 180 && angle >= 60)
            {
                this.tempVertex = 2;
            }
            else if ((angle < 360 && angle > 300) || (angle >= 0 && angle < 60))
            {
                this.tempVertex = 1;
            }
        }
        private bool CheckAngle(Vector3 point, float angle)
        {
            if (angle <= 300 && angle >= 180)
            {
                if ((point - this.vertex3).magnitude < this.radius)//在图形范围内 直接返回true
                {
                    return true;
                }
            }
            else if (angle <= 180 && angle > 60)
            {
                if ((point - this.vertex2).magnitude < this.radius)
                {
                    return true;
                }
            }
            else if ((angle <= 360 && angle > 300) || (angle >= 0 && angle < 60))
            {
                if ((point - this.vertex1).magnitude < this.radius)
                {
                    return true;
                }
            }
            return false;
        }
        public Vector3 GetRecentDistancePos(Vector3 point, float angle)
        {
            this.SetVertex(angle);

            var rPos = point;
            switch (this.tempVertex)
            {
                case 1:
                    rPos = this.vertex1 + (point - this.vertex1).normalized * this.radius;              
                    break;
                case 2:
                    rPos = this.vertex2 + (point - this.vertex2).normalized * this.radius;
                    break;
                case 3:
                    rPos = this.vertex3 + (point - this.vertex3).normalized * this.radius;
                    break;
                default:
                    break;
            }

            if (this.CheckVertexDis(rPos, out var rVertexPos))
            {
                rPos = rVertexPos;
            }
            return rPos;
        }

        public bool CheckVertexDis(Vector3 point, out Vector3 vertexPos)
        {
            if (Vector3.Distance(point, this.vertex1) <= 3)
            {
                vertexPos = this.vertex1;
                return true;
            }

            if (Vector3.Distance(point, this.vertex2) <= 3)
            {
                vertexPos = this.vertex2;
                return true;
            }

            if (Vector3.Distance(point, this.vertex3) <= 3f)
            {
                vertexPos = this.vertex3;
                return true;
            }

            vertexPos = Vector3.zero;
            return false;
        }
    }
}