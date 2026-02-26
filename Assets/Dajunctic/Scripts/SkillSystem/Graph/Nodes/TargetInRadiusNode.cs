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
        protected override void FindAllTargets(IDamageTaker currentMainTarget, List<IDamageTaker> allTargets, float range)
        {
            List<IDamageTaker> foundActors;

            // ── Editor Preview: dummies injected vào context ─────────────────
            if (!Application.isPlaying &&
                _context.nodeOutputs.TryGetValue("__preview_dummies__", out var raw) &&
                raw is List<IDamageTaker> previewDummies)
            {
                var ownerCA = Owner.AsCombatActor();
                foundActors = previewDummies
                    .Where(d =>
                    {
                        var dCA = d.AsCombatActor();
                        return dCA == null || ownerCA == null || dCA.CombatTeam != ownerCA.CombatTeam;
                    })
                    .ToList();
            }
            else
            {
                // ── Runtime: scan bằng Physics ────────────────────────────────
                SkillHelper.ScanTargetInRadius(Owner.AsDamageTaker(), range, out foundActors);
            }

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

                allTargets = new List<IDamageTaker>(foundActors);
                mainTarget = allTargets.Count > 0 ? allTargets[0] : null;
            }

            Debug.Log($"<color=green>[Target<color=red><{Owner.AsCombatActor()?.DataId}></color>]</color> Found: {allTargets?.Count ?? 0} targets.");
        }

        protected override IDamageTaker GetOtherMainTarget(List<IDamageTaker> allTargets)
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsCurrentMainTargetIsValid(IDamageTaker currentMainTarget, float range)
        {
              return currentMainTarget != null
                   && currentMainTarget.AsTeamMember().CombatTeam != Owner.AsTeamMember().CombatTeam
                   && SkillHelper.IsInAbilityTargetingRange(OwnerOffset, OwnerRadius, currentMainTarget, range);
        }
    }
}
