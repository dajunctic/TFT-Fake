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
    }
}
