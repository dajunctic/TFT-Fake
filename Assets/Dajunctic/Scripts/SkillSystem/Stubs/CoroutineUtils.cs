using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem {
    public static class CoroutineExtensions {
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