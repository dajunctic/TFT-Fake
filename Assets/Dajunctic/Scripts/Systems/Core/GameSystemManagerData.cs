using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    /// <summary>
    /// Central ScriptableObject that holds Addressable references to all system data assets.
    /// Assign this once in the GameSystemManager inspector — no other drag-drop needed.
    /// </summary>
    [CreateAssetMenu(menuName = "Dajunctic/Systems/GameSystemManagerData", fileName = "GameSystemManagerData")]
    public class GameSystemManagerData : ScriptableObject
    {
        [Header("System Data (Addressable References)")]
        public AssetReferenceT<SettingsData> settingsData;
        public AssetReferenceT<ShopSystemData> shopSystemData;
        public AssetReferenceT<EconomySystemData> economySystemData;
        public AssetReferenceT<ItemSystemData> itemSystemData;
        public AssetReferenceT<BenchSystemData> benchSystemData;
    }
}
