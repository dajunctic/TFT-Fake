using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class TimerActionNode : ActionNode
    {
        [SerializeField, Input] float duration;
        [SerializeField] float interval = -1;
        [SerializeReference, Input] IActionNode beginAction;
        [SerializeReference, Input] IActionNode intervalAction;
        [SerializeReference, Input] IActionNode endAction;

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
            
            // inAction = GetInputValues(nameof(endAction), endAction);
            // ActionNodeSystem.CreateActionNodes(inAction).Play(this);
            
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
