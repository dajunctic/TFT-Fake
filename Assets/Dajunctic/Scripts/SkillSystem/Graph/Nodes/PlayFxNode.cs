using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayFxNode : SkillNode
    {
        public GameObject vfxPrefab;
        public string attachPoint;
        public float duration = 2f;

        public override void Execute()
        {
            if (vfxPrefab != null)
            {
                var vfx = Instantiate(vfxPrefab, _context.actor.Position, Quaternion.identity);

                if (Application.isPlaying)
                {
                    Destroy(vfx, duration);
                }
                else
                {
#if UNITY_EDITOR
                    if (_context.onSpawnVFX != null)
                        _context.onSpawnVFX(vfx);

                    float startTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                    UnityEditor.EditorApplication.CallbackFunction update = null;
                    update = () =>
                    {
                        if (vfx == null) { UnityEditor.EditorApplication.update -= update; return; }
                        if ((float)UnityEditor.EditorApplication.timeSinceStartup - startTime >= duration)
                        {
                            UnityEditor.EditorApplication.update -= update;
                            DestroyImmediate(vfx);
                        }
                    };
                    UnityEditor.EditorApplication.update += update;
#endif
                }
            }

            TriggerComplete();
        }
    }
}
