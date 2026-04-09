using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class DamageAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Damage")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Apply Damage";

        [HorizontalGroup("Damage")]
        public DamageType DamageType = DamageType.PhysicalDamage;

        [HorizontalGroup("Damage", Width = 50)]
        [LabelText("Multi")]
        public float DamageMultiplier = 1f;

        [Tooltip("The variable key in the context where targets are stored")]
        public string TargetContextKey = "CurrentTargets";

        protected override Color GetColor() => new Color(0.8f, 0f, 0f, 1f); // Red

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            if (context.Caster == null) yield break;

            var targets = context.GetVariable<List<IDamageTaker>>(TargetContextKey);
            
            if (targets != null && targets.Count > 0)
            {
                float baseDamage = context.Caster.GetTotalAtk();
                float finalDamage = baseDamage * DamageMultiplier;

                foreach (var target in targets)
                {
                    if (target == null || (target is MonoBehaviour mb && !mb.gameObject.activeInHierarchy)) continue;
                    target.TakeDamage(new CombineDamage(DamageType, finalDamage));
                }
            }

            yield break;
        }
    }
}
