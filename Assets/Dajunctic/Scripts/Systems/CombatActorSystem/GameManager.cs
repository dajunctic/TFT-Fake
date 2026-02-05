using UnityEngine;

namespace Dajunctic
{
    public class GameManager: Singleton<GameManager>
    {
        [SerializeField] GameManagerSO gameManagerSO;
        [SerializeField] public HexAreaView bossArea;
        [SerializeField] public HexAreaView enemyArea;

        void Start()
        {
            
        }

        public void PlayFx(string fxId, PlayFxData playFxData)
        {
            var position = playFxData.Position;
            var fxViewPrefab = gameManagerSO.fxLists.Find(f => f.Id == fxId).fxViewPrefab;

            var fxView = Instantiate(fxViewPrefab, position, Quaternion.identity);
            fxView.Play(playFxData);
        }

        public MissileView SpawnMissile(string missileId, MissileData missileData)
        {
            var missilePrefab = gameManagerSO.missileLists.Find(m => m.Id == missileId).missilePrefab;
            var missileView = Instantiate(missilePrefab, missileData.launcher, Quaternion.identity);
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