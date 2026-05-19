using System;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;
using System.Threading.Tasks;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class UseSkillGambitAction : BaseGambitAction
    {
        [SerializeField]
        public AbilityGraph skillGraph;

        public override IGambitAction CreateCopy()
        {
            var copy = base.CreateCopy() as UseSkillGambitAction;
            copy.skillGraph = skillGraph;
            return copy;
        }

        protected override bool CheckCanPlayInternal()
        {
            return skillGraph != null && CombatActor.CanAction;
        }

        protected override void PlayInternal()
        {
            ExecuteSkillSequence();
        }

        protected override void StopInternal()
        {
            if (CombatActor is CombatActor actor)
            {
                actor.InterruptAction();
            }
        }

        private async void ExecuteSkillSequence()
        {
            IsCanNotBeInterrupted = true;
            
            // In a real game, you would yield return IEMoveToTarget(range) here.
            // For now, we simulate reaching the target and casting.

            if (skillGraph != null)
            {
                // Clone the graph to prevent shared state issues
                var instanceGraph = skillGraph.Copy() as AbilityGraph;
                instanceGraph.Initialize(CombatActor.AsSkillOwner());
                
                // Set the main target before playing if the graph supports it
                // We pass the target to Play
                instanceGraph.Play(Target);

                while (instanceGraph.IsPlaying)
                {
                    if (CombatActor == null || !CombatActor.Alive)
                    {
                        break;
                    }
                    await Task.Yield();
                }
            }

            TriggerComplete();
        }
    }
}
