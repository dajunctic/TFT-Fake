using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class TacticianActor : CombatActor
    {
        [Header("Tacticians")]
        public int Tier { get; private set; } = 1;
        public bool IsBoss { get; private set; } = false;

        public override string DataId => name;
        public override bool CanBeTarget => true;
        private FishNet.Object.NetworkObject _netObj;
        public bool IsLocalPlayer => _netObj != null && _netObj.IsOwner;
        
        public new int OwnerID 
        {
            get => _netObj != null && _netObj.IsSpawned ? _netObj.OwnerId : base.OwnerID;
            set => base.OwnerID = value;
        }
        private Transform _cameraTransform;
        private Camera _camera;
        private bool _tacticianInitialized;

        public override void Initialize()
        {
            base.Initialize();
            if (_tacticianInitialized) return;
            _tacticianInitialized = true;
            _netObj = GetComponent<FishNet.Object.NetworkObject>();

            if (CombatActorData is TacticianData tacticianData)
            {
                Tier = tacticianData.tier;
                IsBoss = tacticianData.isBoss;
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

            this.Raise(new SpawnHpViewEvent { owner = this, starLevel = Tier });
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            InputManager.OnRightClickEvent -= OnRightClick;
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            InputManager.OnRightClickEvent -= OnRightClick; // Ensure clean state
            if (IsLocalPlayer)
            {
                InputManager.OnRightClickEvent += OnRightClick;
            }
        }


        private void OnRightClick(Vector2 mousePosition)
        {
            if (_camera == null) _camera = Camera.main;
            if (_camera == null) return;

            FollowCamera followCam = _camera.GetComponent<FollowCamera>();
            if (followCam != null && followCam.target != null)
            {
                TacticianActor viewTarget = followCam.target.GetComponent<TacticianActor>();
                if (viewTarget != null && viewTarget.OwnerID != this.OwnerID)
                {
                    if (Gameplay.Instance != null && Gameplay.Instance.CurrentPhase == GameplayPhase.Combat)
                    {
                        return; // Block movement during combat on another arena
                    }
                }
            }

            var ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                var targetPosition = hitInfo.point;

                if (UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out UnityEngine.AI.NavMeshHit navHit, 5f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    targetPosition = navHit.position;
                }

                MovePosition(targetPosition, Speed, RotateSpeed, Time.deltaTime);
            }
        }

        public override void Tick()
        {
            base.Tick();

            if (!IsLocalPlayer) return;

            if (_camera == null) _camera = Camera.main;
            if (_camera != null)
            {
                FollowCamera followCam = _camera.GetComponent<FollowCamera>();
                if (followCam != null)
                {
                    if (followCam.target != this.transform)
                    {
                        followCam.target = this.transform;
                    }

                    TacticianActor viewTarget = followCam.target.GetComponent<TacticianActor>();
                    if (viewTarget != null && viewTarget.OwnerID != this.OwnerID)
                    {
                        if (Gameplay.Instance != null && Gameplay.Instance.CurrentPhase == GameplayPhase.Combat)
                        {
                            return; // Block joystick during combat on another arena
                        }
                    }
                }
            }

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
