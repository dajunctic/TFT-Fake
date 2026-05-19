using System;
using System.Collections.Generic;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public abstract class BaseGambitCondition : IGambitCondition
    {
        [SerializeField]
        public GambitConditionRangeType rangeType;

        [SerializeField]
        public float customRange = 10f; // Simplified for TFT-Fake

        public float Range
        {
            get
            {
                switch (rangeType)
                {
                    case GambitConditionRangeType.Engage:
                        return CombatActor != null ? 5f : 0f; // Arbitrary engage radius
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
            switch (_targetType)
            {
                case GambitTargetType.Enemy:
                    SkillHelper.FindTargetsInRadius(
                        CombatActor.AsTeamMember().EnemyTeam,
                        CombatActor.AsTransform().Position,
                        1.5f,
                        Range,
                        null,
                        _targets,
                        5
                    );
                    break;
                case GambitTargetType.Ally:
                    SkillHelper.FindTargetsInRadius(
                        CombatActor.AsTeamMember().CombatTeam,
                        CombatActor.AsTransform().Position,
                        1.5f,
                        Range,
                        null,
                        _targets,
                        5
                    );
                    break;
                case GambitTargetType.AllyExcludeSelf:
                    SkillHelper.FindTargetsInRadius(
                        CombatActor.AsTeamMember().CombatTeam,
                        CombatActor.AsTransform().Position,
                        1.5f,
                        Range,
                        null,
                        _targets,
                        5
                    );
                    _targets.Remove(CombatActor.AsDamageTaker());
                    break;
                case GambitTargetType.Self:
                    _targets.Add(CombatActor.AsDamageTaker());
                    break;
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
