using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class MythicalAnimalCombatActor : CombatActor
    {
        [Header("Mythical Animal")]
        public int Tier { get; private set; } = 1;
        public bool IsBoss { get; private set; } = false;

        public override string DataId => name;
        public override bool CanBeTarget => true;

        
        private Transform _cameraTransform;
        private Camera _camera;
        public override void Initialize()
        {
            base.Initialize();

            if (CombatActorData is MythicalAnimalActorData mythicalData)
            {
                Tier = mythicalData.tier;
                IsBoss = mythicalData.isBoss;
            }

            if (MoveAgent != null)
            {
                MoveAgent.ToggleMoveCollision(false);
            }

             if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
                _camera = Camera.main;
            }
        
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            InputManager.OnRightClickEvent += OnRightClick;
        }

        private void OnRightClick(Vector2 mousePosition)
        {
            var ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var targetPosition = hitInfo.point;
                MovePosition(targetPosition, Speed, RotateSpeed, Time.deltaTime);
            }
        }

        public override void Tick()
        {
            base.Tick();

            var inputDirection = Vector3.zero;

            if (FloatingJoystick.Instance != null)
            {
                inputDirection = FloatingJoystick.Instance.InputDirection;
            }
            else if (VirtualJoystick.Instance != null) 
            {
                inputDirection = VirtualJoystick.Instance.InputDirection;
            }
            
            if (inputDirection.sqrMagnitude > 0f)
            {
                
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

                MoveDirection(moveDir, Speed, RotateSpeed, Time.deltaTime);
            }
        }

      
    }
}
