using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [System.Serializable]
    public class CampEnemyEntry
    {
        public PoolableObject prefab;
        public Vector3 localPosition;
        public float yRotation;
    }

    [CreateAssetMenu(fileName = "NewCampData", menuName = "Panthera/Enemy Camp Data")]
    public class EnemyCampData : ScriptableObject
    {
        public List<CampEnemyEntry> enemies = new List<CampEnemyEntry>();
    }
}