
using UnityEngine;

namespace Dajunctic
{
    public class PoolView : Singleton<PoolView>
    {
        [SerializeField] private PoolData poolData;
        public bool IsDebug => false;

        void OnEnable()
        {
            this.RegisterListener<SpawnFxEvent>(OnSpawnFx);
            this.RegisterListener<SpawnHpViewEvent>(OnSpawnHpView);
        }

        void OnDisable()
        {
            this.RemoveListener<SpawnFxEvent>(OnSpawnFx);
            this.RemoveListener<SpawnHpViewEvent>(OnSpawnHpView);
        }

        private void OnSpawnHpView(SpawnHpViewEvent param)
        {
            SpawnHpView(param);
        }

        public ArenaChampionCountUI SpawnArenaChampionCountUI(Vector3 position)
        {
            if (poolData.arenaChampionCountUI == null) return null;
            return PoolableObject.Pool.Spawn(poolData.arenaChampionCountUI, position, Quaternion.identity);
        }

        private void OnSpawnFx(SpawnFxEvent param)
        {
            SpawnFxView(param);
        }

        public void SpawnHpView(SpawnHpViewEvent param)
        {
            var position = param.owner.HeadPoint;
            var starLevel = param.starLevel;

            if (param.owner is TacticianActor)
            {
                if (poolData.playerHpView == null) return;
                var instance = PoolableObject.Pool.Spawn(poolData.playerHpView, position, Quaternion.identity);
                instance.CachedTransform.SetParent(param.owner.CachedTransform, false);
                instance.Initialize(param.owner, starLevel);
            }
            else
            {
                if (poolData.hpView == null) return;
                var instance = PoolableObject.Pool.Spawn(poolData.hpView, position, Quaternion.identity);
                instance.CachedTransform.SetParent(param.owner.CachedTransform, false);
                instance.Initialize(param.owner, starLevel);
            }
        }

        public FxView SpawnFxView(SpawnFxEvent param)
        {
            var position = param.position;
            var fxId = param.id;
            var fxViewPrefab = poolData.fxLists.Find(f => f.Id == fxId).fxViewPrefab;

            var fxView = PoolableObject.Pool.Spawn(fxViewPrefab, position, Quaternion.identity);
            fxView.Play(param);
            return fxView;
        }

        public FxView SpawnFx(SpawnFxEvent param)
        {
            return SpawnFxView(param);
        }

        public MissileView SpawnMissile(MissileData missileData)
        {
            return SpawnMissileView(missileData);
        }

        public MissileView SpawnMissileView(MissileData missileData)
        {
            var entry = poolData.missileLists.Find(m => m.Id == missileData.id);
            if (entry == null || entry.missilePrefab == null) return null;

            var missileView = PoolableObject.Pool.Spawn(entry.missilePrefab, missileData.launcher, Quaternion.identity);
            missileView.InitData(missileData);
            missileView.StartFly();
            return missileView;
        }
    }
}