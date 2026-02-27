using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "TraitSystemData", menuName = "Dajunctic/Systems/TraitSystemData")]
    public class TraitSystemData : ScriptableObject
    {
        [Header("All Available Traits")]
        public List<TraitData> allTraits = new List<TraitData>();
    }
}
