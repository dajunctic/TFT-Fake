using GraphProcessor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Interval")]
    public class IntervalNode : AbilityNode
    {
        public float duration;
        public float interval;
        [SerializeField] bool tickOnStart = false;

        [GraphProcessor.Input] public IActionNode triggerAction;

        protected override void PlayInternal()
        {
            StartCoroutine();
        }

        protected override IEnumerator IECoroutine()
        {
            var inDuration = GetInputValue(nameof(duration), duration);
            var inInterval = GetInputValue(nameof(interval), interval);

            if (tickOnStart)
            {
                TriggerTickNodes();
            }

            float elapsed = 0f;
            while (elapsed < inDuration)
            {
                yield return new WaitForSeconds(inInterval);
                elapsed += inInterval;
                TriggerTickNodes();
            }

            Completed();
        }

        private void TriggerTickNodes()
        {
            ActionNodeSystem
                .CreateActionNodes(GetInputValues(nameof(triggerAction), triggerAction))
                .Play(this);
        }
    }
}
