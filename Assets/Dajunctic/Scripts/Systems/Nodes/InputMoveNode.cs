using UnityEngine;

namespace Dajunctic
{
    public class InputMoveNode : Node
    {
        private Transform _cameraTransform;
        private HeroCombatActor _leader;

        public InputMoveNode(CombatActor actor) : base(actor)
        {
            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
            }
            _leader = combatActor as HeroCombatActor;
        }

        public override NodeState Evaluate()
        {
            var inputDirection = Vector2.zero;
            if (FloatingJoystick.Instance != null)
            {
                inputDirection = FloatingJoystick.Instance.InputDirection;
            }
            else if (VirtualJoystick.Instance != null) 
            {
                inputDirection = VirtualJoystick.Instance.InputDirection;
            }

            if (inputDirection.sqrMagnitude == 0f && InputManager.Instance != null)
            {
                inputDirection = InputManager.Instance.GetMoveInputVector();
            }

            if (inputDirection.sqrMagnitude > 0f)
            {
                if (combatActor.IsCasting)
                {
                    combatActor.InterruptAction();
                }
                
                if (_leader != null) _leader.IsMovingByInput = true;
                
                if (SquadManager.Instance != null) 
                {
                    SquadManager.Instance.ClearSharedTarget();
                }
                
                Vector3 moveDir = new Vector3(inputDirection.x, 0, inputDirection.y);
                
                if (_cameraTransform != null)
                {
                    Vector3 cameraForward = _cameraTransform.forward;
                    Vector3 cameraRight = _cameraTransform.right;
                    cameraForward.y = 0;
                    cameraRight.y = 0;
                    cameraForward.Normalize();
                    cameraRight.Normalize();
                    
                    moveDir = (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized;
                }

                combatActor.MoveDirection(moveDir, combatActor.Speed, combatActor.RotateSpeed, Time.deltaTime);
                return ReturnRunning();
            }
            
            if (_leader != null && _leader.IsMovingByInput)
            {
                _leader.IsMovingByInput = false;
                combatActor.ForceStop(); 
            }
            return NodeState.Failure;
        }
    }
}