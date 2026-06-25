using GraphProcessor;
using System.Collections;
using UnityEngine;
using Dajunctic;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/FlyFollow")]
    public class FlyFollowActionNode : ActionNode
    {
        [SerializeField] public float speed = 10f;
        [SerializeField] public float stoppingDistance = 0.1f;
        [SerializeField] public AnchorType anchorType = AnchorType.MidPoint;

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
            var transform = missileView.transform;
            var target = _context.Target;

            Vector3 targetPos = target != null ? target.AsCombatActor().GetAnchorPosition(anchorType) : _context.Destination;

            while (target != null && target.AsTransform().CachedTransform != null)
            {
                targetPos = target.AsCombatActor().GetAnchorPosition(anchorType);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * DeltaTime);
                transform.LookAt(targetPos);

                if (Vector3.Distance(transform.position, targetPos) < stoppingDistance)
                {
                    break;
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
