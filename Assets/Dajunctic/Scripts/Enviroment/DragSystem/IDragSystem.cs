using UnityEngine;

namespace Dajunctic
{
    public interface IDragTarget
    {
        bool TryGetSnapPosition(Vector3 worldPos, out Vector3 snappedPos);
    }

    public interface IDraggable
    {
        void OnDragStart();
        void OnDragUpdate(Vector3 worldPos);
        void OnDrop(Vector3 finalPos);
        Transform GetTransform();
    }
}
