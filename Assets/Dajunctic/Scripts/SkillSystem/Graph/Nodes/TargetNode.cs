using System;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public enum TargetType
    {
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

            // switch (targetType)
            // {
               
            //     // Add other logic as needed
            // }

            if (context.targets.Count > 0)
            {
                context.targetPosition = context.targets[0].Position;
            }

            onComplete?.Invoke();
        }
    }
}
