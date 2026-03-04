#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class PreviewSkillServiceProvider : ISkillServiceProvider
    {
        private readonly PreviewRenderUtility _preview;
        private readonly List<GameObject> _spawnedObjects = new();
        private readonly List<System.Collections.IEnumerator> _activeCoroutines = new();
        private readonly PoolData poolSO;
        private float _lastUpdateTime;
        private bool _showDebug;
        public bool IsDebug => _showDebug;

        private static readonly string LogPrefix = "<color=#4dabf7>[PreviewServices]</color>";
        private static readonly string SuccessColor = "#69db7c";
        private static readonly string WarningColor = "#ff922b";

        public PreviewSkillServiceProvider(PreviewRenderUtility preview, bool showDebug = true)
        {
            _preview = preview;
            _showDebug = showDebug;
            var guids = AssetDatabase.FindAssets("t:PoolData");
            if (guids.Length > 0)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[0]);
                poolSO = AssetDatabase.LoadAssetAtPath<PoolData>(path);
            }
        }

        public void TrackSpawnedObject(GameObject go)
        {
            if (go == null) return;
            _spawnedObjects.Add(go);
            _preview?.AddSingleGO(go);
        }

        public void Cleanup()
        {
            if (_showDebug && (_spawnedObjects.Count > 0 || _activeCoroutines.Count > 0))
                Debug.Log($"{LogPrefix} Cleanup: Destroying {_spawnedObjects.Count} objects and stopping {_activeCoroutines.Count} coroutines.");

            foreach (var go in _spawnedObjects)
            {
                if (go != null) Object.DestroyImmediate(go);
            }
            _spawnedObjects.Clear();
            _activeCoroutines.Clear();
        }

        public void Update()
        {
            float dt = 0.02f; // Default
            if (_lastUpdateTime > 0) dt = (float)EditorApplication.timeSinceStartup - _lastUpdateTime;
            _lastUpdateTime = (float)EditorApplication.timeSinceStartup;

            // 1. Update BaseViews (like FxView.Tick)
            for (int i = _spawnedObjects.Count - 1; i >= 0; i--)
            {
                var go = _spawnedObjects[i];
                if (go == null) { _spawnedObjects.RemoveAt(i); continue; }

                var views = go.GetComponents<BaseView>();
                foreach (var view in views)
                {
                    // Force tick in editor
                    view.Tick();
                }
            }

            // 2. Pump manual coroutines (like Missile movement)
            for (int i = _activeCoroutines.Count - 1; i >= 0; i--)
            {
                if (!_activeCoroutines[i].MoveNext())
                {
                    _activeCoroutines.RemoveAt(i);
                }
            }
        }

        public void StartManualCoroutine(System.Collections.IEnumerator coroutine)
        {
            if (coroutine != null)
            {
                if (_showDebug) Debug.Log($"{LogPrefix} Starting Manual Coroutine: <color=#ced4da>{coroutine.GetType().Name}</color>");
                _activeCoroutines.Add(coroutine);
            }
        }

        public FxView SpawnFx(SpawnFxEvent playFxEvent)
        {
            if (poolSO == null || string.IsNullOrEmpty(playFxEvent.id)) return null;
            var entity = poolSO.fxLists.Find(f => f.Id == playFxEvent.id);
            if (entity == null || entity.fxViewPrefab == null)
            {
                if (_showDebug) Debug.LogWarning($"{LogPrefix} SpawnFx: Prefab NOT FOUND for ID: <color={WarningColor}>{playFxEvent.id}</color>");
                return null;
            }

            if (_showDebug) Debug.Log($"{LogPrefix} Spawning FX: <color={SuccessColor}>{playFxEvent.id}</color> at {playFxEvent.position}");
            var go = Object.Instantiate(entity.fxViewPrefab.gameObject);
            go.hideFlags = HideFlags.HideAndDontSave;
            TrackSpawnedObject(go);

            if (_showDebug)
            {
                var psCount = go.GetComponentsInChildren<ParticleSystem>(true)?.Length ?? 0;
                Debug.Log($"{LogPrefix} FX instantiated: name='{go.name}', active={go.activeSelf}, layer={go.layer}, particleSystems={psCount}");
            }

            var fxView = go.GetComponent<FxView>();
            fxView.Play(playFxEvent);

            // In PreviewRenderUtility, ParticleSystem time may not advance as expected.
            // Force a small simulation step so users can immediately see the effect.
            var particleSystems = go.GetComponentsInChildren<ParticleSystem>(true);
            if (particleSystems != null && particleSystems.Length > 0)
            {
                foreach (var ps in particleSystems)
                {
                    if (ps == null) continue;
                    ps.Simulate(0.02f, true, false, true);
                    ps.Play(true);
                }
            }

            return fxView;
        }

        public MissileView SpawnMissile(MissileData missileData)
        {
            if (poolSO == null)
            {
                if (_showDebug) Debug.LogWarning($"{LogPrefix} SpawnMissile: <color={WarningColor}>PoolData (poolSO) is NULL!</color>");
                return null;
            }
            if (string.IsNullOrEmpty(missileData.id))
            {
                if (_showDebug) Debug.LogWarning($"{LogPrefix} SpawnMissile: <color={WarningColor}>missileData.id is EMPTY!</color>");
                return null;
            }
            var entity = poolSO.missileLists.Find(m => m.Id == missileData.id);
            if (entity == null || entity.missilePrefab == null)
            {
                if (_showDebug) Debug.LogWarning($"{LogPrefix} SpawnMissile: Prefab NOT FOUND for ID: <color={WarningColor}>{missileData.id}</color>");
                return null;
            }

            if (_showDebug) Debug.Log($"{LogPrefix} Spawning Missile: <color={SuccessColor}>{missileData.id}</color> targeting {missileData.destination}");
            var go = Object.Instantiate(entity.missilePrefab.gameObject);
            go.hideFlags = HideFlags.HideAndDontSave;
            TrackSpawnedObject(go);

            var missileView = go.GetComponent<MissileView>();
            missileView.InitData(missileData);
            missileView.StartFly();

            if (!Application.isPlaying)
            {
                var coroutine = missileView.GetEditorCoroutine();
                if (coroutine != null) StartManualCoroutine(coroutine);
            }
            return missileView;
        }
    }
}
#endif
