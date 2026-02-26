using Dajunctic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimatorPlayer, IDamageOwner, IGameObject, IMovable
    {
        public string DataId {get;}
        public Team Team{ get; }
        public bool IsTargetable {get; }


        // 
        public float RotateSpeed {get;}
        public float Speed {get;}

        // SkillGraph
        public SkillGraphRunner GetSkillGraphRunner();


    }
}
