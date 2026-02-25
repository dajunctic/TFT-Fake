#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    /// <summary>
    /// ISkillServiceProvider dùng trong Editor Preview (SkillGraphEditorWindow).
    /// Không cần GameManager hay scene đang chạy.
    /// Tự quản lý list VFX / Missile prefab riêng, hoặc Instantiate trực tiếp.
    /// </summary>
    public class PreviewSkillServiceProvider : ISkillServiceProvider
    {
        private readonly PreviewRenderUtility _preview;
        private readonly List<GameObject> _spawnedObjects = new();

        // Optional lookup tables (mirror của GameManagerSO) để map ID → prefab
        private readonly GameManagerSO _gameManagerSO;

        public PreviewSkillServiceProvider(PreviewRenderUtility preview, GameManagerSO gameManagerSO = null)
        {
            _preview = preview;
            _gameManagerSO = gameManagerSO;
        }

        public GameObject SpawnFx(string fxId, Vector3 position, Quaternion rotation, float duration = -1f)
        {
            FxView prefab = null;

            // Thử tra cứu qua GameManagerSO nếu có
            if (_gameManagerSO != null)
            {
                var entry = _gameManagerSO.fxLists?.Find(f => f.Id == fxId);
                prefab = entry?.fxViewPrefab;
            }

            if (prefab == null)
            {
                Debug.LogWarning($"[Preview] FX '{fxId}' not found in GameManagerSO. Set GameManagerSO in the editor window.");
                return null;
            }

            var instance = Object.Instantiate(prefab.gameObject, position, rotation);
            instance.hideFlags = HideFlags.HideAndDontSave;
            _preview?.AddSingleGO(instance);
            _spawnedObjects.Add(instance);

            // Tự dọn sau duration
            if (duration > 0)
            {
                float startTime = (float)EditorApplication.timeSinceStartup;
                EditorApplication.CallbackFunction update = null;
                update = () =>
                {
                    if (instance == null) { EditorApplication.update -= update; return; }
                    if ((float)EditorApplication.timeSinceStartup - startTime >= duration)
                    {
                        EditorApplication.update -= update;
                        _spawnedObjects.Remove(instance);
                        Object.DestroyImmediate(instance);
                    }
                };
                EditorApplication.update += update;
            }

            return instance;
        }

        public void SpawnMissile(string missileId, MissileData missileData)
        {
            MissileView prefab = null;

            if (_gameManagerSO != null)
            {
                var entry = _gameManagerSO.missileLists?.Find(m => m.Id == missileId);
                prefab = entry?.missilePrefab;
            }

            if (prefab == null)
            {
                Debug.LogWarning($"[Preview] Missile '{missileId}' not found in GameManagerSO.");
                return;
            }

            var instance = Object.Instantiate(prefab, missileData.launcher, Quaternion.identity);
            instance.hideFlags = HideFlags.HideAndDontSave;
            _preview?.AddSingleGO(instance.gameObject);
            _spawnedObjects.Add(instance.gameObject);
            instance.InitData(missileData);
            instance.StartFly();
        }

        /// <summary>Dọn toàn bộ object đã spawn (gọi khi Reset Preview).</summary>
        public void Cleanup()
        {
            foreach (var go in _spawnedObjects)
            {
                if (go != null) Object.DestroyImmediate(go);
            }
            _spawnedObjects.Clear();
        }
    }
}
#endif
