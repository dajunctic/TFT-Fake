using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/TargetNearestEnemy")]
    public class TargetNearestEnemyNode : TargetRadiusEnemyNode
    {
        protected override IDamageTaker GetMainTarget()
        {
            FindTargetInRadius();
            _cachedMainTarget = SkillHelper.FindNearestTarget(Owner.AsTransform().Position, _cachedTargets);
            
            _cachedTargets.Clear();
            if (_cachedMainTarget != null)
            {
                _cachedTargets.Add(_cachedMainTarget);
            }
            
            return _cachedMainTarget;
        }
    }
}
