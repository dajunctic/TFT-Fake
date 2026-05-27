using System.Linq;
using Dajunctic.SkillSystem.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [CreateAssetMenu(menuName = "SkillSystem/Ability/Skill Graph")]
    public class SkillGraph : AbilityGraph<ISkillEntity, ISkillEntityData, SkillLevelData, ISkillOwner>
    {
        public override void Initialize() { base.Initialize(); }
        protected override void StopInternal() { base.StopInternal(); }
        public IDamageTaker GetPlayingTrackingTarget() { return null; }
        protected override void BindTarget(IDamageTaker target) { }
        public void ClearTarget() { }
        public float GetRange() { return 0f; }
    }
}
