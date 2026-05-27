using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{

    [System.Serializable]
    public abstract class AbilityNode: BaseNode
    {
        [GraphProcessor.Output(name = "Self")]
        public AbilityNode self;

        [GraphProcessor.Input(name = "In", allowMultiple = true)]
        public AbilityNode inNode;

        [GraphProcessor.Output(name = "Out")]
        public AbilityNode outNode;

        protected IAbilityOwner Owner;
        protected int Skin;
        protected IActionNodeSystem ActionNodeSystem;
        protected IAbilityEntity Ability;

        List<AbilityNode> _inNodes;
        List<AbilityNode> _outNodes;
        int _inNodeCompletedCount;

        Coroutine _coroutine;
        bool _isPlaying;

        public override string name => GetType().Name;

#if UNITY_EDITOR
        protected virtual void OnValidate() { }
#endif

        protected override void Enable()
        {
            base.Enable();
            self = this;
        }

        public virtual void SetOwner(IAbilityOwner owner)
        {
            Owner = owner;
            if (owner != null) Skin = owner.Skin;
        }

        public void SetAbility(IAbilityEntity ability)
        {
            Ability = ability;
        }

        public void Initialize()
        {
            ActionNodeSystem = this.GetSystem<IActionNodeSystem>();
            var inPort = inputPorts.FirstOrDefault(p => p.fieldName == nameof(inNode));
            _inNodes = inPort?.GetEdges().Select(e => e.outputNode as AbilityNode).Where(n => n != null).ToList() ?? new List<AbilityNode>();

            var outPort = outputPorts.FirstOrDefault(p => p.fieldName == nameof(outNode));
            _outNodes = outPort?.GetEdges().Select(e => e.inputNode as AbilityNode).Where(n => n != null).ToList() ?? new List<AbilityNode>();
            InitializeInternal();
        }

        private static void EvaluateIfTargetingNode(object node)
        {
            if (node == null) return;
            var getMethod = node.GetType().GetMethod("GetMainTarget", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (getMethod != null)
            {
                var mainT = getMethod.Invoke(node, null);
                
                var mainTargetField = node.GetType().GetField("mainTarget", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                mainTargetField?.SetValue(node, mainT);
                
                var cachedTargetsField = node.GetType().GetField("_cachedTargets", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var cachedTargetsVal = cachedTargetsField?.GetValue(node);
                
                var targetsField = node.GetType().GetField("targets", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                targetsField?.SetValue(node, cachedTargetsVal);
            }
        }

        protected T GetInputValue<T>(string fieldName, T fallback = default)
        {
            var inPort = inputPorts.FirstOrDefault(p => p.fieldName == fieldName);
            var edge = inPort?.GetEdges().FirstOrDefault();
            if (edge != null && edge.outputPort != null)
            {
                var outputNode = edge.outputNode;
                EvaluateIfTargetingNode(outputNode);
                var outputField = outputNode.GetType().GetField(edge.outputPort.fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (outputField != null)
                {
                    return (T)outputField.GetValue(outputNode);
                }
            }
            return fallback;
        }

        protected T[] GetInputValues<T>(string fieldName, params T[] fallbacks)
        {
            var inPort = inputPorts.FirstOrDefault(p => p.fieldName == fieldName);
            var edges = inPort?.GetEdges();
            if (edges != null && edges.Count > 0)
            {
                return edges.Select(edge => {
                    var outputNode = edge.outputNode;
                    EvaluateIfTargetingNode(outputNode);
                    var outputField = outputNode.GetType().GetField(edge.outputPort.fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (outputField != null)
                    {
                        return (T)outputField.GetValue(outputNode);
                    }
                    return default(T);
                }).ToArray();
            }
            return fallbacks;
        }

        protected virtual void InitializeInternal() { }

        public void Reset()
        {
            _inNodeCompletedCount = 0;
            _isPlaying = false;
            if (Owner != null)
                Skin = Owner.Skin;
            ResetInternal();
        }

        protected virtual void ResetInternal() { }

        public void Play()
        {
            if (_isPlaying)
                return;
            _isPlaying = true;

            PlayInternal();
        }

        protected virtual void PlayInternal() { }

        protected void StartCoroutine()
        {
            _coroutine = this.StartGlobalCoroutine(IECoroutine());
        }

        void StopCoroutine()
        {
            if (_coroutine != null)
            {
                this.StopGlobalCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        protected virtual IEnumerator IECoroutine()
        {
            yield break;
        }

        public void Stop()
        {
            if (!_isPlaying)
                return;
            _isPlaying = false;
            StopCoroutine();
            StopInternal();
        }

        protected virtual void StopInternal() { }

        public virtual void OnAbilityStop() { }

        public void Cleanup()
        {
            Stop();
            CleanupInternal();
            _outNodes = null;
            _inNodes = null;
            ActionNodeSystem = null;
        }

        protected virtual void CleanupInternal() { }

        protected void Completed()
        {
            Stop();
            if (_outNodes != null)
            {
                foreach (var node in _outNodes)
                {
                    node.OnInNodeCompleted(this);
                }
            }
        }

        public virtual void OnInNodeCompleted(AbilityNode node)
        {
            if (_inNodes == null)
            {
                return;
            }
            _inNodeCompletedCount++;
            if (_inNodeCompletedCount >= _inNodes.Count)
            {
                Play();
            }
        }
    }
}
