using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class ComboSkillNode : BaseSkillNode
    {
        public ComboSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

        protected override IEnumerator ExecuteRoutine(CombatActor target)
        {
            combatActor.ResetAnim();

            SkillData data = _runtimeSkill.Data;
            
            SkillStep currentStep = _runtimeSkill.GetCurrentStep();
            
            int idx = _runtimeSkill.CurrentComboIndex;
            
            float speedMultiplier = 1f;
            if (_slot == SkillSlot.BasicAttack && data.IsCombo)
            {
                float baseDuration = data.GetFrameTime(data.skillSteps[idx].totalFrames);
                float baseCooldown = 1f / combatActor.AtkSpd;

                if (baseCooldown < baseDuration && baseCooldown > 0)
                {
                    speedMultiplier = baseDuration / baseCooldown;
                }
            }
            
            float frameTime = (1f / data.frameRate) / speedMultiplier;
            float endTime = currentStep.totalFrames * frameTime;
            float cancelTime = currentStep.cancelFrame * frameTime;
            float transitionSeconds = currentStep.transitionFrames * frameTime;
            
            _runtimeSkill.CheckComboTimeout();

            combatActor.SetAnimSpeed(speedMultiplier);

            combatActor.ForceStop();

            _runtimeSkill.Use();

            if (_isChaining)
            {
                combatActor.PlayAnim(currentStep.animTrigger, transitionSeconds);
            }
            else
            {
                combatActor.PlayAnim(currentStep.animTrigger);
            }

            bool[] eventFiredFlags = null;
            if (currentStep.events != null)
                eventFiredFlags = new bool[currentStep.events.Length];
            
            float timer = 0f;
            
            while (timer < endTime)
            {
                timer += Time.deltaTime;
                
                if (target != null && !_hasImpacted)
                {
                    combatActor.RotatePosition(target.Position, combatActor.RotateSpeed, Time.deltaTime, false);
                }
                
                if (currentStep.events != null)
                {
                    for (int i = 0; i < currentStep.events.Length; i++)
                    {
                        if (!eventFiredFlags[i])
                        {
                            float evtTime = currentStep.events[i].frame * frameTime;

                            if (timer >= evtTime)
                            {
                                Transform fp = combatActor.GetFirePoint(currentStep.events[i].firePointName);

                                yield return OnSkillExecute(target, data, currentStep, currentStep.events[i], fp);
                                
                                eventFiredFlags[i] = true;

                                if (!_hasImpacted)
                                {
                                    _hasImpacted = true;
                                    _runtimeSkill.AdvanceCombo();
                                }
                            }
                        }
                    }
                }

                if (timer >= cancelTime)
                {
                    if (combatActor.HasValidTarget())
                    {
                        ChainToNextAttack();
                        yield break;
                    }
                }
                yield return null;
            }
            FinishNode();
        }
        
        void ChainToNextAttack()
        {
            _isExecuting = false;
            _executionFinished = true; 
        }
    }
}