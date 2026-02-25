using UnityEngine;
using Dajunctic.SkillSystem.Graph;

namespace Dajunctic
{
    public class GameManager : Singleton<GameManager>, ISkillServiceProvider
    {
        [SerializeField] GameManagerSO gameManagerSO;
        [SerializeField] public HexAreaView bossArea;
        [SerializeField] public HexAreaView enemyArea;

        void Start()
        {
            
        }

        // ─── ISkillServiceProvider ───────────────────────────────────────────

        public GameObject SpawnFx(string fxId, Vector3 position, Quaternion rotation, float duration = -1f)
        {
            var entry = gameManagerSO.fxLists.Find(f => f.Id == fxId);
            if (entry == null || entry.fxViewPrefab == null)
            {
                Debug.LogWarning($"[GameManager] FX '{fxId}' not found.");
                return null;
            }

            var fxView = Instantiate(entry.fxViewPrefab, position, rotation);
            fxView.Play(new PlayFxEvent { Id = fxId, Position = position, duration = duration });
            return fxView.gameObject;
        }

        public void SpawnMissile(string missileId, MissileData missileData)
        {
            var entry = gameManagerSO.missileLists.Find(m => m.Id == missileId);
            if (entry == null || entry.missilePrefab == null)
            {
                Debug.LogWarning($"[GameManager] Missile '{missileId}' not found.");
                return;
            }

            var missileView = Instantiate(entry.missilePrefab, missileData.launcher, Quaternion.identity);
            missileView.InitData(missileData);
            missileView.StartFly();
        }

        // ─── Legacy direct-call helpers (kept for non-graph code) ────────────

        public void PlayFx(string fxId, PlayFxEvent playFxData)
        {
            SpawnFx(fxId, playFxData.Position, Quaternion.identity, playFxData.duration);
        }

        public MissileView SpawnMissileView(string missileId, MissileData missileData)
        {
            var entry = gameManagerSO.missileLists.Find(m => m.Id == missileId);
            if (entry == null || entry.missilePrefab == null) return null;

            var missileView = Instantiate(entry.missilePrefab, missileData.launcher, Quaternion.identity);
            missileView.InitData(missileData);
            missileView.StartFly();
            return missileView;
        }
    }

    public enum Area
    {
        Boss,
        Enemy,
    }
     
}