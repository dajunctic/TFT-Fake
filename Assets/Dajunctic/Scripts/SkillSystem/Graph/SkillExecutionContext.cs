using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillExecutionContext
    {
        public CombatActor actor;
        public List<CombatActor> targets = new();
        public Vector3 targetPosition;
        public Dictionary<string, object> variables = new();
        public Action<GameObject> onSpawnVFX;

        public SkillExecutionContext(CombatActor actor)
        {
            this.actor = actor;
        }
    }
}
