using System;
using System.Collections.Generic;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class NearestGambitCondition : BaseGambitCondition
    {
        protected override IDamageTaker CheckInternal(List<IDamageTaker> targets)
        {
            if (targets.Count == 0)
            {
                return null;
            }
            return SkillHelper.FindNearestTarget(CombatActor.AsTransform().Position, targets);
        }
    }
}
