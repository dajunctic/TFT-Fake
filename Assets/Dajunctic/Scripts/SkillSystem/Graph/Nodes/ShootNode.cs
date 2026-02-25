using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class ShootNode : SkillNode
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;
        public Vector3 launcherOffset;
        public float damageMultiplier = 1f;
        public DamageType damageType = DamageType.PhysicalDamage;

        [NodeInput] public List<IDamageTaker> targets;

        public override void Execute()
        {
            targets = GetInputValue<List<IDamageTaker>>(nameof(targets));
            var actorsToShoot = targets ?? new List<IDamageTaker>();

            if (actorsToShoot.Count == 0 || _context.Services == null || string.IsNullOrEmpty(missileId))
            {
                TriggerComplete();
                return;
            }

            var casterCA = _context.actor.AsCombatActor();
            Vector3 launchPos = casterCA != null
                ? casterCA.CachedTransform.TransformPoint(launcherOffset)
                : _context.actor.AsTransform().Position;

            foreach (var target in actorsToShoot)
            {
                if (target == null) continue;

                float totalAtk = _context.actor.GetTotalAtk();
                var missileData = new MissileData
                {
                    launcher = launchPos,
                    destination = target.AsTransform().Position,
                    targetActor = target.AsCombatActor() as CombatActor,
                    combatActor = casterCA as CombatActor,
                    combineDamage = new CombineDamage(damageType, totalAtk * damageMultiplier)
                };

                _context.Services.SpawnMissile(missileId, missileData);
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
