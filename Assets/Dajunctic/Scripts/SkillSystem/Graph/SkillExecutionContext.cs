using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillExecutionContext
    {
        /// <summary>Actor đang thực thi skill (caster).</summary>
        public ICombatActor actor;

        /// <summary>
        /// Service provider để spawn FX, missile, v.v.
        /// Runtime: GameManager. Editor preview: PreviewSkillServiceProvider.
        /// </summary>
        public ISkillServiceProvider Services;

        public Dictionary<string, object> nodeOutputs = new();

        public SkillExecutionContext(ICombatActor actor, ISkillServiceProvider services = null)
        {
            this.actor = actor;
            this.Services = services;
        }

        public void SetOutput(string nodeGuid, string portName, object value)
        {
            nodeOutputs[$"{nodeGuid}_{portName}"] = value;
        }

        public T GetOutput<T>(string nodeGuid, string portName)
        {
            string key = $"{nodeGuid}_{portName}";
            if (nodeOutputs.TryGetValue(key, out object value) && value is T result)
                return result;
            return default;
        }
    }
}
