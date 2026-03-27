using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace Dajunctic
{
    public class ItemSystem : MonoBehaviour, IGameSystem
    {
        private ItemSystemData _data;

        [Header("Debug")]
        [SerializeField] private ItemData[] debugTestItems;
        public ItemData[] DebugTestItems => debugTestItems;

        private List<ItemData> _itemBench = new List<ItemData>();
        private List<DraggableItem> _spawnedItems = new List<DraggableItem>();
        public List<ItemData> ItemBench => _itemBench;

        private GameSystemManager _manager;

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<ItemSystemData>(GameSystemManager.Instance.Config.itemSystemData);
            _data = await handle.Task;
            Debug.Log("<color=cyan>ItemSystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            Debug.Log("<color=cyan>ItemSystem initialized</color>");
        }

        public void Shutdown()
        {
            _itemBench.Clear();
        }

        public bool AddItemToBench(ItemData item)
        {
            if (_data != null && _itemBench.Count >= _data.maxBenchSlots)
            {
                Debug.LogWarning("Item bench is full!");
                return false;
            }

            _itemBench.Add(item);
            SpawnItemOnBench(item, _itemBench.Count - 1);
            this.Raise(new ItemBenchChangedEvent());
            return true;
        }

        private void SpawnItemOnBench(ItemData item, int slotIndex)
        {
            if (GameplayPopup.Instance == null) return; // Don't spawn if UI is closed

            var benchPositions = GameplayPopup.Instance.ItemBenchPositions;
            if (_data == null || _data.draggableItemPrefab == null)
            {
                Debug.LogWarning("DraggableItemPrefab is not set in ItemSystemData!");
                return;
            }
            if (benchPositions == null || benchPositions.Length == 0)
            {
                Debug.LogWarning("ItemBenchPositions in GameplayPopup is empty!");
                return;
            }
            if (slotIndex >= benchPositions.Length) return;

            Transform parentSlot = benchPositions[slotIndex];

            // Instantiate perfectly inside the UI slot
            DraggableItem instance = Instantiate(_data.draggableItemPrefab, parentSlot);

            // Reset local position and scale for UI elements
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            instance.Initialize(item);
            _spawnedItems.Add(instance);
        }

        public void RefreshAllVisuals()
        {
            ClearAllVisuals();

            for (int i = 0; i < _itemBench.Count; i++)
            {
                SpawnItemOnBench(_itemBench[i], i);
            }
        }

        public void ClearAllVisuals()
        {
            foreach (var instance in _spawnedItems)
            {
                if (instance != null) Destroy(instance.gameObject);
            }
            _spawnedItems.Clear();
        }

        public void RemoveItemFromBench(int index)
        {
            if (index >= 0 && index < _itemBench.Count)
            {
                DraggableItem instance = _spawnedItems[index];
                if (instance != null) Destroy(instance.gameObject);
                _spawnedItems.RemoveAt(index);

                _itemBench.RemoveAt(index);

                // Rearrange remaining items
                RearrangeBench();

                this.Raise(new ItemBenchChangedEvent());
            }
        }

        private void RearrangeBench()
        {
            var benchPositions = GameplayPopup.Instance?.ItemBenchPositions;
            if (benchPositions == null || benchPositions.Length == 0) return;

            for (int i = 0; i < _spawnedItems.Count; i++)
            {
                DraggableItem instance = _spawnedItems[i];
                if (instance != null && i < benchPositions.Length)
                {
                    instance.transform.SetParent(benchPositions[i], false);
                    instance.transform.localPosition = Vector3.zero;
                }
            }
        }

        public bool TryGiveItemToHero(DraggableItem instance, ChampionActor hero)
        {
            if (instance == null || hero == null) return false;
            ItemData item = instance.ItemData;
            if (item == null) return false;

            // Logic for giving/combining items
            // 1. Get the hero's item container
            ItemContainer container = hero.GetComponent<ItemContainer>();
            if (container == null)
            {
                container = hero.gameObject.AddComponent<ItemContainer>();
            }
            // Always ensure it's initialized with the hero reference
            container.Initialize(hero);

            // 2. Try to add or combine
            if (container.TryAddItem(item, _data != null ? _data.recipeDatabase : null))
            {
                // Find and remove from bench by instance to handle duplicates correctly
                int index = _spawnedItems.IndexOf(instance);
                if (index != -1)
                {
                    _itemBench.RemoveAt(index);
                    _spawnedItems.RemoveAt(index);
                }

                // Item successfully consumed
                Destroy(instance.gameObject);

                RearrangeBench();

                this.Raise(new ItemBenchChangedEvent());
                Debug.Log($"Gave {item.itemName} to {hero.name}");
                return true;
            }
            return false;
        }

        #region Debug
        [ContextMenu("Spawn Random Test Item")]
        public void DebugSpawnRandomItem()
        {
            if (debugTestItems == null || debugTestItems.Length == 0)
            {
                Debug.LogWarning("No debug test items assigned in ItemSystem!");
                return;
            }

            int randomIndex = Random.Range(0, debugTestItems.Length);
            AddItemToBench(debugTestItems[randomIndex]);
        }

        private void Update()
        {
            // Kiểm tra xem có đang focus vào UI Input không
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
            {
                var input = EventSystem.current.currentSelectedGameObject.GetComponent<TMPro.TMP_InputField>();
                if (input != null && input.isFocused) return;
                
                var inputLegacy = EventSystem.current.currentSelectedGameObject.GetComponent<UnityEngine.UI.InputField>();
                if (inputLegacy != null && inputLegacy.isFocused) return;
            }

            // Press 'I' to spawn a random item for testing
            if (Input.GetKeyDown(KeyCode.I))
            {
                // Debug.LogError("Spawning random item for testing...");
                DebugSpawnRandomItem();
            }
        }
        #endregion
    }

    public struct ItemBenchChangedEvent : IEvent { }
}
