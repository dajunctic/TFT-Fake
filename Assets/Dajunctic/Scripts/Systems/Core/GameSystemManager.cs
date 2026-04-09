using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace Dajunctic
{
    /// <summary>
    /// Central hub that creates, loads data for, and initializes all game systems.
    /// Systems are bound in code — no drag-drop of individual systems in the Inspector.
    /// Assign only the GameSystemManagerData SO to configure which Addressable assets to load.
    /// </summary>
    public class GameSystemManager : MonoBehaviour
    {
        public static GameSystemManager Instance { get; private set; }

        [Header("Config (single SO — all data refs inside)")]
        [SerializeField] private GameSystemManagerData config;

        // Public accessors (same API as before)
        public SettingsSystem Settings { get; private set; }
        public BenchSystem Bench { get; private set; }
        public FieldSystem Field { get; private set; }
        public ShopSystem Shop { get; private set; }
        public ItemSystem Items { get; private set; }
        public EmotionSystem Emotion { get; private set; }
        public TraitSystem Traits { get; private set; }
        public RoundSystem Round { get; private set; }
        public PlayerSystem Player { get; private set; }
        public AugmentSystem Augment { get; private set; }
        public CarouselSystem Carousel { get; private set; }
        public AISystem AI { get; private set; }
        public ChatSystem Chat { get; private set; }
        public TravelSystem Travel { get; private set; }

        /// <summary>True once all system data has been loaded and systems are initialized.</summary>
        public bool AllSystemsReady { get; private set; }

        private readonly List<IGameSystem> _systems = new List<IGameSystem>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            CreateSystems();
        }

        private async void Start()
        {
            await LoadAllDataAsync();
            InitializeSystems();

            AllSystemsReady = true;
            Debug.Log("<color=green>GameSystemManager: All systems ready!</color>");
        }

        // ─── Phase 1: Create system instances ────────────────────────────────────

        private void CreateSystems()
        {
            Debug.Log("<color=cyan>GameSystemManager: Creating systems...</color>");

            Settings = CreateSystem<SettingsSystem>();
            Bench = CreateSystem<BenchSystem>();
            Field = CreateSystem<FieldSystem>();
            Shop = CreateSystem<ShopSystem>();
            Items = CreateSystem<ItemSystem>();
            Emotion = CreateSystem<EmotionSystem>();
            Traits = CreateSystem<TraitSystem>();
            Round = CreateSystem<RoundSystem>();
            Player = CreateSystem<PlayerSystem>();
            Augment = CreateSystem<AugmentSystem>();
            Carousel = CreateSystem<CarouselSystem>();
            AI = CreateSystem<AISystem>();
            Chat = CreateSystem<ChatSystem>();
            Travel = CreateSystem<TravelSystem>();

            Debug.Log("<color=cyan>GameSystemManager: Systems created.</color>");
        }

        private T CreateSystem<T>() where T : MonoBehaviour, IGameSystem
        {
            var go = new GameObject(typeof(T).Name);
            go.transform.SetParent(transform);
            var system = go.AddComponent<T>();
            _systems.Add(system);
            return system;
        }

        // ─── Phase 2: Load data async via Addressables ───────────────────────────

        private async Task LoadAllDataAsync()
        {
            Debug.Log("<color=cyan>GameSystemManager: Loading system data...</color>");

            // Load sequentially so each system has its data before the next starts
            await Settings.LoadDataAsync();
            await Bench.LoadDataAsync();
            await Field.LoadDataAsync();
            await Shop.LoadDataAsync();
            await Items.LoadDataAsync();
            await Emotion.LoadDataAsync();
            await Traits.LoadDataAsync();
            await Round.LoadDataAsync();
            await Player.LoadDataAsync();
            await Augment.LoadDataAsync();
            await Carousel.LoadDataAsync();
            await AI.LoadDataAsync();
            await Chat.LoadDataAsync();
            await Travel.LoadDataAsync();

            Debug.Log("<color=cyan>GameSystemManager: All data loaded.</color>");
        }

        // ─── Phase 3: Initialize (cross-system wiring) ───────────────────────────

        private void InitializeSystems()
        {
            Debug.Log("<color=cyan>GameSystemManager: Initializing systems...</color>");

            foreach (var system in _systems)
                system.Initialize(this);

            Debug.Log("<color=cyan>GameSystemManager: All systems initialized.</color>");
        }

        // ─── Shutdown ────────────────────────────────────────────────────────────

        private void OnDestroy()
        {
            if (Instance != this) return;

            Debug.Log("<color=yellow>GameSystemManager: Shutting down...</color>");

            // Shutdown in reverse order
            for (int i = _systems.Count - 1; i >= 0; i--)
                _systems[i].Shutdown();

            Instance = null;
        }

        // ─── Generic accessor ─────────────────────────────────────────────────────

        /// <summary>Get a system by type.</summary>
        public T GetSystem<T>() where T : class, IGameSystem
        {
            foreach (var s in _systems)
                if (s is T typed) return typed;

            Debug.LogError($"System of type {typeof(T).Name} not found!");
            return null;
        }

        // ─── Internal: expose config for systems ──────────────────────────────────

        internal GameSystemManagerData Config => config;
    }
}
