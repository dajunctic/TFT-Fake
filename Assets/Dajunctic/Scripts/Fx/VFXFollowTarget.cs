using UnityEngine;
using UnityEngine.Serialization;

namespace Dajunctic
{
    public class VFXFollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private bool smoothFollow = true;
        [SerializeField] private float smoothSpeed = 5f;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            Vector3 targetPos = _target.position;
            
            if (smoothFollow)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
            }
            else
            {
                transform.position = targetPos;
            }
        }
    }
}