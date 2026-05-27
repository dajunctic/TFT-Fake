using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public static class Extensions
    {
        public static Vector2 ToV2(this Vector3 v) => new Vector2(v.x, v.z);
        public static (Rect, Rect) HFixedSplit(this Rect r, float w) => (r, r);
        public static object FirstOrDefault(this GraphProcessor.BaseGraph graph, Func<object, bool> predicate) => null;
        public static float EnergyRegen(this List<object> obj) => 0f;
        public static float Regen(this List<object> obj) => 0f;
        public static bool Invincible(this List<object> obj) => false;
        public static void PlayAnimation(this object obj, string anim) {}
        public static bool IsPlayAnimation(this object obj, string anim) => false;
    }
}
