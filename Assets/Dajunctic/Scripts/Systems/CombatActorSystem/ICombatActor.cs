using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimator, IDamageOwner
    {
        public string DataId {get;}

        
    }
}