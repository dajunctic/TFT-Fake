using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class PositionBehindTargetNode : AbilityNode
    {
        [SerializeField] private float distanceBehind = 1f;
        [SerializeReference, Input] private IDamageTaker target;

        [Output] public Vector3 outPosition;
        [Output] public Vector3 outDirection;

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;

            var foundTarget = GetInputValue(nameof(target), target);
            if (foundTarget == null || !foundTarget.Alive) return null;

            var owner = Owner.AsDamageTaker();
            if (owner == null) return null;

            var targetPosition = foundTarget.Position;

            var positionBehind = targetPosition - foundTarget.Forward * (distanceBehind + foundTarget.CombatRadius + owner.CombatRadius);

            if (port.fieldName == nameof(outPosition))
            {
                return positionBehind;
            }
            if (port.fieldName == nameof(outDirection))
            {
                return foundTarget.Forward;
            }

            return null;
        }
    }
}

