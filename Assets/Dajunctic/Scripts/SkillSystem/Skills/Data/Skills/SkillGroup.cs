using System;
using Dajunctic.SkillSystem.Panthera;
using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class SkillGroup
    {
        [SerializeField, GuidReference(typeof(IAbilityEntityData<>))] public string[] skillIds;
    }
}
