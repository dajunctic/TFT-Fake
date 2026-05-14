using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class PositionTargetNode : AbilityNode
    {
        [SerializeReference, Input] private IDamageTaker target;
        [SerializeField] bool cached = true;
        [SerializeField] Vector3 offset = Vector3.zero;
        [Output] public Vector3 outPosition;
        [Output] public Vector3 outDirection;
        [Output] public float outDistance;

        IDamageTaker _cachedTarget;

        protected override void PlayInternal()
        {
            base.PlayInternal();
            _cachedTarget = GetInputValue(nameof(target), target);
            Completed();
        }

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;

            if (!cached)
            {
                _cachedTarget = GetInputValue(nameof(target), target);
            }
            if (_cachedTarget == null || !_cachedTarget.Alive) return null;

            if (port.fieldName == nameof(outPosition))
            {
                return _cachedTarget.Position + _cachedTarget.AsTransform().TransformDirection(offset);
            }

            if (port.fieldName == nameof(outDirection))
            {
                var direction = (_cachedTarget.Position - Owner.AsTransform().Position).normalized;
                direction.y = 0;
                return direction;
            }

            if (port.fieldName == nameof(outDistance))
            {
                return Vector3.Distance(Owner.AsTransform().Position, _cachedTarget.Position);
            }

            return null;
        }
    }
}

