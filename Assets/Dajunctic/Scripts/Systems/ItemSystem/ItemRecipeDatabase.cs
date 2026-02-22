using UnityEngine;
using System.Collections.Generic;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "ItemRecipeDatabase", menuName = "Dajunctic/Items/RecipeDatabase")]
    public class ItemRecipeDatabase : ScriptableObject
    {
        [System.Serializable]
        public struct Recipe
        {
            public ItemData componentA;
            public ItemData componentB;
            public ItemData result;
        }

        public List<Recipe> recipes = new List<Recipe>();

        public ItemData GetCombinedItem(ItemData a, ItemData b)
        {
            foreach (var recipe in recipes)
            {
                if ((recipe.componentA == a && recipe.componentB == b) ||
                    (recipe.componentA == b && recipe.componentB == a))
                {
                    return recipe.result;
                }
            }
            return null;
        }
    }
}
