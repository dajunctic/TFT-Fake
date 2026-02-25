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
        public CombatActor actor;

        /// <summary>
        /// Lưu kết quả output của các node: key = "nodeGuid_portName"
        /// Dùng để truyền data giữa các node qua NodeOutput port.
        /// </summary>
        public Dictionary<string, object> nodeOutputs = new();

        /// <summary>
        /// Callback khi có VFX cần spawn.
        /// </summary>
        public Action<GameObject> onSpawnVFX;

        public SkillExecutionContext(CombatActor actor)
        {
            this.actor = actor;
        }

        /// <summary>
        /// Lưu output của một node vào context.
        /// </summary>
        public void SetOutput(string nodeGuid, string portName, object value)
        {
            nodeOutputs[$"{nodeGuid}_{portName}"] = value;
        }

        /// <summary>
        /// Lấy output từ một node khác đã chạy trước.
        /// </summary>
        public T GetOutput<T>(string nodeGuid, string portName)
        {
            string key = $"{nodeGuid}_{portName}";
            if (nodeOutputs.TryGetValue(key, out object value) && value is T result)
                return result;
            return default;
        }
    }
}
