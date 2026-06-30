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
        public float GetRange()
        {
            float minRange = 0f;
            bool found = false;
            foreach (var node in nodes)
            {
                if (node is IHasRange rangeNode)
                {
                    float r = rangeNode.GetRange();
                    if (!found || r < minRange)
                    {
                        minRange = r;
                        found = true;
                    }
                }
            }
            return found ? minRange : 0f;
        }
    }
}
