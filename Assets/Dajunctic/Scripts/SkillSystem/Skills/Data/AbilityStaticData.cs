using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class AbilityStaticData
    {
        [SerializeField] public AbilityType abilityType;
        [SerializeField] public LocalizedString localizedName;

        public void SetLocalizeString(string name)
        {
#if UNITY_EDITOR
            var key = $"{name}_name";
            localizedName = LocalizationToolWindow.CreateLocalizedStringKeyValue(key, "<missing>");
#endif
        }
    }
}
