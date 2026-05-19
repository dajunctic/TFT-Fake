using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class TargetNearestEnemyNode : TargetRadiusEnemyNode
    {

        public override object GetValue(NodePort port)
        {
            if (Owner == null) return null;
            
            GetMainTarget();

            if (port.fieldName == nameof(targets))
            {
                return _cachedTargets;
            }

            if (port.fieldName == nameof(mainTarget))
            {
                return _cachedMainTarget;
            }

            return base.GetValue(port);

        }

    }
}