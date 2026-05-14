using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using XNode;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif


namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class AbilityLevelData
    {
        [SerializeField]
        AbilityDescription[] description;

        [SerializeField]
        NodeGraph graph;

        [SerializeField]
        NodeGraph[] otherGraphs;

        [SerializeField]
        AbilityProperty[] properties;

        public NodeGraph Graph => graph;
        public NodeGraph[] OtherGraphs => otherGraphs;

        public virtual T CreateCopy<T>()
            where T : AbilityLevelData, new()
        {
            var copy = new T();
            copy.description = description.Select(d => d.CreateCopy()).ToArray();
            copy.graph = graph?.Copy();
            copy.otherGraphs =
                otherGraphs != null
                    ? otherGraphs.Select(g => g.Copy()).ToArray()
                    : Array.Empty<NodeGraph>();
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

        public void SetGraph(NodeGraph nodeGraph)
        {
            graph = nodeGraph;
        }

        public void SetProperties(string[] propertyNames)
        {
            properties = propertyNames.Select(n => new AbilityProperty(n)).ToArray();
        }

#if UNITY_EDITOR

        public void SetLocalizeString(string name)
        {
            for (var i = 0; i < description.Length; i++)
            {
                description[i].SetLocalizeString(name, i);
            }
        }

        public void SetSmartString()
        {
            var s_smartVariableRegex = new Regex(
                @"\{([a-zA-Z0-9_.-]+)(:[^}]+)?\}",
                RegexOptions.Compiled
            );
            foreach (var desc in description)
            {
                var localizedString = desc.localizedDescription;
                var collection = LocalizationEditorSettings.GetStringTableCollection(
                    "string_table"
                );
                var locales = LocalizationEditorSettings.GetLocales();
                foreach (var locale in locales)
                {
                    var table = collection.GetTable(locale.Identifier) as StringTable;
                    var entry = table.GetEntryFromReference(localizedString.TableEntryReference);
                    if (entry != null)
                    {
                        if (!entry.IsSmart)
                        {
                            entry.IsSmart = true;
                            Debug.Log(
                                "Set entry as smart: "
                                    + entry.Key
                                    + " in table: "
                                    + table.TableCollectionName
                            );
                        }
                        if (locale.Identifier == "en")
                        {
                            var rawString = entry.Value;
                            var requiredVarNames = new HashSet<string>();
                            foreach (Match match in s_smartVariableRegex.Matches(rawString))
                            {
                                requiredVarNames.Add(match.Groups[1].Value);
                            }

                            foreach (var varName in requiredVarNames)
                            {
                                if (!localizedString.TryGetValue(varName, out var variable))
                                {
                                    var s = new StringVariable();
                                    s.Value = "<missing value>";
                                    localizedString[varName] = s;
                                    Debug.Log(
                                        "Added missing variable: "
                                            + varName
                                            + " to localized string: "
                                            + entry.Key
                                    );
                                }
                            }
                        }
                        EditorUtility.SetDirty(table);
                    }
                }
            }
        }

        public void FixCaptainPassiveDesc()
        {
            for (var i = 1; i < description.Length; i++)
            {
                description[i].localizedDescription.TableEntryReference = description[0]
                    .localizedDescription
                    .TableEntryReference;
                foreach (var key in description[0].localizedDescription.Keys)
                {
                    if (!description[i].localizedDescription.ContainsKey(key))
                    {
                        description[i].localizedDescription[key] = description[
                            0
                        ].localizedDescription[key];
                    }
                }
            }
        }
#endif
    }

    [Serializable]
    public class AbilityDescription
    {
        [SerializeField]
        public LocalizedString localizedDescription;

        public AbilityDescription(LocalizedString localizedDescription)
        {
            this.localizedDescription = localizedDescription;
        }

        public AbilityDescription CreateCopy()
        {
            return new AbilityDescription(localizedDescription);
        }

        public void SetLocalizeString(string name, int index)
        {
#if UNITY_EDITOR
            var key = $"{name}_desc_{index}";
            localizedDescription = LocalizationToolWindow.CreateLocalizedStringKeyValue(
                key,
                "<missing>"
            );
#endif
        }
    }

    public enum CooldownType
    {
        Time,
        Energy,
    }
}

