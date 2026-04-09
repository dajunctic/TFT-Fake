using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class DelayAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Delay")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Delay";

        [HorizontalGroup("Delay")]
        [Tooltip("Time to wait in seconds")]
        public float Duration = 1f;

        protected override Color GetColor() => new Color(0.8f, 0.8f, 0f, 1f); // Yellow

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            if (Duration > 0)
            {
                yield return new WaitForSeconds(Duration);
            }
        }
    }
}
