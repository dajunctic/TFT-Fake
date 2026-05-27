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

        public void ShowEmotion(int emoteIndex)
        {
            if (_data == null) return;
            if (_cooldownTimer > 0f)
            {
                Debug.LogWarning("[EmotionSystem] Emote on cooldown.");
                return;
            }

            TacticianActor localTactician = GetLocalTactician();
            if (localTactician == null)
            {
                Debug.LogWarning("[EmotionSystem] Cannot find local player's TacticianActor.");
                return;
            }

            _cooldownTimer = _data.cooldownDuration;

            var netMovement = localTactician.GetComponent<TacticianNetworkMovement>();
            if (netMovement != null)
            {
                netMovement.CmdPlayEmote(emoteIndex);
            }
            else
            {
                
                SpawnEmotionOnActor(localTactician, emoteIndex);
            }
        }

        public void SpawnEmotionOnActor(TacticianActor actor, int emoteIndex)
        {
            if (_data == null || actor == null) return;

            var sprite = _data.emotionSprites.GetIndex(emoteIndex);
            if (sprite == null) return;

            var emotionView = Instantiate(_data.emotionViewPrefab, actor.CachedTransform);
            emotionView.CachedTransform.position = actor.HeadPoint + Vector3.up * 0.3f;
            emotionView.PlayEmotion(sprite);
        }

        private TacticianActor GetLocalTactician()
        {
            
            var playerSystem = GameSystemManager.Instance?.Player;
            if (playerSystem != null)
            {
                var localPlayer = playerSystem.LocalPlayer;
                if (localPlayer?.Tactician != null)
                    return localPlayer.Tactician;
            }

            return FindFirstObjectByType<TacticianActor>();
        }
    }
}
