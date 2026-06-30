using System;
using System.Collections.Generic;
using Dajunctic;
using UnityEngine;
using Dajunctic.SkillSystem.Gambits;

public class CombatActorData : BaseSO
{
    [Header("Prefab")]
    public GameObject prefab;

    public ActorMovement movement;
    public ActorBaseStats stats;

    [Header("AI Gambits (Priority: Top to Bottom)")]
    public List<Gambit> gambits = new List<Gambit>();
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
public class ActorBaseStats
{
    [Header("Life & Defense")]
    public float maxHp = 500f;
    public float armor = 20f;
    public float magicResist = 20f;

    [Header("Offense")]
    public float attackDamage = 50f;
    public float abilityPower = 100f;
    public float attackSpeed = 0.65f;
    public float critChance = 0.25f;
    public float critDamage = 1.4f;

    [Header("Mana")]
    public float maxMana = 100f;
    public float startingMana = 0f;
}
