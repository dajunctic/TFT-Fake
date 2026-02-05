using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class BaseSkillNode : Node
    {
        protected SkillSlot _slot;
        protected RuntimeSkill _runtimeSkill;
        protected bool _isExecuting;
        protected bool _executionFinished;
        protected int _capturedSessionId;
        protected bool _isChaining;
        protected bool _hasImpacted;
        
        public BaseSkillNode(CombatActor actor, SkillSlot slot) : base(actor)
        {
            _slot = slot;
            _runtimeSkill = combatActor.GetSkill(_slot);
        }

        public override NodeState Evaluate()
        {
            if (_isExecuting)
            {
                if (_executionFinished)
                {
                    _isExecuting = false;
                    _executionFinished = false;
                    return NodeState.Success;
                }
                
                if (combatActor.ActionSessionId != _capturedSessionId || !combatActor.IsCasting)
                {
                    Abort();
                    return NodeState.Failure;
                }

                return ReturnRunning();
            }

            if (_runtimeSkill == null) return NodeState.Failure;
            
            if (!_runtimeSkill.IsReady) return NodeState.Failure;

            if (!combatActor.HasValidTarget()) return NodeState.Failure;

            var target = combatActor.CurrentTarget;
            float castRange = _runtimeSkill.Data.castRange;
            float distSqr = (target.Position - combatActor.Position).sqrMagnitude;
    
            if (distSqr > castRange * castRange)
            {
                combatActor.MovePosition(target.Position, combatActor.Speed, combatActor.RotateSpeed);
                return NodeState.Running;
            }
            
            combatActor.ForceStop();
            _isChaining = combatActor.IsCasting && combatActor.CurrentActiveSlot == _slot;
            _isExecuting = true;
            _executionFinished = false;
            _capturedSessionId = combatActor.ActionSessionId;
            _hasImpacted = false;
            combatActor.SetCasting(true, _slot);
            combatActor.StartCoroutine(ExecuteRoutine(target));
            return ReturnRunning();
        }

        protected virtual IEnumerator ExecuteRoutine(CombatActor target)
        {
            combatActor.ResetAnim();
            
            SkillData data = _runtimeSkill.Data;
            
            combatActor.ForceStop();

            _runtimeSkill.Use();
            SkillStep currentStep = _runtimeSkill.GetCurrentStep();

            float frameTime = (1f / data.frameRate);
            float endTime = currentStep.totalFrames * frameTime;

            _runtimeSkill.Use();

            combatActor.PlayAnim(currentStep.animTrigger);

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

                                _hasImpacted = true;
                            }
                        }
                    }
                }
                yield return null;
            }
            FinishNode();
        }

        protected virtual IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
        {
            yield break;
        }
        
        private void Abort()
        {
            _isExecuting = false;
            _executionFinished = false;
            if (!_hasImpacted) _runtimeSkill?.ResetCooldown();
        }
        protected void FinishNode()
        {
            combatActor.SetAnimSpeed(1f);
            combatActor.SetCasting(false, _slot);
            _isExecuting = false;
            _executionFinished = true;
        }
    }
}