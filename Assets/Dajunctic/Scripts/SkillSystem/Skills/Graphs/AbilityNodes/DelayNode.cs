using GraphProcessor;
using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Delay")]
    public class DelayNode : AbilityNode
    {
        [GraphProcessor.Input(name = "delay")] public float delay;
        [GraphProcessor.Input] float atkSpd = 1f;

        protected override void PlayInternal()
        {
            StartCoroutine();
        }

        protected override IEnumerator IECoroutine()
        {
            var inDelay = GetInputValue(nameof(delay), delay);
            var inAtkSpd = GetInputValue(nameof(atkSpd), atkSpd);
            yield return new WaitForSeconds(inDelay / inAtkSpd);
            Completed();
        }
    }
}
