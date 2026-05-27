using UnityEngine;

namespace Dajunctic
{
    public class DraggableUnit : MonoBehaviour, IDraggable
    {
        [Header("Drag Settings")]
        [SerializeField] private float pickUpHeight = 0.5f;
        [SerializeField] private float scaleMultiplier = 1.1f;

        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _originalScale;
        private bool _isDragging = false;
        private float _currentHeight = 0f;
        private Vector3 _moveVelocity; 

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

        public void OnDragStart()
        {
            _isDragging = true;
            _originalPosition = transform.position;
            _targetPosition = _originalPosition;

            transform.localScale = _originalScale * scaleMultiplier;
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            
            _targetPosition = worldPos;
        }

        public void OnDrop(Vector3 finalPos)
        {
            _isDragging = false;
            _targetPosition = finalPos;

            transform.localScale = _originalScale;
        }

        private void Update()
        {
            
            float targetHeight = _isDragging ? pickUpHeight : 0f;
            _currentHeight = Mathf.Lerp(_currentHeight, targetHeight, Time.deltaTime * 10f);

            Vector3 targetPosWithHeight = _targetPosition + Vector3.up * _currentHeight;
            
            if (_isDragging)
            {
                
                Vector3 lastPos = transform.position;
                transform.position = targetPosWithHeight;
                _moveVelocity = Vector3.zero; 

                Vector3 moveDir = (transform.position - lastPos);
                if (moveDir.sqrMagnitude > 0.0001f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(Vector3.up + moveDir * 5f, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
                }
            }
            else
            {
                
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
