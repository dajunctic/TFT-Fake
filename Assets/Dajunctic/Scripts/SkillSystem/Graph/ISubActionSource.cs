using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public interface IActionSource { }

    public class HitData
    {
        public List<IDamageTaker> targets;
        public List<Vector3> hitPoints;
    }

    public interface IHitDataProvider : IActionSource
    {
        HitData GetHitData();
    }

    public class FxData
    {
        public List<IDamageTaker> targets;
        public List<Vector3> spawnPositions;
    }

    public interface IFxDataProvider : IActionSource
    {
        FxData GetFxData();
    }
}
