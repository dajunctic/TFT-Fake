using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;

namespace Dajunctic
{
    public class CarouselSystem : MonoBehaviour, IGameSystem
    {
        private GameSystemManager _manager;
        private CarouselArena _arena;
        private List<CarouselUnit> _unitsInCarousel = new List<CarouselUnit>();
        private List<CarouselBarrier> _barriers = new List<CarouselBarrier>();
        private List<PlayerData> _sortedPlayers = new List<PlayerData>();
        
        private int _releaseGroupIndex = 0;
        private float _releaseTimer = 0f;
        [SerializeField] private float releaseInterval = 4f;

        private bool _isActive = false;

        public async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        public void Initialize(GameSystemManager manager)
        {
            _manager = manager;
            CarouselUnit.OnUnitPicked += HandleUnitPicked;
        }

        public void StartCarousel()
        {
            Debug.Log("<color=yellow>CarouselSystem: Starting Carousel!</color>");
            
            _arena = FindFirstObjectByType<CarouselArena>();
            if (_arena == null)
            {
                Debug.LogError("CarouselSystem: No CarouselArena found in scene!");
                return;
            }

            _arena.Initialize();
            _isActive = true;
            _releaseGroupIndex = 0;
            _releaseTimer = releaseInterval;

            SetupCarousel();
            SetupPlayers();
        }

        private void SetupCarousel()
        {
            // Clear old units
            CleanupCarousel();

            // Just for demonstration, pick random champions and items
            // In a real game, this would come from a Pool or RoundData
            var allChampionData = _manager.Shop.AllHeroes ?? new List<ChampionData>();
            
            var allItemData = _manager.Items.DebugTestItems != null ? 
                             _manager.Items.DebugTestItems.ToList() : new List<ItemData>();

            if (allChampionData.Count() == 0) return;

            int unitCount = 9; // Typical TFT carousel count
            float angleStep = 360f / unitCount;

            for (int i = 0; i < unitCount; i++)
            {
                ChampionData hero = allChampionData[UnityEngine.Random.Range(0, allChampionData.Count())];
                ItemData item = allItemData.Count() > 0 ? allItemData[UnityEngine.Random.Range(0, allItemData.Count())] : null;

                Vector3 localPos = Quaternion.Euler(0, i * angleStep, 0) * Vector3.forward * _arena.championRadius;
                Vector3 worldPos = _arena.center.position + localPos;

                GameObject heroObj = Instantiate(hero.prefab, worldPos, Quaternion.LookRotation(-localPos, Vector3.up), _arena.center);
                ChampionActor actor = heroObj.GetComponent<ChampionActor>();
                actor.Initialize();

                CarouselUnit unit = heroObj.AddComponent<CarouselUnit>();
                unit.Initialize(actor, item);
                _unitsInCarousel.Add(unit);
            }
        }

        private void SetupPlayers()
        {
            var players = _manager.Player.Players;
            _sortedPlayers = players.OrderBy(p => p.HP).ToList();

            // Clear old barriers
            foreach (var b in _barriers) if (b != null) Destroy(b.gameObject);
            _barriers.Clear();

            int playerCount = _sortedPlayers.Count;
            for (int i = 0; i < playerCount; i++)
            {
                PlayerData p = _sortedPlayers[i];
                if (p.Tactician == null) continue;

                // Teleport tactician using automated pos
                var playerSpawn = _arena.GetPlayerSpawn(i, playerCount);
                p.Tactician.Teleport(playerSpawn.pos, true);
                p.Tactician.CachedTransform.rotation = playerSpawn.rot;

                // Spawn barrier using automated pos
                if (_arena.barrierPrefab != null)
                {
                    var barrierSpawn = _arena.GetBarrierSpawn(i, playerCount);
                    GameObject barrierObj = Instantiate(_arena.barrierPrefab, barrierSpawn.pos, barrierSpawn.rot);
                    CarouselBarrier barrier = barrierObj.GetComponent<CarouselBarrier>();
                    if (barrier != null)
                    {
                        barrier.SetActive(true);
                        _barriers.Add(barrier);
                    }
                }
            }
        }

        private void Update()
        {
            if (!_isActive || _arena == null) return;

            // Rotate carousel
            _arena.center.Rotate(Vector3.up, _arena.rotationSpeed * Time.deltaTime);

            // Handle player release
            if (_releaseGroupIndex * 2 < _sortedPlayers.Count)
            {
                _releaseTimer -= Time.deltaTime;
                if (_releaseTimer <= 0)
                {
                    ReleaseNextPair();
                    _releaseTimer = releaseInterval;
                }
            }
        }

        private void ReleaseNextPair()
        {
            int p1Index = _releaseGroupIndex * 2;
            int p2Index = p1Index + 1;

            if (p1Index < _barriers.Count) _barriers[p1Index].SetActive(false);
            if (p2Index < _barriers.Count) _barriers[p2Index].SetActive(false);

            Debug.Log($"<color=cyan>CarouselSystem: Releasing players {p1Index} and {p2Index}</color>");
            _releaseGroupIndex++;
        }

        private void HandleUnitPicked(CarouselUnit unit, TacticianActor tactician)
        {
            PlayerData player = _manager.Player.Players.FirstOrDefault(p => p.Id == tactician.OwnerID);
            if (player == null) return;

            Debug.Log($"<color=green>Player {player.Name} picked {unit.Champion.ChampionId} with {unit.Item?.itemName}</color>");

            // Give to player's bench
            if (_manager.Bench.HasEmptySlot(player.Id))
            {
                // Remove from carousel rotation
                unit.transform.SetParent(null);
                _unitsInCarousel.Remove(unit);
                
                // Add to bench
                _manager.Bench.AddHeroToBench(player.Id, unit.Champion.CombatActorData as ChampionData);
                
                // Give item
                if (unit.Item != null)
                {
                    // For local player, add to item bench. For NPCs, we'll just ignore for now or 
                    // in future attach to the hero on bench.
                    if (player.Team == Team.Player)
                    {
                        _manager.Items.AddItemToBench(unit.Item);
                    }
                }

                Destroy(unit.gameObject);
            }

            // Teleport player back
            Arena playerArena = _manager.Field.GetArena(player.Id);
            if (playerArena != null)
            {
                tactician.Teleport(playerArena.TacticianSpawnPoint.position, true);
            }
        }

        private void CleanupCarousel()
        {
            foreach (var unit in _unitsInCarousel)
            {
                if (unit != null) Destroy(unit.gameObject);
            }
            _unitsInCarousel.Clear();

            foreach (var b in _barriers)
            {
                if (b != null) Destroy(b.gameObject);
            }
            _barriers.Clear();
        }

        public void Shutdown()
        {
            CarouselUnit.OnUnitPicked -= HandleUnitPicked;
            CleanupCarousel();
        }
    }
}
