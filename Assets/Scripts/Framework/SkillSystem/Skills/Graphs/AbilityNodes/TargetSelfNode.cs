using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/TargetSelf")]
    public class TargetSelfNode : AbilityNode
    {
        [SerializeReference, Output] protected List<IDamageTaker> targets;
        [SerializeReference, Output] protected IDamageTaker mainTarget;

        protected List<IDamageTaker> _cachedTargets = new List<IDamageTaker>();
        protected IDamageTaker _cachedMainTarget;

        protected IDamageTaker GetMainTarget()
        {
            _cachedTargets.Clear();
            _cachedMainTarget = Owner != null ? Owner.AsDamageTaker() : null;
            if (_cachedMainTarget != null)
            {
                _cachedTargets.Add(_cachedMainTarget);
            }
            Debug.LogError($"[TargetSelfNode] GetMainTarget evaluated. Owner/Caster: {(_cachedMainTarget as CombatActor)?.gameObject?.name}, Targets count: {_cachedTargets.Count}");
            return _cachedMainTarget;
        }
    }
}
