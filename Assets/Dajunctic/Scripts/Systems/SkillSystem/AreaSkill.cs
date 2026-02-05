using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Dajunctic
{
    public class AreaSkill : BaseSkillEffect
    {
        [Header("Settings")] 
        [SerializeField] private float attackRadius = 3f;
        [SerializeField] private float tickInterval = 0.5f;
        [SerializeField] private bool followTarget = false;
        [SerializeField] private float followSpeed = 10f;

        private CombatActor _owner;
        private CombineDamage _damageSnapshot;
        private LayerMask _targetLayer;
        private float _duration;
        private PoolableObject _hitPrefab;

        public void Initialize(CombatActor owner, CombineDamage combineDamage, LayerMask hitLayer, float duration,
            PoolableObject hitPrefab)
        {
            _owner = owner;
            _damageSnapshot = combineDamage;
            _targetLayer = hitLayer;
            _duration = duration;
            _hitPrefab = hitPrefab;
            ResetVisuals();
            StartCoroutine(LifeCycleRoutine());
        }

        private IEnumerator LifeCycleRoutine()
        {
            float elapsedTime = 0f;
            float nextTickTime = 0f;

            while (elapsedTime < _duration)
            {
                if (followTarget && _owner != null)
                {
                    var currentTarget = _owner.CurrentTarget;

                    if (currentTarget != null && currentTarget.gameObject.activeInHierarchy)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, currentTarget.Position,
                            followSpeed * Time.deltaTime);
                    }
                }

                if (elapsedTime >= nextTickTime)
                {
                    HitTarget();
                    nextTickTime += tickInterval;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            StartCoroutine(DespawnCoroutine());
        }

        private void HitTarget()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, attackRadius, _targetLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<CombatActor>(out var damageable))
                {
                    damageable.TakeDamage(_damageSnapshot);
                }
            }
            SpawnHitVFX(_hitPrefab);
        }
    }
}