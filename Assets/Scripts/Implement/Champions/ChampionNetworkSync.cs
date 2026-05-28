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

        public override void OnStartServer()
        {
            base.OnStartServer();
            _actor = GetComponent<ChampionActor>();
            if (_actor != null)
            {
                _actor.OnHpChanged += OnHpChangedServer;
            }
        }

        public override void OnStopServer()
        {
            base.OnStopServer();
            if (_actor != null)
            {
                _actor.OnHpChanged -= OnHpChangedServer;
            }
        }

        private void OnHpChangedServer(float ratio)
        {
            RpcUpdateHp(ratio);
        }

        [ObserversRpc(RunLocally = false)]
        public void RpcUpdateHp(float hpRatio)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            if (_actor != null)
            {
                _actor.ForceSetHp(hpRatio * _actor.MaxHp);
            }
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        public void RpcInitialize(int ownerId, Vector2Int coord, string heroId, int starLevel)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            
            _actor.OwnerID = ownerId;
            _actor.CurrentBenchCoord = coord;

            var data = GameSystemManager.Instance?.Shop?.ShopSystemData?
                       .allHeroes.FirstOrDefault(h => h.Id == heroId);
                       
            if (data != null)
            {
                _actor.SetCombatData(data);
            }
            else
            {
                Debug.LogWarning($"[ChampionNetworkSync] RpcInitialize: heroId='{heroId}' not found in ShopSystemData — keeping prefab combatActorData.");
            }

            _actor.Initialize();
            _actor.SetStarLevel(starLevel);
            GameSystemManager.Instance?.Bench?.RegisterHeroToTile(_actor, coord, ownerId);

            Vector3 worldPos = GameSystemManager.Instance.Bench.GetWorldPosition(ownerId, coord);
            _actor.Teleport(worldPos, false);
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        public void RpcSetStarLevel(int level)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            _actor.SetStarLevel(level);
        }

        [ObserversRpc(RunLocally = true)]
        public void RpcPlayAnimation(string animName, float transitionDuration)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            if (_actor != null)
            {
                _actor.ResetAnim();
                _actor.PlayAnim(animName, transitionDuration);
            }
        }

        [ObserversRpc(RunLocally = true)]
        public void RpcPlayTimeline(string timelineAssetName)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            if (_actor != null)
            {
                var director = _actor.GetComponent<UnityEngine.Playables.PlayableDirector>();
                if (director != null)
                {
                    if (!string.IsNullOrEmpty(timelineAssetName))
                    {
                        var asset = Resources.Load<UnityEngine.Playables.PlayableAsset>($"Timelines/{timelineAssetName}");
                        if (asset != null)
                        {
                            director.playableAsset = asset;
                        }
                    }
                    director.Play();
                }
            }
        }

        [ObserversRpc(RunLocally = true, BufferLast = true)]
        public void RpcUpdateCoordinates(Vector2Int benchCoord, Vector2Int fieldCoord, Vector3 position)
        {
            if (_actor == null) _actor = GetComponent<ChampionActor>();
            if (_actor == null) return;

            if (GameSystemManager.Instance != null)
            {
                if (benchCoord.x != -1)
                {
                    GameSystemManager.Instance.Bench?.RegisterHeroToTile(_actor, benchCoord, _actor.OwnerID);
                }
                else if (fieldCoord.x != -1)
                {
                    GameSystemManager.Instance.Field?.RegisterHeroToTile(_actor, fieldCoord, _actor.OwnerID);
                }
            }

            _actor.Teleport(position, false);
            if (_actor.MoveAgent != null)
            {
                _actor.MoveAgent.SetEnable(true);
                _actor.MoveAgent.Warp(position);
            }
        }
    }
}
