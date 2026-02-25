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

        public bool isValid;

        public override object GetValue(string portName)
        {     
         
            ClearTargets();

            if (portName == nameof(targets)) return targets;
            if (portName == nameof(mainTarget)) return mainTarget;
            return base.GetValue(portName);
        }

        public void ClearTargets()
        {
            targets = new List<IDamageTaker>();
            mainTarget = null;
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
            mainTarget = null;
        }
    }
}
