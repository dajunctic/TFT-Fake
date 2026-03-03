using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.ActionNodes
{
    public class PlayAnimationActionNode : ActionNode
    {
        public string animationName;
        public float transitionDuration = 0.1f;

        public override void Execute(object source)
        {
            if (string.IsNullOrEmpty(animationName)) return;

            var data = GetFxData(source);
            if (data != null && data.targets != null)
            {
                foreach (var target in data.targets)
                {
                    if (target == null) continue;

                    var combatActor = target.AsCombatActor();
                    if (combatActor != null)
                    {
                        combatActor.PlayAnim(animationName, transitionDuration);
                    }
                }
            }

            base.Execute(source);
        }
    }
}
