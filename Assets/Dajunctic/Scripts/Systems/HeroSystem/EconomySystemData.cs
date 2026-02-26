using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/EconomySystemData", fileName = "EconomySystemData")]
    public class EconomySystemData : ScriptableObject
    {
        public int initialGold = 10;
        public int initialLevel = 1;
    }
}
