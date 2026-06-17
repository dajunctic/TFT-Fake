using System.Collections;
using System.Reflection;
using UnityEngine;
using GraphProcessor;
using System.Linq;

namespace Dajunctic.SkillSystem.Logic
{

    [System.Serializable]
    public abstract class ActionNode : BaseNode, IActionNode
    {
        [SerializeField]
        int priority = 0;

        [GraphProcessor.Output(name = "Self")]
        public IActionNode self;

        public IAbilityOwner Owner { get; private set; }
        protected int Skin;
        protected IActionNodeSystem ActionNodeSystem;

        public long InstanceId { get; set; }
        public int Priority => priority;
        public IActionNode Blueprint { get; set; }
        public bool IsInitialized { get; private set; }

        Coroutine _coroutine;

        public override string name => GetType().Name;

        protected override void Enable()
        {
            base.Enable();
            self = this;
        }

        public IActionNode CreateCopy()
        {
            var copy = (ActionNode)this.MemberwiseClone();
            copy.Owner = Owner;
            copy.graph = graph;
            if (Owner != null) copy.Skin = Owner.Skin;
            return copy;
        }

        public virtual void SetOwner(IAbilityOwner owner)
        {
            Owner = owner;
            if (owner != null) Skin = owner.Skin;
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            ActionNodeSystem = this.GetSystem<IActionNodeSystem>();

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

        public void Play(object source)
        {
            if (!IsInitialized)
            {
                Debug.LogError($"{GetType()} {InstanceId} is not initialized!");
                return;
            }
            if (Blueprint == null)
            {
                Debug.LogError(
                    $"{GetType()} {InstanceId} want to play while it is a blueprint. Must be a copy!"
                );
                return;
            }

            PrePlayInternal(source);
            PlayInternal(source);
        }

        protected virtual void PrePlayInternal(object source) { }

        protected virtual void PlayInternal(object source) { }

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
            if (!IsInitialized)
            {
                return;
            }

            StopCoroutine();
            StopInternal();
        }

        protected virtual void StopInternal() { }

        public void Cleanup()
        {
            if (!IsInitialized)
            {
                return;
            }
            IsInitialized = false;

            CleanupInternal();

            ActionNodeSystem = null;
        }

        protected virtual void CleanupInternal() { }

        public void TriggerDespawn()
        {
            ActionNodeSystem?.Despawn(this);
        }
    }
}
