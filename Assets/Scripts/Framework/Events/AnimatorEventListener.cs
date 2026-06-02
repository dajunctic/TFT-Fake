using System;
using UnityEngine;

namespace Dajunctic
{
    public enum RootMotionMode
    {
        Disable = 0,         // Discard root motion (prevents drifting entirely)
        ApplyToParent = 1,   // Forward root motion deltaPosition and deltaRotation to the parent CombatActor
        ApplyLocally = 2     // Allow root motion to affect the local model transform (default behavior)
    }

    public class AnimatorEventListener : MonoBehaviour
    {
        [Header("Root Motion Configuration")]
        [SerializeField] private RootMotionMode rootMotionMode = RootMotionMode.Disable;
        [SerializeField] private bool autoResetTransform = true;

        private CombatActor _actor;
        private Animator _animator;

        private Vector3 _initialLocalPosition;
        private Quaternion _initialLocalRotation;

        // Custom event for external systems to hook into root motion movement
        public event Action<Vector3, Quaternion> OnAnimatorMoveEvent;

        private void Awake()
        {
            _actor = GetComponentInParent<CombatActor>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _initialLocalPosition = transform.localPosition;
            _initialLocalRotation = transform.localRotation;
        }

        private void OnAnimatorMove()
        {
            if (_animator == null) return;

            // Trigger external listeners
            OnAnimatorMoveEvent?.Invoke(_animator.deltaPosition, _animator.deltaRotation);

            switch (rootMotionMode)
            {
                case RootMotionMode.Disable:
                    // Implementing OnAnimatorMove stops Unity from applying root motion automatically.
                    // We reset the local transform to its initial offset to prevent any floating-point drift.
                    if (autoResetTransform)
                    {
                        transform.localPosition = _initialLocalPosition;
                        transform.localRotation = _initialLocalRotation;
                    }
                    break;

                case RootMotionMode.ApplyToParent:
                    Vector3 deltaPos = _animator.deltaPosition;
                    Quaternion deltaRot = _animator.deltaRotation;

                    if (_actor != null)
                    {
                        // Apply movement to the parent
                        if (_actor.MoveAgent != null && _actor.MoveAgent.IsEnabled)
                        {
                            // Move parent using the MoveAgent's physics-safe or navmesh-safe method
                            _actor.MoveAgent.MoveAmount(deltaPos);
                            
                            // Rotate parent safely
                            Vector3 newForward = deltaRot * _actor.transform.forward;
                            _actor.RotateDirection(newForward, 360f, Time.deltaTime, true);
                        }
                        else
                        {
                            // Fallback to direct transform modification if no active move agent
                            _actor.transform.position += deltaPos;
                            _actor.transform.rotation *= deltaRot;
                        }
                    }

                    // Keep visual model locked to parent position to avoid visual drift
                    if (autoResetTransform)
                    {
                        transform.localPosition = _initialLocalPosition;
                        transform.localRotation = _initialLocalRotation;
                    }
                    break;

                case RootMotionMode.ApplyLocally:
                    // Manually apply root motion to the local transform
                    transform.localPosition += transform.parent.InverseTransformDirection(_animator.deltaPosition);
                    transform.localRotation *= _animator.deltaRotation;
                    break;
            }
        }

        public void OnAnimTrigger(string param)
        {
        }
    }
}

