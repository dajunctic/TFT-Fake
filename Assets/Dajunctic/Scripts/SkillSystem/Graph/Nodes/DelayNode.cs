using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DelayNode : SkillNode
    {
        public float duration = 1f;

        public override void Execute()
        {
            Delay(duration);
        }
    }
}
