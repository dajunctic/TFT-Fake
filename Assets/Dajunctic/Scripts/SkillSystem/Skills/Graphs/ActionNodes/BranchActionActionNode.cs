using GraphProcessor;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable]
    public abstract class BranchActionActionNode : ActionNode
    {
        [GraphProcessor.Input(name = "trueAction")] public IActionNode trueAction;
        [GraphProcessor.Input(name = "falseAction")] public IActionNode falseAction;

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
