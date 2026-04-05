using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class EntryNode : SkillNode
    {
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;
    }
}
