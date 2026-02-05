using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public static class SkillHelper
    {
        public static void ScanTargetInRadius(Vector3 finderPos, float radius, LayerMask targetLayer, out List<CombatActor> foundActors)
        {
             var colliders = Physics.OverlapSphere(finderPos, radius, targetLayer);
            foundActors = new List<CombatActor>();

            foreach (var collider in colliders)
            {
                var actor = collider.gameObject.GetComponent<CombatActor>();
                
                if (actor != null)
                {
                    foundActors.Add(actor);
                }
            }
        }

        public static CombatActor FindNearestTarget(Vector3 finderPos, List<CombatActor> candidates)
        {
            CombatActor bestTarget = null;
            float minSqrDist = Mathf.Infinity;

            foreach (var actor in candidates)
            {
                if (actor == null || !actor.gameObject.activeInHierarchy) continue;
                float sqrDist = (finderPos - actor.Position).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    bestTarget = actor;
                }
            }
            return bestTarget;
        }

        public static bool IsInAttackRange(CombatActor attacker, CombatActor target, float skillRange)
        {
            if (target == null) return false;
            float dist = Vector3.Distance(attacker.Position, target.Position);
            return dist <= skillRange;
        }
    }
}