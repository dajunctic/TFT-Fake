using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    public static class MathUtils
    {
        public static Vector3 GetRandomPointInRadius(Vector3 center, float radius)
        {
            return new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)) + center;
        }

        public static bool InRange(Vector3 c, Vector3 a, float r, bool ignoreY = true)
        {
            return SqrDistance(c, a, ignoreY) <= r * r;
        }

        public static float SqrDistance(Vector3 a, Vector3 b, bool ignoreY = true)
        {
            var d = a - b;
            return d.x * d.x + d.z * d.z + (ignoreY ? 0 : d.y * d.y);
        }

        public static float Distance(Vector3 a, Vector3 b, bool ignoreY = true)
        {
            return Mathf.Sqrt(SqrDistance(a, b, ignoreY));
        }

        public static bool IsCircleAndArc2DIntersection(Vector2 p1, float r1, Vector2 p2, float r2, Vector2 dir, float angle)
        {
            Debug.Log("Not Implement");
            return false;
        }
        public static bool IsCircleAndRectangle2DIntersection(Vector2 p1, float r1, Vector2 p2, Vector2 dir, float w, float h)
        {
            Debug.Log("Not Implement");
            return false;
        }
        public static bool IsCircleAndRectangle2DIntersection(Vector2 p1, float r1, Vector2 p2, Vector2 size, Vector2 dir)
        {
            Debug.Log("Not Implement");
            return false;   
        }
        public static Vector3 RandomInCircle(float radius)
        {
            Debug.Log("Not Implement");
            return Vector3.zero;
        }
        public static Vector3 RandomInCircle(Vector3 center, float radius)
        {
            Debug.Log("Not Implement");
            return Vector3.zero;
        }
    }
}
