using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class RemoveTargetNode : AbilityNode
    {
        [SerializeReference, Input] List<IDamageTaker> inTargets;
        [SerializeReference, Input] List<IDamageTaker> removeTargets;
        [SerializeReference, Output] public List<IDamageTaker> outTargets;

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;

            if (port.fieldName == nameof(outTargets))
            {
                var targets = GetInputValue(nameof(inTargets), inTargets);
                var removes = GetInputValue(nameof(removeTargets), removeTargets);

                if (targets == null || targets.Count == 0)
                {
                    return new List<IDamageTaker>();
                }

                if (removes == null || removes.Count == 0)
                {
                    return new List<IDamageTaker>(targets);
                }

                var result = new List<IDamageTaker>();

                foreach (var target in targets)
                {
                    if (target != null && !removes.Contains(target))
                    {
                        result.Add(target);
                    }
                }

                return result;
            }

            return null;
        }
    }
}

