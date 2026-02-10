using UnityEngine;

namespace Dajunctic
{
    public interface IDragTarget
    {
        bool TryGetSnapPosition(Vector3 worldPos, out Vector3 snappedPos);
        void OnDragStart();
        void OnDragEnd();
    }

    public interface IDraggable
    {
        void OnDragStart();
        void OnDragUpdate(Vector3 worldPos);
        void OnDrop(Vector3 finalPos);
        void ResetPosition();
        Transform GetTransform();
    }
}
