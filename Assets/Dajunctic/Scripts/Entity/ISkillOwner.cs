using System;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic
{
    public interface ISkillOwner: IAbilityOwner
    {
        SkillGroup UltimateGroup { get; }
        ISkillEntity GetSkill(object val);
    }
}