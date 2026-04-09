using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands
{
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract class SkillAction
    {
        // Core execution method. Must return an IEnumerator for sequence yielding.
        public abstract IEnumerator Execute(CommandExecutionContext context);

        // Helper for colors in Odin Inspector
        protected virtual Color GetColor() => Color.white;
    }
}
