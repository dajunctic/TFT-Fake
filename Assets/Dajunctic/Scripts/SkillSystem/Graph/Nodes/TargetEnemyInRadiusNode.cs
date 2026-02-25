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

    public class TargetEnemyInRadiusNode : SkillNode
    {
        public TargetType targetType;
        public float radius = 5f;
        public bool targetAll = true;
        [ShowIf("@targetAll==false")] public int count = 1;

        [NodeOutput] public List<IDamageTaker> targets;
        [NodeOutput] public IDamageTaker mainTarget;

        private bool isValid;

        protected override void OnInit()
        {
            base.OnInit();
            ClearTargets();
        }

        public override object GetValue(string portName)
        {
            FindTargets();

            if (portName == nameof(targets)) return targets;
            if (portName == nameof(mainTarget)) return mainTarget;
            return base.GetValue(portName);
        }

        private void FindTargets()
        {
            if (mainTarget == null) isValid = false;
            if (isValid) return;

            List<IDamageTaker> foundActors;

            // ── Editor Preview: dummies injected vào context ─────────────────
            if (!Application.isPlaying &&
                _context.nodeOutputs.TryGetValue("__preview_dummies__", out var raw) &&
                raw is List<IDamageTaker> previewDummies)
            {
                // Lọc theo team (khác team với caster)
                var ownerCA = owner.AsCombatActor();
                foundActors = previewDummies
                    .Where(d =>
                    {
                        var dCA = d.AsCombatActor();
                        return dCA == null || ownerCA == null || dCA.Team != ownerCA.Team;
                    })
                    .ToList();
            }
            else
            {
                // ── Runtime: scan bằng Physics ────────────────────────────────
                SkillHelper.ScanTargetInRadius(owner.AsDamageTaker(), radius, out foundActors);
            }

            if (foundActors != null && foundActors.Count > 0)
            {
                switch (targetType)
                {
                    case TargetType.NearestEnemy:
                        foundActors = foundActors.OrderBy(a => Vector3.Distance(owner.AsTransform().Position, a.AsTransform().Position)).ToList();
                        break;
                    case TargetType.FarthestEnemy:
                        foundActors = foundActors.OrderByDescending(a => Vector3.Distance(owner.AsTransform().Position, a.AsTransform().Position)).ToList();
                        break;
                    case TargetType.Random:
                        foundActors = foundActors.OrderBy(_ => Random.value).ToList();
                        break;
                }

                if (!targetAll)
                    foundActors = foundActors.Take(count).ToList();

                targets = new List<IDamageTaker>(foundActors);
                mainTarget = targets.Count > 0 ? targets[0] : null;
            }

            Debug.Log($"<color=green>[TargetInRadius<color=red><{owner.AsCombatActor()?.DataId}></color>]</color> Found: {targets?.Count ?? 0} targets.");
            isValid = true;
        }

        public void ClearTargets()
        {
            targets = new List<IDamageTaker>();
            mainTarget = null;
            isValid = false;
        }

        public override void Reset()
        {
            base.Reset();
            targets = null;
            mainTarget = null;
            isValid = false;
        }
    }
}
