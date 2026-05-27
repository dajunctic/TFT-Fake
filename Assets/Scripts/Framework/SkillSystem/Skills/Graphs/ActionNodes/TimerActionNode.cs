using GraphProcessor;
using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/TimerAction")]
    public class TimerActionNode : ActionNode
    {
        [GraphProcessor.Input(name = "duration")] public float duration;
        [SerializeField] float interval = -1;
        [SerializeReference, GraphProcessor.Input(name = "beginAction")] IActionNode beginAction;
        [SerializeReference, GraphProcessor.Input(name = "intervalAction")] IActionNode intervalAction;
        [SerializeReference, GraphProcessor.Input(name = "endAction")] IActionNode endAction;

        protected override void PlayInternal(object source)
        {
            if (!IsInitialized) return;
            
            StartCoroutine();
        }

        protected override IEnumerator IECoroutine()
        {
            var inDuration = GetInputValue(nameof(duration), duration);

            var inAction = GetInputValues(nameof(beginAction), beginAction);
            ActionNodeSystem.CreateActionNodes(inAction).Play(this);

            if (interval > 0)
            {
                inAction = GetInputValues(nameof(intervalAction), intervalAction);
                var tick = Mathf.RoundToInt(inDuration / interval);
                for (var i = 0; i < tick; i++)
                {
                    yield return new WaitForSeconds(interval);
                    ActionNodeSystem.CreateActionNodes(inAction).Play(this);
                }
            }
            else
            {
                yield return new WaitForSeconds(inDuration);
            }

            TriggerDespawn();
        }

        protected override void StopInternal()
        {
            var inAction = GetInputValues(nameof(endAction), endAction);
            ActionNodeSystem.CreateActionNodes(inAction).Play(this);
            base.StopInternal();
        }
    }
}
