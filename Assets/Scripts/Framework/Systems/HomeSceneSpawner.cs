using UnityEngine;
using FishNet;

namespace Dajunctic
{
    public class HomeSceneSpawner : MonoBehaviour
    {
        [Header("Prefabs to Spawn on Server")]
        public GameObject[] systemPrefabs;

        private void Start()
        {
            if (InstanceFinder.IsServerStarted)
            {
                SpawnPrefabs();
            }
            else if (InstanceFinder.ServerManager != null)
            {
                InstanceFinder.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            }
            else
            {
                Debug.LogWarning("[HomeSceneSpawner] InstanceFinder.ServerManager is null. Cannot subscribe to connection events.");
            }
        }

        private void OnDestroy()
        {
            if (InstanceFinder.ServerManager != null)
                InstanceFinder.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        }

        private void ServerManager_OnServerConnectionState(FishNet.Transporting.ServerConnectionStateArgs args)
        {
            if (args.ConnectionState == FishNet.Transporting.LocalConnectionState.Started)
            {
                SpawnPrefabs();
            }
        }

        private void SpawnPrefabs()
        {
            if (systemPrefabs != null)
            {
                foreach (var prefab in systemPrefabs)
                {
                    if (prefab != null)
                    {
                        var obj = Instantiate(prefab);
                        InstanceFinder.ServerManager.Spawn(obj);
                        Debug.Log($"[HomeSceneSpawner] Spawned {prefab.name} dynamically!");
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
