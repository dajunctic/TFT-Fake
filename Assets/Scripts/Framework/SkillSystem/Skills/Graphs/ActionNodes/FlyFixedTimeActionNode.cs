using GraphProcessor;
using System.Collections;
using UnityEngine;
using Dajunctic;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/FlyFixedTime")]
    public class FlyFixedTimeActionNode : ActionNode
    {
        [SerializeField] public float duration = 1.0f;
        [SerializeField] public AnchorType anchorType = AnchorType.MidPoint;
        [SerializeField] public AnimationCurve horizontal;
        [SerializeField] public AnimationCurve vertical;

        private MissileContext _context;

        protected override void PlayInternal(object source)
        {
            if (!IsInitialized) return;
            _context = source as MissileContext;
            if (_context != null)
            {
                StartCoroutine();
            }
            else
            {
                TriggerDespawn();
            }
        }

        private float DeltaTime => Application.isPlaying ? Time.deltaTime : 0.02f;

        protected override IEnumerator IECoroutine()
        {
            if (_context == null || _context.MissileView == null)
            {
                TriggerDespawn();
                yield break;
            }

            var missileView = _context.MissileView;
            var target = _context.Target;
            var destination = _context.Destination;

            var transform = missileView.transform;

            Vector3 targetPos = target != null ? target.AsCombatActor().GetAnchorPosition(anchorType) : destination;
            float time = 0;
            Vector3 startPos = transform.position;

            while (time < duration)
            {
                time += DeltaTime;
                float pct = time / duration;
                Vector3 currentLinearPos = Vector3.Lerp(startPos, targetPos, pct);

                float hOffset = (horizontal != null && horizontal.length > 0) ? horizontal.Evaluate(pct) : 0;
                float vOffset = (vertical != null && vertical.length > 0) ? vertical.Evaluate(pct) : 0;

                Vector3 offset = transform.up * vOffset + transform.right * hOffset;
                transform.position = currentLinearPos + offset;

                if (pct < 1.0f)
                {
                    transform.LookAt(currentLinearPos + offset);
                }
                yield return null;
            }

            // Hit Target!
            _context.ShootNode.OnMissileHit(_context);

            // Destroy MissileView
            if (missileView != null)
            {
                if (Application.isPlaying)
                    GameObject.Destroy(missileView.gameObject);
                else
                    GameObject.DestroyImmediate(missileView.gameObject);
            }

            TriggerDespawn();
        }

        protected override void StopInternal()
        {
            _context = null;
            base.StopInternal();
        }
    }
}
