using System.Linq;
using Dajunctic.SkillSystem.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [CreateAssetMenu(menuName = "Dajunctic.SkillSystem/Ability/Skill Graph")]
    public class SkillGraph : AbilityGraph<ISkillEntity, ISkillEntityData, SkillLevelData, ISkillOwner>
    {
        [Button, ContextMenu("Check Consume Node")]
        public void CheckConsumeNode()
        {
            if (name.Contains("basic") || name.Contains("die") || name.Contains("crit"))
            {
                return;   
            }
        }

        public override void Initialize() { base.Initialize(); }
        protected override void StopInternal() { base.StopInternal(); }
        public IDamageTaker GetPlayingTrackingTarget() { return null; }
        protected override void BindTarget(IDamageTaker target) { }
        public void ClearTarget() { }
        public float GetRange() { return 0f; }
    }
}

