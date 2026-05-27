using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/ItemSystemData", fileName = "ItemSystemData")]
    public class ItemSystemData : ScriptableObject
    {
        public ItemRecipeDatabase recipeDatabase;
        public DraggableItem draggableItemPrefab;
        public int maxBenchSlots = 10;
    }
}
