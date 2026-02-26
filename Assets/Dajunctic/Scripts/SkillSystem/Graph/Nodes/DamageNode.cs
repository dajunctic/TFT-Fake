using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageNode : SkillNode
    {
        public float damage = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [SerializeField, NodeInput] private List<IDamageTaker> targets;

        public override void Execute()
        {
            targets = GetInputValue<List<IDamageTaker>>(nameof(targets));
            var actorsToHit = targets ?? new List<IDamageTaker>();

            foreach (var target in actorsToHit)
            {
                if (target == null) continue;
                float baseDamage = _context.actor.GetTotalAtk();
                float finalDamage = baseDamage * damage;
                target.TakeDamage(new CombineDamage(damageType, finalDamage));
            }

            Complete();
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }
    }
}
