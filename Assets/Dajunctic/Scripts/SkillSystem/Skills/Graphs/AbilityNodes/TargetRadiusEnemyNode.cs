using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class TargetRadiusEnemyNode : AbilityNode
    {

        [SerializeField] protected float radius;
        [SerializeReference, Output] protected List<IDamageTaker> targets;
        [SerializeReference, Output] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets;
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
