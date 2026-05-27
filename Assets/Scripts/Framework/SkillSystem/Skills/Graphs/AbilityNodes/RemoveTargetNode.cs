using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/RemoveTarget")]
    public class RemoveTargetNode : AbilityNode
    {
        [GraphProcessor.Input(name = "inTargets")] public List<IDamageTaker> inTargets;
        [GraphProcessor.Input(name = "removeTargets")] public List<IDamageTaker> removeTargets;
        [GraphProcessor.Output] public List<IDamageTaker> outTargets;
    }
}
