using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class EmotionSystem : MonoBehaviour, IGameSystem
    {
        private EmotionSystemData _data;
        private float _cooldownTimer;

        public EmotionSystemData Data => _data;
        public bool IsOnCooldown => _cooldownTimer > 0f;

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<EmotionSystemData>(
                GameSystemManager.Instance.Config.emotionSystemData);
            _data = await handle.Task;
            Debug.Log("<color=cyan>EmotionSystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            Debug.Log("<color=cyan>EmotionSystem initialized</color>");
        }

        public void Shutdown()
        {
            Debug.Log("<color=yellow>EmotionSystem shutdown</color>");
        }

        private void Update()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
        }

        /// <summary>
        /// Called by local client when player picks an emote.
        /// Finds the LOCAL player's tactician, applies cooldown, then sends ServerRpc so all clients see it.
        /// </summary>
        public void ShowEmotion(int emoteIndex)
        {
            if (_data == null) return;
            if (_cooldownTimer > 0f)
            {
                Debug.LogWarning("[EmotionSystem] Emote on cooldown.");
                return;
            }

            // Find local player's tactician (correct for multiplayer)
            TacticianActor localTactician = GetLocalTactician();
            if (localTactician == null)
            {
                Debug.LogWarning("[EmotionSystem] Cannot find local player's TacticianActor.");
                return;
            }

            // Apply cooldown locally to prevent spam
            _cooldownTimer = _data.cooldownDuration;

            // Route through NetworkBehaviour: ServerRpc → ObserversRpc → all clients spawn
            var netMovement = localTactician.GetComponent<TacticianNetworkMovement>();
            if (netMovement != null)
            {
                netMovement.CmdPlayEmote(emoteIndex);
            }
            else
            {
                // Offline fallback: spawn locally only
                SpawnEmotionOnActor(localTactician, emoteIndex);
            }
        }

        /// <summary>
        /// Spawns the EmotionView prefab on top of the given actor.
        /// Called by TacticianNetworkMovement.RpcPlayEmote on ALL clients (including host).
        /// </summary>
        public void SpawnEmotionOnActor(TacticianActor actor, int emoteIndex)
        {
            if (_data == null || actor == null) return;

            var sprite = _data.emotionSprites.GetIndex(emoteIndex);
            if (sprite == null) return;

            var emotionView = Instantiate(_data.emotionViewPrefab, actor.CachedTransform);
            emotionView.CachedTransform.position = actor.HeadPoint + Vector3.up * 0.3f;
            emotionView.PlayEmotion(sprite);
        }

        /// <summary>
        /// Returns the TacticianActor belonging to the local player.
        /// Uses PlayerSystem in multiplayer, falls back to FindFirstObjectByType in offline mode.
        /// </summary>
        private TacticianActor GetLocalTactician()
        {
            // Multiplayer: use PlayerSystem to find local player
            var playerSystem = GameSystemManager.Instance?.Player;
            if (playerSystem != null)
            {
                var localPlayer = playerSystem.LocalPlayer;
                if (localPlayer?.Tactician != null)
                    return localPlayer.Tactician;
            }

            // Offline fallback
            return FindFirstObjectByType<TacticianActor>();
        }
    }
}
