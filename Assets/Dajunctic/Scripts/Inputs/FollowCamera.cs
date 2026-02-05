using Dajunctic;
using UnityEngine;

namespace Dajunctic
{

    public class FollowCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Settings")]
        public Vector3 offset = new Vector3(0, 15, -10);

        public float smoothTime = 0.3f;

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            if (target == null)
            {
                if (HeroCombatActor.Leader != null)
                {
                    target = HeroCombatActor.Leader.transform;
                }
                else
                {
                    return;
                }
            }

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            //transform.LookAt(target);
        }
    }
}