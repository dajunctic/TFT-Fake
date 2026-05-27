using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/PlayerSystemData", fileName = "PlayerSystemData")]
    public class PlayerSystemData : ScriptableObject
    {
        public TacticianData defaultTacticianData;
        public TacticianData[] availableTacticians;
        public GameObject playerDataSyncPrefab;
    }
}
