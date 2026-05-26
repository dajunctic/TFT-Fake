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
        private static readonly MockActionNodeSystem _mockActionNodeSystem = new MockActionNodeSystem();

        public static T GetSystem<T>(this object obj) where T : class {
            if (typeof(T) == typeof(IActionNodeSystem))
            {
                return _mockActionNodeSystem as T;
            }
            return null;
        }
        public static void ListenEvent(this object obj, object e) {}
        public static void StopListenEvent(this object obj, object e) {}
        public static void SendEvent(this object obj, object e) {}
        public static T GetData<T>(this object obj, string id) => default;
    }

    public class MockActionNodeSystem : IActionNodeSystem
    {
        public void Despawn(Dajunctic.SkillSystem.Logic.IActionNode node)
        {
            if (node is Dajunctic.SkillSystem.Logic.ActionNode actionNode)
            {
                actionNode.Cleanup();
            }
        }

        public Dajunctic.SkillSystem.Logic.IActionNode[] CreateActionNodes(object graph, object nodes = null)
        {
            if (graph is Dajunctic.SkillSystem.Logic.IActionNode singleNode)
            {
                var copy = singleNode.CreateCopy();
                if (copy is Dajunctic.SkillSystem.Logic.ActionNode actionNode)
                {
                    actionNode.Initialize();
                }
                return new Dajunctic.SkillSystem.Logic.IActionNode[] { copy };
            }
            if (graph is Dajunctic.SkillSystem.Logic.IActionNode[] arr)
            {
                var results = new System.Collections.Generic.List<Dajunctic.SkillSystem.Logic.IActionNode>();
                foreach (var n in arr)
                {
                    if (n != null)
                    {
                        var copy = n.CreateCopy();
                        if (copy is Dajunctic.SkillSystem.Logic.ActionNode actionNode)
                        {
                            actionNode.Initialize();
                        }
                        results.Add(copy);
                    }
                }
                return results.ToArray();
            }
            if (graph is System.Collections.IEnumerable enumerable)
            {
                var results = new System.Collections.Generic.List<Dajunctic.SkillSystem.Logic.IActionNode>();
                foreach (var item in enumerable)
                {
                    if (item is Dajunctic.SkillSystem.Logic.IActionNode n)
                    {
                        var copy = n.CreateCopy();
                        if (copy is Dajunctic.SkillSystem.Logic.ActionNode actionNode)
                        {
                            actionNode.Initialize();
                        }
                        results.Add(copy);
                    }
                }
                return results.ToArray();
            }
            return new Dajunctic.SkillSystem.Logic.IActionNode[0];
        }
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