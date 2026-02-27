using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/ShopSystemData", fileName = "ShopSystemData")]
    public class ShopSystemData : ScriptableObject
    {
        public ShopData shopData;
        public List<HeroData> allHeroes = new List<HeroData>();
    }
}
