using UnityEngine;
using System;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class WaitNode : SkillNode
    {
        public float duration;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            if (Application.isPlaying)
            {
                context.actor.StartCoroutine(WaitCoroutine(onComplete));
            }
            else
            {
#if UNITY_EDITOR
                float startTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                UnityEditor.EditorApplication.CallbackFunction update = null;
                update = () =>
                {
                    if ((float)UnityEditor.EditorApplication.timeSinceStartup - startTime >= duration)
                    {
                        UnityEditor.EditorApplication.update -= update;
                        onComplete?.Invoke();
                    }
                };
                UnityEditor.EditorApplication.update += update;
#endif
            }
        }

        private IEnumerator WaitCoroutine(Action onComplete)
        {
            yield return new WaitForSeconds(duration);
            onComplete?.Invoke();
        }
    }
}
