using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GraphProcessor;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class AbilityLevelData
    {
        [SerializeField]
        AbilityDescription[] description;

        [SerializeField]
        BaseGraph graph;

        [SerializeField]
        BaseGraph[] otherGraphs;

        [SerializeField]
        AbilityProperty[] properties;

        public BaseGraph Graph => graph;
        public BaseGraph[] OtherGraphs => otherGraphs;

        public virtual T CreateCopy<T>()
            where T : AbilityLevelData, new()
        {
            var copy = new T();
            copy.graph = graph != null ? ScriptableObject.Instantiate(graph) as BaseGraph : null;
            copy.otherGraphs =
                otherGraphs != null
                    ? otherGraphs.Select(g => ScriptableObject.Instantiate(g) as BaseGraph).ToArray()
                    : Array.Empty<BaseGraph>();
            copy.properties =
                properties != null
                    ? properties.Select(p => p.CreateCopy()).ToArray()
                    : Array.Empty<AbilityProperty>();
            return copy;
        }

        public AbilityDescription GetDescription(int level)
        {
            if (description == null || description.Length == 0)
            {
                return null;
            }
            var lv = Mathf.Clamp(level, 0, description.Length - 1);
            return description[lv];
        }

        public AbilityDescription[] GetAllDescription()
        {
            return description;
        }

        public Dictionary<string, IAbilityProperty> GetProperties(int level)
        {
            var results = new Dictionary<string, IAbilityProperty>();
            if (properties == null || properties.Length == 0)
            {
                return results;
            }

            foreach (var property in properties)
            {
                if (property.value == null || property.value.Length == 0)
                {
                    continue;
                }
                var lv = Mathf.Clamp(level, 0, property.value.Length - 1);
                results.Add(property.propertyName, property.value[lv].value);
            }
            return results;
        }

        public void SetGraph(BaseGraph nodeGraph)
        {
            graph = nodeGraph;
        }

        public void SetProperties(string[] propertyNames)
        {
            properties = propertyNames.Select(n => new AbilityProperty(n)).ToArray();
        }
    }

    [Serializable]
    public class AbilityDescription
    {
       
    }

    public enum CooldownType
    {
        Time,
        Energy,
    }
}

