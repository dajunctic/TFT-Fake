using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public enum TargetType
    {
        CurrentTarget,
        NearestEnemy,
        FarthestEnemy,
        MostEnemiesInRadius
    }

    public class TargetNode : SkillNode
    {
        public TargetType targetType;
        public float radius = 5f;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            context.targets.Clear();

            switch (targetType)
            {
                case TargetType.CurrentTarget:
                    if (context.actor.CurrentTarget != null)
                        context.targets.Add(context.actor.CurrentTarget);
                    break;
                // Add other logic as needed
            }

            if (context.targets.Count > 0)
            {
                context.targetPosition = context.targets[0].Position;
            }

            onComplete?.Invoke();
        }
    }
}
