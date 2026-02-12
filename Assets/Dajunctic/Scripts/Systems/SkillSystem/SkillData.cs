using System;
using Dajunctic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum SkillSlot
{
    BasicAttack,
    Skill,
    Ultimate
}


[CreateAssetMenu(fileName = "NewSkill", menuName = "Panthera/Skill Data")]
public class SkillData : BaseSO
{
    public SkillSlot slot;
    public float castRange;
    public float duration;
    public float cooldown; 

    [GuidReference("tl", typeof(IDummyId))] public string timelineId;
}
