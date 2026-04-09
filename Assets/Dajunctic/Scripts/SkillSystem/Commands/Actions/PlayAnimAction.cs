using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class PlayAnimAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Anim")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Play Anim";

        [HorizontalGroup("Anim")]
        public string AnimationName = "Cast";

        [HorizontalGroup("Anim", Width = 60)]
        [LabelWidth(30)]
        public float CrossFade = 0.1f;

        public bool WaitForFinish = true;

        [ShowIf("WaitForFinish")]
        public float FallbackDuration = 1f; // Just in case animation events don't fire

        protected override Color GetColor() => new Color(0f, 0.8f, 0.8f, 1f); // Cyan

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            var actor = context.Caster;
            if (actor == null) yield break;

            actor.PlayAnim(AnimationName, CrossFade);
            actor.ResetAnim();

            if (WaitForFinish)
            {
                float timeElapsed = 0f;
                // Wait until animation is actually finished or fallback duration kicks in
                while (!actor.IsAnimFinished && timeElapsed < FallbackDuration)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
}
