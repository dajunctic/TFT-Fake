namespace Dajunctic.SkillSystem.Data
{
    public interface IAbilityEntityData<T> : IData
        where T : AbilityLevelData
    {
        AbilityType AbilityType { get; }
        int MaxLevel { get; }
        AbilityStaticData StaticData { get; }
        T LevelData { get; }
    }

    public enum AbilityType
    {
        Ultimate,
        Skill1,
        Skill2,
        Basic,
        Passive1,
        Passive2,
        Passive3,
        Fragment,
        TeamCompPassive,
        Die,
        CaptainBuff,
        PassiveTrait,
        Skill3,
        Skill4,
    }
}
