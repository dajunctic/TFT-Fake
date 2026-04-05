using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public abstract class TargetNode : SkillNode
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        [XNode.Node.OutputAttribute] public List<IDamageTaker> allTargets;
        [XNode.Node.OutputAttribute] public IDamageTaker mainTarget;

        [SerializeField] protected float radius = 5f;
        [SerializeField] protected Vector3 offset;
        [SerializeField] protected TargetType targetType;
        [SerializeField] protected bool targetAll = true;
        [SerializeField] protected int count = 1;

        private IDamageTaker _mainTarget;
        private List<IDamageTaker> _allTargets;

        protected float Range => radius;
        protected Vector3 OwnerOffset => Owner != null ? Owner.AsTransform().TransformPoint(offset) : offset;
        protected float OwnerRadius => Owner != null ? Owner.AsCombatActor().CombatRadius : 0f;

        public IDamageTaker UpdateTargets()
        {
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();
            _allTargets.Clear();
            FindAllTargets(ref _mainTarget, _allTargets, radius);
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

        public override object GetValue(NodePort port)
        {
            if (_allTargets == null) _allTargets = new List<IDamageTaker>();

            // Re-calculate targets to ensure data is fresh
            FindAllTargets(ref _mainTarget, _allTargets, Range);

            // If current main target is lost or invalid, pick a new one
            if (_mainTarget == null || !_allTargets.Contains(_mainTarget))
            {
                _mainTarget = GetOtherMainTarget(_allTargets);
            }

            allTargets = _allTargets;
            mainTarget = _mainTarget;

            if (port.fieldName == nameof(allTargets)) return allTargets;
            if (port.fieldName == nameof(mainTarget)) return mainTarget;
            return null;
        }

        protected abstract bool IsCurrentMainTargetIsValid(IDamageTaker currentMainTarget, float range);
        protected abstract void FindAllTargets(ref IDamageTaker currentMainTarget, List<IDamageTaker> allTargets, float range);
        protected abstract IDamageTaker GetOtherMainTarget(List<IDamageTaker> allTargets);

    }
}
