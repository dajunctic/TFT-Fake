using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public enum TargetType
    {
        NearestEnemy,
        FarthestEnemy,
        Random
    }

    public class TargetInRadiusNode : TargetNode
    {
        protected override void FindAllTargets(ref IDamageTaker currentMainTarget, List<IDamageTaker> currentAllTargets, float range)
        {
            if (!Application.isPlaying) return;

            List<IDamageTaker> foundActors = null;
            SkillHelper.ScanTargetInRadius(Owner.AsDamageTaker(), range, out foundActors);

            currentAllTargets.Clear();
            if (foundActors != null && foundActors.Count > 0)
            {
                switch (targetType)
                {
                    case TargetType.NearestEnemy:
                        foundActors = foundActors.OrderBy(a => Vector3.Distance(Owner.AsTransform().Position, a.AsTransform().Position)).ToList();
                        break;
                    case TargetType.FarthestEnemy:
                        foundActors = foundActors.OrderByDescending(a => Vector3.Distance(Owner.AsTransform().Position, a.AsTransform().Position)).ToList();
                        break;
                    case TargetType.Random:
                        foundActors = foundActors.OrderBy(_ => Random.value).ToList();
                        break;
                }

                if (!targetAll)
                    foundActors = foundActors.Take(count).ToList();

                currentAllTargets.AddRange(foundActors);
            }

            if (currentAllTargets.Count > 0)
            {
                currentMainTarget = currentAllTargets[0];
            }
            else
            {
                currentMainTarget = null;
            }
        }

        protected override IDamageTaker GetOtherMainTarget(List<IDamageTaker> allTargets)
        {
            return allTargets != null && allTargets.Count > 0 ? allTargets[0] : null;
        }

        protected override bool IsCurrentMainTargetIsValid(IDamageTaker currentMainTarget, float range)
        {
            if (Owner == null || currentMainTarget == null) return false;
            return currentMainTarget.AsTeamMember().CombatTeam != Owner.AsTeamMember().CombatTeam
                 && SkillHelper.IsInAbilityTargetingRange(OwnerOffset, OwnerRadius, currentMainTarget, range);
        }
    }
}
