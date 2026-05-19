using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    public abstract class BranchActionActionNode : ActionNode
    {
        [SerializeField, Input(ShowBackingValue.Never)] IActionNode trueAction;
        [SerializeField, Input(ShowBackingValue.Never)] IActionNode falseAction;

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
