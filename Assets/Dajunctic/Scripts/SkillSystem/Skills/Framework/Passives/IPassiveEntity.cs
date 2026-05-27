using Dajunctic.SkillSystem.Data;

namespace Dajunctic.SkillSystem.Logic
{
    public interface IPassiveEntity : IAbilityEntity<IPassiveEntityData, PassiveLevelData, IPassiveOwner>
    {
        bool IsUnlocked { get; }
    }
}
