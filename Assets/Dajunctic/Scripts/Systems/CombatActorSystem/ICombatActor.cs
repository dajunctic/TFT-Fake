using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimator, IDamageOwner, IGameObject
    {
        public string DataId {get;}
        public Team Team{ get; }
        public bool IsTargetable {get; }
        
    }
}