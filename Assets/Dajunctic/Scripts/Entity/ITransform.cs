using UnityEngine;

namespace Dajunctic
{
    public interface ITransform: IEntity
    {
        public Transform CachedTransform { get; }
        public Vector3 Position {get; }
        public Vector3 Forward { get; }

        public Vector3 GetAnchorPosition(AnchorType anchorType);

    }
}