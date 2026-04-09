using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class ShootMissileAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Missile")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Shoot Missile";

        [HorizontalGroup("Missile")]
        [GuidReference("missile", typeof(IDummyId))] 
        public string MissileId;

        public Vector3 LauncherOffset;
        public DamageType DamageType = DamageType.PhysicalDamage;
        public float DamageMultiplier = 1f;

        [Tooltip("The variable key in the context where targets are stored.")]
        public string TargetContextKey = "CurrentTargets";

        public bool WaitForHit = false;

        protected override Color GetColor() => new Color(0f, 0.5f, 1f, 1f); // Blue

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            if (context.Services == null || string.IsNullOrEmpty(MissileId)) yield break;

            var targets = context.GetVariable<List<IDamageTaker>>(TargetContextKey);
            if (targets == null || targets.Count == 0) yield break;

            var damageDealer = context.Caster as CombatActor;
            Vector3 launchPos = damageDealer != null
                ? damageDealer.TransformPoint(LauncherOffset)
                : context.Caster.AsTransform().Position;

            float totalAtk = context.Caster.GetTotalAtk();
            int hitCount = 0;
            int totalMissiles = 0;

            foreach (var target in targets)
            {
                if (target == null || (target is MonoBehaviour mb && !mb.gameObject.activeInHierarchy)) continue;

                var missileData = new MissileData
                {
                    id = MissileId,
                    launcher = launchPos,
                    destination = target.AsTransform().Position,
                    damageTaker = target,
                    damageDealer = damageDealer,
                    combineDamage = new CombineDamage(DamageType, totalAtk * DamageMultiplier)
                };

                var missile = context.Services.SpawnMissile(missileData);
                if (missile != null)
                {
                    totalMissiles++;
                    if (WaitForHit)
                    {
                        missile.OnHitEvent += (t) => hitCount++;
                    }
                }
            }

            if (WaitForHit && totalMissiles > 0)
            {
                // Wait until all missiles have hit or a timeout occurs (safety mechanism)
                float timeout = 5f;
                float timeElapsed = 0f;
                while (hitCount < totalMissiles && timeElapsed < timeout)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
