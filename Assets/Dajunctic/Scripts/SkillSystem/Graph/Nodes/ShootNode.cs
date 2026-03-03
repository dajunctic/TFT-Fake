using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class ShootNode : SkillNode, IFxDataProvider
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;
        public Vector3 launcherOffset;
        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [ActionInput] public List<ActionNode> hitActions;
        [ActionInput] public List<ActionNode> despawnActions;

        [NodeInput] public List<IDamageTaker> targets;

        private IDamageTaker currentTarget;

        public override void Execute()
        {
            targets = GetInputValue<List<IDamageTaker>>(nameof(targets));
            var inTargets = targets ?? new List<IDamageTaker>();

            if (_context.Services != null && _context.Services.IsDebug)
            {
                Debug.Log($"<color=#4dabf7>[ShootNode]</color> Execute: targets={inTargets.Count}, missileId='{missileId}'");
            }

            if (inTargets.Count == 0 || _context.Services == null || string.IsNullOrEmpty(missileId))
            {
                if (_context.Services != null && _context.Services.IsDebug)
                {
                    if (inTargets.Count == 0) Debug.LogWarning("<color=#4dabf7>[ShootNode]</color> No targets found!");
                    if (string.IsNullOrEmpty(missileId)) Debug.LogWarning("<color=#4dabf7>[ShootNode]</color> missileId is EMPTY!");
                }
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

            if (hitActions != null)
            {
                foreach (var action in hitActions)
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
