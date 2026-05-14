using Dajunctic;

using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ITransform, IAnimatorPlayer, IDamageDealer, IGameObject, IMovable, ITeamMemeber
    {
        public string DataId {get;}

        public float CombatRadius {get; }
        public float RotateSpeed {get;}
        public float Speed {get;}




    }
}
