using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class ItemSystem : MonoBehaviour, IGameSystem
    {
        [SerializeField] private ItemRecipeDatabase recipeDatabase;
        [SerializeField] private DraggableItem draggableItemPrefab;
        [SerializeField] private Transform[] benchPositions; // 10 slots on the ground
        [SerializeField] private int maxBenchSlots = 10;
        
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
            if (draggableItemPrefab == null || slotIndex >= benchPositions.Length) return;

            Vector3 pos = benchPositions[slotIndex].position;
            DraggableItem instance = Instantiate(draggableItemPrefab, pos, Quaternion.identity, transform);
            instance.Initialize(item);
            _spawnedItems[item] = instance;
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
            for (int i = 0; i < _itemBench.Count; i++)
            {
                ItemData item = _itemBench[i];
                if (_spawnedItems.TryGetValue(item, out var instance))
                {
                    instance.transform.position = benchPositions[i].position;
                }
            }
        }

        public void TryGiveItemToHero(ItemData item, HeroCombatActor hero)
        {
            if (item == null || hero == null) return;

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
                _spawnedItems.Remove(item); // The DraggableItem destroys itself in OnDrop or we handle it here
                
                RearrangeBench();
                
                this.Raise(new ItemBenchChangedEvent());
                Debug.Log($"Gave {item.itemName} to {hero.name}");
            }
        }
    }

    public struct ItemBenchChangedEvent : IEvent { }
}
