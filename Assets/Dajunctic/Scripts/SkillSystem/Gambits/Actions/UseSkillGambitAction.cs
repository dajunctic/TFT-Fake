using IDamageTaker = Dajunctic.IDamageTaker;
using System;
using UnityEngine;
using Dajunctic.SkillSystem.Logic;
using Dajunctic;
using System.Threading.Tasks;

namespace Dajunctic.SkillSystem.Gambits
{
    [Serializable]
    public class UseSkillGambitAction : BaseGambitAction
    {
        [SerializeField]
        public SkillGraph skillGraph;

        public override IGambitAction CreateCopy()
        {
            var copy = base.CreateCopy() as UseSkillGambitAction;
            copy.skillGraph = skillGraph;
            return copy;
        }

        protected override bool CheckCanPlayInternal()
        {
            var actor = CombatActor as CombatActor;
            return skillGraph != null && actor != null && !actor.IsCasting;
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
                var instanceGraph = skillGraph.Copy() as SkillGraph;
                instanceGraph.SetOwner(CombatActor as ISkillOwner);
                instanceGraph.Initialize();
                
                bool isFinished = false;
                instanceGraph.OnExitEvent += () => isFinished = true;

                // We don't pass the target directly because the graph resolves it via Nodes
                instanceGraph.Play(null);

                while (!isFinished)
                {
                    var actor = CombatActor as CombatActor;
                    if (actor == null || actor.Hp <= 0)
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
