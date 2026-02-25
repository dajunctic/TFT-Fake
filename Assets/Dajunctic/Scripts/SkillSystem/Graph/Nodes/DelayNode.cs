using UnityEngine;
using System.Collections;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DelayNode : SkillNode
    {
        public float duration = 1f;

        public override void Execute()
        {
            if (Application.isPlaying)
            {
                _context.actor.StartCoroutine(WaitCoroutine());
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
                        TriggerComplete();
                    }
                };
                UnityEditor.EditorApplication.update += update;
#endif
            }
        }

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(duration);
            TriggerComplete();
        }
    }
}
