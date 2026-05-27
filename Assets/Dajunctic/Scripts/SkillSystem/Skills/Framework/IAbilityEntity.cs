using System.Collections.Generic;
using Dajunctic.SkillSystem.Data;

namespace Dajunctic.SkillSystem.Logic
{
    public interface IAbilityEntity : IEntity
    {
        int Level { get; }
        bool IsMaxed { get; }
        float Range { get; }
        bool IsPlaying { get; }
        bool IsUsable { get; }
        void ResetFields();
        void Play(IDamageTaker target = null);
        void Stop();
        IDamageTaker GetTrackingTarget();
        void ClearTarget();
        Dictionary<string, IAbilityProperty> GetProperties();
        AbilityDescription[] GetAllDescription();
        IAbilityLevelProvider LevelProvider { get; }
        void SetLevelProvider(IAbilityLevelProvider levelProvider);
        AbilityType GetAbilityType();
    }

    public interface IAbilityEntity<TOwner> : IAbilityEntity
        where TOwner : IAbilityOwner
    {
        TOwner Owner { get; }
        void SetOwner(TOwner owner);
    }

    public interface IAbilityEntity<TAbilityEntityData, TAbilityLevelData, TOwner>
        : IAbilityEntity<TOwner>
        where TAbilityEntityData : IAbilityEntityData<TAbilityLevelData>
        where TAbilityLevelData : AbilityLevelData
        where TOwner : IAbilityOwner
    {
        TAbilityEntityData Data { get; }
        TAbilityLevelData LevelData { get; }
    }
}
