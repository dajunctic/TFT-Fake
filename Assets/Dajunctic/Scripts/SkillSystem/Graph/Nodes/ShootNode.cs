using UnityEngine;
using System;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class ShootNode : SkillNode
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))] public string missileId;
        public Vector3 launcherOffset;
        public float damageMultiplier = 1f;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            if (context.targets.Count == 0)
            {
                onComplete?.Invoke();
                return;
            }

            foreach (var target in context.targets)
            {
                var missileData = new MissileData();
                missileData.launcher = context.actor.CachedTransform.TransformPoint(launcherOffset);
                missileData.targetActor = target;
                missileData.combatActor = context.actor;
                missileData.combineDamage = new CombineDamage(DamageType.PhysicalDamage, context.actor.GetTotalAtk() * damageMultiplier);

                var missile = GameManager.Instance.SpawnMissile(missileId, missileData);
                // We don't wait for missile hit in this simple implementation, 
                // but we could add a callback if needed.
            }

            onComplete?.Invoke();
        }
    }
}
