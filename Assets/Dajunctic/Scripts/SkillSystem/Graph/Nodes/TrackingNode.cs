using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class TrackingNode : SkillNode
    {
        [NodeInput] public IDamageTaker target;
        
        [SerializeField] private float duration;
        [SerializeField] private bool isManual;
        [SerializeField] private Vector3 direction;
        [SerializeField] private bool immediately;

        public override void Execute()
        {
            target = GetInputValue<IDamageTaker>(nameof(target));
            if (target != null)
            {
                duration = 1f;

            }
            else
            {
                
            }

            TriggerComplete();
        }

        // public override IEnumerator IECoroutine()
        // {
            
        // }

        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}