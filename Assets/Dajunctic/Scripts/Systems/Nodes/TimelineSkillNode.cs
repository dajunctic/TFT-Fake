using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace Dajunctic
{
    public class TimelineSkillNode : BaseSkillNode
    {
        public TimelineSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

        protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
        {
            if (string.IsNullOrEmpty(data.timelineId))
            {
                Debug.LogError($"TimelineId is empty in skill {data.name}");
                yield break;
            }

            combatActor.timelineSkill.Clear();
            // if (target != null)
            // {
            //     combatActor.timelineSkill.SkillTargets.Add(target);
            // }

            combatActor.Raise(new PlayTimelineEvent { Id = data.timelineId });

            if (data.duration > 0)
            {
                yield return new WaitForSeconds(data.duration);
            }
            else
            {
                yield return null;
            }
        }
    }
}
