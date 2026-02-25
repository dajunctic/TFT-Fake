#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    /// <summary>
    /// ISkillServiceProvider dùng trong Editor Preview (SkillGraphEditorWindow).
    /// Không cần GameManager hay scene đang chạy.
    /// Các node có vfxPrefab set trực tiếp sẽ fallback Instantiate tự nhiên.
    /// Missile preview chỉ log (không có vật lý real trong preview scene).
    /// </summary>
    public class PreviewSkillServiceProvider : ISkillServiceProvider
    {
        private readonly PreviewRenderUtility _preview;
        private readonly List<GameObject> _spawnedObjects = new();

        public PreviewSkillServiceProvider(PreviewRenderUtility preview)
        {
            _preview = preview;
        }

        public GameObject SpawnFx(string fxId, Vector3 position, Quaternion rotation, float duration = -1f)
        {
            // Trong editor preview không có PoolSO, chỉ log
            Debug.Log($"[Preview] SpawnFx: '{fxId}' at {position} (no pool in preview - set vfxPrefab directly on PlayFxNode)");
            return null;
        }

        public void SpawnMissile(string missileId, MissileData missileData)
        {
            // Missile không thể bay trong PreviewRenderUtility (không có physics scene)
            Debug.Log($"[Preview] SpawnMissile: '{missileId}' → target {missileData.targetActor?.name} (missile preview not supported)");
        }

        /// <summary>Track một VFX instance đã được một node tự spawn để cleanup sau.</summary>
        public void TrackSpawnedObject(GameObject go)
        {
            if (go == null) return;
            _spawnedObjects.Add(go);
            _preview?.AddSingleGO(go);
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
