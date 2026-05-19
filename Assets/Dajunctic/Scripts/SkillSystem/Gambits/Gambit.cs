using System;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class Gambit
    {
        [SerializeField]
        public GambitTargetType targetType = GambitTargetType.Enemy;

        [SerializeReference]
        public IGambitCondition condition;

        [SerializeReference]
        public IGambitAction action;

        public Gambit CreateCopy()
        {
            return new Gambit
            {
                targetType = targetType,
                condition = condition?.CreateCopy(),
                action = action?.CreateCopy(),
            };
        }

        public void Initialize(ICombatActor combatActor)
        {
            condition?.Initialize(combatActor, targetType);
            action?.Initialize(combatActor, condition);
        }

        public void Cleanup()
        {
            condition?.Cleanup();
            action?.Cleanup();
        }

        public void Refresh()
        {
            condition?.Refresh();
            action?.Refresh();
        }
    }

    public interface IGambitCondition
    {
        IGambitCondition CreateCopy();
        void Initialize(ICombatActor combatActor, GambitTargetType targetType);
        void Cleanup();
        void Refresh();
        IDamageTaker Check();
    }

    public interface IGambitAction
    {
        IGambitAction CreateCopy();
        bool IsPlaying { get; }
        bool IsCanNotBeInterrupted { get; }
        IGambitCondition Condition { get; }
        void Initialize(ICombatActor combatActor, IGambitCondition condition);
        void Cleanup();
        void Refresh();
        bool CheckCanPlay();
        void Play(IDamageTaker target);
        void Stop();
    }

    public enum GambitTargetType
    {
        Enemy,
        Ally,
        AllyExcludeSelf,
        Self,
    }
}
