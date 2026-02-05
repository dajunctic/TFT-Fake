using UnityEngine;

namespace Dajunctic
{
    public class PatrolNode: Node
    {
        private HexAreaView _hexAreaView;
        private Vector3 _targetPosition;
        private bool _isMoving;

        public PatrolNode(CombatActor combatActorView, HexAreaView hexAreaView)
        {
            _hexAreaView = hexAreaView;
            combatActor = combatActorView;
            _targetPosition = combatActor.Position;
        }

        public override NodeState Evaluate()
        {
            if (_hexAreaView == null || _hexAreaView.Data == null) return NodeState.Failure;

            var distance = Vector3.Distance(combatActor.Position, _targetPosition);
            _isMoving = distance > 0.1f;

            if (combatActor.MoveAgent.Velocity.magnitude < 0.1f)
            {
                _targetPosition = _hexAreaView.GetRandomPosition();
                combatActor.MovePosition(_targetPosition, combatActor.PatrolSpeed, combatActor.RotateSpeed);
            }

            return NodeState.Success;
        }
    }
}