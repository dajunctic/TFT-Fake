using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic
{
    public class PoolView: Singleton<PoolView>, ISkillServiceProvider
    {
        [SerializeField] private PoolData poolData;

        void Start()
        {
            this.RegisterListener<SpawnFxEvent>((param) => {SpawnFxView(param);});
        }

        public FxView SpawnFxView(SpawnFxEvent playFxEvent)
        {
            var position = playFxEvent.position;
            var fxId = playFxEvent.id;
            var fxViewPrefab = poolData.fxLists.Find(f => f.Id == fxId).fxViewPrefab;

            var fxView = Instantiate(fxViewPrefab, position, Quaternion.identity);
            fxView.Play(playFxEvent);
            return fxView;
        }

        public FxView SpawnFx(SpawnFxEvent playFxEvent)
        {
            return SpawnFxView(playFxEvent);
        }

        public MissileView SpawnMissile(string missileId, MissileData missileData)
        {
            return SpawnMissileView(missileId, missileData);
        }

        public MissileView SpawnMissileView(string missileId, MissileData missileData)
        {
            var entry = poolData.missileLists.Find(m => m.Id == missileId);
            if (entry == null || entry.missilePrefab == null) return null;

            var missileView = Instantiate(entry.missilePrefab, missileData.launcher, Quaternion.identity);
            missileView.InitData(missileData);
            missileView.StartFly();
            return missileView;
        }
    }
}