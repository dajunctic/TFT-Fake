using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Quản lý spawn và tracking quái PvE trên tất cả arena.
    /// Gắn cùng GameObject với Gameplay (cùng scene prefab).
    /// </summary>
    public class PveWaveSpawner : MonoBehaviour
    {
        public static PveWaveSpawner Instance { get; private set; }

        // Track enemies per arena: arenaOwnerId → list of spawned DummyActors
        private readonly Dictionary<int, List<DummyActor>> _activeEnemies =
            new Dictionary<int, List<DummyActor>>();

        // Track the active enemy team per arena so late-deployed champions can be assigned
        private readonly Dictionary<int, SimpleTeam> _enemyTeamPerArena =
            new Dictionary<int, SimpleTeam>();

        private bool _waveActive;

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        private void OnEnable()
        {
            this.RegisterListener<EnemyDiedEvent>(OnEnemyDied);
        }

        private void OnDisable()
        {
            this.RemoveListener<EnemyDiedEvent>(OnEnemyDied);
        }

        // ─── Public API ───────────────────────────────────────────────────────

        /// <summary>
        /// Spawn wave cho tất cả arena dựa trên RoundData.
        /// Gọi từ Gameplay.cs khi phase Combat + roundType là PvE.
        /// </summary>
        public void SpawnWaveForAllArenas(RoundData roundData)
        {
            Debug.Log($"[PveWaveSpawner] SpawnWaveForAllArenas called. roundData={roundData?.name}");

            if (roundData == null || roundData.enemyWave == null || roundData.enemyWave.Count == 0)
            {
                Debug.LogWarning("[PveWaveSpawner] RoundData has no enemy wave configured!");
                return;
            }

            ClearAllEnemies();
            _waveActive = true;

            var fieldSystem = GameSystemManager.Instance?.Field;
            if (fieldSystem == null)
            {
                Debug.LogError("[PveWaveSpawner] FieldSystem not found!");
                return;
            }

            var arenas = fieldSystem.GetAllArenas();
            Debug.Log($"[PveWaveSpawner] Arenas found: {arenas.Count}");

            if (arenas.Count == 0)
            {
                Debug.LogWarning("[PveWaveSpawner] No arenas registered in FieldSystem!");
                return;
            }

            foreach (var arena in arenas)
            {
                SpawnWaveOnArena(roundData, arena);
            }

            Debug.Log($"<color=lime>[PveWaveSpawner] Wave spawned on {arenas.Count} arenas.</color>");
        }

        /// <summary>
        /// Xóa tất cả quái đang active (gọi khi round kết thúc).
        /// </summary>
        public void ClearAllEnemies()
        {
            _waveActive = false;
            foreach (var list in _activeEnemies.Values)
            {
                foreach (var enemy in list)
                {
                    if (enemy != null)
                        Destroy(enemy.gameObject);
                }
            }
            _activeEnemies.Clear();
            _enemyTeamPerArena.Clear();
            Debug.Log("[PveWaveSpawner] All enemies cleared.");
        }

        /// <summary>
        /// Trả về EnemyTeam hiện tại của arena. Champion kéo vào sân giữa combat gọi API này.
        /// </summary>
        public ICombatTeam GetEnemyTeamForArena(int arenaOwnerId)
        {
            _enemyTeamPerArena.TryGetValue(arenaOwnerId, out var team);
            return team;
        }

        /// <summary>Tổng số quái còn sống trên tất cả arena.</summary>
        public int GetTotalAliveEnemies()
        {
            int count = 0;
            foreach (var list in _activeEnemies.Values)
                count += list.Count(e => e != null && e.Alive && e.gameObject.activeInHierarchy);
            return count;
        }

        // ─── Spawn Logic ──────────────────────────────────────────────────────

        private void SpawnWaveOnArena(RoundData roundData, Arena arena)
        {
            Debug.Log($"[PveWaveSpawner] SpawnWaveOnArena → Arena {arena.OwnerID} | GuestFieldArea={(arena.GuestFieldArea != null ? "OK" : "NULL")}");

            if (arena.GuestFieldArea == null || arena.GuestFieldArea.Data == null)
            {
                Debug.LogWarning($"[PveWaveSpawner] Arena {arena.OwnerID} has no GuestFieldArea!");
                return;
            }

            // Lấy danh sách tất cả tile coords của GuestFieldArea, sort theo pattern
            var availableCoords = GetCoordsForPattern(
                arena.GuestFieldArea.Data,
                roundData.spawnPattern
            );

            Debug.Log($"[PveWaveSpawner] Arena {arena.OwnerID}: {availableCoords.Count} tiles, {roundData.enemyWave.Count} entries");

            var enemyList = new List<DummyActor>();
            int coordIndex = 0;

            foreach (var entry in roundData.enemyWave)
            {
                Debug.Log($"[PveWaveSpawner] Entry: actorData={(entry.actorData != null ? entry.actorData.name : "NULL")}, prefab={(entry.actorData?.prefab != null ? entry.actorData.prefab.name : "NULL")}, count={entry.count}");

                if (entry.actorData == null || entry.actorData.prefab == null)
                {
                    Debug.LogWarning("[PveWaveSpawner] Wave entry missing actorData or actorData.prefab — skipping.");
                    continue;
                }

                for (int i = 0; i < entry.count; i++)
                {
                    if (coordIndex >= availableCoords.Count)
                    {
                        Debug.LogWarning("[PveWaveSpawner] Not enough tiles for all enemies in wave!");
                        break;
                    }

                    Vector2Int coord = availableCoords[coordIndex++];
                    Vector3 worldPos = arena.GetGuestFieldWorldPosition(coord);

                    GameObject go = Instantiate(entry.actorData.prefab, worldPos, arena.GuestFieldArea.CachedTransform.rotation);
                    go.name = $"Enemy_{entry.actorData.name}_{i}";

                    // Network spawn nếu có NetworkObject (để client cũng thấy)
                    var nob = go.GetComponent<FishNet.Object.NetworkObject>();
                    if (nob != null)
                    {
                        FishNet.InstanceFinder.ServerManager.Spawn(go);
                        Debug.Log($"[PveWaveSpawner] NetworkSpawn: {go.name}");
                    }

                    var dummy = go.GetComponent<DummyActor>();
                    if (dummy == null)
                    {
                        Debug.LogError($"[PveWaveSpawner] Prefab {entry.actorData.prefab.name} has no DummyActor component!");
                        Destroy(go);
                        continue;
                    }

                    // Setup: máu có giới hạn, dùng data từ SO
                    dummy.SetInfiniteHp(false);
                    dummy.SetCombatData(entry.actorData);
                    // Đặt team Opponent để Gambit condition của champion nhận ra là kẻ địch
                    dummy.SetTeam(Team.Opponent);
                    dummy.OwnerID = arena.OwnerID; // gắn vào arena tương ứng
                    dummy.Initialize();

                    enemyList.Add(dummy);
                    Debug.Log($"<color=lime>[PveWaveSpawner] Spawned {go.name} at {worldPos} | Team=Opponent</color>");
                }
            }

            if (!_activeEnemies.ContainsKey(arena.OwnerID))
                _activeEnemies[arena.OwnerID] = new List<DummyActor>();
            _activeEnemies[arena.OwnerID].AddRange(enemyList);

            // Build SimpleTeam từ enemy list và gán cho champions → BT tìm được target
            var enemyTeam = new SimpleTeam();
            foreach (var dummy in enemyList)
                enemyTeam.Add(dummy);

            // Cache team so late-deployed champions (dragged during combat) can also receive it
            _enemyTeamPerArena[arena.OwnerID] = enemyTeam;

            var fieldSystem = GameSystemManager.Instance?.Field;
            if (fieldSystem != null)
            {
                foreach (var champion in fieldSystem.GetHeroesOnField(arena.OwnerID))
                {
                    champion.SetEnemyTeam(enemyTeam);
                    Debug.Log($"[PveWaveSpawner] Set EnemyTeam ({enemyList.Count} enemies) on {champion.name}");
                }
            }

            Debug.Log($"<color=lime>[PveWaveSpawner] Arena {arena.OwnerID}: spawned {enemyList.Count} enemies.</color>");
        }

        // ─── Coord Sorting by Pattern ─────────────────────────────────────────

        private List<Vector2Int> GetCoordsForPattern(HexAreaData hexData, SpawnPattern pattern)
        {
            var all = hexData.ActiveTiles.Select(t => t.coordinates).ToList();

            switch (pattern)
            {
                case SpawnPattern.FrontRow:
                    // Sort by Y descending (hàng cao nhất = xa nhất = "front" với guest)
                    // rồi trong cùng Y, sort X tăng dần
                    return all.OrderByDescending(c => c.y).ThenBy(c => c.x).ToList();

                case SpawnPattern.Scattered:
                    // Shuffle ngẫu nhiên
                    var shuffled = all.OrderBy(_ => Random.value).ToList();
                    return shuffled;

                case SpawnPattern.MiddleColumn:
                    // Lấy X median rồi sort theo Y
                    if (all.Count == 0) return all;
                    var xs = all.Select(c => c.x).OrderBy(x => x).ToList();
                    int midX = xs[xs.Count / 2];
                    return all.Where(c => c.x == midX).OrderByDescending(c => c.y).ToList();

                default:
                    return all;
            }
        }

        // ─── Event Handler ────────────────────────────────────────────────────

        private void OnEnemyDied(EnemyDiedEvent evt)
        {
            if (!_waveActive) return;

            int remaining = GetTotalAliveEnemies();
            Debug.Log($"[PveWaveSpawner] Enemy died ({evt.enemy?.name}). Remaining alive: {remaining}");

            if (remaining <= 0)
            {
                _waveActive = false;
                this.Raise(new AllEnemiesDeadEvent());
                Debug.Log("<color=lime>[PveWaveSpawner] All enemies dead! Raising AllEnemiesDeadEvent.</color>");
            }
        }
    }

    // ─── Events ──────────────────────────────────────────────────────────────────

    /// <summary>Fire khi toàn bộ quái trong wave PvE bị tiêu diệt.</summary>
    public struct AllEnemiesDeadEvent : IEvent { }
}
