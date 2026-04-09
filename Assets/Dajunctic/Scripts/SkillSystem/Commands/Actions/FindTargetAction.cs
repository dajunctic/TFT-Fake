using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic.SkillSystem.Commands.Actions
{
    [Serializable]
    public class FindTargetAction : SkillAction
    {
        [GUIColor("GetColor")]
        [HorizontalGroup("Target")]
        [PropertyOrder(-1)]
        [HideLabel]
        [DisplayAsString]
        public string label = "Find Target";

        [HorizontalGroup("Target")]
        public Graph.Nodes.TargetType TargetType;

        [Tooltip("The radius to scan for targets")]
        public float Radius = 5f;

        [Tooltip("Number of targets to capture")]
        public int Count = 1;

        public bool SaveToContext = true;
        [ShowIf("SaveToContext")]
        public string ContextKey = "CurrentTargets";

        protected override Color GetColor() => new Color(0.8f, 0.4f, 0f, 1f); // Orange

        public override IEnumerator Execute(CommandExecutionContext context)
        {
            var actor = context.Caster as CombatActor; // Need concrete for SkillHelper
            if (actor == null) yield break;

            List<CombatActor> foundActors = null;
            SkillHelper.ScanTargetInRadius(actor, Radius, out foundActors);

            if (foundActors != null && foundActors.Count > 0)
            {
                // Sort based on TargetType
                switch (TargetType)
                {
                    case Graph.Nodes.TargetType.NearestEnemy:
                        foundActors.Sort((a, b) => Vector3.Distance(actor.Position, a.Position)
                                           .CompareTo(Vector3.Distance(actor.Position, b.Position)));
                        break;
                    case Graph.Nodes.TargetType.FarthestEnemy:
                        foundActors.Sort((a, b) => Vector3.Distance(actor.Position, b.Position)
                                           .CompareTo(Vector3.Distance(actor.Position, a.Position)));
                        break;
                    case Graph.Nodes.TargetType.Random:
                        // Simple shuffle (Unity Random inside Sort is not stable, this is just simulation)
                        foundActors.Sort((a, b) => UnityEngine.Random.value.CompareTo(UnityEngine.Random.value));
                        break;
                }

                // Truncate list if Count is specified
                if (foundActors.Count > Count)
                {
                    foundActors = foundActors.GetRange(0, Count);
                }

                if (SaveToContext)
                {
                    // Convert to IDamageTaker list for compatibility with rest of system
                    List<IDamageTaker> damageTakers = new List<IDamageTaker>();
                    foreach(var a in foundActors) damageTakers.Add(a);
                    
                    context.SetVariable(ContextKey, damageTakers);
                }
            }

            yield break; // Synchronous operation
        }
    }
}
