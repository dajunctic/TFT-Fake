using System.Collections.Generic;
using UnityEngine;
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
        public override bool CanBeTarget => false;
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

        // Joystick network sync
        private float _lastJoystickSyncTime;
        private const float JoystickSyncInterval = 0.1f;
        private bool _wasJoystickActive;

        protected virtual void Start()
        {
            if (_netObj == null) _netObj = GetComponent<FishNet.Object.NetworkObject>();
        }

        private bool _tacticianLinked = false;
        protected virtual void Update()
        {
            // Nếu chưa đăng ký và IsOwner đã sẵn sàng → đăng ký input
            if (!_eventsListened && IsLocalPlayer)
            {
                StopListenEvents(); // clean + reset _eventsListened
                ListenEvents();     // đăng ký + set _eventsListened = true
                // Warp lại MoveAgent về vị trí thực tế (FishNet đã sync transform tới đây rồi)
                RewarpMoveAgent();
            }

            if (!_tacticianLinked)
            {
                int resolvedOwnerId = _netObj != null && _netObj.IsSpawned ? _netObj.OwnerId : -1;

                if (resolvedOwnerId == -1 && GameSystemManager.Instance != null && GameSystemManager.Instance.Field != null)
                {
                    var arenas = GameSystemManager.Instance.Field.GetAllArenas();
                    if (arenas != null)
                    {
                        foreach (var a in arenas)
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
                                // Warp ngay sau khi link: transform đã ở đúng vị trí rồi
                                RewarpMoveAgent();
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

            // Luôn update _netObj và _networkMovement (có thể null lúc Awake)
            if (_netObj == null) _netObj = GetComponent<FishNet.Object.NetworkObject>();
            if (_networkMovement == null) _networkMovement = GetComponent<TacticianNetworkMovement>();

            if (_tacticianInitialized)
            {
                // Đã init rồi → chỉ cần warp lại MoveAgent về đúng vị trí hiện tại
                // (trường hợp Awake init với vị trí prefab sai)
                RewarpMoveAgent();
                return;
            }
            _tacticianInitialized = true;

            if (CombatActorData is TacticianData tacticianData)
            {
                Tier = tacticianData.tier;
                IsBoss = tacticianData.isBoss;
            }

            if (MoveAgent != null)
            {
                MoveAgent.ToggleMoveCollision(false);
                // Warp về đúng vị trí hiện tại (tránh trường hợp Awake init ở pos sai)
                RewarpMoveAgent();
            }

            // Tắt CapsuleCollider để linh thú không đẩy tướng khi di chuyển
            // NavMeshAgent vẫn xử lý movement bình thường; click raycast dùng layer khác
            var col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            if (Camera.main != null)
            {
                _cameraTransform = Camera.main.transform;
                _camera = Camera.main;
            }

            this.Raise(new SpawnHpViewEvent { owner = this, starLevel = Tier });
        }

        /// <summary>Warp MoveAgent về vị trí thực của actor. Gọi sau khi actor đã spawn ở đúng vị trí.</summary>
        public void RewarpMoveAgent()
        {
            if (MoveAgent != null && MoveAgent.Initialized)
            {
                MoveAgent.Warp(transform.position);
            }
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            InputManager.OnRightClickEvent -= OnRightClick;
            _eventsListened = false; // Reset để Update() có thể đăng ký lại khi IsOwner sẵn sàng
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            InputManager.OnRightClickEvent -= OnRightClick; // Ensure clean state
            if (IsLocalPlayer)
            {
                InputManager.OnRightClickEvent += OnRightClick;
                _eventsListened = true;
            }
        }

        private bool _eventsListened = false;

        private void OnRightClick(Vector2 mousePosition)
        {
            if (_camera == null) _camera = Camera.main;
            if (_camera == null) { Debug.LogError("[OnRightClick] Camera is null!"); return; }

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
                bool onNavMesh = UnityEngine.AI.NavMesh.SamplePosition(targetPosition, out UnityEngine.AI.NavMeshHit navHit, 5f, UnityEngine.AI.NavMesh.AllAreas);
                if (onNavMesh) targetPosition = navHit.position;

                // Đảm bảo NavMeshAgent ở đúng vị trí actor trước khi move
                RewarpMoveAgent();

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

        private float _tickDebugTimer;
        public override void Tick()
        {
            // DEBUG: confirm Tick is running (log once per second max)
            _tickDebugTimer += Time.deltaTime;
            if (_tickDebugTimer > 1f)
            {
                _tickDebugTimer = 0;
                // Debug.Log($"[Tick ALIVE] {gameObject.name} IsLocalPlayer={IsLocalPlayer} frame={Time.frameCount}");
            }

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

                // Project a destination ahead and use MovePosition (same as click-to-move)
                Vector3 projected = transform.position + moveDir * (Speed * 0.5f);
                MovePosition(projected, Speed, RotateSpeed, 0.05f);

                // Sync projected destination to observers (throttled)
                if (IsLocalPlayer && _networkMovement != null && Time.time - _lastJoystickSyncTime >= JoystickSyncInterval)
                {
                    _networkMovement.CmdMoveTo(projected);
                    _lastJoystickSyncTime = Time.time;
                }
                _wasJoystickActive = true;
            }
            else if (_wasJoystickActive)
            {
                // Joystick released → send final position so observer stops at correct spot
                _wasJoystickActive = false;
                ForceStop();
                if (IsLocalPlayer && _networkMovement != null)
                {
                    _networkMovement.CmdMoveTo(transform.position);
                }
            }
        }


    }
}
