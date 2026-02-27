using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "TacticiansData", menuName = "Dajunctic/TacticiansData")]
    public class TacticianData : CombatActorData
    {
        [Header("Tacticians Info")]
        public string displayName;
        public string animalType;
        public Sprite icon;
        public GameObject prefab;
        
        [Header("Special Attributes")]
        public int tier = 1; 
        public bool isBoss = false;
    }
}
