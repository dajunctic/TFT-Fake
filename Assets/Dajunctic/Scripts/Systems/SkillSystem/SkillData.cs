using System;
using Dajunctic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public enum SkillSlot
{
    BasicAttack,
    Skill,
    Ultimate
}
public enum SkillExecutionType
{
    Melee,
    Projectile,
    SpawnAtTarget,
    Dash,
    Timeline
}
[Serializable]
public struct SkillEvent
{
    public int frame;
    public string firePointName;
    public float damageMultiplier;
}
[Serializable]
public struct SkillStep
{
    public string animTrigger;
    public int totalFrames; 
    public int transitionFrames; 
    public int cancelFrame;
    public CombineDamage combineDamage;
    public SkillEvent[] events;
    public PoolableObject overrideMuzzlePrefab;
    public PoolableObject overrideProjectilePrefab;
    public PoolableObject overrideHitPrefab;
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Panthera/Skill Data")]
public class SkillData : BaseSO
{
    public SkillSlot slot;
    public SkillExecutionType executionType;

    public float frameRate = 30f;
    public float castRange;
    public float duration;
    public int cooldownFrames; 
    public float comboTolerance = 2f;

    public PoolableObject muzzlePrefab;
    public PoolableObject projectilePrefab;
    public PoolableObject hitPrefab;
    
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "animTrigger")]
    public SkillStep[] skillSteps;
    [GuidReference("tl", typeof(IDummyId))] public string timelineId;

    public float GetFrameTime(int frames) => frames / frameRate;
    public bool IsCombo => skillSteps is { Length: > 1 };
}