using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/TargetRadiusEnemy")]
    public class TargetRadiusEnemyNode : AbilityNode
    {

        [SerializeField] protected float radius;
        [SerializeReference, Output] protected List<IDamageTaker> targets;
        [SerializeReference, Output] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets;
        protected IDamageTaker _cachedMainTarget;

        protected IDamageTaker GetMainTarget()
        {
            FindTargetInRadius();
            _cachedMainTarget = SkillHelper.FindNearestTarget(Owner.AsTransform().Position, _cachedTargets);
            return _cachedMainTarget;
        }

        protected void FindTargetInRadius()
        {
            // If Need Refresh Target List

            SkillHelper.FindTargetsInRadius(Owner.AsTeamMember().EnemyTeam, Owner.AsTransform().Position, 1.5f, radius, null, _cachedTargets, 5);
        }

    }
}
