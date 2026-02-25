using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField, NodeOutput] public List<IDamageTaker> targets;
        [SerializeField, NodeOutput] public IDamageTaker mainTarget;

        public override void Execute()
        {
            targets = new List<IDamageTaker>();

            // TODO: thực hiện logic tìm enemy trong bán kính
            // logic implementation here...

            TriggerComplete();
        }

        public override object GetValue(string portName)
        {
            if (portName == nameof(targets)) return targets;
            if (portName == nameof(mainTarget)) return mainTarget;
            return base.GetValue(portName);
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }
    }
}
