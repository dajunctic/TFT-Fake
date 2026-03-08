using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/RoundSystemData", fileName = "RoundSystemData")]
    public class RoundSystemData : ScriptableObject
    {
        public List<StageData> stages = new List<StageData>();
        public bool loopLastStage = true;
        public float defaultPlanningTime = 30f;
        public float defaultCombatTime = 30f;
    }
}
