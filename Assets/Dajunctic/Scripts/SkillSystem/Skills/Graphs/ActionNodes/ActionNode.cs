using System.Collections;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    /// <summary>
    /// A node type that instantiated every time it is triggered
    /// Life cycle: Create copy => Trigger => Despawn
    /// </summary>
    public abstract class ActionNode : Node, IActionNode
    {
        [SerializeField]
        int priority = 0;

        [SerializeField, Output(ShowBackingValue.Never)]
        IActionNode self;

        public IAbilityOwner Owner { get; private set; }
        protected int Skin;
        protected IActionNodeSystem ActionNodeSystem;

        public long InstanceId { get; set; }
        public int Priority => priority;
        public IActionNode Blueprint { get; set; }
        public bool IsInitialized { get; private set; }

        Coroutine _coroutine;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(self))
            {
                return this;
            }

            return null;
        }

        public IActionNode CreateCopy()
        {
            var copy = Instantiate(this);
            copy.Owner = Owner;
            copy.graph = graph;
            copy.Skin = Owner.Skin;
            return copy;
        }

        public virtual void SetOwner(IAbilityOwner owner)
        {
            Owner = owner;
            Skin = owner.Skin;
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;

            ActionNodeSystem = this.GetSystem<IActionNodeSystem>();

            InitializeInternal();
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

