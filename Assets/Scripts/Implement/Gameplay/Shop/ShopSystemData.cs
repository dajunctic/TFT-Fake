using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/ShopSystemData", fileName = "ShopSystemData")]
    public class ShopSystemData : ScriptableObject
    {
        public ShopData shopData;
        public List<ChampionData> allHeroes = new List<ChampionData>();
    }
}
