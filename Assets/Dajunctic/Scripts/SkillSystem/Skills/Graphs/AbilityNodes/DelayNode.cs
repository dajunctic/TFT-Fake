using System.Collections;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public class DelayNode : AbilityNode
    {
        [SerializeField, Input] float delay;
        [SerializeField, Input] float atkSpd = 1f;

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
