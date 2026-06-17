using GraphProcessor;
using Dajunctic.SkillSystem.Data;
using UnityEngine;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Property/Int")]
    public class IntPropertyNode : PropertyNode
    {
        [SerializeField] public string propertyName;
        [SerializeField] public int fallbackValue;

        [GraphProcessor.Output(name = "value")] 
        public int value;

        protected override void ResetInternal()
        {
            base.ResetInternal();
            if (Ability != null && Ability.GetProperties().TryGetValue(propertyName, out var prop) && prop is IntConfig intConfig)
            {
                value = intConfig.GetData();
            }
            else
            {
                value = fallbackValue;
            }
        }
    }
}
