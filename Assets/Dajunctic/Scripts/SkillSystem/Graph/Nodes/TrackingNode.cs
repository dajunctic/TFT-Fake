using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class TrackingNode : SkillNode
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        [XNode.Node.InputAttribute] public IDamageTaker target;

        [SerializeField] private float duration;
        [SerializeField] private bool immediately;

        [SerializeField] private bool isManual;
        [XNode.Node.InputAttribute] private Vector3 manualDirection;

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
                Rotate();
                Complete();
            }

            else
            {
                StartCoroutine();
            }

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
                    Debug.LogError("Can not found Target");
                }

                if (_inTarget != null && _inTarget.CanBeTarget && _inTarget != Owner)
                {
                    Owner.AsMovable().RotatePosition(_inTarget.MidPoint, Owner.AsCombatActor().RotateSpeed, Time.deltaTime, immediately);
                }

            }
        }

        public override IEnumerator IECoroutine()
        {
            var timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                Rotate();
                yield return null;
                timeElapsed += Time.deltaTime;
            }

            Complete();
        }


        public override void Reset()
        {
            base.Reset();
            target = null;
        }
    }
}
