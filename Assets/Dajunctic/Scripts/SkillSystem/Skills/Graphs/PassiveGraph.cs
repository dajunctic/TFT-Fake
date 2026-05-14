using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [CreateAssetMenu(menuName = "Dajunctic.SkillSystem/Ability/Passive Graph")]
    public class PassiveGraph : AbilityGraph<IPassiveEntity, IPassiveEntityData, PassiveLevelData, IPassiveOwner>
    {
    }
}
