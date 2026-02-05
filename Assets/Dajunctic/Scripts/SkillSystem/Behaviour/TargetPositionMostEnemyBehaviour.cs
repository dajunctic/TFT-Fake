using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    [Serializable]
    public class TargetPositionMostEnemySetting
    {
        public float radius;
        public LayerMask targetLayer;
        public float areaRadius;
    }

    public class TargetPositionMostEnemyBehaviour: SkillBehaviour<TargetPositionMostEnemySetting>
    {

        private List<CombatActor> _enemiesInRange = new List<CombatActor>();


        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            if (combatActor == null) return;
            
            SkillHelper.ScanTargetInRadius(combatActor.Position, Settings.areaRadius, Settings.targetLayer, out var foundActors);

            _enemiesInRange = foundActors;

            var R = Settings.radius;
            var RSq = R * R;

            context.TargetPositions.Clear();

            if (_enemiesInRange.Count == 0)
            {
                context.TargetPositions.Add(combatActor.Position);
                return;
            }

           
            var enemyPositions = _enemiesInRange.Select(e => e.Position).ToArray();

            var n = enemyPositions.Length;

            int maxCount = 0;
            float minVariance = float.MaxValue;
            Vector3 bestPosition = enemyPositions[0];

            List<Vector3> candidates = new List<Vector3>(enemyPositions);

            for (var i = 0; i < n; i++)
            {
                for (var j = i + 1; j < n; j++)
                {
                    var p1 = enemyPositions[i];
                    var p2 = enemyPositions[j];
                    var dSq = MathUtils.SqrDistance(p1, p2);

                    if (dSq > 4 * RSq) continue;

                    var M = (p1 + p2) / 2.0f;
                    candidates.Add(M);

                    var rSq = dSq / 4.0f;
                    var h = Mathf.Sqrt(Mathf.Max(0, RSq - rSq));
                    var V = p2 - p1;
                    var perp = Vector3.Cross(V, Vector3.up).normalized;

                    candidates.Add(M + perp * h);
                    candidates.Add(M - perp * h);
                }
            }

            foreach (var pos in candidates)
            {
                int currentCount = CountEnemies(pos, RSq);
                if (currentCount == 0) continue;

                float currentVariance = CalculateDistanceVariance(pos, RSq);

                if (currentCount > maxCount)
                {
                    maxCount = currentCount;
                    minVariance = currentVariance;
                    bestPosition = pos;
                }
                else if (currentCount == maxCount)
                {
                    if (currentVariance < minVariance)
                    {
                        minVariance = currentVariance;
                        bestPosition = pos;
                    }
                }
            }

            context.TargetPositions.Add(bestPosition);
        }


        private int CountEnemies(Vector3 center, float radiusSq)
        {
            int count = 0;
            foreach (var enemy in _enemiesInRange)
            {
                if (MathUtils.SqrDistance(center, enemy.Position) <= radiusSq)
                {
                    count++;
                }
            }
            return count;
        }

        private float CalculateDistanceVariance(Vector3 center, float radiusSq)
        {
            var distancesSq = new List<float>();
            foreach (var enemy in _enemiesInRange)
            {
                float dSq = MathUtils.SqrDistance(center, enemy.Position);
                if (dSq <= radiusSq)
                {
                    distancesSq.Add(dSq);
                }
            }

            if (distancesSq.Count <= 1) return 0f;

            float sum = 0f;
            foreach (var d in distancesSq) sum += d;
            float mean = sum / distancesSq.Count;

            float variance = 0f;
            foreach (var d in distancesSq)
            {
                variance += (d - mean) * (d - mean);
            }
            return variance / distancesSq.Count;
        }
    }
}