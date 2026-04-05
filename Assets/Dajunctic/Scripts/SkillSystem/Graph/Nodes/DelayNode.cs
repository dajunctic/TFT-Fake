using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DelayNode : SkillNode
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        public float duration = 1f;

        public override void Execute()
        {
            Delay(duration);
        }
    }
}
