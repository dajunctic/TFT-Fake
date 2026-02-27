using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "SpriteLists", menuName = "Dajunctic/UI/SpriteLists")]
    public class SpriteLists: BaseSO
    {
        [SerializeField] private Sprite[] sprites;

        public Sprite[] Sprites => sprites;

        public Sprite GetIndex(int index)
        {
            if (index < 0 || index >= sprites.Length)
            {
                Debug.LogError($"Index {index} is out of bounds for sprites array of length {sprites.Length}.");
                return null;
            }
            return sprites[index];
        }
    }
}