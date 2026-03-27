using UnityEngine;

namespace Dajunctic
{
    public class CarouselArena : MonoBehaviour
    {
        [Header("Carousel Setup")]
        public Transform center;
        public float championRadius = 5f;
        public float rotationSpeed = 30f;

        [Header("Player Positions (Automated)")]
        public float playerRadius = 12f;
        public float barrierRadius = 9f;

        [Header("Player Positions (Optional Manual Overrides)")]
        public Transform[] playerSpawnPoints;
        public Transform[] barrierSpawnPoints;
        
        [Header("Prefabs")]
        public GameObject barrierPrefab;

        public void Initialize()
        {
            if (center == null) center = transform;
        }

        public (Vector3 pos, Quaternion rot) GetPlayerSpawn(int index, int total)
        {
            if (playerSpawnPoints != null && playerSpawnPoints.Length > index && playerSpawnPoints[index] != null)
                return (playerSpawnPoints[index].position, playerSpawnPoints[index].rotation);

            float angle = (360f / total) * index;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 pos = center.position + direction * playerRadius;
            Quaternion rot = Quaternion.LookRotation(-direction, Vector3.up);
            return (pos, rot);
        }

        public (Vector3 pos, Quaternion rot) GetBarrierSpawn(int index, int total)
        {
            if (barrierSpawnPoints != null && barrierSpawnPoints.Length > index && barrierSpawnPoints[index] != null)
                return (barrierSpawnPoints[index].position, barrierSpawnPoints[index].rotation);

            float angle = (360f / total) * index;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 pos = center.position + direction * barrierRadius;
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            return (pos, rot);
        }

        private void OnDrawGizmos()
        {
            if (center == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(center.position, championRadius);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(center.position, playerRadius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, barrierRadius);
        }
    }
}
