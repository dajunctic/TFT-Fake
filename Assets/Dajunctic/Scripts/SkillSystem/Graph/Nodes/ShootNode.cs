using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class ShootNode : SkillNode
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;
        public Vector3 launcherOffset;
        public float damageMultiplier = 1f;

        [NodeInput] public List<CombatActor> targets;

        public override void Execute()
        {
            var actorsToShoot = targets ?? new List<CombatActor>();
            if (actorsToShoot.Count == 0)
            {
                TriggerComplete();
                return;
            }

            foreach (var target in actorsToShoot)
            {
                var missileData = new MissileData
                {
                    launcher = _context.actor.CachedTransform.TransformPoint(launcherOffset),
                    targetActor = target,
                    combatActor = _context.actor,
                    combineDamage = new CombineDamage(DamageType.PhysicalDamage, _context.actor.GetTotalAtk() * damageMultiplier)
                };
                GameManager.Instance.SpawnMissile(missileId, missileData);
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
