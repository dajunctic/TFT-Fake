using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class FloatingTextSystem : MonoBehaviour, IGameSystem
    {
        public static FloatingTextSystem Instance { get; private set; }

        private FloatingTextSystemData _data;
        public FloatingTextSystemData Data => _data;

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<FloatingTextSystemData>(GameSystemManager.Instance.Config.floatingTextSystemData);
            _data = await handle.Task;
        }

        public void Initialize(GameSystemManager manager)
        {
            Instance = this;
 
            // Subscribe to global damage event
            if (EventDispatcherView.Instance != null)
            {
                EventDispatcherView.Instance.RegisterListener<DamageTakenGlobalEvent>(OnDamageTaken);
                EventDispatcherView.Instance.RegisterListener<HealGlobalEvent>(OnHeal);
            }
        }
 
        public void Shutdown()
        {
            if (EventDispatcherView.Instance != null)
            {
                EventDispatcherView.Instance.RemoveListener<DamageTakenGlobalEvent>(OnDamageTaken);
                EventDispatcherView.Instance.RemoveListener<HealGlobalEvent>(OnHeal);
            }
            if (Instance == this)
            {
                Instance = null;
            }
        }
 
        private void OnDamageTaken(DamageTakenGlobalEvent evt)
        {
            if (_data == null || _data.floatingTextPrefab == null)
            {
                Debug.LogWarning("[FloatingTextSystem] Floating Text Prefab is not loaded or assigned in ScriptableObject data!");
                return;
            }
 
            if (evt == null || evt.Target == null || evt.Damage == null) return;
 
            // Calculate spawn position
            Vector3 spawnPos = evt.Target.HeadPoint; // HeadPoint is already above the actor
            
            // Add a small randomized offset so multiple numbers do not pile directly on top of each other
            float randomX = Random.Range(-_data.randomOffsetRange, _data.randomOffsetRange);
            float randomZ = Random.Range(-_data.randomOffsetRange, _data.randomOffsetRange);
            float randomY = Random.Range(-0.1f, 0.2f);
            spawnPos += new Vector3(randomX, randomY, randomZ);
 
            // Spawn the Floating Text via pool
            Camera mainCam = Camera.main;
            Quaternion rot = mainCam != null ? mainCam.transform.rotation : Quaternion.identity;
            
            FloatingText floatingTextInstance = PoolableObject.Pool.Spawn(_data.floatingTextPrefab, spawnPos, rot);
            
            if (floatingTextInstance != null)
            {
                floatingTextInstance.Setup(evt.FinalDamage, evt.Damage.DamageType, evt.Damage.IsCritical);
            }
        }

        private void OnHeal(HealGlobalEvent evt)
        {
            if (_data == null || _data.floatingTextPrefab == null) return;
            if (evt == null || evt.Target == null) return;

            // Calculate spawn position
            Vector3 spawnPos = evt.Target.HeadPoint;
            
            // Add a small randomized offset
            float randomX = Random.Range(-_data.randomOffsetRange, _data.randomOffsetRange);
            float randomZ = Random.Range(-_data.randomOffsetRange, _data.randomOffsetRange);
            float randomY = Random.Range(-0.1f, 0.2f);
            spawnPos += new Vector3(randomX, randomY, randomZ);

            Camera mainCam = Camera.main;
            Quaternion rot = mainCam != null ? mainCam.transform.rotation : Quaternion.identity;

            FloatingText floatingTextInstance = PoolableObject.Pool.Spawn(_data.floatingTextPrefab, spawnPos, rot);

            if (floatingTextInstance != null)
            {
                floatingTextInstance.SetupHeal(evt.FinalHeal);
            }
        }
    }
}
