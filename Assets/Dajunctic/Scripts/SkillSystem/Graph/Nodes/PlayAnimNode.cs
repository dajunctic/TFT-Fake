using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayAnimNode : SkillNode
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        public string animationName;
        public float transitionDuration = 0.1f;
        public float duration;
        public bool waitForFinish = true;

        public override void Execute()
        {
            var actor = _context?.actor.AsCombatActor();
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
            yield return new WaitForSeconds(duration);
            Complete();
        }
    }
}
