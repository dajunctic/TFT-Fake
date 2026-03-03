using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public abstract class TargetNode : SkillNode
    {
        [NodeOutput] public List<IDamageTaker> allTargets;
        [NodeOutput] public IDamageTaker mainTarget;

        [SerializeField] protected float radius = 5f;
        [SerializeField] protected Vector3 offset;
        [SerializeField] protected TargetType targetType;
        [SerializeField] protected bool targetAll = true;
        [ShowIf("@targetAll==false")] protected int count = 1;

        private IDamageTaker _mainTarget;
        private List<IDamageTaker> _allTargets;

        protected float Range => radius;
        protected Vector3 OwnerOffset => Owner.AsTransform().TransformPoint(offset);
        protected float OwnerRadius => Owner.AsCombatActor().CombatRadius;

        public IDamageTaker UpdateTargets()
        {
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();
            _allTargets.Clear();
            FindAllTargets(_mainTarget, _allTargets, radius);
            if (_mainTarget == null || !_allTargets.Contains(_mainTarget))
            {
                _mainTarget = GetOtherMainTarget(_allTargets);
            }
            allTargets = _allTargets;
            mainTarget = _mainTarget;
            return _mainTarget;
        }

        public void ClearTargets()
        {
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();
            _allTargets.Clear();
            _mainTarget = null;
            allTargets = _allTargets;
            mainTarget = _mainTarget;
        }

        public override void Reset()
        {
            base.Reset();
            _allTargets = null;
            _mainTarget = null;
            allTargets = null;
            mainTarget = null;
        }

        protected override void OnInit()
        {
            base.OnInit();
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();
            ClearTargets();
        }

        public override object GetValue(string portName)
        {
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();
            FindAllTargets(_mainTarget, _allTargets, Range);
            allTargets = _allTargets;
            mainTarget = _mainTarget;

            if (string.Equals(portName, nameof(allTargets), StringComparison.OrdinalIgnoreCase)) return allTargets;
            if (string.Equals(portName, nameof(mainTarget), StringComparison.OrdinalIgnoreCase)) return mainTarget;
            return base.GetValue(portName);
        }

        protected abstract bool IsCurrentMainTargetIsValid(IDamageTaker currentMainTarget, float range);
        protected abstract void FindAllTargets(IDamageTaker currentMainTarget, List<IDamageTaker> allTargets, float range);
        protected abstract IDamageTaker GetOtherMainTarget(List<IDamageTaker> allTargets);

        public void BindTarget(IDamageTaker target)
        {
            _mainTarget = target;
        }
    }
}