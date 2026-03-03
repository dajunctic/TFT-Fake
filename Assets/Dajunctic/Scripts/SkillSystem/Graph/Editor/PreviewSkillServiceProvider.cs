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

        public PreviewSkillServiceProvider(PreviewRenderUtility preview)
        {
            _preview = preview;
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
            _activeCoroutines.Add(coroutine);
        }

        public FxView SpawnFx(SpawnFxEvent playFxEvent)
        {
            if (poolSO == null || string.IsNullOrEmpty(playFxEvent.id)) return null;
            var entity = poolSO.fxLists.Find(f => f.Id == playFxEvent.id);
            if (entity == null || entity.fxViewPrefab == null) return null;

            var go = Object.Instantiate(entity.fxViewPrefab.gameObject);
            go.hideFlags = HideFlags.HideAndDontSave;
            TrackSpawnedObject(go);

            var fxView = go.GetComponent<FxView>();
            fxView.Play(playFxEvent);
            return fxView;
        }

        public MissileView SpawnMissile(MissileData missileData)
        {
            if (poolSO == null || string.IsNullOrEmpty(missileData.id)) return null;
            var entity = poolSO.missileLists.Find(m => m.Id == missileData.id);
            if (entity == null || entity.missilePrefab == null) return null;

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
