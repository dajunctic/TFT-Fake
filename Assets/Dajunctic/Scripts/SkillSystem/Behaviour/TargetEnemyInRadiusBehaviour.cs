using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    [Serializable]
    public class TargetEnemyInRadiusSetting
    {
        public float radius;
        public LayerMask targetLayer;
        public TargetRadiusType targetType;
        
        public bool targetAll = true;

        [HideIf("@targetAll")]
        [MinValue(1)]
        public int count = 1;
    }

    public class TargetEnemyInRadiusBehaviour : SkillBehaviour<TargetEnemyInRadiusSetting>
    {
        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            if (combatActor == null || hasExecuted) return;

            context.EnemyTargets.Clear();

            var colliders = Physics.OverlapSphere(combatActor.Position, Settings.radius, Settings.targetLayer);
            List<CombatActor> foundActors = new List<CombatActor>();

            foreach (var collider in colliders)
            {
                var actor = collider.gameObject.GetComponent<CombatActor>();
                
                if (actor != null && actor != combatActor)
                {
                    foundActors.Add(actor);
                }
            }

            if (foundActors.Count > 0)
            {
                switch (Settings.targetType)
                {
                    case TargetRadiusType.Nearest:
                        foundActors = foundActors.OrderBy(a => Vector3.Distance(combatActor.Position, a.Position)).ToList();
                        break;
                    case TargetRadiusType.Farthest:
                        foundActors = foundActors.OrderByDescending(a => Vector3.Distance(combatActor.Position, a.Position)).ToList();
                        break;
                    case TargetRadiusType.Random:
                        foundActors = foundActors.OrderBy(a => a).ToList();
                        foundActors.Shuffle();
                        break;
                }

                if (!Settings.targetAll)
                {
                    foundActors = foundActors.Take(Settings.count).ToList();
                }

                context.EnemyTargets.AddRange(foundActors);
            }

            Debug.Log($"<color=green>[TargetInRadius<color=red><{combatActor.DataId}></color>]</color> Found: {context.EnemyTargets.Count} targets.");
            hasExecuted = true;
        }
    }

    public enum TargetRadiusType
    {
        Nearest,
        Farthest,
        Random,
    }
}