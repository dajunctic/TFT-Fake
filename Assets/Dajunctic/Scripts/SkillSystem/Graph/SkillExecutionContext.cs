using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillExecutionContext
    {
        public ICombatActor actor;
        public ISkillServiceProvider Services;
        public Dictionary<string, object> nodeOutputs = new();

        public SkillExecutionContext(ICombatActor actor, ISkillServiceProvider services = null)
        {
            this.actor = actor;
            this.Services = services;
        }
    }
}
