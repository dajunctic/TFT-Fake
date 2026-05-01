using FishNet.Object;
using UnityEngine;

namespace Dajunctic
{
    public class TacticianNetworkMovement : NetworkBehaviour
    {
        private TacticianActor _actor;

        private void Awake()
        {
            _actor = GetComponent<TacticianActor>();
        }

        [ServerRpc]
        public void CmdMoveTo(Vector3 position)
        {
            RpcMoveTo(position, transform.position);
        }

        [ObserversRpc(ExcludeOwner = true)]
        public void RpcMoveTo(Vector3 position, Vector3 serverPos)
        {
            if (_actor == null) return;
            
            // Correct drift if needed
            if (Vector3.Distance(transform.position, serverPos) > 3f)
            {
                _actor.Teleport(serverPos, false, false);
            }
            _actor.MovePosition(position, _actor.Speed, _actor.RotateSpeed, Time.deltaTime);
        }

        [ServerRpc]
        public void CmdTeleport(Vector3 position, bool checkNavMesh, bool fx)
        {
            RpcTeleport(position, checkNavMesh, fx);
        }

        [ObserversRpc(ExcludeOwner = true)]
        public void RpcTeleport(Vector3 position, bool checkNavMesh, bool fx)
        {
            if (_actor == null) return;
            _actor.Teleport(position, checkNavMesh, fx);
        }

        private Vector3 _lastSentMoveDir;
        private float _lastSendTime;
        private Vector3 _currentMoveDir;

        public void SendMoveDirection(Vector3 dir)
        {
            if (dir == Vector3.zero && _lastSentMoveDir == Vector3.zero) return;

            if (Vector3.Distance(dir, _lastSentMoveDir) > 0.1f || Time.time - _lastSendTime > 0.2f)
            {
                _lastSentMoveDir = dir;
                _lastSendTime = Time.time;
                CmdMoveDirection(dir, transform.position);
            }
        }

        [ServerRpc]
        public void CmdMoveDirection(Vector3 direction, Vector3 position)
        {
            RpcMoveDirection(direction, position);
        }

        [ObserversRpc(ExcludeOwner = true)]
        public void RpcMoveDirection(Vector3 direction, Vector3 serverPos)
        {
            if (_actor == null) { Debug.LogError("[RpcMoveDir] _actor is null!"); return; }
            if (_actor.MoveAgent == null) Debug.LogWarning($"[RpcMoveDir] MoveAgent is null on {gameObject.name}!");
            else if (!_actor.MoveAgent.Initialized) Debug.LogWarning($"[RpcMoveDir] MoveAgent NOT initialized on {gameObject.name}!");
            
            // Correct drift if needed
            if (Vector3.Distance(transform.position, serverPos) > 3f)
            {
                _actor.Teleport(serverPos, false, false);
            }
            _currentMoveDir = direction;
        }

        private void Update()
        {
            if (!IsOwner && _actor != null && _currentMoveDir.sqrMagnitude > 0f)
            {
                if (_actor.MoveAgent == null)
                {
                    Debug.LogWarning($"[TacticianNetMove] MoveAgent is null for {gameObject.name}, can't move!");
                    return;
                }
                _actor.MoveDirection(_currentMoveDir, _actor.Speed, _actor.RotateSpeed, Time.deltaTime);
            }
        }
    }
}
