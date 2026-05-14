using System;
using Dajunctic.SkillSystem.Data;

namespace Dajunctic.SkillSystem.Logic
{
    public interface IAbilityLevelProvider
    {
        event Action OnSkillLevelChangeEvent;
        event Action OnPassiveLevelChangeEvent;
        int GetSkillLevel(AbilityType abilityType);
        int GetPassiveLevel(AbilityType abilityType);
    }
}
