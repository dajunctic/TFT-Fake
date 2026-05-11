using FishNet.Object;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    [RequireComponent(typeof(ChampionActor))]
    public class ChampionNetworkSync : NetworkBehaviour
    {
        private ChampionActor _actor;

        public override void OnStartClient()
        {
            base.OnStartClient();
            _actor = GetComponent<ChampionActor>();
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        public void RpcInitialize(int ownerId, Vector2Int coord, string heroId, int starLevel)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            
            _actor.OwnerID = ownerId;
            _actor.CurrentBenchCoord = coord;

            // Find data
            var data = GameSystemManager.Instance?.Shop?.ShopSystemData?
                       .allHeroes.FirstOrDefault(h => h.Id == heroId);
                       
            if (data != null)
            {
                _actor.SetCombatData(data);
            }

            _actor.SetStarLevel(starLevel);
            _actor.Initialize();
            GameSystemManager.Instance?.Bench?.RegisterHeroToTile(_actor, coord, ownerId);
            
            // Re-warp to correctly snap to bench if initializing late
            Vector3 worldPos = GameSystemManager.Instance.Bench.GetWorldPosition(ownerId, coord);
            _actor.Teleport(worldPos, false);
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        public void RpcSetStarLevel(int level)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            _actor.SetStarLevel(level);
        }
    }
}
