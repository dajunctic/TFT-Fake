using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DamageTakerSplitNode: SkillNode
    {
        [NodeInput] public IDamageTaker target;
        [NodeOutput] public Vector3 position;
        [NodeOutput] public Vector3 forward;

        public override object GetValue(string portName)
        {
            var inTarget = GetInputValue<IDamageTaker>(nameof(target));
            position = Vector3.zero;
            forward = Vector3.forward;

            if (inTarget != null)
            {
                position = inTarget.AsTransform().Position;
                forward = inTarget.AsTransform().Forward;
            }

            if (portName == nameof(position)) return position;
            if (portName == nameof(forward)) return forward;

            return base.GetValue(portName);
        }

    }
}