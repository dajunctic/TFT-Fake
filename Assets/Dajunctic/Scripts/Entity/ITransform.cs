using UnityEngine;

namespace Dajunctic
{
    public interface ITransform: IEntity
    {
        public Vector3 Position {get; }
        public Vector3 Forward { get; }

    }
}