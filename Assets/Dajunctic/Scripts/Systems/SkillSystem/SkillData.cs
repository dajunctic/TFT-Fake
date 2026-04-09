using System;
using Dajunctic;
using Sirenix.OdinInspector;
using UnityEngine;
using Dajunctic.SkillSystem.Commands;
using UnityEngine;

public enum SkillSlot
{
    BasicAttack,
    Skill,
    Ultimate
}


[CreateAssetMenu(fileName = "NewSkill", menuName = "Dajunctic/Skill Data")]
public class SkillData : BaseSO
{
    public SkillSlot slot;
    public float castRange;
    public float cooldown; 

    public SkillTimelineSO skillTimeline;
}

