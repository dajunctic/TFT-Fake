using System;
using System.Collections.Generic;
using Dajunctic.SkillSystem.Logic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class FullManaGambitCondition : BaseGambitCondition
    {
        protected override IDamageTaker CheckInternal(List<IDamageTaker> targets)
        {
            if (CombatActor.Mana < CombatActor.MaxMana)
            {
                return null;
            }

            if (targets.Count == 0)
            {
                return null;
            }
            // Defaults to nearest if multiple targets available when full mana
            return SkillHelper.FindNearestTarget(CombatActor.AsTransform().Position, targets);
        }
    }
}
