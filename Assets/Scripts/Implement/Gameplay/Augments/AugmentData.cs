using UnityEngine;

namespace Dajunctic
{
    public enum AugmentTier
    {
        Silver,
        Gold,
        Prismatic
    }

    [CreateAssetMenu(menuName = "Dajunctic/Augments/AugmentData", fileName = "AugmentData")]
    public class AugmentData : ScriptableObject
    {
        public string augmentId;
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public AugmentTier tier;

        public int goldGrant;
        public int xpGrant;
        public float healthBonus;
        public string traitIdToBoost;
        public int traitBoostAmount;
    }
}
