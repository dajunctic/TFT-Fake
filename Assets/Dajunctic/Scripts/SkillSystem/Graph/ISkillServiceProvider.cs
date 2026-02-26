using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public interface ISkillServiceProvider
    {
        FxView SpawnFx(SpawnFxEvent playFxEvent);
        MissileView SpawnMissile(string missileId, MissileData missileData);
    }
}
