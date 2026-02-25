using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public static class SkillHelper
    {
        public static void ScanTargetInRadius(IDamageTaker finder, float radius, out List<IDamageTaker> foundActors)
        {
             ScanTargetInRadius(finder, finder.AsTransform().Position, radius, out foundActors);
        }

        public static void ScanTargetInRadius(IDamageTaker finder, Vector3 position, float radius, out List<IDamageTaker> foundActors)
        {
            var colliders = Physics.OverlapSphere(position, radius, LayerMask.GetMask(Gameplay.HeroLayerName));
            foundActors = new List<IDamageTaker>();

            foreach (var collider in colliders)
            {
                var actor = collider.gameObject.GetComponent<IDamageTaker>();
                
                if (actor != null && actor != finder && actor.AsCombatActor().Team != finder.AsCombatActor().Team && actor.AsCombatActor().IsTargetable)
                {
                    foundActors.Add(actor);
                }
            }
        }

        public static IDamageTaker FindNearestTarget(Vector3 finderPos, List<IDamageTaker> candidates)
        {
            IDamageTaker bestTarget = null;
            float minSqrDist = Mathf.Infinity;

            foreach (var actor in candidates)
            {
                // if (actor == null) continue;
                if (actor == null || !actor.AsGameObject().ActiveInHierarchy) continue;
                float sqrDist = (finderPos - actor.AsTransform().Position).sqrMagnitude;
                if (sqrDist < minSqrDist)
                {
                    minSqrDist = sqrDist;
                    bestTarget = actor;
                }
            }
            return bestTarget;
        }

        public static bool IsInAttackRange(IDamageTaker attacker, IDamageTaker target, float skillRange)
        {
            if (target == null) return false;
            float dist = Vector3.Distance(attacker.AsTransform().Position, target.AsTransform().Position);
            return dist <= skillRange;
        }
    }
}