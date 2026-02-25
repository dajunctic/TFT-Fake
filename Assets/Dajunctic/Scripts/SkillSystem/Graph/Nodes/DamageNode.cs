using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageNode : SkillNode
    {
        public float damage = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [NodeInput] public List<CombatActor> targets;

        public override void Execute()
        {
            var actorsToHit = targets ?? new List<CombatActor>();

            foreach (var target in actorsToHit)
            {
                if (target == null) continue;
                float baseDamage = _context.actor.GetTotalAtk();
                float finalDamage = baseDamage * damage;
                target.TakeDamage(new CombineDamage(damageType, finalDamage));
            }

            TriggerComplete();
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }
    }
}
