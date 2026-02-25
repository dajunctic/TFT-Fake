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
            _context.actor.PlayAnim(animationName, transitionDuration);

            if (waitForFinish)
            {
                if (Application.isPlaying)
                {
                    _context.actor.StartCoroutine(WaitForAnimation());
                }
                else
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.CallbackFunction update = null;
                    update = () =>
                    {
                        if (_context.actor == null || _context.actor.IsAnimFinished)
                        {
                            UnityEditor.EditorApplication.update -= update;
                            TriggerComplete();
                        }
                    };
                    UnityEditor.EditorApplication.update += update;
#endif
                }
            }
            else
            {
                TriggerComplete();
            }
        }

        private System.Collections.IEnumerator WaitForAnimation()
        {
            while (!_context.actor.IsAnimFinished)
                yield return null;
            TriggerComplete();
        }
    }
}
