using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageTakerSplitNode : SkillNode
    {
        [XNode.Node.InputAttribute] public IDamageTaker target;
        [XNode.Node.OutputAttribute] public Vector3 targetPosition;
        [XNode.Node.OutputAttribute] public Vector3 forward;

        public override object GetValue(NodePort port)
        {
            var inTarget = GetInputValue<IDamageTaker>(nameof(target));

            if (port.fieldName == nameof(targetPosition))
            {
                return inTarget != null ? inTarget.AsTransform().Position : Vector3.zero;
            }
            if (port.fieldName == nameof(forward))
            {
                return inTarget != null ? inTarget.AsTransform().Forward : Vector3.forward;
            }

            return null;
        }

    }
}