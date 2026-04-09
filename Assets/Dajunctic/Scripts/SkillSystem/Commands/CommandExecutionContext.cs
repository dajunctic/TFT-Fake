using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Dajunctic.SkillSystem.Graph;

namespace Dajunctic.SkillSystem.Commands
{
    public class CommandExecutionContext
    {
        public ICombatActor Caster { get; private set; }
        public ISkillServiceProvider Services { get; private set; }
        
        // Dynamic context variables (replacing xNode's input/output ports)
        // Store targets, locations, specific hit data, etc.
        private Dictionary<string, object> _variables = new Dictionary<string, object>();

        public CommandExecutionContext(ICombatActor caster, ISkillServiceProvider services)
        {
            Caster = caster;
            Services = services;
        }

        public void SetVariable(string key, object value)
        {
            _variables[key] = value;
        }

        public T GetVariable<T>(string key, T defaultValue = default)
        {
            if (_variables.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }

        public void ClearVariables()
        {
            _variables.Clear();
        }
    }
}
