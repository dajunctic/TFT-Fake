using System.Collections;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;
using System.Collections.Generic;

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
        public void Despawn(IActionNode node)
        {
            if (node is ActionNode actionNode)
            {
                actionNode.Cleanup();
            }
        }

        public IActionNode[] CreateActionNodes(object graph, object nodes = null)
        {
            if (graph is IActionNode singleNode)
            {
                var copy = singleNode.CreateCopy();
                if (copy is ActionNode actionNode)
                {
                    actionNode.Initialize();
                }
                return new IActionNode[] { copy };
            }
            if (graph is IActionNode[] arr)
            {
                var results = new List<IActionNode>();
                foreach (var n in arr)
                {
                    if (n != null)
                    {
                        var copy = n.CreateCopy();
                        if (copy is ActionNode actionNode)
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
                var results = new List<IActionNode>();
                foreach (var item in enumerable)
                {
                    if (item is IActionNode n)
                    {
                        var copy = n.CreateCopy();
                        if (copy is ActionNode actionNode)
                        {
                            actionNode.Initialize();
                        }
                        results.Add(copy);
                    }
                }
                return results.ToArray();
            }
            return new IActionNode[0];
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
