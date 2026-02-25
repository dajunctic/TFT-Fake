using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class TrackingNode : SkillNode
    {
        [NodeInput] public List<CombatActor> actorsToTrack;

        public override void Execute()
        {
            if (actorsToTrack != null && actorsToTrack.Count > 0)
            {
                Debug.Log($"[Tracking] Node {guid} is tracking {actorsToTrack.Count} targets.");
                // Thực hiện logic tracking ở đây...
            }
            else
            {
                Debug.LogWarning($"[Tracking] Node {guid} has no targets to track.");
            }

            TriggerComplete();
        }

        public override void Reset()
        {
            base.Reset();
            actorsToTrack = null;
        }
    }
}