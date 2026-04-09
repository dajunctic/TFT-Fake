using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class PlayFxAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Fx")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Play Fx";

        [HorizontalGroup("Fx")]
        [GuidReference("fx", typeof(IDummyId))] 
        public string FxId;

        public AnchorType SpawnAnchor = AnchorType.MidPoint;
        public float Duration = 2f;
        
        [Tooltip("The variable key in the context where targets are stored. If empty, spawns on caster.")]
        public string TargetContextKey = "CurrentTargets";

        protected override Color GetColor() => new Color(0.8f, 0f, 0.8f, 1f); // Purple

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            if (context.Services == null || string.IsNullOrEmpty(FxId)) yield break;

            List<IDamageTaker> targets = null;
            if (!string.IsNullOrEmpty(TargetContextKey))
            {
                targets = context.GetVariable<List<IDamageTaker>>(TargetContextKey);
            }

            // If no targets found, or no key provided, default to caster
            if (targets == null || targets.Count == 0)
            {
                targets = new List<IDamageTaker>() { context.Caster.AsDamageTaker() };
            }

            foreach (var target in targets)
            {
                if (target == null) continue;

                Vector3 spawnPos = target.AsCombatActor().GetAnchorPosition(SpawnAnchor);
                Quaternion spawnRot = Quaternion.LookRotation(target.AsTransform().Forward);

                var playFxEvent = new SpawnFxEvent
                {
                    id = FxId,
                    position = spawnPos,
                    rotation = spawnRot,
                    duration = Duration
                };

                context.Services.SpawnFx(playFxEvent);
            }
        }
    }
}
