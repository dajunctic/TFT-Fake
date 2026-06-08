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
            
            var actor = CombatActor as CombatActor;
            if (actor != null && Target != null)
            {
                float attackRange = actor.CombatActorData != null ? actor.CombatActorData.stats.attackRange : 1.5f;
                float targetRadius = 0.25f;
                if (Target is CombatActor targetActor && targetActor.CombatActorData != null)
                {
                    targetRadius = targetActor.CombatActorData.movement.radius;
                }
                float ownerRadius = actor.CombatActorData != null ? actor.CombatActorData.movement.radius : 0.25f;
                
                float stopDistance = attackRange + ownerRadius + targetRadius;

                while (Target != null && Target.Alive && actor.Hp > 0)
                {
                    var targetPos = Target.AsTransform().Position;
                    float dist = Vector3.Distance(actor.Position, targetPos);
                    
                    if (dist <= stopDistance)
                    {
                        break;
                    }

                    actor.MovePosition(targetPos, actor.Speed, actor.RotateSpeed, stopDistance);
                    await Task.Yield();
                }

                actor.ForceStop();

                if (Target != null && Target.Alive && actor.Hp > 0 && skillGraph != null)
                {
                    var instanceGraph = ScriptableObject.Instantiate(skillGraph) as SkillGraph;
                    
                    instanceGraph.Initialize();
                    instanceGraph.SetOwner(CombatActor as ISkillOwner);

                    bool isFinished = false;
                    instanceGraph.OnExitEvent += () => isFinished = true;

                    instanceGraph.Play(null);

                    while (!isFinished)
                    {
                        if (actor == null || actor.Hp <= 0)
                        {
                            break;
                        }
                        await Task.Yield();
                    }
                }
            }

            TriggerComplete();

            if (CombatActor is CombatActor combatActor)
            {
                combatActor.SetCasting(false);

                // Check if this was an ability (triggered by full mana) or auto-attack
                if (Condition is FullManaGambitCondition)
                {
                    // Ability cast complete — reset mana to starting mana
                    combatActor.ResetMana();
                }
                else
                {
                    // Auto-attack complete — attacker gains mana (TFT mechanic)
                    combatActor.GainMana(combatActor.Stats?.ManaPerAttack?.Value ?? 10f);
                }
            }
        }
    }
}
