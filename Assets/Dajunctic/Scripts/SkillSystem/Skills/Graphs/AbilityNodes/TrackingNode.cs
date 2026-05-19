using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class TrackingNode : AbilityNode
    {

        [SerializeReference, Input] public IDamageTaker mainTarget;

        protected override void PlayInternal()
        {
            base.PlayInternal();


            
        }

        void Rotate()
        {
            var target = GetInputValue(nameof(mainTarget), mainTarget);


        
        }


    }
}
