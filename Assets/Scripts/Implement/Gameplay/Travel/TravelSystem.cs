using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public class TravelSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private List<CombatPair> _combatPairs = new List<CombatPair>();
        private Dictionary<CombatActor, (int originalArenaId, Vector3 originalPos, Vector2Int originalCoord, bool isChampion)> _travelingUnits = new Dictionary<CombatActor, (int, Vector3, Vector2Int, bool)>();
        private TravelSystemData _data;
        private string PortalFxGuid => _data != null ? _data.portalFxGuid : string.Empty;

        public async System.Threading.Tasks.Task LoadDataAsync()
        {
            if (GameSystemManager.Instance.Config != null && GameSystemManager.Instance.Config.travelSystemData != null)
            {
                var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TravelSystemData>(GameSystemManager.Instance.Config.travelSystemData);
                _data = await handle.Task;
            }
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            this.RegisterListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }

        public void Shutdown()
        {
            this.RemoveListener<GameplayPhaseChangedEvent>(OnPhaseChanged);
        }

        private void OnPhaseChanged(GameplayPhaseChangedEvent evt)
        {
            if (evt.Phase == GameplayPhase.Combat)
            {
                GenerateMatchmaking();
                ExecuteTravelCards();
            }
            else if (evt.Phase == GameplayPhase.Planning)
            {
                ReturnAllUnits();
            }
        }

        public void GenerateMatchmaking()
        {
            _combatPairs.Clear();
            var activePlayers = _manager.Player.Players.Where(p => p.HP > 0).ToList();
            if (activePlayers.Count < 2) return;

            var shuffled = activePlayers.OrderBy(x => Random.value).ToList();

            for (int i = 0; i < shuffled.Count; i += 2)
            {
                if (i + 1 < shuffled.Count)
                {
                    _combatPairs.Add(new CombatPair { HomeId = shuffled[i].Id, GuestId = shuffled[i+1].Id });
                }
                else
                {
                    
                    int randomEnemyId = shuffled[Random.Range(0, shuffled.Count - 1)].Id;
                    _combatPairs.Add(new CombatPair { HomeId = shuffled[i].Id, GuestId = randomEnemyId, IsGhost = true });
                }
            }
        }

        public void ExecuteTravelCards()
        {
            _travelingUnits.Clear();
            foreach (var pair in _combatPairs)
            {
                Arena homeArena = _manager.Field.GetArena(pair.HomeId);
                Arena guestArena = _manager.Field.GetArena(pair.GuestId);
                if (homeArena == null) continue;

                if (guestArena != null) SpawnPortal(guestArena.transform.position);
                Vector3 guestSpawnPos = homeArena.GuestSpawnPoint != null ? homeArena.GuestSpawnPoint.position : homeArena.transform.position;
                SpawnPortal(guestSpawnPos);

                var guestUnits = _manager.Field.GetAllHeroes().Where(u => u.OwnerID == pair.GuestId).ToList();
                
                foreach (var unit in guestUnits)
                {
                    
                    _travelingUnits[unit] = (pair.GuestId, unit.CachedTransform.position, unit.CurrentFieldCoord, true);
                    
                    int idx = guestUnits.IndexOf(unit);

                    Vector3 spawnPos;
                    if (homeArena.GuestFieldArea != null)
                    {
                        
                        spawnPos = homeArena.GetGuestFieldWorldPosition(unit.CurrentFieldCoord);
                    }
                    else
                    {
                        
                        spawnPos = guestSpawnPos + Vector3.right * (idx % 4 * 1.2f) + Vector3.forward * (idx / 4 * 1.2f);
                    }

                    unit.Teleport(spawnPos, true);

                    _manager.Field.RegisterGuestHeroToTile(unit, unit.CurrentFieldCoord, pair.HomeId);
                }

                var guestData = _manager.Player.Players.FirstOrDefault(p => p.Id == pair.GuestId);
                if (guestData != null && guestData.Tactician != null)
                {
                    var tactician = guestData.Tactician;
                    _travelingUnits[tactician] = (pair.GuestId, tactician.CachedTransform.position, Vector2Int.zero, false);
                    
                    tactician.Teleport(guestSpawnPos, true);
                }
            }
        }

        public void ReturnAllUnits()
        {
            foreach (var kvp in _travelingUnits)
            {
                CombatActor unit = kvp.Key;
                if (unit == null) continue;

                var (originalArenaId, originalPos, originalCoord, isChampion) = kvp.Value;
                Arena originalArena = _manager.Field.GetArena(originalArenaId);
                
                if (originalArena != null)
                {
                    if (isChampion && unit is ChampionActor champion)
                    {
                        Vector3 homePos = originalArena.GetFieldWorldPosition(originalCoord);
                        champion.Teleport(homePos, true);
                        _manager.Field.RegisterHeroToTile(champion, originalCoord, originalArenaId);
                    }
                    else
                    {
                        unit.Teleport(originalPos, true);
                    }
                }
            }
            _travelingUnits.Clear();
        }

        private void SpawnPortal(Vector3 pos)
        {
            if (string.IsNullOrEmpty(PortalFxGuid)) return;
            this.Raise(new SpawnFxEvent { id = PortalFxGuid, position = pos, duration = 2f });
        }

        public List<CombatPair> GetCombatPairs() => _combatPairs;

        [System.Serializable]
        public class CombatPair
        {
            public int HomeId;
            public int GuestId;
            public bool IsGhost;

            public CombatPair() { }
            public CombatPair(int home, int guest, bool ghost = false)
            {
                HomeId = home;
                GuestId = guest;
                IsGhost = ghost;
            }
        }
    }
}
