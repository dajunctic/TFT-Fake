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
            Debug.Log($"[TargetInRadiusNode] FindAllTargets called on node '{name}'");
            List<IDamageTaker> foundActors = null;

            // ── Editor Preview ─────────────────
            if (!Application.isPlaying && _context != null &&
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
            else if (Application.isPlaying)
            {
                // ── Runtime ────────────────────────────────
                SkillHelper.ScanTargetInRadius(Owner.AsDamageTaker(), range, out foundActors);
            }

            allTargets.Clear();
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

                allTargets.AddRange(foundActors);
            }

            if (allTargets.Count > 0)
            {
                BindTarget(allTargets[0]);
            }
            else
            {
                BindTarget(null);
            }

            Debug.Log($"<color=green>[TargetInRadiusNode <color=red><{Owner.AsCombatActor()?.DataId}></color>]</color> Found: {allTargets.Count} targets.");
        }

        protected override IDamageTaker GetOtherMainTarget(List<IDamageTaker> allTargets)
        {
            return allTargets != null && allTargets.Count > 0 ? allTargets[0] : null;
        }

        protected override bool IsCurrentMainTargetIsValid(IDamageTaker currentMainTarget, float range)
        {
            return currentMainTarget != null
                 && currentMainTarget.AsTeamMember().CombatTeam != Owner.AsTeamMember().CombatTeam
                 && SkillHelper.IsInAbilityTargetingRange(OwnerOffset, OwnerRadius, currentMainTarget, range);
        }
    }
}
