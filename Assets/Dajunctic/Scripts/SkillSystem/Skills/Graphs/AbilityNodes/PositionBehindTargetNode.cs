using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/PositionBehindTarget")]
    public class PositionBehindTargetNode : AbilityNode
    {
        [SerializeField] private float distanceBehind = 1f;
        [GraphProcessor.Input] private IDamageTaker target;

        [GraphProcessor.Output] public Vector3 outPosition;
        [GraphProcessor.Output] public Vector3 outDirection;
    }
}
