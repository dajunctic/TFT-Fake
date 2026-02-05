using System.Collections;
using Dajunctic;
using UnityEngine;

public class ProjectileSkillNode : BaseSkillNode
{
    public ProjectileSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

    protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
    {
        SkillExecutor.ExecuteProjectile(combatActor, target, data, step, evt, firepoint);
        yield break;
    }
}

public class ProjectileComboSkillNode : ComboSkillNode
{
    public ProjectileComboSkillNode(CombatActor actor, SkillSlot slot) : base(actor, slot) { }

    protected override IEnumerator OnSkillExecute(CombatActor target, SkillData data, SkillStep step, SkillEvent evt, Transform firepoint)
    {
        SkillExecutor.ExecuteProjectile(combatActor, target, data, step, evt, firepoint);
        yield break;
    }
}