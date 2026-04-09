using Dajunctic;
using Dajunctic.SkillSystem.Graph;
using Dajunctic.SkillSystem.Commands;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimatorPlayer, IDamageDealer, IGameObject, IMovable, ITeamMemeber
    {
        public string DataId {get;}

        public float CombatRadius {get; }
        public float RotateSpeed {get;}
        public float Speed {get;}

        public SkillCommandRunner GetSkillCommandRunner();


    }
}
