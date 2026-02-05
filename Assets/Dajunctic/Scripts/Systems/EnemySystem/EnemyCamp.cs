using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Dajunctic
{
    public class EnemyCamp : BaseView
    {
        [SerializeField] public HexAreaView hexAreaView;
        [SerializeField] private EnemyCampData campData;

        [SerializeField] private bool spawnOnStart = true;

        private List<CombatActor> _activeEnemies = new List<CombatActor>();
        private bool _isRespawning = false;

        public override void Initialize()
        {
            base.Initialize();
            if (spawnOnStart && campData != null)
            {
                SpawnCamp();
            }
        }

        public void SpawnCamp()
        {
            if (campData == null) return;
            
            _isRespawning = false;
            _activeEnemies.Clear();

            foreach (var entry in campData.enemies)
            {
                if (entry.prefab == null) continue;
                
                var obj = PoolableObject.Pool.Spawn(entry.prefab, hexAreaView.GetRandomPosition(), Quaternion.identity);

                var actor = obj.GetComponent<EnemyCombatActor>();
                if (actor != null)
                {
                    actor.Teleport(hexAreaView.GetRandomPosition(), true);
                    actor.hexAreaView = hexAreaView;
                    actor.Initialize();
                    _activeEnemies.Add(actor);
                    // actor.OnDead += OnOneEnemyDead; 
                }
            }
        }

        private void OnOneEnemyDead(CombatActor actor)
        {
            // actor.OnDead -= OnOneEnemyDead;
            // _activeEnemies.Remove(actor);
            //
            // if (_activeEnemies.Count == 0 && !_isRespawning)
            // {
            //     Debug.Log($"Camp [{name}] cleared! Respawning in {campData.respawnTime}s...");
            //     _isRespawning = true;
            //     Invoke(nameof(SpawnCamp), campData.respawnTime);
            // }
        }

        public void DespawnCamp()
        {
            foreach (var enemy in _activeEnemies)
            {
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    enemy.GetComponent<PoolableObject>()?.Despawn();
                }
            }
            _activeEnemies.Clear();
        }
    }
}