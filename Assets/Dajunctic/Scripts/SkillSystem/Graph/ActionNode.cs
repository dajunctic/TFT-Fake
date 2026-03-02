using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class ActionNode : SkillNode
    {
        [NodeOutput] public SkillNode self;

        public virtual void Execute(object source)
        {
            Complete();
        }

        public override object GetValue(string portName)
        {
            if (portName == nameof(self)) return this;
            return base.GetValue(portName);
        }

        public override void Reset()
        {
            base.Reset();
            // Action nodes don't have per-execution data input for now (resolved from source)
        }
    }
}
