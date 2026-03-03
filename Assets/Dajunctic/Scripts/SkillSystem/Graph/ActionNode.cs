using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    public abstract class ActionNode : SkillNode
    {
        [NodeOutput] public ActionNode self;

        public virtual void Execute(object source)
        {
            Complete();
        }

        public override object GetValue(string portName)
        {
            if (IsPortNameMatch(portName, nameof(self))) return this;
            return base.GetValue(portName);
        }

        public override void Reset()
        {
            base.Reset();
            // Action nodes don't have per-execution data input for now (resolved from source)
        }

        protected HitData GetHitData(object source)
        {
            if (source is IHitDataProvider hitProvider) return hitProvider.GetHitData();
            return null;
        }

        protected FxData GetFxData(object source)
        {
            if (source is IFxDataProvider fxProvider) return fxProvider.GetFxData();
            return null;
        }
    }
}
