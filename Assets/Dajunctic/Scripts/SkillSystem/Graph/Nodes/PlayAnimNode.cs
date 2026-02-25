using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayAnimNode : SkillNode
    {
        public string animationName;
        public float transitionDuration = 0.1f;
        public bool waitForFinish = true;

        public override void Execute()
        {
            var actor = _context.actor.AsCombatActor();
            if (actor == null)
            {
                TriggerComplete();
                return;
            }

            actor.PlayAnim(animationName, transitionDuration);
            actor.ResetAnim(); // kích hoạt lại IsAnimFinished = false

            if (!waitForFinish)
            {
                TriggerComplete();
                return;
            }

            if (Application.isPlaying)
            {
                // Runtime: chạy qua SkillGraphRunner coroutine
                var runner = actor.GetSkillGraphRunner();
                if (runner != null)
                {
                    runner.StartCoroutine(WaitForAnimation((CombatActor)actor));
                    return;
                }
                TriggerComplete();
            }
#if UNITY_EDITOR
            else
            {
                // Editor Preview: poll qua EditorApplication.update
                UnityEditor.EditorApplication.CallbackFunction update = null;
                update = () =>
                {
                    if (actor == null)
                    {
                        UnityEditor.EditorApplication.update -= update;
                        TriggerComplete();
                        return;
                    }
                    if (actor.IsAnimFinished)
                    {
                        UnityEditor.EditorApplication.update -= update;
                        TriggerComplete();
                    }
                };
                UnityEditor.EditorApplication.update += update;
            }
#endif
        }

        private IEnumerator WaitForAnimation(CombatActor actor)
        {
            yield return null; // Đợi 1 frame để animation bắt đầu
            while (!actor.IsAnimFinished)
            {
                yield return null;
            }
            TriggerComplete();
        }
    }
}
