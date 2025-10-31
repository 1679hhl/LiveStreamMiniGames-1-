using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    [System.Serializable]
    public class Line
    {
        public Vector3 Up   = Vector3.up;
        public Vector3 Pos1 = Vector3.zero;
        public Vector3 Pos2 = Vector3.one * 10.0f;

        public static bool LineIntersectLine(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            if (!(Mathf.Min(a.x, b.x) <= Mathf.Max(c.x, d.x) && 
                  Mathf.Min(c.z, d.z) <= Mathf.Max(a.z, b.z) && 
                  Mathf.Min(c.x, d.x) <= Mathf.Max(a.x, b.x) && 
                  Mathf.Min(a.z, b.z) <= Mathf.Max(c.z, d.z)))
                return false;
            /*
            跨立实验：
            如果两条线段相交，那么必须跨立，就是以一条线段为标准，另一条线段的两端点一定在这条线段的两段
            也就是说a b两点在线段cd的两端，c d两点在线段ab的两端
            */
            float u, v, w, z;//分别记录两个向量
            u = (c.x - a.x) * (b.z - a.z) - (b.x - a.x) * (c.z - a.z);
            v = (d.x - a.x) * (b.z - a.z) - (b.x - a.x) * (d.z - a.z);
            w = (a.x - c.x) * (d.z - c.z) - (d.x - c.x) * (a.z - c.z);
            z = (b.x - c.x) * (d.z - c.z) - (d.x - c.x) * (b.z - c.z);
            return (u * v <= 0.00000001f && w * z <= 0.00000001f);
        }

        private static Vector3 temp1 = Vector3.zero;
        public static bool LineIntersectCircle(Vector3 rPos1, Vector3 rPos2, Vector3 rCenter, float fRadius)
        {
            var fADistSqrt = Vector3.SqrMagnitude(rPos1 - rCenter);
            var fBDistSqrt = Vector3.SqrMagnitude(rPos2 - rCenter);

            if (fADistSqrt <= fRadius * fRadius || fBDistSqrt <= fRadius * fRadius)
            {
                return true;
            }

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

            if (a * a + b * b < 1e-5f) return false; // 说明是一个点，并且这个点没在圆内

            if (Mathf.Abs(a * rCenter.x + b * rCenter.z + c) < 1e-13f)    // 点在直线上
            {
                return false;
            }
            else
            {
                float newX = (b * b * rCenter.x - a * b * rCenter.z - a * c) / (a * a + b * b);
                float newY = (-a * b * rCenter.x + a * a * rCenter.z - b * c) / (a * a + b * b);
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

                // 判断垂足是否在圆内
                var fDistNewSqrt = Vector3.SqrMagnitude(rCenter - temp1);
                return fDistNewSqrt <= fRadius * fRadius;
            }
        }

        public static bool RectIntersect(Rect r1, Rect r2)
        {
            if (Mathf.Abs((r1.xMin + r1.xMax) / 2 - (r2.xMin + r2.xMax) / 2) < ((r1.xMax + r2.xMax - r1.xMin - r2.xMin) / 2) && 
                Mathf.Abs((r1.yMin + r1.yMax) / 2 - (r2.yMin + r2.yMax) / 2) < ((r1.yMax + r2.yMax - r1.yMin - r2.yMin) / 2))
                return true;
            return false;
        }
    }
}
