using System;
using FMODUnity;
using Dajunctic.SkillSystem.Panthera;
using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class AbilityEntityData<T> : IAbilityEntityData<T> where T : AbilityLevelData
    {
        [SerializeField, NamedId] string id;
        [SerializeField] int maxLevel = 1;
        [SerializeField] AbilityStaticData staticData;
        [SerializeField] T levelData;

        public AbilityType AbilityType => staticData.abilityType;
        public string Id => id;
        public int MaxLevel => maxLevel;
        public AbilityStaticData StaticData => staticData;
        public T LevelData => levelData;
    }
}
