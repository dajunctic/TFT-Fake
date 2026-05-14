using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public abstract class BranchActionActionNode : ActionNode
    {
        [SerializeReference, Input] IActionNode trueAction;
        [SerializeReference, Input] IActionNode falseAction;

        public abstract bool IsTrue { get; }
        
        protected override void PlayInternal(object source)
        {
            if (!IsInitialized) return;

            if (IsTrue)
            {
                var inAction = GetInputValues(nameof(trueAction), trueAction);
                ActionNodeSystem.CreateActionNodes(inAction).Play(source);
            }
            else
            {
                var inAction = GetInputValues(nameof(falseAction), falseAction);
                ActionNodeSystem.CreateActionNodes(inAction).Play(source);
            }
            
            TriggerDespawn();
        }
        
    }
}
