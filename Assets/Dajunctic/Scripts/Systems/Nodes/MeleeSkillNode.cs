// Node đơn

using System.Collections;
using Dajunctic;
using UnityEngine;
public class MeleeSkillNode : BaseSkillNode
{
    public MeleeSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

    protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
    {
        SkillExecutor.ExecuteMelee(combatActor, target, data, step, evt, firepoint);
        yield break;
    }
}

public class MeleeComboSkillNode : ComboSkillNode
{
    public MeleeComboSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

    protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
    {
        SkillExecutor.ExecuteMelee(combatActor, target, data, step, evt, firepoint);
        yield break;
    }
}