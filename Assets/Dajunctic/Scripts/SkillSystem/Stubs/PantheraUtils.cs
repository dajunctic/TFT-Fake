using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Panthera {
    public static class PantheraExtensions {
        public static Coroutine StartGlobalCoroutine(this object obj, IEnumerator routine) {
            return CoroutineRunner.Instance.StartCoroutine(routine);
        }
        public static void StopGlobalCoroutine(this object obj, Coroutine routine) {
            if(routine != null) CoroutineRunner.Instance.StopCoroutine(routine);
        }
        public static T GetSystem<T>(this object obj) where T : class {
            return null;
        }
        public static void ListenEvent(this object obj, object e) {}
        public static void StopListenEvent(this object obj, object e) {}
        public static void SendEvent(this object obj, object e) {}
        public static T GetData<T>(this object obj, string id) => default;
    }

    public class CoroutineRunner : MonoBehaviour {
        private static CoroutineRunner _instance;
        public static CoroutineRunner Instance {
            get {
                if (_instance == null) {
                    var go = new GameObject("CoroutineRunner");
                    _instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }
    }
}

namespace Dajunctic.SkillSystem.Panthera.Utils {
    public static class MathUtils {
        public static float SqrDistance(Vector3 a, Vector3 b) => (a - b).sqrMagnitude;
        public static bool InRange(float val, float min, float max) => val >= min && val <= max;
        public static bool InRange(Vector3 a, Vector3 b, float dist) => (a - b).sqrMagnitude <= dist * dist;
        public static bool IsCircleAndArc2DIntersection(Vector2 p1, float r1, Vector2 p2, float r2, Vector2 dir, float angle) => false;
        public static bool IsCircleAndRectangle2DIntersection(Vector2 p1, float r1, Vector2 p2, Vector2 dir, float w, float h) => false;
        public static bool IsCircleAndRectangle2DIntersection(Vector2 p1, float r1, Vector2 p2, Vector2 size, Vector2 dir) => false;
        public static Vector3 RandomInCircle(float radius) => Vector3.zero;
        public static Vector3 RandomInCircle(Vector3 center, float radius) => Vector3.zero;
    }
    public static class ReflectionUtils {
        public static List<Type> GetAllTypes(Type baseType) => new List<Type>();
        public static List<Type> GetAllTypes<T>() => new List<Type>();
    }
}
