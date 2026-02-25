using UnityEngine;
using System;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class SkillNode : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 gridPosition;
        [HideInInspector] public SkillGraph graph;

        protected SkillExecutionContext _context;
        protected ISkillOwner owner;
        private Action _onComplete;

        public void Init(SkillExecutionContext context, Action onComplete)
        {
            _context = context;
            _onComplete = onComplete;
            owner = _context.actor;
            OnInit();
        }

        protected virtual void OnInit() { }

        public virtual void Execute()
        {
            TriggerComplete();
        }

        public virtual IEnumerator IECoroutine()
        {
            yield return null;
        }

        public void TriggerComplete()
        {
            _onComplete?.Invoke();
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
            return null;
        }

        /// <summary>
        /// Pulls a value from the node connected to the specified input port.
        /// </summary>
        protected T GetInputValue<T>(string portName, T defaultValue = default)
        {
            if (graph == null) return defaultValue;

            // Find connection to this port
            var link = graph.links.Find(l => l.targetNodeGuid == guid && l.targetPortName == portName);
            if (link == null) return defaultValue;

            // Find source node
            var sourceNode = graph.nodes.Find(n => n.guid == link.baseNodeGuid);
            if (sourceNode == null) return defaultValue;

            // Pull value from source
            object value = sourceNode.GetValue(link.portName);
            if (value is T tValue) return tValue;

            return defaultValue;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeInputAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class NodeOutputAttribute : Attribute { }
}
