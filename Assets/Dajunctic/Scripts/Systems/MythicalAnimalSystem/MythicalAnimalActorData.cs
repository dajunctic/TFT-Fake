using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "MythicalAnimalData", menuName = "Dajunctic/MythicalAnimalData")]
    public class MythicalAnimalActorData : CombatActorData
    {
        [Header("Mythical Animal Info")]
        public string displayName;
        public string animalType; // e.g., "Dragon", "Phoenix", "Unicorn"
        public Sprite icon;
        public GameObject prefab;
        
        [Header("Special Attributes")]
        public int tier = 1; // Power tier (1-3)
        public bool isBoss = false;
    }
}
