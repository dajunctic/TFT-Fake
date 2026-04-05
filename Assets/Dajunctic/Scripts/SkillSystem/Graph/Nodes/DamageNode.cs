using System.Collections.Generic;
using System.Linq;
using Dajunctic.SkillSystem.Graph.ActionNodes;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    [XNode.Node.NodeTint("#8B0000")]
    [XNode.Node.NodeWidth(250)]
    public class DamageNode : SkillNode, IHitDataProvider, IFxDataProvider
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [XNode.Node.InputAttribute] private List<IDamageTaker> targets;

        [XNode.Node.InputAttribute]
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
                        action.Init(_context, null);
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

        public HitData GetHitData()
        {
            return new HitData
            {
                targets = GetInputValue<List<IDamageTaker>>(nameof(targets)) ?? new List<IDamageTaker>(),
                hitPoints = (GetInputValue<List<IDamageTaker>>(nameof(targets))?.Select(t => t.MidPoint).ToList()) ?? new List<Vector3>()
            };
        }

        public FxData GetFxData()
        {
            return new FxData
            {
                targets = GetInputValue<List<IDamageTaker>>(nameof(targets)) ?? new List<IDamageTaker>()
            };
        }
    }
}
