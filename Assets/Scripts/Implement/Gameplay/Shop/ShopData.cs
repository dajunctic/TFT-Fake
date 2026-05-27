using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "ShopData", menuName = "Dajunctic/ShopData")]
    public class ShopData : ScriptableObject
    {
        [Header("Costs")]
        public int rerollCost = 2;
        public int buyXpCost = 4;
        public int xpPerBuy = 4;

        [Header("Roll Probabilities")]
        public List<LevelProbability> probabilities = new List<LevelProbability>();

        [Serializable]
        public class LevelProbability
        {
            public int level;
            public float[] rarityChances = new float[5]; 
        }

        public float[] GetChancesForLevel(int level)
        {
            var prob = probabilities.Find(p => p.level == level);
            if (prob != null) return prob.rarityChances;
            return new float[] { 1, 0, 0, 0, 0 }; 
        }
    }
}
