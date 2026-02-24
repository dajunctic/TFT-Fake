using UnityEngine;
using System;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class AnimationNode : SkillNode
    {
        public string animationName;
        public float transitionDuration = 0.1f;
        public bool waitForFinish = true;

        public override void Execute(SkillExecutionContext context, Action onComplete)
        {
            context.actor.PlayAnim(animationName, transitionDuration);
            if (waitForFinish)
            {
                if (Application.isPlaying)
                {
                    context.actor.StartCoroutine(WaitForAnimation(context.actor, onComplete));
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.CallbackFunction update = null;
                    update = () =>
                    {
                        if (context.actor == null || context.actor.IsAnimFinished)
                        {
                            UnityEditor.EditorApplication.update -= update;
                            onComplete?.Invoke();
                        }
                    };
                    UnityEditor.EditorApplication.update += update;
#endif
                }
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        private System.Collections.IEnumerator WaitForAnimation(CombatActor actor, Action onComplete)
        {
            while (!actor.IsAnimFinished)
            {
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
