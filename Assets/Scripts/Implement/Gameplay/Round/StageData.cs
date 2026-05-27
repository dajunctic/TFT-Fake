using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Round/StageData", fileName = "StageData")]
    public class StageData : ScriptableObject
    {
        public int stageNumber;
        public List<RoundData> rounds = new List<RoundData>();
    }
}
