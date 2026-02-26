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
        private readonly PoolData poolSO;

        public PreviewSkillServiceProvider(PreviewRenderUtility preview)
        {
            _preview = preview;
           
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
        }

        public FxView SpawnFx(SpawnFxEvent playFxEvent)
        {
            throw new System.NotImplementedException();
        }

        public MissileView SpawnMissile(string missileId, MissileData missileData)
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif
