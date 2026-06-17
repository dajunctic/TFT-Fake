using System;
using System.Linq;
using Dajunctic.SkillSystem.Data;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    public class AbilityGraph<TAbilityEntity, TAbilityEntityData, TAbilityLevelData, TOwner>
        : BaseGraph
        where TAbilityEntity : IAbilityEntity<TAbilityEntityData, TAbilityLevelData, TOwner>
        where TAbilityEntityData : IAbilityEntityData<TAbilityLevelData>
        where TAbilityLevelData : AbilityLevelData
        where TOwner : IAbilityOwner
    {
        public event Action OnExitEvent;

        TAbilityEntity _ability;
        AbilityNode[] _nodes;
        ActionNode[] _actionNodes;
        PropertyNode[] _propertyNodes;
        EntryNode _entry;
        ExitNode _exit;
        bool _isPlaying;

        public void SetOwner(TOwner owner)
        {
            foreach (var node in _nodes)
            {
                node.SetOwner(owner);
            }

            foreach (var node in _actionNodes)
            {
                node.SetOwner(owner);
            }

            foreach (var node in _propertyNodes)
            {
                node.SetOwner(owner);
            }
        }

        public void SetAbility(TAbilityEntity ability)
        {
            _ability = ability;
            foreach (var node in _nodes)
            {
                node.SetAbility(_ability);
            }

            foreach (var node in _propertyNodes)
            {
                node.SetAbility(_ability);
            }
        }

        public virtual void Initialize()
        {
            _isPlaying = false;

            _nodes = nodes.OfType<AbilityNode>().ToArray();
            _actionNodes = nodes.OfType<ActionNode>().ToArray();
            _propertyNodes = nodes.OfType<PropertyNode>().ToArray();
            _entry = nodes.OfType<EntryNode>().FirstOrDefault();
            _exit = nodes.OfType<ExitNode>().FirstOrDefault();

            foreach (var node in _nodes)
            {
                node.Initialize();
            }

            foreach (var node in _propertyNodes)
            {
                node.Initialize();
            }
        }

        public void Play(IDamageTaker target = null)
        {
            if (_isPlaying)
                return;
            _isPlaying = true;

            foreach (var node in _nodes)
            {
                node.Reset();
            }

            foreach (var node in _propertyNodes)
            {
                node.Reset();
            }

            BindTarget(target);

            ListenExitEvent();
            _entry.Play();
        }

        protected virtual void BindTarget(IDamageTaker target) { }

        public void Stop()
        {
            if (!_isPlaying)
                return;
            _isPlaying = false;

            StopListenExitEvent();
            StopInternal();
            foreach (var node in _nodes)
            {
                node.Stop();
                node.OnAbilityStop();
            }
        }

        protected virtual void StopInternal() { }

        public void Cleanup()
        {
            StopListenExitEvent();
            foreach (var node in _nodes)
            {
                node.Cleanup();
            }

            foreach (var node in _propertyNodes)
            {
                node.Cleanup();
            }
            _propertyNodes = null;
            _nodes = null;
            _actionNodes = null;
        }

        void OnExit()
        {
            StopListenExitEvent();
            OnExitEvent?.Invoke();
        }

        void ListenExitEvent()
        {
            if (_exit != null)
            {
                _exit.OnExitEvent += OnExit;
            }
        }

        void StopListenExitEvent()
        {
            if (_exit != null)
            {
                _exit.OnExitEvent -= OnExit;
            }
        }
    }
}
