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
                
                if (actor != null && actor != finder && actor.AsCombatActor().CombatTeam != finder.AsCombatActor().CombatTeam && actor.AsCombatActor().CanBeTarget)
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

        static bool IsInRange(Vector3 position, float finderRadius, Vector3 targetPosition, float targetRadius, float range, float offset)
        {
            range = GetDistance(finderRadius, targetRadius, range, offset);
            return MathUtils.InRange(position, targetPosition, range);
        }

        public static float GetDistance(float finderRadius, float targetRadius, float range, float offset)
        {
            range += finderRadius + targetRadius + offset;
            return range;
        }

        public static bool IsInAbilityTargetingRange(Vector3 finderPosition, float finderRadius, IDamageTaker target, float abilityRange)
        {
            if (target == null || !target.CanBeTarget)
            {
                return false;
            }

            return IsInRange(finderPosition, finderRadius, target.AsTransform().Position, target.AsCombatActor().CombatRadius,abilityRange,0);
        }
    }
}