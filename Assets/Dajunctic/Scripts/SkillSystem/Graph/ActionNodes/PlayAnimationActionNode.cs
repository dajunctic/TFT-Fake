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
            if (source == null || string.IsNullOrEmpty(animationName)) return;

            if (source is ISubActionSource actionSource)
            {
                var data = actionSource.GetData();
                if (data == null || data.damageTakers == null) return;

                foreach (var target in data.damageTakers)
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
