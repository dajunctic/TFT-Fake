using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public abstract class BaseGambitCondition : IGambitCondition
    {
        [SerializeField]
        public GambitConditionRangeType rangeType;

        [SerializeField]
        public float customRange = 10f; 

        public float Range
        {
            get
            {
                switch (rangeType)
                {
                    case GambitConditionRangeType.Engage:
                        return CombatActor != null ? 5f : 0f; 
                    case GambitConditionRangeType.Global:
                        return 100f;
                    case GambitConditionRangeType.Custom:
                        return customRange;
                    default:
                        return 0f;
                }
            }
        }

        protected ICombatActor CombatActor { get; private set; }

        GambitTargetType _targetType;
        List<IDamageTaker> _targets = new();

        public virtual IGambitCondition CreateCopy()
        {
            var copy = Activator.CreateInstance(GetType()) as BaseGambitCondition;
            copy.rangeType = rangeType;
            copy.customRange = customRange;
            return copy;
        }

        public virtual void Initialize(ICombatActor combatActor, GambitTargetType targetType)
        {
            CombatActor = combatActor;
            _targetType = targetType;
        }

        public virtual void Cleanup()
        {
            CombatActor = null;
        }

        public virtual void Refresh() { }

        public IDamageTaker Check()
        {
            if (CombatActor == null)
            {
                return null;
            }
            if (!CheckTargetInRange())
            {
                return null;
            }
            return CheckInternal(_targets);
        }

        public virtual bool CheckTargetInRange()
        {
            _targets.Clear();
            var actor = CombatActor as CombatActor;
            if (actor == null) return false;

            var allActors = Dajunctic.CombatActor.ActiveActors;
            foreach (var target in allActors)
            {
                if (target.Hp <= 0 || !target.gameObject.activeInHierarchy || !target.CanBeTarget) continue;

                float dist = Vector3.Distance(actor.Position, target.Position);
                if (dist > Range) continue;

                switch (_targetType)
                {
                    case GambitTargetType.Enemy:
                        if (target.CombatTeam != actor.CombatTeam)
                            _targets.Add(target);
                        break;
                    case GambitTargetType.Ally:
                        if (target.CombatTeam == actor.CombatTeam)
                            _targets.Add(target);
                        break;
                    case GambitTargetType.AllyExcludeSelf:
                        if (target.CombatTeam == actor.CombatTeam && target != actor)
                            _targets.Add(target);
                        break;
                    case GambitTargetType.Self:
                        if (target == actor)
                            _targets.Add(target);
                        break;
                }
            }
            if (_targets.Count == 0)
            {
                return false;
            }
            return true;
        }

        protected abstract IDamageTaker CheckInternal(List<IDamageTaker> targets);
    }

    public enum GambitConditionRangeType
    {
        Engage,
        Global,
        Custom,
    }
}
