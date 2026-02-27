using UnityEngine;

namespace Dajunctic
{
    public enum ItemType
    {
        Component,
        Full,
        Emblem,
        Consumable
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "Dajunctic/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        [TextArea] public string description;
        public Sprite icon;
        public ItemType type;

        [Header("Stats Boost")]
        public float bonusHp;
        public float bonusAtk;
        public float bonusAtkSpd;
        public float bonusArmor;
        public float bonusMagicResist;
        public float bonusAbilityPower;
        public float bonusMana;
        public float bonusCritChance;
        public float bonusCritDamage;

        // For components, we might want to know if it can be combined
        public bool isCombinable => type == ItemType.Component;
    }
}
