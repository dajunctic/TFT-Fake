using UnityEngine;
using System;
using System.Collections.Generic;

namespace Dajunctic.SkillSystem.Graph
{
    public class SkillExecutionContext
    {
        /// <summary>
        /// Actor đang thực thi skill (caster).
        /// </summary>
        public ICombatActor actor;

        public Dictionary<string, object> nodeOutputs = new();

        public Action<GameObject> onSpawnVFX;

        public SkillExecutionContext(ICombatActor actor)
        {
            this.actor = actor;
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
