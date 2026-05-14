using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class CancelNode : AbilityNode
    {
        [SerializeField, Input] AbilityNode targetNode;
        
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
