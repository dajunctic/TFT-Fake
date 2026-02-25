using UnityEngine;
using System;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageNode : SkillNode
    {
        public float damage = 1f; // multiplier
        public DamageType damageType = DamageType.PhysicalDamage;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            foreach (var targetActor in context.targets)
            {
                if (targetActor != null)
                {
                    float baseDamage = context.actor.GetTotalAtk();
                    float finalDamage = baseDamage * this.damage;

                    var damage = new CombineDamage(damageType, finalDamage);
                    targetActor.TakeDamage(damage);
                }
            }
            
            onComplete?.Invoke();
        }
    }
}
