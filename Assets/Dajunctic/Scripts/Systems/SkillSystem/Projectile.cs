using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public class Projectile : BaseSkillEffect
    {
        [Header("Settings")]
        [SerializeField] private float attackRadius = 0.2f;
        [SerializeField] private float speed = 20f;
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] public float stoppingDistance = 0.1f;
        
        private CombineDamage _combineDamage;
        private CombatActor _target;
        private LayerMask _targetLayer;
        private PoolableObject _hitPrefab;

        public void Initialize(CombineDamage combineDamage, LayerMask hitLayer, CombatActor target, PoolableObject hitPrefab)
        {
            _combineDamage = combineDamage;
            _targetLayer = hitLayer;
            _target = target;
            _hitPrefab = hitPrefab;
            ResetVisuals();
            
            StartCoroutine(IEFlyFollow());
        }

        IEnumerator IEFlyFollow()
        {
            Transform targetTransform = _target != null ? _target.MidPoint.CachedTransform : null;
            while (targetTransform != null)
            {
                Vector3 targetPos = targetTransform.position;
                float step = speed * Time.deltaTime;

                Vector3 direction = (targetPos - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnSpeed * Time.deltaTime); 
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                if (Vector3.Distance(transform.position, targetPos) < stoppingDistance)
                {
                    HitTarget(); 
                    yield break;
                }
                yield return null;
            }
            HitTarget(); 
        }

        void HitTarget()
        {
            if (attackRadius > 0)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, _targetLayer);
                foreach (var hit in hits)
                {
                    if (hit.TryGetComponent<CombatActor>(out var damageable))
                    {
                        damageable.TakeDamage(_combineDamage);
                    }
                }
            }
            else
            {
                _target.TakeDamage(_combineDamage);
            }

            SpawnHitVFX(_hitPrefab);
            StartCoroutine(DespawnCoroutine());
        }
    }
}