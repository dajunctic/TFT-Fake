using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Cache")]
    public class CacheNode : AbilityNode
    {

        [GraphProcessor.Input(name = "inTarget")] public IDamageTaker inTarget;
        [GraphProcessor.Input(name = "inPosition")] public Vector3 inPosition;
        [GraphProcessor.Output(name = "outTarget")] public IDamageTaker outTarget;
        [GraphProcessor.Output(name = "outPosition")] public Vector3 outPosition;

        protected override void PlayInternal()
        {
            base.PlayInternal();
            outTarget = GetInputValue(nameof(inTarget), inTarget);
            outPosition = GetInputValue(nameof(inPosition), inPosition);
            Completed();
        }
    }
}
