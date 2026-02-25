using UnityEngine;
using System;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayFxNode : SkillNode
    {
        public GameObject vfxPrefab;
        public string attachPoint;
        public float duration = 2f;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            Transform parent = null;

            if (vfxPrefab != null)
            {
                var vfx = Instantiate(vfxPrefab, parent != null ? parent.position : context.actor.transform.position, Quaternion.identity);
                if (parent != null) vfx.transform.SetParent(parent);

                if (Application.isPlaying)
                {
                    Destroy(vfx, duration);
                }
                else
                {
#if UNITY_EDITOR
                    if (context.onSpawnVFX != null)
                    {
                        context.onSpawnVFX(vfx);
                    }
                    
                    float startTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                    UnityEditor.EditorApplication.CallbackFunction update = null;
                    update = () =>
                    {
                        if (vfx == null)
                        {
                            UnityEditor.EditorApplication.update -= update;
                            return;
                        }

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

            onComplete?.Invoke();
        }
    }
}
