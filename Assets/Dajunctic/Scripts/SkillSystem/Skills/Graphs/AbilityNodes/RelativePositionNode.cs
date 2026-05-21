using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/RelativePosition")]
    public class RelativePositionNode : AbilityNode
    {
        [GraphProcessor.Input(name = "inTarget")] public IDamageTaker inTarget;
        [GraphProcessor.Input(name = "inDirection")] public Vector3 inDirection;
        [GraphProcessor.Input(name = "inDistance")] public float inDistance;
        [SerializeField] Vector3 offsetRaycast;
        [SerializeField] bool isLocal;
        [GraphProcessor.Output(name = "outPosition")] public Vector3 outPosition;
        [GraphProcessor.Output(name = "outDistance")] public float outDistance;

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
    }
}

