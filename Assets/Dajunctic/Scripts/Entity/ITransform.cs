using UnityEngine;

namespace Dajunctic
{
    public interface ITransform: IEntity
    {
        public Transform CachedTransform { get; }
        public Vector3 Position {get; }
        public Vector3 Forward { get; }
        public Vector3 TransformPoint(Vector3 point);
        public Vector3 TransformDirection(Vector3 direction);
        public Vector3 GetAnchorPosition(AnchorType anchorType);
        Transform GetTransform();
        Transform GetTransform(object obj);

    }
}