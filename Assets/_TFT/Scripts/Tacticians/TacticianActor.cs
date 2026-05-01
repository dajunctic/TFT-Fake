using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object;

namespace Dajunctic
{
    public class TacticianActor : CombatActor
    {
        private TacticianNetworkMovement _networkMovement;
        [Header("Tacticians")]
        public int Tier { get; private set; } = 1;
        public bool IsBoss { get; private set; } = false;

        public override string DataId => name;
        public override bool CanBeTarget => true;
        private FishNet.Object.NetworkObject _netObj;
        public bool IsLocalPlayer => _netObj != null && _netObj.IsOwner;
        
        public new int OwnerID 
        {
            get => _netObj != null && _netObj.IsSpawned && _netObj.OwnerId != -1 ? _netObj.OwnerId : base.OwnerID;
            set => base.OwnerID = value;
        }

        protected override ActorMovementType ActorMovementType => ActorMovementType.Navmesh;

        private Transform _cameraTransform;
        private Camera _camera;
        private bool _tacticianInitialized;

        protected virtual void Start()
        {
            if (_netObj == null) _netObj = GetComponent<FishNet.Object.NetworkObject>();
        }

        private bool _tacticianLinked = false;
        protected virtual void Update()
        {
            if (!_eventsListened && IsLocalPlayer)
            {
                _eventsListened = true;
                StopListenEvents();
                ListenEvents();
            }

            if (!_tacticianLinked)
            {
                int resolvedOwnerId = _netObj != null && _netObj.IsSpawned ? _netObj.OwnerId : -1;
                
                if (resolvedOwnerId == -1 && GameSystemManager.Instance != null && GameSystemManager.Instance.Field != null)
                {
                    var arenas = GameSystemManager.Instance.Field.GetAllArenas();
                    if (arenas != null)
                    {
                        foreach(var a in arenas)
                        {
                            Vector3 spawnPos = a.TacticianSpawnPoint != null ? a.TacticianSpawnPoint.position : a.transform.position;
                            if (Vector3.Distance(transform.position, spawnPos) < 2f)
                            {
                                resolvedOwnerId = a.OwnerID;
                                break;
                            }
                        }
                    }
                }

                if (resolvedOwnerId != -1)
                {
                    base.OwnerID = resolvedOwnerId;
                    if (GameSystemManager.Instance != null && GameSystemManager.Instance.Player != null)
                    {
                        foreach (var p in GameSystemManager.Instance.Player.Players)
                        {
                            if (p.Id == resolvedOwnerId)
                            {
                                p.Tactician = this;
                                _tacticianLinked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

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
            
            _networkMovement = GetComponent<TacticianNetworkMovement>();
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

        private bool _eventsListened = false;

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
                if (IsLocalPlayer && _networkMovement != null)
                {
                    _networkMovement.CmdMoveTo(targetPosition);
                }
            }
        }

        public override void Teleport(Vector3 position, bool checkNavMesh, bool fx = false)
        {
            base.Teleport(position, checkNavMesh, fx);
            if (IsLocalPlayer && _networkMovement != null)
            {
                _networkMovement.CmdTeleport(position, checkNavMesh, fx);
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
                    // Removed unconditional camera lock to allow viewing other players' arenas

                    if (followCam.target != null)
                    {
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
                if (IsLocalPlayer && _networkMovement != null)
                {
                    _networkMovement.SendMoveDirection(moveDir);
                }
            }
            else
            {
                if (IsLocalPlayer && _networkMovement != null)
                {
                    _networkMovement.SendMoveDirection(Vector3.zero);
                }
            }
        }

      
    }
}
