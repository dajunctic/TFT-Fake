using UnityEngine;

namespace Dajunctic
{
    public class DraggableUnit : MonoBehaviour, IDraggable
    {
        [Header("Drag Settings")]
        [SerializeField] private float smoothTime = 0.08f;
        [SerializeField] private float pickUpHeight = 0.5f;
        [SerializeField] private float scaleMultiplier = 1.1f;

        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _originalScale;
        private bool _isDragging = false;
        private float _currentHeight = 0f;
        private Vector3 _moveVelocity; // Required for SmoothDamp

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnDragStart()
        {
            _isDragging = true;
            _originalPosition = transform.position;
            _targetPosition = _originalPosition;
            
            // Visual feedback: Scale up slightly
            transform.localScale = _originalScale * scaleMultiplier;
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            // The DragManager passes either the mouse-on-ground or the hex-snapped position
            _targetPosition = worldPos;
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;
            _targetPosition = finalPos;
            
            // Revert visual changes
            transform.localScale = _originalScale;
        }

        private void Update()
        {
            // Calculate height offset: smooth transition for pickup/drop
            float targetHeight = _isDragging ? pickUpHeight : 0f;
            _currentHeight = Mathf.Lerp(_currentHeight, targetHeight, Time.deltaTime * 10f);

            // Smoothly move towards target position
            Vector3 targetPosWithHeight = _targetPosition + Vector3.up * _currentHeight;
            
            if (_isDragging)
            {
                // Instant follow for that "sticky" mouse feel
                Vector3 lastPos = transform.position;
                transform.position = targetPosWithHeight;
                _moveVelocity = Vector3.zero; // Reset velocity
                
                // Tilt the unit towards movement direction (optional, but keep it for flavor)
                Vector3 moveDir = (transform.position - lastPos);
                if (moveDir.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(Vector3.up + moveDir * 5f, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
                }
            }
            else
            {
                // When dropped, snap/slide to final position and reset rotation
                transform.position = Vector3.MoveTowards(transform.position, targetPosWithHeight, Time.deltaTime * 20f);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 10f);
            }
        }

        public Transform GetTransform() => transform;

        public void ResetPosition()
        {
            _isDragging = false;
            transform.position = _originalPosition;
            _targetPosition = _originalPosition;
            transform.localScale = _originalScale;
        }
    }
}
