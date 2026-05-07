using FishNet.Object;
using UnityEngine;

namespace Dajunctic
{
    public class TacticianNetworkMovement : NetworkBehaviour
    {
        private TacticianActor _actor;

        // Synced destination — observer's Update() calls MovePosition every frame (mirrors offline Tick behavior)
        private Vector3 _syncedDestination = Vector3.positiveInfinity;
        private bool _hasDestination = false;

        private void Awake()
        {
            _actor = GetComponent<TacticianActor>();
        }

        // ── Click to move ─────────────────────────────────────────────────────
        [ServerRpc]
        public void CmdMoveTo(Vector3 destination)
        {
            RpcSetDestination(destination);
        }

        /// <summary>
        /// Observer: just store the destination. Update() will call MovePosition every frame.
        /// This mirrors offline behavior where Tick() calls MovePosition continuously.
        /// </summary>
        [ObserversRpc(ExcludeOwner = true)]
        private void RpcSetDestination(Vector3 destination)
        {
            _syncedDestination = destination;
            _hasDestination = true;
        }

        // ── Teleport (đổi sân đấu, v.v.) ─────────────────────────────────────
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

        // ── Emote (biểu cảm) ─────────────────────────────────────────────────

        /// <summary>
        /// Owner calls this to request the server broadcast an emote.
        /// requireOwnership = true (default) ensures only the owner can trigger their own emote.
        /// </summary>
        [ServerRpc]
        public void CmdPlayEmote(int emoteIndex)
        {
            RpcPlayEmote(emoteIndex);
        }

        /// <summary>
        /// Server → all clients (including owner) spawn the EmotionView on this tactician.
        /// RunLocally = true so the owner also sees their own emote immediately.
        /// </summary>
        [ObserversRpc(RunLocally = true)]
        private void RpcPlayEmote(int emoteIndex)
        {
            if (_actor == null) return;

            var emotionSystem = GameSystemManager.Instance?.Emotion;
            if (emotionSystem == null) return;

            emotionSystem.SpawnEmotionOnActor(_actor, emoteIndex);
        }

        // ── Observer update: continuous MovePosition (mirrors offline Tick) ───
        private void Update()
        {
            if (IsOwner || _actor == null || !_hasDestination) return;
            if (_actor.MoveAgent == null || !_actor.MoveAgent.Initialized) return;

            // Call MovePosition every frame like offline Tick() does → smooth NavMesh movement
            _actor.MovePosition(_syncedDestination, _actor.Speed, _actor.RotateSpeed, Time.deltaTime);

            // Clear destination once actor arrives
            if (Vector3.Distance(_actor.transform.position, _syncedDestination) < 0.2f)
            {
                _hasDestination = false;
                _syncedDestination = Vector3.positiveInfinity;
            }
        }
    }
}
