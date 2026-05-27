using FishNet.Object;
using UnityEngine;

namespace Dajunctic
{
    public class TacticianNetworkMovement : NetworkBehaviour
    {
        private TacticianActor _actor;

        private Vector3 _syncedDestination = Vector3.positiveInfinity;
        private bool _hasDestination = false;

        private void Awake()
        {
            _actor = GetComponent<TacticianActor>();
        }

        [ServerRpc]
        public void CmdMoveTo(Vector3 destination)
        {
            RpcSetDestination(destination);
        }

        [ObserversRpc(ExcludeOwner = true)]
        private void RpcSetDestination(Vector3 destination)
        {
            _syncedDestination = destination;
            _hasDestination = true;
        }

        [ServerRpc]
        public void CmdTeleport(Vector3 position, bool checkNavMesh, bool fx)
        {
            RpcTeleport(position, checkNavMesh, fx);
        }

        [ObserversRpc(ExcludeOwner = true)]
        private void RpcTeleport(Vector3 position, bool checkNavMesh, bool fx)
        {
            if (_actor == null) return;
            _hasDestination = false;
            _syncedDestination = Vector3.positiveInfinity;
            _actor.Teleport(position, checkNavMesh, fx);
        }

        [ServerRpc]
        public void CmdPlayEmote(int emoteIndex)
        {
            RpcPlayEmote(emoteIndex);
        }

        [ObserversRpc(RunLocally = true)]
        private void RpcPlayEmote(int emoteIndex)
        {
            if (_actor == null) return;

            var emotionSystem = GameSystemManager.Instance?.Emotion;
            if (emotionSystem == null) return;

            emotionSystem.SpawnEmotionOnActor(_actor, emoteIndex);
        }

        private void Update()
        {
            if (_actor == null) _actor = GetComponent<TacticianActor>();
            if (IsOwner || _actor == null || !_hasDestination) return;
            if (_actor.MoveAgent == null || !_actor.MoveAgent.Initialized) return;

            _actor.MovePosition(_syncedDestination, _actor.Speed, _actor.RotateSpeed, Time.deltaTime);

            if (Vector3.Distance(_actor.transform.position, _syncedDestination) < 0.15f)
            {
                _hasDestination = false;
                _syncedDestination = Vector3.positiveInfinity;
                _actor.ForceStop();
            }
        }
    }
}
