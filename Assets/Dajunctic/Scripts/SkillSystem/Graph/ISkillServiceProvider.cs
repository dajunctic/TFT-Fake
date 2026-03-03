using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public interface ISkillServiceProvider
    {
        bool IsDebug { get; }
        FxView SpawnFx(SpawnFxEvent playFxEvent);
        MissileView SpawnMissile(MissileData missileData);
    }
}
