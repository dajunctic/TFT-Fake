using System;
using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class MissileView : BaseView
    {
        [SerializeField] public MissileType missileType;
        [SerializeField] public float radius = 0.1f;
        [SerializeField] public float speed = 10f;
        [SerializeField] AnchorType anchorType;
        [SerializeField] public AnimationCurve horizontal;
        [SerializeField] public AnimationCurve vertical;
        [SerializeField] public float duration = -1;
        [SerializeField] public float stoppingDistance = 0.1f;

        private Vector3 launcher;
        private Vector3 destination;
        private IDamageTaker damageTaker;
        private IDamageDealer damageDealer;
        private CombineDamage combineDamage;

        private Vector3 targetPos;
        public event Action<IDamageTaker> OnHitEvent;

        public void InitData(MissileData missileData)
        {
            launcher = missileData.launcher;
            destination = missileData.destination;
            damageTaker = missileData.damageTaker;
            transform.position = launcher;
            damageDealer = missileData.damageDealer;
            combineDamage = missileData.combineDamage;
        }

        public void StartFly()
        {
            StopAllCoroutines();

            IEnumerator coroutine = null;
            switch (missileType)
            {
                case MissileType.Follow:
                    coroutine = IEFlyFollow();
                    break;
                case MissileType.Straight:
                    coroutine = IEFlyStraight();
                    break;
                case MissileType.FixedTime:
                    coroutine = IEFlyFixedTime();
                    break;
            }

            Debug.LogError("âsdasdasd");

            if (coroutine == null) return;

            if (Application.isPlaying)
            {
                StartCoroutine(coroutine);
            }
            else
            {
#if UNITY_EDITOR
                
                _editorCoroutine = coroutine;
#endif
            }
        }

#if UNITY_EDITOR
        private IEnumerator _editorCoroutine;
        public IEnumerator GetEditorCoroutine() => _editorCoroutine;
#endif

        public void UpdateTargetPos()
        {
            if (damageTaker != null)
            {
                targetPos = damageTaker.AsCombatActor().GetAnchorPosition(anchorType);
            }
            else
            {
                targetPos = destination;
            }
        }

        public IEnumerator IEFlyFollow()
        {
            if (damageDealer == null) yield break;

            Debug.LogError("Có chạy ở đây");
            while (damageTaker != null && damageTaker.AsTransform().CachedTransform != null)
            {
                UpdateTargetPos();
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * DeltaTime);

                transform.LookAt(targetPos);

                if (Vector3.Distance(transform.position, targetPos) < stoppingDistance)
                {
                    OnHitTarget();
                    yield break;
                }
                yield return null;
            }
            OnHitTarget();
        }

        public IEnumerator IEFlyStraight()
        {
            if (damageDealer != null)
            {
                UpdateTargetPos();
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
            OnHitTarget();
        }

        public IEnumerator IEFlyFixedTime()
        {
            if (damageDealer != null)
            {
                float time = 0;
                Vector3 startPos = transform.position;
                UpdateTargetPos();

                while (time < duration)
                {
                    time += DeltaTime;
                    float pct = time / duration;

                    Vector3 currentLinearPos = Vector3.Lerp(startPos, targetPos, pct);

                    float hOffset = horizontal.length > 0 ? horizontal.Evaluate(pct) : 0;
                    float vOffset = vertical.length > 0 ? vertical.Evaluate(pct) : 0;

                    Vector3 offset = transform.up * vOffset + transform.right * hOffset;

                    transform.position = currentLinearPos + offset;

                    if (pct < 1.0f)
                    {
                        transform.LookAt(currentLinearPos + offset);
                    }

                    yield return null;
                }
            }
            OnHitTarget();
        }

        private void OnHitTarget()
        {

            Debug.LogError("May co van de gi khong");
            if (Application.isPlaying) damageTaker.TakeDamage(combineDamage);
            
            OnHitEvent?.Invoke(damageTaker);

            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
           
        }
    }

    public class MissileData
    {
        public string id;
        public Vector3 launcher;

        public Vector3 destination;

        public IDamageTaker damageTaker;
        public IDamageDealer damageDealer;
        public CombineDamage combineDamage;
    }

    public enum MissileType

    {

        Follow,

        Straight,

        FixedTime

    }
}
