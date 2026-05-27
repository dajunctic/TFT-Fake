using UnityEngine;

namespace Dajunctic
{
    public static class DebugUtils
    {
        public static void DrawCircle(Vector3 p, float r, Color c, float d) {}
        public static void DrawArc(Vector3 p, Vector3 dir, float a, float r, Color c, float d) {}
        public static void DrawBox(Vector3 p, Vector3 dir, float w, float h, Color c, float d) {}
        public static void DrawWireBox(Vector3 p, Quaternion rot, Vector3 size, Color c, float d) {}
        public static void DrawWireBox(Vector2 p, Quaternion rot, Vector3 size, Color c, float d) {}
        public static void DrawWireSphere(Vector2 p, float r, Color c, float d) {}
    }
}
