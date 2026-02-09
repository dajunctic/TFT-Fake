using UnityEngine;

namespace Dajunctic
{
    public class DraggableUnit : MonoBehaviour, IDraggable
    {
        private Vector3 originalPosition;
        private bool isDragging = false;

        public void OnDragStart()
        {
            isDragging = true;
            originalPosition = transform.position;
            // Optionally: Disable physics/navmesh agent during drag
        }

        public void OnDragUpdate(Vector3 worldPos)
        {
            // Update visual position (with some height offset like TFT)
            transform.position = worldPos + Vector3.up * 0.5f;
        }

        public void OnDrop(Vector3 finalPos)
        {
            isDragging = false;
            transform.position = finalPos;
        }

        public Transform GetTransform() => transform;

        public void ResetPosition()
        {
            transform.position = originalPosition;
        }
    }
}
