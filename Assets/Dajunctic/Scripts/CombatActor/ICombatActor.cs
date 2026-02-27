using Dajunctic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimatorPlayer, IDamageDealer, IGameObject, IMovable, ITeamMemeber
    {
        public string DataId {get;}

        public float CombatRadius {get; }
        public float RotateSpeed {get;}
        public float Speed {get;}

        public SkillGraphRunner GetSkillGraphRunner();


    }
}
