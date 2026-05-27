using Dajunctic;

using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ITransform, IAnimatorPlayer, IDamageDealer, IGameObject, IMovable, ITeamMemeber
    {
        public string DataId {get;}

        public float RotateSpeed {get;}
        public float Speed {get;}
        void SetStaggerReduction(float v);
        void ClearTarget();
        object Stats { get; }

    }
}
