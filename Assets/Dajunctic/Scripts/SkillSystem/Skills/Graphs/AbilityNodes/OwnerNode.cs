using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class OwnerNode : AbilityNode
    {
        [SerializeField, Output(ShowBackingValue.Never)] ICombatActorEntity owner;

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
