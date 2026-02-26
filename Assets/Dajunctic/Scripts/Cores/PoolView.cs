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
            this.RegisterListener<SpawnHpViewEvent>((param) => {SpawnHpView(param);});
        }

        public void SpawnHpView(SpawnHpViewEvent param)
        {
            var position = param.owner.HeadPoint;
            var starLevel = param.starLevel;

            var hpViewPrefab = poolData.hpView;
            var hpView = PoolableObject.Pool.Spawn(hpViewPrefab, position, Quaternion.identity);
            hpView.Initialize(param.owner, starLevel);
        }

        public FxView SpawnFxView(SpawnFxEvent param)
        {
            var position = param.position;
            var fxId = param.id;
            var fxViewPrefab = poolData.fxLists.Find(f => f.Id == fxId).fxViewPrefab;

            var fxView =  PoolableObject.Pool.Spawn(fxViewPrefab, position, Quaternion.identity);
            fxView.Play(param);
            return fxView;
        }

        public FxView SpawnFx(SpawnFxEvent param)
        {
            return SpawnFxView(param);
        }

        public MissileView SpawnMissile(string missileId, MissileData missileData)
        {
            return SpawnMissileView(missileId, missileData);
        }

        public MissileView SpawnMissileView(string missileId, MissileData missileData)
        {
            var entry = poolData.missileLists.Find(m => m.Id == missileId);
            if (entry == null || entry.missilePrefab == null) return null;

            var missileView =  PoolableObject.Pool.Spawn(entry.missilePrefab, missileData.launcher, Quaternion.identity);
            missileView.InitData(missileData);
            missileView.StartFly();
            return missileView;
        }
    }
}