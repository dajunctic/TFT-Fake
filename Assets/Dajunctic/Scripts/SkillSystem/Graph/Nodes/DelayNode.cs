using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class DelayNode : SkillNode
    {
        public float duration = 1f;

        public override void Execute()
        {
            if (Application.isPlaying)
            {
                // Runtime: chạy qua MonoBehaviour coroutine của SkillGraphRunner
                var runner = _context.actor.AsCombatActor()?.GetSkillGraphRunner();
                if (runner != null)
                {
                    runner.StartCoroutine(WaitCoroutine());
                    return;
                }
            }

#if UNITY_EDITOR
            // Editor Preview: dùng EditorApplication.update
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

        private IEnumerator WaitCoroutine()
        {
            yield return new WaitForSeconds(duration);
            TriggerComplete();
        }
    }
}
