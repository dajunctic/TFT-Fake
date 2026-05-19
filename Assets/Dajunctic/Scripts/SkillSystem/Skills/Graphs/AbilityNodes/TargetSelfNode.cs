using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class TargetSelfNode : AbilityNode
    {
        [SerializeField, Output(ShowBackingValue.Never)] protected List<IDamageTaker> targets;
        [SerializeField, Output(ShowBackingValue.Never)] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets = new List<IDamageTaker>();
        protected IDamageTaker _cachedMainTarget;

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;
            
            GetMainTarget();

            if (port.fieldName == nameof(targets))
            {
                return _cachedTargets;
            }

            if (port.fieldName == nameof(mainTarget))
            {
                return _cachedMainTarget;
            }

            return base.GetValue(port);
        }

        protected IDamageTaker GetMainTarget()
        {
            _cachedTargets.Clear();
            _cachedMainTarget = Owner.AsDamageTaker();
            if (_cachedMainTarget != null)
            {
                _cachedTargets.Add(_cachedMainTarget);
            }
            return _cachedMainTarget;
        }
    }
}
