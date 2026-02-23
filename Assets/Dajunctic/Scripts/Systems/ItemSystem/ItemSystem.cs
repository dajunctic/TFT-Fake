using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class ItemSystem : MonoBehaviour, IGameSystem
    {
        [SerializeField] private ItemRecipeDatabase recipeDatabase;
        [SerializeField] private DraggableItem draggableItemPrefab;
        [SerializeField] private int maxBenchSlots = 10;
        
        [Header("Debug")]
        [SerializeField] private ItemData[] debugTestItems;
        
        private List<ItemData> _itemBench = new List<ItemData>();
        private Dictionary<ItemData, DraggableItem> _spawnedItems = new Dictionary<ItemData, DraggableItem>();
        public List<ItemData> ItemBench => _itemBench;

        private GameSystemManager _manager;

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
            if (_itemBench.Count >= maxBenchSlots)
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
            if (draggableItemPrefab == null)
            {
                Debug.LogWarning("DraggableItemPrefab is not assigned in ItemSystem!");
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
            DraggableItem instance = Instantiate(draggableItemPrefab, parentSlot);
            
            // Reset local position and scale for UI elements
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            instance.Initialize(item);
            _spawnedItems[item] = instance;
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
            foreach (var kvp in _spawnedItems)
            {
                if (kvp.Value != null) Destroy(kvp.Value.gameObject);
            }
            _spawnedItems.Clear();
        }

        public void RemoveItemFromBench(int index)
        {
            if (index >= 0 && index < _itemBench.Count)
            {
                ItemData item = _itemBench[index];
                if (_spawnedItems.TryGetValue(item, out var instance))
                {
                    if (instance != null) Destroy(instance.gameObject);
                    _spawnedItems.Remove(item);
                }

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

            for (int i = 0; i < _itemBench.Count; i++)
            {
                ItemData item = _itemBench[i];
                if (_spawnedItems.TryGetValue(item, out var instance))
                {
                    if (i < benchPositions.Length)
                    {
                        instance.transform.SetParent(benchPositions[i], false);
                        instance.transform.localPosition = Vector3.zero;
                    }
                }
            }
        }

        public bool TryGiveItemToHero(ItemData item, HeroCombatActor hero)
        {
            if (item == null || hero == null) return false;

            // Logic for giving/combining items
            // 1. Get the hero's item container
            ItemContainer container = hero.GetComponent<ItemContainer>();
            if (container == null)
            {
                container = hero.gameObject.AddComponent<ItemContainer>();
                container.Initialize(hero);
            }

            // 2. Try to add or combine
            if (container.TryAddItem(item, recipeDatabase))
            {
                // Item successfully consumed
                _itemBench.Remove(item);
                if (_spawnedItems.TryGetValue(item, out var instance))
                {
                    if (instance != null) Destroy(instance.gameObject);
                    _spawnedItems.Remove(item);
                }
                
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
            // Press 'I' to spawn a random item for testing
            if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.LogError("Spawning random item for testing...");
                DebugSpawnRandomItem();
            }
        }
        #endregion
    }

    public struct ItemBenchChangedEvent : IEvent { }
}
