using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core.Math
{
    public class Frustum
    {
        public List<Plane> Planes;

        public Frustum()
        {
            this.Planes = new List<Plane>();
            for (int i = 0; i < 6; i++)
            {
                this.Planes.Add(new Plane());
            }
        }

        public void ExtractFromClipMatrix(Matrix4x4 clipMatrix)
        {
            this.Planes[0].Set(	//left plane
			    clipMatrix.m03 + clipMatrix.m00,
			    clipMatrix.m13 + clipMatrix.m10,
			    clipMatrix.m23 + clipMatrix.m20,
			    clipMatrix.m33 + clipMatrix.m30);

            this.Planes[1].Set(	//right plane
		    	clipMatrix.m03 - clipMatrix.m00,
		    	clipMatrix.m13 - clipMatrix.m10,
		    	clipMatrix.m23 - clipMatrix.m20,
		    	clipMatrix.m33 - clipMatrix.m30);

            this.Planes[2].Set(	//top plane
		    	clipMatrix.m03 - clipMatrix.m01,
		    	clipMatrix.m13 - clipMatrix.m11,
		    	clipMatrix.m23 - clipMatrix.m21,
		    	clipMatrix.m33 - clipMatrix.m31);

            this.Planes[3].Set(	//bottom plane
		    	clipMatrix.m03 + clipMatrix.m01,
		    	clipMatrix.m13 + clipMatrix.m11,
		    	clipMatrix.m23 + clipMatrix.m21,
		    	clipMatrix.m33 + clipMatrix.m31);

            this.Planes[4].Set(	//near plane
		    	clipMatrix.m02,
		    	clipMatrix.m12,
		    	clipMatrix.m22,
		    	clipMatrix.m32);

            this.Planes[5].Set(	//far plane
		    	clipMatrix.m03 - clipMatrix.m02,
		    	clipMatrix.m13 - clipMatrix.m12,
		    	clipMatrix.m23 - clipMatrix.m22,
		    	clipMatrix.m33 - clipMatrix.m32);

		    for (int i = 0; i < 6; ++i)
		    {
                this.Planes[i].Normalize();
		    }
        }

        public bool TestCube(AABB cube)
        {
            if (!AABB.Plane3DTestCube(Planes[0], cube) ||
                !AABB.Plane3DTestCube(Planes[1], cube) ||
                !AABB.Plane3DTestCube(Planes[2], cube) ||
                !AABB.Plane3DTestCube(Planes[3], cube) ||
                !AABB.Plane3DTestCube(Planes[4], cube) ||
                !AABB.Plane3DTestCube(Planes[5], cube))
            {
                return false;
            }
            return true;
        }
    }
}
