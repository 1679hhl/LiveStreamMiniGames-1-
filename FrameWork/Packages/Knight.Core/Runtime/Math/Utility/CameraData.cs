using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core.Math
{
    public class CameraData
    {
        public Frustum      Frustum;
        public Matrix4x4    ViewMatrix;
        public Matrix4x4    ProjMatrix;

        public Matrix4x4    CameraToWolrdMatrix;

        public Camera       Camera;
        private BoxCollider rBoxCollider;
        public float        FarClipPlane;

        public CameraData(Camera rCamera, float fFarClipPlane)
        {
            this.Frustum = new Frustum();

            this.ViewMatrix = Matrix4x4.identity;
            this.ProjMatrix = Matrix4x4.identity;
            this.CameraToWolrdMatrix = Matrix4x4.identity;
            this.Camera = rCamera;
            rBoxCollider = this.Camera.gameObject.GetComponent<BoxCollider>();
            if (rBoxCollider == null)
                rBoxCollider = this.Camera.gameObject.AddComponent<BoxCollider>();
           
            this.FarClipPlane = fFarClipPlane;
        }

        public bool TestCube(AABB rCube)
        {
            if (this.Frustum == null) return false;
            return this.Frustum.TestCube(rCube);
        }


        public bool TestCube(BoxCollider rCollider)
        {
            if (!rBoxCollider) return false;
            if (!rCollider) return false;

            //if (this.Frustum == null) return false;
            //bool ret = this.Frustum.TestCube(rCube);
            Bounds rendererBounds = rBoxCollider.bounds;
            Bounds colliderBounds = rCollider.bounds;
            bool rendererIsInsideBox = colliderBounds.Intersects(rendererBounds);
            //如果摄像机处于Collider里面 则需要进行替换材质
            if (rendererIsInsideBox)
                return true;
            return false;
        }

        public void UpdateViewProjMatrix()
        {
            if (this.Camera == null) return;

            var look = this.Camera.transform.forward;
            var right = this.Camera.transform.right;
            var up = this.Camera.transform.up;

            var pos = this.Camera.transform.position;

            float x = -Vector3.Dot(right, pos);
            float y = -Vector3.Dot(up, pos);
            float z = -Vector3.Dot(look, pos);

            this.ViewMatrix.m00 = right.x;
            this.ViewMatrix.m01 = up.x;
            this.ViewMatrix.m02 = look.x;
            this.ViewMatrix.m03 = 0.0f;

            this.ViewMatrix.m10 = right.y;
            this.ViewMatrix.m11 = up.y;
            this.ViewMatrix.m12 = look.y;
            this.ViewMatrix.m13 = 0.0f;

            this.ViewMatrix.m20 = right.z;
            this.ViewMatrix.m21 = up.z;
            this.ViewMatrix.m22 = look.z;
            this.ViewMatrix.m23 = 0.0f;

            this.ViewMatrix.m30 = x;
            this.ViewMatrix.m31 = y;
            this.ViewMatrix.m32 = z;
            this.ViewMatrix.m33 = 1.0f;
            this.CameraToWolrdMatrix = this.Camera.cameraToWorldMatrix;
            this.ProjMatrix = Matrix4x4.Perspective(this.Camera.fieldOfView, this.Camera.aspect, this.Camera.nearClipPlane, this.FarClipPlane);
        }

        public void Update(float fDeltaTime)
        {
            if (this.Camera == null) return;

            this.UpdateViewProjMatrix();
            this.Frustum.ExtractFromClipMatrix(this.ViewMatrix * this.ProjMatrix*this.CameraToWolrdMatrix);
        }
    }
}
