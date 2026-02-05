using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IEntity, IDamageTaker
    {
        public string DataId {get;}
        public Vector3 Position { get; }
        public Vector3 Forward { get; }
    }
}