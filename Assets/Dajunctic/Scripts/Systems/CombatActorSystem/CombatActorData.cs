using System;
using System.Collections.Generic;
using Dajunctic;

public class CombatActorData: BaseSO
{
    public ActorMovement movement;
    public ActorAttribute attribute;
    public ActorSkillAttribute skillAttribute;
    public CombatStat combatStat;
    public List<SkillData> skills = new List<SkillData>();
}

[Serializable]
public class ActorMovement
{
    public float moveSpeed = 3f;
    public float rotateSpeed = 4f;
    public float height = 1.2f;
    public float radius = 0.25f;
    public float acceleration = 16f;
    public ActorMovementType movementType;
}

[Serializable]
public class ActorAttribute
{
    public float maxHp = 0f;
    public float physicalArmor;
    public float magicalArmor;
}

[Serializable]
public struct CombatStat
{
    public float radius;
    public float atk;
    public float atkSpd;
    public float atkRange;
    public float criticalChance;
    public float criticalDmg;
}

[Serializable]
public class ActorSkillAttribute
{
    public int energy;
}