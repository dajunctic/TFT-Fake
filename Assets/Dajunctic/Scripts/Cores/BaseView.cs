using System.Linq;
using KBCore.Refs;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Dajunctic
{
    public class BaseView : MonoBehaviour, IBaseView, IEntity
    {
        [Header("Base")]
        [SerializeField] protected Ticker ticker;

        [SerializeField] bool initialize;
        [SerializeField] TickType tick;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (ticker != null) return;
            var guids = AssetDatabase.FindAssets($"t:{nameof(Ticker)}");
            if (guids == null || guids.Length == 0)
            {
                Debug.LogError("Can not find ticker asset in Project!");
                return;
            }

            string firstGuid = guids.FirstOrDefault();
            string path = AssetDatabase.GUIDToAssetPath(firstGuid);
            ticker = AssetDatabase.LoadAssetAtPath<Ticker>(path);

            if (ticker == null)
            {
                Debug.LogError($"Found GUID {firstGuid} but failed to load Ticker asset at {path}");
            }
        }

#endif

        public bool IsInitialized => initialize;


        Transform mTransform;
        public Transform CachedTransform => gameObject.GetAndCacheComponent(ref mTransform);

        public string Id => name;

        void Awake()
        {
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
            if (tick.HasFlag(TickType.EarlyTick)) ticker.SubEarlyTick(this);
            if (tick.HasFlag(TickType.Tick)) ticker.SubTick(this);
            if (tick.HasFlag(TickType.LateTick)) ticker.SubLateTick(this);
            if (tick.HasFlag(TickType.FixedTick)) ticker.SubFixedTick(this);
        }

        public virtual void UnsubTick()
        {
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

        public virtual void Cleanup()
        {
            
        }
    }
}
