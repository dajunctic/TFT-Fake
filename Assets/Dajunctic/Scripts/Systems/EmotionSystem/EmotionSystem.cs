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
            if (_cooldownTimer > 0f) return;

            var combatActor = FindFirstObjectByType<MythicalAnimalCombatActor>();
            if (combatActor == null) return;

            var emotionView = Instantiate(_data.emotionViewPrefab, combatActor.CachedTransform);
            emotionView.CachedTransform.position = combatActor.HeadPoint + Vector3.up * 0.3f;
            emotionView.PlayEmotion(_data.emotionSprites.GetIndex(emoteIndex));

            _cooldownTimer = _data.cooldownDuration;
        }
    }
}
