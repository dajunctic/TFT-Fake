using UnityEngine;

namespace Dajunctic
{
    public static class SkillExecutor
    {
        public static void ExecuteProjectile(CombatActor combatActor, CombatActor target, SkillData data,
            SkillStep step, SkillEvent evt, Transform firePoint)
        {
            var projPrefab = step.overrideProjectilePrefab != null
                ? step.overrideProjectilePrefab
                : data.projectilePrefab;
            var muzzlePrefab = step.overrideMuzzlePrefab != null ? step.overrideMuzzlePrefab : data.muzzlePrefab;
            var hitPrefab = step.overrideHitPrefab != null ? step.overrideHitPrefab : data.hitPrefab;

            if (projPrefab is Projectile validProj)
            {
                Vector3 spawnPos = firePoint.position;
                Vector3 targetPos = target.MidPoint.Position;
                Vector3 direction = (targetPos - spawnPos).normalized;
                Quaternion spawnRot = Quaternion.LookRotation(direction);

                if (muzzlePrefab != null) 
                    PoolableObject.Pool.Spawn(data.muzzlePrefab, spawnPos, spawnRot);
                
                var proj = PoolableObject.Pool.Spawn(validProj, spawnPos, spawnRot);
                CombineDamage finalDamage = step.combineDamage;
                finalDamage.damage *= evt.damageMultiplier;
                proj.Initialize(finalDamage, combatActor.TargetLayer, target, data.hitPrefab);
            }
        }

        public static void ExecuteMelee(CombatActor combatActor, CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firePoint)
        {
            var hitPrefab = step.overrideHitPrefab != null ? step.overrideHitPrefab : data.hitPrefab;

            CombineDamage finalDamage = step.combineDamage;
            finalDamage.damage *= evt.damageMultiplier;
            
            target.TakeDamage(finalDamage);
            
            if (hitPrefab != null)
            {
                PoolableObject.Pool.Spawn(hitPrefab, target.MidPoint.Position, Quaternion.identity);
            }
        }

        public static void ExecuteSpawnAtTarget(CombatActor combatActor, CombatActor target, SkillData data, SkillStep step)
        {
            var effectPrefab = step.overrideProjectilePrefab != null ? step.overrideProjectilePrefab : data.projectilePrefab;
            var hitPrefab = step.overrideHitPrefab != null ? step.overrideHitPrefab : data.hitPrefab;

            if (effectPrefab is AreaSkill validEffect)
            {
                Vector3 spawnPos = target.Position;
                var spawnedObj = PoolableObject.Pool.Spawn(validEffect, spawnPos, Quaternion.identity);
                spawnedObj.Initialize(combatActor, step.combineDamage,
                    combatActor.TargetLayer, data.duration, hitPrefab);
            }
        }
    }
}