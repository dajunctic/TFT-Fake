using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class RelativePositionNode : AbilityNode
    {
        [SerializeReference, Input] IDamageTaker inTarget;
        [SerializeField, Input] Vector3 inDirection;
        [SerializeField, Input] float inDistance;
        [SerializeField] Vector3 offsetRaycast;
        [SerializeField] bool isLocal;
        [SerializeField, Output] Vector3 outPosition;
        [SerializeField, Output] float outDistance;

        Vector3 _curPosition;
        float _curDistance;

        protected override void PlayInternal()
        {
            base.PlayInternal();
            _curPosition = Owner.AsTransform().Position;  

            var target = GetInputValue(nameof(inTarget), inTarget);
            if (target == null || !target.Alive) return;

            var direction = GetInputValue(nameof(inDirection), inDirection);
            var distance = GetInputValue(nameof(inDistance), inDistance);

            direction.y = 0;
            direction.Normalize();

            if (isLocal)
            {
                direction = target.AsTransform().TransformDirection(direction);
            }

            _curPosition = target.AsTransform().Position + direction * distance;
            var curY = _curPosition.y;

            if (NavMesh.Raycast(target.AsTransform().Position + offsetRaycast, _curPosition + offsetRaycast, out var hit, NavMesh.AllAreas))
            {
                _curPosition = hit.position;
                _curPosition.y = curY;
            }

            _curDistance = Vector3.Distance(target.Position, _curPosition);
            Completed();
        }

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;

            if (port.fieldName == nameof(outPosition))
            {
                return _curPosition;
            }
            else if (port.fieldName == nameof(outDistance))
            {
                return _curDistance;
            }

            return null;
        }
    }
}

