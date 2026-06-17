using GraphProcessor;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
            var inDuration = duration;
            var inInterval = interval;

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

            Debug.LogError("19001508");
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
