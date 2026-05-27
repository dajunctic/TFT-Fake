using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{

    [CreateAssetMenu(menuName = "Dajunctic/Systems/GameSystemManagerData", fileName = "GameSystemManagerData")]
    public class GameSystemManagerData : ScriptableObject
    {
        [Header("System Data (Addressable References)")]
        public AssetReferenceT<SettingsData> settingsData;
        public AssetReferenceT<ShopSystemData> shopSystemData;
        public AssetReferenceT<ItemSystemData> itemSystemData;
        public AssetReferenceT<BenchSystemData> benchSystemData;
        public AssetReferenceT<EmotionSystemData> emotionSystemData;
        public AssetReferenceT<TraitSystemData> traitSystemData;
        public AssetReferenceT<RoundSystemData> roundSystemData;
        public AssetReferenceT<PlayerSystemData> playerSystemData;
        public AssetReferenceT<TravelSystemData> travelSystemData;
    }
}
