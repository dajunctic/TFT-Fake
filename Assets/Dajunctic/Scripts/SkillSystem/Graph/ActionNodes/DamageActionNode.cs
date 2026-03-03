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
            if (_context?.actor == null) return;

            var data = GetHitData(source);
            if (data != null && data.targets != null)
            {
                float baseDamage = _context.actor.GetTotalAtk();
                float finalDamage = baseDamage * damageMultiplier;

                foreach (var target in data.targets)
                {
                    if (target == null) continue;
                    target.TakeDamage(new CombineDamage(damageType, finalDamage));
                }
            }

            base.Execute(source);
        }
    }
}
