using UnityEngine;
using System.Collections.Generic;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class ShootNode : SkillNode, IFxDataProvider
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;
        public Vector3 launcherOffset;
        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [XNode.Node.InputAttribute] private List<ActionNode> hitActions;
        [XNode.Node.InputAttribute] private List<ActionNode> despawnActions;

        [XNode.Node.InputAttribute] public List<IDamageTaker> targets;

        private IDamageTaker currentTarget;

        public override void Execute()
        {
            targets = GetInputValue<List<IDamageTaker>>(nameof(targets));
            var inTargets = targets ?? new List<IDamageTaker>();

            if (inTargets.Count == 0 || _context?.Services == null || string.IsNullOrEmpty(missileId))
            {
                Complete();
                return;
            }

            var damageDealer = _context.actor.AsCombatActor();
            Vector3 launchPos = damageDealer != null
                ? damageDealer.CachedTransform.TransformPoint(launcherOffset)
                : _context.actor.AsTransform().Position;

            foreach (var target in inTargets)
            {
                if (target == null) continue;

                float totalAtk = _context.actor.GetTotalAtk();
                var missileData = new MissileData
                {
                    id = missileId,
                    launcher = launchPos,
                    destination = target.AsTransform().Position,
                    damageTaker = target,
                    damageDealer = damageDealer,
                    combineDamage = new CombineDamage(damageType, totalAtk * damageMultiplier)
                };

                var missile = _context.Services.SpawnMissile(missileData);
                missile.OnHitEvent += OnHitEvent;
            }

            Complete();
        }

        void OnHitEvent(IDamageTaker target)
        {
            currentTarget = target;

            var inActions = GetInputValue<List<ActionNode>>(nameof(hitActions));
            if (inActions != null)
            {
                foreach (var action in inActions)
                {
                    if (action != null)
                    {
                        action.Init(_context, null);
                        action.Execute(this);
                    }
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
        }

        public FxData GetFxData()
        {
            return new FxData
            {
                targets = new List<IDamageTaker> { currentTarget }
            };
        }
    }
}
