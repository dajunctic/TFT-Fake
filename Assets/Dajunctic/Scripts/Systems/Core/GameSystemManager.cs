using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Central hub that manages all game systems.
    /// Provides access to systems and handles their lifecycle.
    /// </summary>
    public class GameSystemManager : MonoBehaviour
    {
        public static GameSystemManager Instance { get; private set; }

        [Header("System References")]
        [SerializeField] private BenchSystem benchSystem;
        [SerializeField] private FieldSystem fieldSystem;
        [SerializeField] private EconomySystem economySystem;
        [SerializeField] private ShopSystem shopSystem;

        // Public accessors
        public BenchSystem Bench => benchSystem;
        public FieldSystem Field => fieldSystem;
        public EconomySystem Economy => economySystem;
        public ShopSystem Shop => shopSystem;

        private void Awake()
        {
            // Singleton setup
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Find systems in children if not assigned
            if (benchSystem == null) benchSystem = GetComponentInChildren<BenchSystem>();
            if (fieldSystem == null) fieldSystem = GetComponentInChildren<FieldSystem>();
            if (economySystem == null) economySystem = GetComponentInChildren<EconomySystem>();
            if (shopSystem == null) shopSystem = GetComponentInChildren<ShopSystem>();

            InitializeSystems();
        }

        private void InitializeSystems()
        {
            Debug.Log("<color=cyan>GameSystemManager: Initializing all systems...</color>");

            // Initialize in dependency order
            economySystem?.Initialize(this);
            benchSystem?.Initialize(this);
            fieldSystem?.Initialize(this);
            shopSystem?.Initialize(this);

            Debug.Log("<color=green>GameSystemManager: All systems initialized!</color>");
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                ShutdownSystems();
                Instance = null;
            }
        }

        private void ShutdownSystems()
        {
            Debug.Log("<color=yellow>GameSystemManager: Shutting down all systems...</color>");

            // Shutdown in reverse order
            shopSystem?.Shutdown();
            fieldSystem?.Shutdown();
            benchSystem?.Shutdown();
            economySystem?.Shutdown();
        }

        /// <summary>
        /// Get a system by type. Useful for generic access.
        /// </summary>
        public T GetSystem<T>() where T : class, IGameSystem
        {
            if (typeof(T) == typeof(BenchSystem)) return benchSystem as T;
            if (typeof(T) == typeof(FieldSystem)) return fieldSystem as T;
            if (typeof(T) == typeof(EconomySystem)) return economySystem as T;
            if (typeof(T) == typeof(ShopSystem)) return shopSystem as T;
            
            Debug.LogError($"System of type {typeof(T).Name} not found!");
            return null;
        }
    }
}
