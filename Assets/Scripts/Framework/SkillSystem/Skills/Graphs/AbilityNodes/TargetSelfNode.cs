using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/TargetSelf")]
    public class TargetSelfNode : AbilityNode
    {
        [SerializeReference, Output] protected List<IDamageTaker> targets;
        [SerializeReference, Output] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets = new List<IDamageTaker>();
        protected IDamageTaker _cachedMainTarget;

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
