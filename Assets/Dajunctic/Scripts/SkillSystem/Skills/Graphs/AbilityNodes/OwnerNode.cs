using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class OwnerNode : AbilityNode
    {
        [SerializeReference, Output] ICombatActorEntity owner;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(owner))
            {
                return Owner;
            }
            return owner;
        }
    }
}
