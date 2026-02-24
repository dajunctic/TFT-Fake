using UnityEngine;
using System;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class SkillNode : ScriptableObject
    {
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;

        public virtual void Execute(SkillExecutionContext context, Action onComplete)
        {
            onComplete?.Invoke();
        }
    }
}
