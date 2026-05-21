using GraphProcessor;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Cancel")]
    public class CancelNode : AbilityNode
    {
        [GraphProcessor.Input(name = "targetNode")] public AbilityNode targetNode;
        
        protected override void PlayInternal()
        {
            var inTargetNode = GetInputValue(nameof(targetNode), targetNode);
            if (inTargetNode != null)
            {
                inTargetNode.Stop();
            }
            Completed();
        }
    }
}
