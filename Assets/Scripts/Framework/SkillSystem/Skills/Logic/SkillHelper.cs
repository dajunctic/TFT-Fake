#if UNITY_EDITOR

#endif
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public static class SkillHelper
    {
        static IDamageTaker _cachedDamageTaker;

        public static IDamageTaker FindFarthestTarget(
            Vector3 finderPosition,
            List<IDamageTaker> allTargets,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK,
            bool mustInCombat = false
        )
        {
            _cachedDamageTaker = null;
            var maxDist = Mathf.NegativeInfinity;
            for (var i = 0; i < allTargets.Count; i++)
            {
                if (
                    allTargets[i] != null
                    && allTargets[i].CanBeTarget
                    && (!mustInCombat || allTargets[i] is ICombatActorEntity ca && ca.IsCombat)
                )
                {
                    var pos = allTargets[i].Position;
                    if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                    {
                        continue;
                    }
                    var dist =
                        MathUtils.SqrDistance(finderPosition, pos) - allTargets[i].CombatRadius;
                    if (maxDist < dist)
                    {
                        maxDist = dist;
                        _cachedDamageTaker = allTargets[i];
                    }
                }
            }
            return _cachedDamageTaker;
        }

        public static IDamageTaker FindNearestTarget(
            Vector3 finderPosition,
            List<IDamageTaker> allTargets,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK
        )
        {
            _cachedDamageTaker = null;
            var minDist = Mathf.Infinity;
            for (var i = 0; i < allTargets.Count; i++)
            {
                if (allTargets[i] != null && allTargets[i].CanBeTarget)
                {
                    var pos = allTargets[i].Position;
                    if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                    {
                        continue;
                    }
                    var dist =
                        MathUtils.SqrDistance(finderPosition, pos) - allTargets[i].CombatRadius;
                    if (minDist > dist)
                    {
                        minDist = dist;
                        _cachedDamageTaker = allTargets[i];
                    }
                }
            }
            return _cachedDamageTaker;
        }

        public static IDamageTaker FindHighestHpTarget(List<IDamageTaker> allTargets)
        {
            _cachedDamageTaker = null;
            var minHp = Mathf.NegativeInfinity;
            for (var i = 0; i < allTargets.Count; i++)
            {
                if (allTargets[i] != null && allTargets[i].CanBeTarget)
                {
                    if (minHp < allTargets[i].HpRatio)
                    {
                        minHp = allTargets[i].HpRatio;
                        _cachedDamageTaker = allTargets[i];
                    }
                }
            }

            return _cachedDamageTaker;
        }

        public static IDamageTaker FindLowestHpTarget(List<IDamageTaker> allTargets)
        {
            var minHp = Mathf.Infinity;
            _cachedDamageTaker = null;
            for (var j = 0; j < allTargets.Count; j++)
            {
                if (allTargets[j] != null && allTargets[j].CanBeTarget)
                {
                    if (minHp > allTargets[j].HpRatio)
                    {
                        minHp = allTargets[j].HpRatio;
                        _cachedDamageTaker = allTargets[j];
                    }
                }
            }
            return _cachedDamageTaker;
        }

        public static void FindLowestHpTarget(
            List<IDamageTaker> allTargets,
            int count,
            List<IDamageTaker> results
        )
        {
            results.Clear();
            for (var i = 0; i < count; i++)
            {
                var minHp = Mathf.Infinity;
                _cachedDamageTaker = null;
                for (var j = 0; j < allTargets.Count; j++)
                {
                    if (allTargets[j] != null && allTargets[j].CanBeTarget)
                    {
                        if (minHp > allTargets[j].HpRatio && !results.Contains(allTargets[j]))
                        {
                            minHp = allTargets[j].HpRatio;
                            _cachedDamageTaker = allTargets[j];
                        }
                    }
                }

                if (_cachedDamageTaker != null)
                {
                    results.Add(_cachedDamageTaker);
                }
            }
        }

        public static IDamageTaker FindLowestEnergyTarget(List<IDamageTaker> allTargets)
        {
            var minEnergy = Mathf.Infinity;
            _cachedDamageTaker = null;
            for (var j = 0; j < allTargets.Count; j++)
            {
                if (
                    allTargets[j] != null
                    && allTargets[j].CanBeTarget
                    && allTargets[j] is ISkillOwner cso
                )
                {
                    if (cso.UltimateGroup != null && cso.UltimateGroup.Skills.Count > 0)
                    {
                        var ultimate = cso.UltimateGroup.Skills[0];
                        if (minEnergy > ultimate.Energy)
                        {
                            minEnergy = ultimate.Energy;
                            _cachedDamageTaker = allTargets[j];
                        }
                    }
                }
            }
            return _cachedDamageTaker;
        }

        public static void FindLowestEnergyTarget(
            List<IDamageTaker> allTargets,
            int count,
            List<IDamageTaker> results
        )
        {
            results.Clear();
            for (var i = 0; i < count; i++)
            {
                var minEnergy = Mathf.Infinity;
                _cachedDamageTaker = null;
                for (var j = 0; j < allTargets.Count; j++)
                {
                    if (
                        allTargets[j] != null
                        && allTargets[j].CanBeTarget
                        && allTargets[j] is ISkillOwner cso
                    )
                    {
                        if (cso.UltimateGroup != null && cso.UltimateGroup.Skills.Count > 0)
                        {
                            var ultimate = cso.UltimateGroup.Skills[0];
                            if (minEnergy > ultimate.Energy && !results.Contains(allTargets[j]))
                            {
                                minEnergy = ultimate.Energy;
                                _cachedDamageTaker = allTargets[j];
                            }
                        }
                    }
                }
                if (_cachedDamageTaker != null)
                {
                    results.Add(_cachedDamageTaker);
                }
            }
        }

        public static void FindHighestAtkSpdTarget(
            List<IDamageTaker> allTargets,
            int count,
            List<IDamageTaker> results
        )
        {
            results.Clear();
            for (var i = 0; i < count; i++)
            {
                var maxAtkSpd = Mathf.NegativeInfinity;
                _cachedDamageTaker = null;
                for (var j = 0; j < allTargets.Count; j++)
                {
                    if (
                        allTargets[j] != null
                        && allTargets[j].CanBeTarget
                        && allTargets[j] is ICombatStatOwner cso
                    )
                    {
                        if (maxAtkSpd < cso.AtkSpd && !results.Contains(allTargets[j]))
                        {
                            maxAtkSpd = cso.AtkSpd;
                            _cachedDamageTaker = allTargets[j];
                        }
                    }
                }
                if (_cachedDamageTaker != null)
                {
                    results.Add(_cachedDamageTaker);
                }
            }
        }

        public static void FindTargetsInRadius(
            ICombatTeam team,
            Vector3 finderPosition,
            float finderRadius,
            float range,
            HashSet<IDamageTaker> excepts,
            List<IDamageTaker> results,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK,
            bool mustCanBeTarget = false
        )
        {
            results.Clear();
            if (team == null || !team.IsInitialized)
            {
                return;
            }

#if VISUALIZE_DEBUG
            DebugUtils.DrawCircle(finderPosition, range, Color.red, 1f);
#endif

            for (var i = 0; i < team.Members.Count; i++)
            {
                if (
                    team.Members[i] == null
                    || !team.Members[i].Alive
                    || mustCanBeTarget && !team.Members[i].CanBeTarget
                )
                {
                    continue;
                }
                var pos = team.Members[i].Position;
                if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                {
                    continue;
                }
                if (
                    MathUtils.InRange(
                        finderPosition,
                        pos,
                        GetDistance(finderRadius, team.Members[i].CombatRadius, range, 0)
                    )
                )
                {
                    if (excepts == null || !excepts.Contains(team.Members[i]))
                    {
                        if (!results.Contains(team.Members[i]))
                        {
                            results.Add(team.Members[i]);
                        }
                    }
                }
            }
        }

        public static bool HasTargetInRadius(
            ICombatTeam team,
            Vector3 finderPosition,
            float finderRadius,
            float range,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK
        )
        {
            if (team == null || !team.IsInitialized)
            {
                return false;
            }

            for (var i = 0; i < team.Members.Count; i++)
            {
                if (team.Members[i] == null || !team.Members[i].CanBeTarget)
                {
                    continue;
                }
                var pos = team.Members[i].Position;
                if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                {
                    continue;
                }
                if (
                    MathUtils.InRange(
                        finderPosition,
                        pos,
                        GetDistance(finderRadius, team.Members[i].CombatRadius, range, 0)
                    )
                )
                {
                    return true;
                }
            }

            return false;
        }

        public static void FindTargetsInCone(
            ICombatTeam team,
            Vector3 finderPosition,
            float finderRadius,
            float range,
            Vector3 direction,
            float angle,
            HashSet<IDamageTaker> excepts,
            List<IDamageTaker> results,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK
        )
        {
            results.Clear();
            if (team == null || !team.IsInitialized)
            {
                return;
            }
            range += finderRadius;

#if VISUALIZE_DEBUG
            DebugUtils.DrawArc(finderPosition, direction, range, angle, Color.red, 2f);
#endif

            for (var i = 0; i < team.Members.Count; i++)
            {
                if (team.Members[i] == null || !team.Members[i].Alive)
                {
                    continue;
                }
                var pos = team.Members[i].Position;
                if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                {
                    continue;
                }
                if (
                    MathUtils.IsCircleAndArc2DIntersection(
                        pos.ToV2(),
                        team.Members[i].CombatRadius,
                        finderPosition.ToV2(),
                        range,
                        direction.ToV2(),
                        angle
                    )
                )
                {
                    if (excepts == null || !excepts.Contains(team.Members[i]))
                    {
                        if (!results.Contains(team.Members[i]))
                        {
                            results.Add(team.Members[i]);
                        }
                    }
                }
            }
        }

        public static void FindTargetsInRectangle(
            ICombatTeam team,
            Vector3 finderPosition,
            float finderRadius,
            Vector2 size,
            Vector3 direction,
            HashSet<IDamageTaker> excepts,
            List<IDamageTaker> results,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK
        )
        {
            results.Clear();
            if (team == null || !team.IsInitialized)
            {
                return;
            }
            size.y += finderRadius;
            var center = finderPosition + direction.normalized * size.y * 0.5f;
#if VISUALIZE_DEBUG
            DebugUtils.DrawWireBox(
                center,
                Quaternion.LookRotation(direction),
                new Vector3(size.x, 0, size.y),
                Color.red,
                1f
            );
#endif
            for (var i = 0; i < team.Members.Count; i++)
            {
                if (team.Members[i] == null || !team.Members[i].Alive)
                {
                    continue;
                }
                var pos = team.Members[i].Position;
                var radius = team.Members[i].CombatRadius;
                if (Mathf.Abs(pos.y - center.y) > minHeighCheck)
                {
                    continue;
                }
                DebugUtils.DrawWireBox(
                    center.ToV2(),
                    Quaternion.LookRotation(direction.ToV2()),
                    new Vector3(size.x, size.y, 0),
                    Color.yellow,
                    1f
                );
                DebugUtils.DrawWireSphere(pos.ToV2(), radius, Color.yellow, 1f);
                if (
                    MathUtils.IsCircleAndRectangle2DIntersection(
                        pos.ToV2(),
                        radius,
                        center.ToV2(),
                        size,
                        direction.ToV2()
                    )
                )
                {
                    if (excepts == null || !excepts.Contains(team.Members[i]))
                    {
                        if (!results.Contains(team.Members[i]))
                        {
                            results.Add(team.Members[i]);
                        }
                    }
                }
            }
        }

        public static void FindTargetsInAnnulus(
            ICombatTeam team,
            Vector3 finderPosition,
            float finderRadius,
            float innerRange,
            float outerRange,
            HashSet<IDamageTaker> excepts,
            List<IDamageTaker> results,
            float minHeighCheck = GameConfig.MIN_HEIGH_CHECK
        )
        {
            results.Clear();
            if (team == null || !team.IsInitialized)
            {
                return;
            }
            innerRange += finderRadius;
            outerRange += finderRadius;
#if VISUALIZE_DEBUG
            DebugUtils.DrawCircle(finderPosition, innerRange, Color.red, 1f);
            DebugUtils.DrawCircle(finderPosition, outerRange, Color.red, 1f);
#endif

            for (var i = 0; i < team.Members.Count; i++)
            {
                if (team.Members[i] == null || !team.Members[i].Alive)
                {
                    continue;
                }
                var pos = team.Members[i].Position;
                if (Mathf.Abs(pos.y - finderPosition.y) > minHeighCheck)
                {
                    continue;
                }
                var dist =
                    MathUtils.SqrDistance(finderPosition, pos) - team.Members[i].CombatRadius;
                if (dist >= innerRange * innerRange && dist <= outerRange * outerRange)
                {
                    if (excepts == null || !excepts.Contains(team.Members[i]))
                    {
                        if (!results.Contains(team.Members[i]))
                        {
                            results.Add(team.Members[i]);
                        }
                    }
                }
            }
        }

        public static bool IsInAbilityTargetingRange(
            Vector3 finderPosition,
            float finderRadius,
            IDamageTaker target,
            float abilityRange
        )
        {
            if (target == null || !target.CanBeTarget)
            {
                return false;
            }

            return IsInRange(
                finderPosition,
                finderRadius,
                target.Position,
                target.CombatRadius,
                abilityRange,
                0
            );
        }

        static bool IsInRange(
            Vector3 position,
            float finderRadius,
            Vector3 targetPosition,
            float targetRadius,
            float range,
            float offset
        )
        {
            range = GetDistance(finderRadius, targetRadius, range, offset);
            return MathUtils.InRange(position, targetPosition, range);
        }

        public static float GetDistance(
            float finderRadius,
            float targetRadius,
            float range,
            float offset
        )
        {
            range += finderRadius + targetRadius + offset;
            return range;
        }
    }
}
