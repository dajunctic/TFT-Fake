using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class SkillNode : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 gridPosition;
        [HideInInspector] public SkillGraph graph;

        protected SkillExecutionContext _context;
        protected ISkillOwner Owner { get; set; }
        private Action _onComplete;

        private float _lastEditorTime;
        protected float DeltaTime
        {
            get
            {
                if (Application.isPlaying) return Time.deltaTime;
#if UNITY_EDITOR
                if (_lastEditorTime == 0) _lastEditorTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                float dt = (float)UnityEditor.EditorApplication.timeSinceStartup - _lastEditorTime;
                _lastEditorTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                return dt;
#else
                return 0.02f;
#endif
            }
        }

        public void Init(SkillExecutionContext context, Action onComplete)
        {
            _context = context;
            _onComplete = onComplete;
            Owner = _context.actor;
            OnInit();
        }

        protected virtual void OnInit() { }

        public virtual void Execute()
        {
            Complete();
        }

        protected void StartCoroutine()
        {
            var coroutine = IECoroutine();
            if (coroutine == null) return;

            if (Application.isPlaying)
            {
                this.StartGlobalCoroutine(coroutine);
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.CallbackFunction update = null;
                update = () =>
                {
                    // Pump the coroutine
                    if (!coroutine.MoveNext())
                    {
                        UnityEditor.EditorApplication.update -= update;
                    }
                };
                UnityEditor.EditorApplication.update += update;
#endif
            }
        }

        public virtual IEnumerator IECoroutine()
        {
            yield break;
        }

        public void Complete()
        {
            _onComplete?.Invoke();
        }

        protected void Delay(float duration, Action onComplete = null)
        {
            if (duration <= 0)
            {
                onComplete?.Invoke();
                Complete();
                return;
            }

            if (Application.isPlaying)
            {
                var runner = _context.actor.AsCombatActor()?.GetSkillGraphRunner();
                if (runner != null)
                {
                    runner.StartCoroutine(DelayCoroutine(duration, onComplete));
                }
                else
                {
                    onComplete?.Invoke();
                    Complete();
                }
            }
            else
            {
#if UNITY_EDITOR
                float startTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                UnityEditor.EditorApplication.CallbackFunction update = null;
                update = () =>
                {
                    if ((float)UnityEditor.EditorApplication.timeSinceStartup - startTime >= duration)
                    {
                        UnityEditor.EditorApplication.update -= update;
                        onComplete?.Invoke();
                        Complete();
                    }
                };
                UnityEditor.EditorApplication.update += update;
#endif
            }
        }

        private IEnumerator DelayCoroutine(float duration, Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
            Complete();
        }

        public virtual void Reset()
        {
            _context = null;
            _onComplete = null;
        }

        /// <summary>
        /// Returns the value for a specific output port. Override in subclasses.
        /// </summary>
        public virtual object GetValue(string portName)
        {
            if (string.IsNullOrEmpty(portName)) return null;

            // By default, try to find a field with a matching name (case-insensitive, normalized)
            var field = GetType().GetField(portName.Replace(" ", ""),
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.IgnoreCase);

            if (field != null) return field.GetValue(this);

            // Fallback: iterate through all fields using the helper
            var allFields = GetType().GetFields(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            foreach (var f in allFields)
            {
                if (IsPortNameMatch(portName, f.Name))
                {
                    return f.GetValue(this);
                }
            }

            Debug.LogWarning($"[SkillNode] GetValue('{portName}') failed for node '{name}': No matching field found.");
            return null;
        }

        /// <summary>
        /// Helper to compare port names by ignoring spaces and case.
        /// Useful for subclasses overriding GetValue.
        /// </summary>
        protected bool IsPortNameMatch(string inputPortName, string targetFieldName)
        {
            if (string.IsNullOrEmpty(inputPortName) || string.IsNullOrEmpty(targetFieldName)) return false;
            return string.Equals(inputPortName.Replace(" ", ""), targetFieldName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Pulls a value from the node connected to the specified input port.
        /// </summary>
        protected T GetInputValue<T>(string portName, T defaultValue = default)
        {
            if (graph == null)
            {
                Debug.LogWarning($"[SkillNode] GetInputValue('{portName}') failed: graph is null on node '{name}' ({guid})");
                return defaultValue;
            }

            // Find all connections to this port
            var links = graph.links.FindAll(l =>
                l.targetNodeGuid == guid &&
                (
                    IsPortNameMatch(l.targetPortName, portName) ||
                    IsPortNameMatch(l.portName, portName)
                ));

            if (links == null || links.Count == 0)
            {
                var allIncoming = graph.links.FindAll(l => l.targetNodeGuid == guid);
                string linksInfo = string.Join(", ", allIncoming.Select(l => $"'{l.targetPortName}' from {l.portName}"));
                Debug.Log($"[SkillNode] GetInputValue('{portName}') found 0 links for node '{name}'. Incoming links available: [{linksInfo}]");
                return defaultValue;
            }

            Debug.Log($"[SkillNode] GetInputValue('{portName}') found {links.Count} links for node '{name}'");
            Debug.Log($"[SkillNode] Link: " + string.Join(", ", links.Select(l => $"'{l.targetPortName}' from {l.portName}")));

            // Handle List types by aggregating all connected values
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                var elementType = typeof(T).GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(typeof(T));

                foreach (var link in links)
                {
                    var sourceNode = graph.nodes.Find(n => n.guid == link.baseNodeGuid);
                    if (sourceNode == null) continue;

                    object sourceValue = sourceNode.GetValue(link.portName);
                    if (sourceValue == null) continue;

                    if (elementType.IsInstanceOfType(sourceValue))
                    {
                        list.Add(sourceValue);
                    }
                    else if (sourceValue is IList sourceList)
                    {
                        foreach (var item in sourceList)
                        {
                            if (elementType.IsInstanceOfType(item))
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
                return (T)list;
            }

            // Single value handling
            var firstLink = links[0];
            var firstSourceNode = graph.nodes.Find(n => n.guid == firstLink.baseNodeGuid);
            if (firstSourceNode == null) return defaultValue;

            object value = firstSourceNode.GetValue(firstLink.portName);
            if (value is T tValue) return tValue;

            return defaultValue;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeInputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeOutputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class ActionInput : Attribute { }
}
