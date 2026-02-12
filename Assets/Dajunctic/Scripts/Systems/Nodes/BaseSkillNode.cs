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
            _isExecuting = true;
            _executionFinished = false;
            _capturedSessionId = combatActor.ActionSessionId;
            combatActor.SetCasting(true, _slot);
            combatActor.StartCoroutine(ExecuteRoutine(target));
            return ReturnRunning();
        }

        protected virtual IEnumerator ExecuteRoutine(CombatActor target)
        {
            SkillData data = _runtimeSkill.Data;
            _runtimeSkill.Use();

            // Rotate towards target immediately
            if (target != null)
            {
                Vector3 direction = (target.Position - combatActor.Position).normalized;
                direction.y = 0;
                if (direction != Vector3.zero)
                {
                    combatActor.RotateRotation(direction, combatActor.RotateSpeed, 1f, true);
                }
            }

            // Play Timeline/Skill
            if (!string.IsNullOrEmpty(data.timelineId))
            {
                combatActor.Raise(new PlayTimelineEvent { Id = data.timelineId });
            }

            if (data.duration > 0)
            {
                yield return new WaitForSeconds(data.duration);
            }
            else
            {
                yield return null;
            }

            FinishNode();
        }

        private void Abort()
        {
            _isExecuting = false;
            _executionFinished = false;
        }

        protected void FinishNode()
        {
            combatActor.SetCasting(false, _slot);
            _isExecuting = false;
            _executionFinished = true;
        }
    }
}
