using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class TrackingNode : SkillNode
    {
        [NodeInput] public IDamageTaker target;
        
        [SerializeField] private float duration;
        [SerializeField] private bool immediately;

        [SerializeField] private bool isManual;
        [NodeInput] private Vector3 manualDirection;

        private IDamageTaker _inTarget;
        private Vector3 _inManualDirection;


        public override void Execute()
        {
            _inTarget = GetInputValue<IDamageTaker>(nameof(target));
            _inManualDirection = GetInputValue<Vector3>(nameof(manualDirection));

            if (_inManualDirection == Vector3.zero)
            {
                _inManualDirection = Owner.AsTransform().Forward;
            }

            if (immediately)
            {
                
            }

            StartCoroutine();

            Complete();
        }

        private void Rotate()
        {
            if (isManual)
            {
                Owner.AsMovable().RotateDirection(_inManualDirection, Owner.AsCombatActor().RotateSpeed, Time.deltaTime, immediately);
            }
            else
            {
                _inTarget = GetInputValue<IDamageTaker>(nameof(target));
                if (_inTarget == null || !_inTarget.CanBeTarget)
                {
                    
                }

                if (_inTarget != null && _inTarget.CanBeTarget && _inTarget != Owner)
                {
                    Owner.AsMovable().RotatePosition(_inTarget.MidPoint, Owner.AsCombatActor().RotateSpeed, Time.deltaTime, immediately);
                }   

            }
        }

        public override IEnumerator IECoroutine()
        {
            var actor = Owner.AsCombatActor();
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