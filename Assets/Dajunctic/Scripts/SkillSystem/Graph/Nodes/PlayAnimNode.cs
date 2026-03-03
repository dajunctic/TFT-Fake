using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayAnimNode : SkillNode
    {
        public string animationName;
        public float transitionDuration = 0.1f;
        public bool waitForFinish = true;

        public override void Execute()
        {
            var actor = _context.actor.AsCombatActor();
            if (actor == null)
            {
                Complete();
                return;
            }

            actor.PlayAnim(animationName, transitionDuration);
            actor.ResetAnim();

            if (waitForFinish)
            {
                StartCoroutine();
            }
            else
            {
                Complete();
            }
        }

        public override IEnumerator IECoroutine()
        {
            var actor = _context.actor.AsCombatActor();
            yield return null;
            while (actor != null && !actor.IsAnimFinished)
            {
                yield return null;
            }
            Complete();
        }
    }
}
