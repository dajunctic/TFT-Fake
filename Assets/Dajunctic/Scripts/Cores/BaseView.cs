using System.Linq;
using KBCore.Refs;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Dajunctic
{
    public class BaseView : PoolableObject, IBaseView, IEntity
    {
        [Header("Base")]
        protected Ticker ticker;

        private float _lastEditorTime;
        protected float DeltaTime
        {
            get
            {
                if (Application.isPlaying) return Time.deltaTime;
#if UNITY_EDITOR
                if (_lastEditorTime == 0) _lastEditorTime = (float)EditorApplication.timeSinceStartup;
                float dt = (float)EditorApplication.timeSinceStartup - _lastEditorTime;
                _lastEditorTime = (float)EditorApplication.timeSinceStartup;
                return dt;
#else
                return 0.02f;
#endif
            }
        }

        [SerializeField] bool initialize;
        [SerializeField] TickType tick;

        public bool Initialized => _isInitialized;
        private bool _isInitialized;

        Transform mTransform;
        public Transform CachedTransform => gameObject.GetAndCacheComponent(ref mTransform);

        public string Id => name;

        protected virtual void Awake()
        {
            if (Application.isPlaying && TickerView.Instance != null)
            {
                ticker = TickerView.Instance.ticker;
            }
            if (initialize) Initialize();
        }

        void Start()
        {
        }

        void OnEnable()
        {
            ListenEvents();
            SubTick();
            DoEnable();
        }

        void OnDisable()
        {
            DoDisable();
            UnsubTick();
            StopListenEvents();
        }

        public virtual void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;
        }

        public virtual void InitializeNetwork()
        {
            
        }

        public virtual void DoEnable()
        {

        }

        public virtual void DoDisable()
        {

        }

        public virtual void ListenEvents()
        {

        }

        public virtual void StopListenEvents()
        {

        }

        public virtual void SubTick()
        {
            if (ticker == null && TickerView.Instance != null) ticker = TickerView.Instance.ticker;
            if (ticker == null) return;

            if (tick.HasFlag(TickType.EarlyTick)) ticker.SubEarlyTick(this);
            if (tick.HasFlag(TickType.Tick)) ticker.SubTick(this);
            if (tick.HasFlag(TickType.LateTick)) ticker.SubLateTick(this);
            if (tick.HasFlag(TickType.FixedTick)) ticker.SubFixedTick(this);
        }

        public virtual void UnsubTick()
        {
            if (ticker == null) return;

            if (tick.HasFlag(TickType.EarlyTick)) ticker.UnsubEarlyTick(this);
            if (tick.HasFlag(TickType.Tick)) ticker.UnsubTick(this);
            if (tick.HasFlag(TickType.LateTick)) ticker.UnsubLateTick(this);
            if (tick.HasFlag(TickType.FixedTick)) ticker.UnsubFixedTick(this);
        }

        void Update()
        {
            // Tick();
        }

        public virtual void Tick()
        {

        }

        public virtual void EarlyTick()
        {

        }

        public virtual void LateTick()
        {

        }

        public virtual void FixedTick()
        {

        }

        public virtual void Cleanup()
        {

        }
    }
}
