using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.ActionNodes
{
    public class DamageActionNode : ActionNode
    {
        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        public override void Execute(object source)
        {
            if (_context?.actor == null || source == null) return;

            if (source is ISubActionSource actionSource)
            {
                var data = actionSource.GetData();
                if (data == null || data.damageTakers == null) return;

                float baseDamage = _context.actor.GetTotalAtk();
                float finalDamage = baseDamage * damageMultiplier;

                foreach (var target in data.damageTakers)
                {
                    if (target == null) continue;
                    target.TakeDamage(new CombineDamage(damageType, finalDamage));
                }
            }

            base.Execute(source);
        }
    }
}
