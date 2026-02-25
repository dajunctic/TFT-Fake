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

        private IDamageTaker _inTarget;

        public override void Execute()
        {
            _inTarget = GetInputValue<IDamageTaker>(nameof(target));
            if (_inTarget != null)
            {
                duration = 1f;

            }
            else
            {
                
            }

            TriggerComplete();
        }

        public override IEnumerator IECoroutine()
        {
            var actor = owner.AsCombatActor();
            while (true)
            {
                

                actor.RotatePosition(_inTarget.AsTransform().Position, actor.AsCombatActor().RotateSpeed, Time.deltaTime, immediately);
                yield return null;
            }
        }


        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}