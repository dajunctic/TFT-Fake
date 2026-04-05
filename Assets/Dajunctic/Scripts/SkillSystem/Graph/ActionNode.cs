using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph
{
    [XNode.Node.NodeTint("#4B0082")]
    public abstract class ActionNode : SkillNode
    {
        [XNode.Node.OutputAttribute] public ActionNode self;

        public virtual void Execute(object source)
        {
            Complete();
        }

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(self)) return this;
            return null;
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
