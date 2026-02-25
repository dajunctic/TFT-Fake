using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimatorOwner, IDamageOwner
    {
        public string DataId {get;}

        
    }
}