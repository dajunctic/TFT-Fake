using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class FindTargetNode : Node
    {
        private float _range;

        public FindTargetNode(CombatActor actor, float range) : base(actor)
        {
            _range = range;
        }

        public override NodeState Evaluate()
        {
            if (combatActor.HasValidTarget())
            {
                var currentTarget = combatActor.CurrentTarget; 
                if (Vector3.Distance(combatActor.Position, currentTarget.Position) <= _range * 1.5f)
                {
                    return NodeState.Success;
                }
            }

            SkillHelper.ScanTargetInRadius(combatActor, _range, out List<CombatActor> potentialTargets);

            CombatActor bestTarget = SkillHelper.FindNearestTarget(combatActor.Position, potentialTargets);

            if (bestTarget != null)
            {
                combatActor.SetTarget(bestTarget);
                
                if (SquadManager.Instance != null)
                {
                    SquadManager.Instance.SetSharedTarget(bestTarget);
                }

                return NodeState.Success;
            }
            
            combatActor.SetTarget(null);
            return NodeState.Failure;
        }
    }
}