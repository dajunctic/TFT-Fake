using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.AI.Navigation;
using TMPro;

namespace Dajunctic
{
    public class Arena : NetworkBehaviour
    {
        [Header("Settings (Synced)")]
        private readonly SyncVar<int> _ownerId = new SyncVar<int>(-1);
        private readonly SyncVar<string> _ownerName = new SyncVar<string>("None");

        public int OwnerID { get => _ownerId.Value; set => _ownerId.Value = value; }
        public string OwnerName { get => _ownerName.Value; set => _ownerName.Value = value; }

        [Header("References")]
        public HexAreaView FieldArea;
        public HexAreaView GuestFieldArea;
        public SquareAreaView BenchArea;
        public Transform GuestSpawnPoint;
        public Transform TacticianSpawnPoint;
        [SerializeField, GuidReference("fx", typeof(IDummyId))] private string fxGuid;

        private void Awake()
        {
            _ownerId.OnChange += OnOwnerIdChanged;
            var surface = GetComponent<NavMeshSurface>();
            if (surface != null)
            {
                surface.BuildNavMesh();
            }
        }

        private void OnOwnerIdChanged(int prev, int next, bool asServer)
        {

            if (next != -1)
            {
                RegisterToSystems();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (OwnerID != -1) RegisterToSystems();
        }

        public void SetOwnerServer(int ownerId, string ownerName)
        {
            OwnerID = ownerId;
            OwnerName = ownerName;
            
            if (FieldArea != null) FieldArea.Initialize();
            if (GuestFieldArea != null) GuestFieldArea.Initialize();
            if (BenchArea != null) BenchArea.Initialize();
            SpawnChampionCountUI();

            RegisterToSystems();
        }

        public void Initialize(int ownerId, string ownerName)
        {
            OwnerID = ownerId;
            OwnerName = ownerName;
            if (FieldArea != null) FieldArea.Initialize();
            if (GuestFieldArea != null) GuestFieldArea.Initialize();
            if (BenchArea != null) BenchArea.Initialize();
            SpawnChampionCountUI();
        }

        private void Start()
        {
            SpawnChampionCountUI();
        }

        private void SpawnChampionCountUI()
        {
            if (PoolView.Instance != null && GetComponentInChildren<ArenaChampionCountUI>() == null)
            {
                var countUI = PoolView.Instance.SpawnArenaChampionCountUI(transform.position);
                if (countUI != null)
                {
                    countUI.transform.SetParent(transform);
                    countUI.transform.localPosition = new Vector3(0, 0.173f, 0);
                    countUI.transform.localRotation = Quaternion.Euler(90f, 0, 0);
                    countUI.Initialize(this);
                }
            }
        }

        private void RegisterToSystems()
        {
            if (GameSystemManager.Instance == null) return;

            GameSystemManager.Instance.Field.RegisterArena(this);

            GameSystemManager.Instance.Bench.RegisterArena(this, fxGuid);

            Debug.Log($"Arena registered for player {OwnerID} on {(IsServerInitialized ? "Server" : "Client")}");
        }

        public Vector3 GetFieldWorldPosition(Vector2Int coord)
        {
            if (FieldArea == null || FieldArea.Data == null) return transform.position;
            Vector3 localPos = FieldArea.Data.HexToWorld(Vector3.zero, coord);
            return FieldArea.CachedTransform.TransformPoint(localPos);
        }

        public Vector3 GetGuestFieldWorldPosition(Vector2Int coord)
        {
            if (GuestFieldArea == null || GuestFieldArea.Data == null) return transform.position;
            Vector3 localPos = GuestFieldArea.Data.HexToWorld(Vector3.zero, coord);
            return GuestFieldArea.CachedTransform.TransformPoint(localPos);
        }

        public Vector3 GetBenchWorldPosition(Vector2Int coord)
        {
            if (BenchArea == null || BenchArea.Data == null) return transform.position;
            Vector3 localPos = BenchArea.Data.SquareToWorld(Vector3.zero, coord);
            return BenchArea.CachedTransform.TransformPoint(localPos);
        }

        public bool TrySnapToField(Vector3 worldPos, out Vector2Int coord)
        {
            coord = new Vector2Int(-1, -1);
            if (FieldArea == null || FieldArea.Data == null) return false;

            Vector3 localPos = FieldArea.CachedTransform.InverseTransformPoint(worldPos);
            coord = FieldArea.Data.WorldToHex(localPos, Vector3.zero);

            return FieldArea.Data.TryGetTile(coord, out _);
        }

        public bool TrySnapToGuestField(Vector3 worldPos, out Vector2Int coord)
        {
            coord = new Vector2Int(-1, -1);
            if (GuestFieldArea == null || GuestFieldArea.Data == null) return false;

            Vector3 localPos = GuestFieldArea.CachedTransform.InverseTransformPoint(worldPos);
            coord = GuestFieldArea.Data.WorldToHex(localPos, Vector3.zero);

            return GuestFieldArea.Data.TryGetTile(coord, out _);
        }

        public bool TrySnapToBench(Vector3 worldPos, out Vector2Int coord)
        {
            coord = new Vector2Int(-1, -1);
            if (BenchArea == null || BenchArea.Data == null) return false;

            Vector3 localPos = BenchArea.CachedTransform.InverseTransformPoint(worldPos);
            coord = BenchArea.Data.WorldToSquare(localPos, Vector3.zero);

            return BenchArea.Data.TryGetTile(coord, out _);
        }
    }
}
