using UnityEngine;
using System.Collections;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Tracking")]
    public class TrackingNode : AbilityNode
    {
        [SerializeReference, Input] public IDamageTaker mainTarget;
        public float duration;
        public float rotateSpeed = 10f;

        protected override void PlayInternal()
        {
            StartCoroutine();
        }

        protected override IEnumerator IECoroutine()
        {
            var inDuration = GetInputValue(nameof(duration), duration);
            var inRotateSpeed = GetInputValue(nameof(rotateSpeed), rotateSpeed);

            var movable = Owner.AsMovable();
            if (movable == null)
            {
                Completed();
                yield break;
            }

            float elapsed = 0f;
            while (elapsed < inDuration)
            {
                var target = GetInputValue(nameof(mainTarget), mainTarget);
                if (target != null && target.Alive)
                {
                    Vector3 targetPos = target.MidPoint;
                    movable.RotatePosition(targetPos, inRotateSpeed, Time.deltaTime, false);
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            Completed();
        }
    }
}
