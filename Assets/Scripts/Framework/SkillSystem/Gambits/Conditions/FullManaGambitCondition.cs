using IDamageTaker = Dajunctic.IDamageTaker;
using System;
using System.Collections.Generic;
using Dajunctic.SkillSystem.Logic;
using Dajunctic;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class FullManaGambitCondition : BaseGambitCondition
    {
        protected override Dajunctic.IDamageTaker CheckInternal(List<Dajunctic.IDamageTaker> targets)
        {
            var actor = CombatActor as CombatActor;
            if (actor == null || actor.Energy < actor.MaxEnergy)
            {
                return null;
            }

            if (targets.Count == 0) return null;

            Dajunctic.IDamageTaker nearest = null;
            float minDist = float.MaxValue;
            foreach (var t in targets)
            {
                var targetActor = t as CombatActor;
                if (targetActor == null) continue;

                float d = UnityEngine.Vector3.Distance(actor.Position, targetActor.Position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = t;
                }
            }
            return nearest;
        }
    }
}
