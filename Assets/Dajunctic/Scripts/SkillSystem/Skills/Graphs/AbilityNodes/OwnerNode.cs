using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Owner")]
    public class OwnerNode : AbilityNode
    {
        [GraphProcessor.Output(name = "owner")] public ICombatActorEntity owner;
    }
}
