using GraphProcessor;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Property/Float")]
    public class FloatPropertyNode : PropertyNode
    {
        [SerializeField] public string propertyName;
        [SerializeField] public float fallbackValue;

        [GraphProcessor.Output(name = "value")] 
        public float value;

        protected override void ResetInternal()
        {
            base.ResetInternal();
            if (Ability != null && Ability.GetProperties().TryGetValue(propertyName, out var prop) && prop is FloatConfig floatConfig)
            {
                value = floatConfig.GetData();
            }
            else
            {
                value = fallbackValue;
            }
        }
    }
}
