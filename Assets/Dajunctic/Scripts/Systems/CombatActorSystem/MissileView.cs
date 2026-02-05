using System;
using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class MissileView : MonoBehaviour
    {
        [SerializeField] public MissileType missileType;
        [SerializeField] public float radius = 0.1f;
        [SerializeField] public float speed = 10f;
        [SerializeField] AnchorType anchorType;
        [SerializeField] LayerMask targetLayer;
        [SerializeField] public AnimationCurve horizontal;
        [SerializeField] public AnimationCurve vertical;
        [SerializeField] public float duration = -1;
        [SerializeField] public float stoppingDistance = 0.1f;

        private Vector3 launcher;
        private Vector3 destination;
        private CombatActor targetActor;
        private CombatActor combatActor;
        private CombineDamage combineDamage;

        private Vector3 targetPos;
        public event Action<CombatActor> OnHitTargetEvent;


        public void InitData(MissileData missileData)
        {
            launcher = missileData.launcher;
            destination = missileData.destination;
            targetActor = missileData.targetActor;
            transform.position = launcher;
            combatActor = missileData.combatActor;
            combineDamage = missileData.combineDamage;
        }

        public void StartFly()
        {
            StopAllCoroutines();

            switch (missileType)
            {
                case MissileType.Follow:
                    StartCoroutine(IEFlyFollow());
                    break;
                case MissileType.Straight:
                    StartCoroutine(IEFlyStraight());
                    break;
                case MissileType.FixedTime:
                    StartCoroutine(IEFlyFixedTime());
                    break;
            }
        }

        public void UpdateTargetPos()
        {
            if (targetActor != null)
            {
                targetPos = targetActor.GetAnchorPosition(anchorType);
            }
            else
            {
                targetPos = destination;
            }
        }

        public IEnumerator IEFlyFollow()
        {
            if (combatActor == null) yield break;
            while (targetActor != null && targetActor.CachedTransform != null)
            {
                UpdateTargetPos();
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                
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
            if (combatActor != null) {
                UpdateTargetPos();
                targetPos = transform.position + combatActor.Forward;
                Vector3 direction = (targetPos - launcher).normalized;
                while (Vector3.Distance(transform.position, targetPos) > stoppingDistance)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                    
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
            if (combatActor != null)
            {
                float time = 0;
                Vector3 startPos = transform.position;
                UpdateTargetPos();

                while (time < duration)
                {
                    time += Time.deltaTime;
                    float pct = time / duration; 

                    Vector3 currentLinearPos = Vector3.Lerp(startPos, targetPos, pct);

                    float hOffset = horizontal.length > 0 ? horizontal.Evaluate(pct) : 0;
                    float vOffset = vertical.length > 0 ? vertical.Evaluate(pct) : 0;
                    
                    Vector3 offset = transform.up * vOffset + transform.right * hOffset;
                    
                    transform.position = currentLinearPos + offset;
                    
                    if (pct < 1.0f) {
                        transform.LookAt(currentLinearPos + offset); 
                    }

                    yield return null;
                }
            }
            OnHitTarget();
        }

        private void OnHitTarget()
        {
            targetActor.TakeDamage(combineDamage);
            OnHitTargetEvent?.Invoke(targetActor);

            Destroy(gameObject);
        }
    }

    public class MissileData

    {

        public Vector3 launcher;

        public Vector3 destination;  

        public CombatActor targetActor;
        public CombatActor combatActor;
        public CombineDamage combineDamage;

    }

    public enum MissileType

    {

        Follow,

        Straight,

        FixedTime

    }
}