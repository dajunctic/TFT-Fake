using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public abstract class TargetNode: SkillNode
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
            _allTargets.Clear();
            FindAllTargets(_mainTarget, _allTargets, radius);
            if (_mainTarget == null || !_allTargets.Contains(_mainTarget))
            {
                _mainTarget = GetOtherMainTarget(_allTargets);
            }
            return _mainTarget;
        }

         public void ClearTargets()
        {
            _allTargets.Clear();
            _mainTarget = null;
        }

        public override void Reset()
        {
            base.Reset();
            _allTargets = null;
            _mainTarget = null;
        }

       

        protected override void OnInit()
        {
            base.OnInit();
            ClearTargets();
        }

        public override object GetValue(string portName)
        {
            FindAllTargets(_mainTarget, _allTargets, Range);

            if (portName == nameof(allTargets)) return allTargets;
            if (portName == nameof(mainTarget)) return mainTarget;
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