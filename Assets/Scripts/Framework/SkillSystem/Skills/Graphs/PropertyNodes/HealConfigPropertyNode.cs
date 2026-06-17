using GraphProcessor;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Property/HealConfig")]
    public class HealConfigPropertyNode : PropertyNode
    {
        [SerializeField] public string propertyName;
        [SerializeField] public HealConfig fallbackValue;

        [GraphProcessor.Output(name = "value")] 
        public HealConfig value;

        protected override void ResetInternal()
        {
            base.ResetInternal();
            if (Ability != null && Ability.GetProperties().TryGetValue(propertyName, out var prop) && prop is HealConfig healConfig)
            {
                value = healConfig;
            }
            else
            {
                value = fallbackValue;
            }
        }
    }
}
