using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core.Math
{
    public class Plane
    {
        public Vector4 Data;
        
        public Plane()
        {
        }

        public Plane(float a, float b, float c, float d)
        {
            this.Set(a, b, c, d);
        }

        public Plane(Vector3 n, float d)
        {
            this.Data.x = n.x;
            this.Data.y = n.y;
            this.Data.z = n.z;
            this.Data.w = d;
        }

        public void Set(float a, float b, float c, float d)
        {
            this.Data.x = a;
            this.Data.y = b;
            this.Data.z = c;
            this.Data.w = d;
        }

        public float Distance(Vector3 v)
        {
            return this.Data.x * v.x + this.Data.y * v.y + this.Data.z * v.z + this.Data.w;
        }

        public void Normalize()
        {
            this.Data.Normalize();
        }

        private Vector3 mTempPlane;
        public Vector3 ProjectPointToPlane(Vector3 rTargetPoint)
        {
            float fd = this.Distance(rTargetPoint);
            this.mTempPlane.x = this.Data.x * (-fd);
            this.mTempPlane.y = this.Data.y * (-fd);
            this.mTempPlane.z = this.Data.z * (-fd);
            return rTargetPoint + this.mTempPlane;
        }

        public float NormalDot(Vector3 n)
        {
            return this.Data.x * n.x + this.Data.y * n.y + this.Data.z * n.z;
        }
    }
}
