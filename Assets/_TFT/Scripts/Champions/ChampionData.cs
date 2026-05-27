using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "ChampionData", menuName = "Dajunctic/Champions/ChampionData")]
    public class ChampionData : CombatActorData
    {
        [Header("Display Info")]
        public string displayName;
        public Sprite shopIcon;

        [Header("Meta Data")]
        public int rarity = 1; 

        [Header("Traits")]
        public List<TraitData> traits = new();

        public List<ITrait> GetTraits()
        {
            return traits.Cast<ITrait>().ToList();
        }
    }
}
