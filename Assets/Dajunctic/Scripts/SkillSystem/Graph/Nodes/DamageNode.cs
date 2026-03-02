using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph.ActionNodes;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageNode : SkillNode, ISubActionSource
    {
        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [NodeInput] private List<IDamageTaker> targets;

        [SerializeReference]
        private List<ActionNode> hitActions = new List<ActionNode>();

        public override void Execute()
        {
            targets = GetInputValue<List<IDamageTaker>>(nameof(targets));
            var actorsToHit = targets ?? new List<IDamageTaker>();

            if (actorsToHit.Count == 0)
            {
                Complete();
                return;
            }

            float baseDamage = _context.actor.GetTotalAtk();
            float finalDamage = baseDamage * damageMultiplier;

            foreach (var target in actorsToHit)
            {
                if (target == null) continue;
                target.TakeDamage(new CombineDamage(damageType, finalDamage));
            }

            // Execute sub-actions
            var actions = GetInputValue<List<ActionNode>>(nameof(hitActions), hitActions);
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    if (action != null)
                    {
                        action.Init(_context, null); // Sub-actions don't need independent completion for now
                        action.Execute(this);
                    }
                }
            }

            Complete();
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }

        public SubActionData GetData()
        {
            return new SubActionData
            {
                damageTakers = targets ?? new List<IDamageTaker>(),
                // could add positions/transforms here if needed
            };
        }
    }
}
