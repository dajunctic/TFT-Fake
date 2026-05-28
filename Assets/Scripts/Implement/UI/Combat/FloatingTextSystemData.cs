using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "FloatingTextSystemData", menuName = "Dajunctic/Systems/FloatingTextSystemData")]
    public class FloatingTextSystemData : ScriptableObject
    {
        [Header("Prefabs")]
        public FloatingText floatingTextPrefab;

        [Header("Settings")]
        public float randomOffsetRange = 0.4f;
    }
}
