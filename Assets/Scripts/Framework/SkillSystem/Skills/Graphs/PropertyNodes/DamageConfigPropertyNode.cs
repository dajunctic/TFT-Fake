using GraphProcessor;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Property/DamageConfig")]
    public class DamageConfigPropertyNode : PropertyNode
    {
        [SerializeField] public string propertyName;
        [SerializeField] public DamageConfig fallbackValue;

        [GraphProcessor.Output(name = "value")] 
        public DamageConfig value;

        protected override void ResetInternal()
        {
            base.ResetInternal();
            if (Ability != null && Ability.GetProperties().TryGetValue(propertyName, out var prop) && prop is DamageConfig damageConfig)
            {
                value = damageConfig;
            }
            else
            {
                value = fallbackValue;
            }
        }
    }
}
