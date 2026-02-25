using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class TrackingNode : SkillNode
    {
        [SerializeField, NodeInput] public IDamageTaker target;

        public override void Execute()
        {
            target = GetInputValue<IDamageTaker>(nameof(target));
            if (target != null)
            {
                Debug.Log($"[Tracking] Node {guid} is tracking target.");
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
            target = null;
        }
    }
}