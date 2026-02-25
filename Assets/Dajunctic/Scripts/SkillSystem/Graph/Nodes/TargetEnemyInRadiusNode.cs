using System.Collections;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public enum TargetType
    {
        NearestEnemy,
        FarthestEnemy,
        MostEnemiesInRadius
    }

    public class TargetEnemyInRadiusNode : SkillNode
    {
        public TargetType targetType;
        public float radius = 5f;

        [NodeOutput] public List<CombatActor> targets;

        public override void Execute()
        {
            targets = new List<CombatActor>();

            // TODO: thực hiện logic tìm enemy trong bán kính
            // Ví dụ:
            // var allActors = Object.FindObjectsOfType<CombatActor>();
            // foreach (var a in allActors)
            //     if (a != _context.actor && Vector3.Distance(a.transform.position, _context.actor.transform.position) <= radius)
            //         targets.Add(a);

            _context.SetOutput(guid, nameof(targets), targets);
            TriggerComplete();
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }
    }
}
