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
            Debug.LogError("TargetNearestEnemyNode: _cachedTargets.Count" + _cachedTargets.Count);

            _cachedMainTarget = SkillHelper.FindNearestTarget(Owner.AsTransform().Position, _cachedTargets, enableDebug: true);
            
            _cachedTargets.Clear();
            if (_cachedMainTarget != null)
            {
                _cachedTargets.Add(_cachedMainTarget);
            }

            Debug.LogError(_cachedTargets.Count);
            
            return _cachedMainTarget;
        }
    }
}
