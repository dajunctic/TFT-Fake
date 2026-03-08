using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Round/RoundData", fileName = "RoundData")]
    public class RoundData : ScriptableObject
    {
        public RoundType roundType;
        public float planningDuration = 30f;
        public float combatDuration = 30f;
        public string displayName;
        public Sprite icon;
        public int bonusGold;
        public bool hasAugment;
    }
}
