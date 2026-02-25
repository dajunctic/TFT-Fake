using System;
using Dajunctic;
using Dajunctic.SkillSystem.Graph;
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
    public float cooldown; 

    public SkillGraph skillGraph;
}

