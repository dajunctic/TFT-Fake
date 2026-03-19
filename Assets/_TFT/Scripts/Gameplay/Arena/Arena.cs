using UnityEngine;

namespace Dajunctic
{
    public class Arena : MonoBehaviour
    {
        [Header("Settings")]
        public int OwnerID = -1;
        public string OwnerName = "None";

        [Header("References")]
        public HexAreaView FieldArea;
        public SquareAreaView BenchArea;
        public Transform GuestSpawnPoint;
        public Transform TacticianSpawnPoint;
        [SerializeField, GuidReference("fx", typeof(IDummyId))] private string fxGuid;

        public void Initialize(int ownerId, string ownerName)
        {
            OwnerID = ownerId;
            OwnerName = ownerName;

            if (FieldArea != null) FieldArea.Initialize();
            if (BenchArea != null) BenchArea.Initialize();

            RegisterToSystems();
        }

        private void Awake()
        {
            if (OwnerID != -1) RegisterToSystems();
        }

        private void RegisterToSystems()
        {
            if (GameSystemManager.Instance == null) return;
            GameSystemManager.Instance.Field.RegisterArena(this);
            GameSystemManager.Instance.Bench.RegisterArena(this, fxGuid);
            Debug.Log($"Arena registered for player {OwnerID}");
        }

        public Vector3 GetFieldWorldPosition(Vector2Int coord)
        {
            if (FieldArea == null || FieldArea.Data == null) return transform.position;
            Vector3 localPos = FieldArea.Data.HexToWorld(Vector3.zero, coord);
            return FieldArea.CachedTransform.TransformPoint(localPos);
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
