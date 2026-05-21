using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/PositionTarget")]
    public class PositionTargetNode : AbilityNode
    {
        [GraphProcessor.Input] private IDamageTaker target;
        [SerializeField] bool cached = true;
        [SerializeField] Vector3 offset = Vector3.zero;
        [GraphProcessor.Output] public Vector3 outPosition;
        [GraphProcessor.Output] public Vector3 outDirection;
        [GraphProcessor.Output] public float outDistance;

        IDamageTaker _cachedTarget;

        protected override void PlayInternal()
        {
            base.PlayInternal();
            _cachedTarget = GetInputValue(nameof(target), target);
            Completed();
        }
    }
}

