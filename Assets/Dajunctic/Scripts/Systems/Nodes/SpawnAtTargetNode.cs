using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class SpawnAtTargetNode : BaseSkillNode
    {
        public SpawnAtTargetNode(CombatActor actor, SkillSlot slot) : base(actor, slot)
        {
        }

        protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
        {
            SkillExecutor.ExecuteSpawnAtTarget(combatActor, target, data, step);
            yield break;
        }
    }
}