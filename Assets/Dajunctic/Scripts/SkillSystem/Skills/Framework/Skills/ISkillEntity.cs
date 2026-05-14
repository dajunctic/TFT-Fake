using System;
using Dajunctic.SkillSystem.Data;

namespace Dajunctic.SkillSystem.Logic
{
    public interface ISkillEntity : IAbilityEntity<ISkillEntityData, SkillLevelData, ISkillOwner>
    {
        event Action OnEnergyChangedEvent;
        SkillType SkillType { get; set; }
        float Cooldown { get; }
        float RequiredEnergy { get; }
        float Energy { get; }
        bool CannotBeInterrupt { get; }
        bool CanRecoverEnergy { get; }
        bool IsMaxPlayed { get; }
        float ElapsedTimeRatio { get; }
        void UpdateCooldown(float deltaTime);
        void ResetCooldown(bool isBeginCombat);
        void SetCooldown(float value);
    }

    public enum SkillType
    {
        BasicAttack,
        BasicCriticalAttack,
        Skill,
        Ultimate
    }
}

