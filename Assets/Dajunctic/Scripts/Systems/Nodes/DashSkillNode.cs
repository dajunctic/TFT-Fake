using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class DashSkillNode : BaseSkillNode
    {
        public DashSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

        protected override IEnumerator ExecuteRoutine(CombatActor target)
        {
            combatActor.ResetAnim();
            SkillStep step = _runtimeSkill.GetCurrentStep();
            SkillData data = _runtimeSkill.Data;

            float frameTime = 1f / data.frameRate;
            float totalDuration = step.totalFrames * frameTime;
            float transitionSeconds = step.transitionFrames * frameTime;

            Vector3 startPos = combatActor.Position;
            Vector3 targetPos = target.Position;
            Vector3 dir = (targetPos - startPos).normalized;
            Vector3 endPos = targetPos - dir * 1.5f;

            combatActor.PlayAnim(step.animTrigger, transitionSeconds);
            _runtimeSkill.Use();

            float timer = 0f;
            bool hitExecuted = false;
            
            float impactTime = (step.events != null && step.events.Length > 0) 
                ? step.events[0].frame * frameTime 
                : totalDuration * 0.5f;

            while (timer < totalDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / totalDuration);

                combatActor.Teleport(Vector3.Lerp(startPos, endPos, t), false); 
                
                if (!hitExecuted && timer >= impactTime)
                {
                    SkillEvent evt = (step.events != null && step.events.Length > 0) 
                        ? step.events[0] 
                        : new SkillEvent { damageMultiplier = 1f };
                    
                    SkillExecutor.ExecuteMelee(combatActor, target, data, step, evt, combatActor.transform);
                    hitExecuted = true;
                }

                yield return null;
            }
            
            FinishNode();
        }
    }
}