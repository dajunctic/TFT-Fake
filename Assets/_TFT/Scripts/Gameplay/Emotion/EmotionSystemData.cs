using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "EmotionSystemData", menuName = "Dajunctic/Systems/EmotionSystemData")]
    public class EmotionSystemData : ScriptableObject
    {
        [Header("Emotion Settings")]
        public float cooldownDuration = 2f;

        [Header("Prefabs")]
        public EmotionView emotionViewPrefab;

        [Header("Sprites")]
        public SpriteLists emotionSprites;
    }
}
