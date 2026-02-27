using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "TacticiansData", menuName = "Dajunctic/TacticiansData")]
    public class MythicalAnimalActorData : CombatActorData
    {
        [Header("Tacticians Info")]
        public string displayName;
        public string animalType; // e.g., "Dragon", "Phoenix", "Unicorn"
        public Sprite icon;
        public GameObject prefab;
        
        [Header("Special Attributes")]
        public int tier = 1; // Power tier (1-3)
        public bool isBoss = false;
    }
}
