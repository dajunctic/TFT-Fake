using GraphProcessor;
using System.Collections;
using UnityEngine;
using Dajunctic;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Action/FlyStraight")]
    public class FlyStraightActionNode : ActionNode
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
            var target = _context.Target;
            var destination = _context.Destination;

            var transform = missileView.transform;
            var launcher = _context.Launcher;

            Vector3 targetPos = target != null ? target.AsCombatActor().GetAnchorPosition(anchorType) : destination;
            var damageDealer = _context.ShootNode.Owner.AsDamageDealer();
            if (damageDealer != null)
            {
                targetPos = transform.position + damageDealer.AsTransform().Forward;
                Vector3 direction = (targetPos - launcher).normalized;
                while (Vector3.Distance(transform.position, targetPos) > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * DeltaTime);
                    if (direction != Vector3.zero)
                        transform.rotation = Quaternion.LookRotation(direction);
                    yield return null;
                }
                transform.position = targetPos;
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
