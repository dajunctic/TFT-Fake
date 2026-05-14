using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    /// <summary>
    /// A node type that only run 1 time each time <see cref="IAbilityEntity"/> play.
    /// All inNode must be completed to run.
    /// Notify all outNode when completed.
    /// </summary>
    public abstract class AbilityNode
        : Node,
            ICanTick,
            ICanSendEvent,
            ICanListenEvent,
            ICanGetSystem
    {
        [SerializeReference, Output]
        AbilityNode self;

        [SerializeReference, Input]
        AbilityNode inNode;

        [SerializeReference, Output]
        AbilityNode outNode;

        protected IAbilityOwner Owner;
        protected int Skin;
        protected IActionNodeSystem ActionNodeSystem;
        protected IAbilityEntity Ability;

        List<AbilityNode> _inNodes;
        List<AbilityNode> _outNodes;
        int _inNodeCompletedCount;

        Coroutine _coroutine;
        bool _isPlaying;

#if UNITY_EDITOR
        protected virtual void OnValidate() { }
#endif

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(self))
            {
                return this;
            }
            return null;
        }

        public virtual void SetOwner(IAbilityOwner owner)
        {
            Owner = owner;
            Skin = owner.Skin;
        }

        public void SetAbility(IAbilityEntity ability)
        {
            Ability = ability;
        }

        public void Initialize()
        {
            ActionNodeSystem = this.GetSystem<IActionNodeSystem>();
            _inNodes = GetInputPort(nameof(inNode))
                .GetConnections()
                .Select(port => port.node)
                .OfType<AbilityNode>()
                .ToList();
            _outNodes = GetOutputPort(nameof(outNode))
                .GetConnections()
                .Select(port => port.node)
                .OfType<AbilityNode>()
                .ToList();
            InitializeInternal();
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

        /// <summary>
        /// Always call when a ability stop
        /// </summary>
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

