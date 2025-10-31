using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core.Math
{
    public class AABB
    {
        public Vector3 Center;
        public Vector3 Radius;

        public Vector3 Pos0;
        public Vector3 Pos1;

        public Vector3 Normal0;
        public Vector3 Normal1;
        public Vector3 Normal2;
        public Vector3 Normal3;

        public AABB()
        {
        }

        public AABB(Vector3 p0, Vector3 p1)
        {
            this.Pos0 = p0;
            this.Pos1 = p1;

            this.Center = (this.Pos0 + this.Pos1) * 0.5f;
            this.Radius = (this.Pos1 - this.Pos0) * 0.5f;

            this.UpdateNormal();
        }

        public AABB(Vector3 center, float width, float height, float depth)
        {
            this.Center = center;

            this.Radius.x = width  / 2.0f;
            this.Radius.y = height / 2.0f;
            this.Radius.z = depth  / 2.0f;

            this.Pos0 = this.Center - this.Radius;
            this.Pos1 = this.Center + this.Radius;

            this.UpdateNormal();
        }

        // 可能有问题
        public void Transform(Matrix4x4 rTransformMat)
        {
            this.Pos0 = rTransformMat.MultiplyPoint(this.Pos0);
            this.Pos1 = rTransformMat.MultiplyPoint(this.Pos1);

            this.Center = (this.Pos0 + this.Pos1) / 2.0f;
            this.Radius = (this.Pos1 - this.Pos0) / 2.0f;

            this.UpdateNormal();
        }
        
        public void UpdateNormal()
        {
            this.Normal0.x = this.Pos1.x - this.Center.x;
            this.Normal0.y = this.Pos1.y - this.Center.y;
            this.Normal0.z = this.Pos1.z - this.Center.z;

            this.Normal1.x = this.Pos1.x - this.Center.x;
            this.Normal1.y = this.Pos0.y - this.Center.y;
            this.Normal1.z = this.Pos1.z - this.Center.z;

            this.Normal2.x = this.Pos0.x - this.Center.x;
            this.Normal2.y = this.Pos0.y - this.Center.y;
            this.Normal2.z = this.Pos1.z - this.Center.z;

            this.Normal3.x = this.Pos0.x - this.Center.x;
            this.Normal3.y = this.Pos1.y - this.Center.y;
            this.Normal3.z = this.Pos1.z - this.Center.z;
        }

        public static bool Plane3DTestCube(Plane P, AABB R)
        {
            float dCenter = P.Distance(R.Center);
            if (dCenter >= 0) return true;

            dCenter = Mathf.Abs(dCenter);

            if (Mathf.Abs(P.NormalDot(R.Normal0)) > dCenter || 
                Mathf.Abs(P.NormalDot(R.Normal1)) > dCenter || 
                Mathf.Abs(P.NormalDot(R.Normal2)) > dCenter || 
                Mathf.Abs(P.NormalDot(R.Normal3)) > dCenter)
            {
                return true;
            }

            return false;
        }
    }
}
