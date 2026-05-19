using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class CacheNode : AbilityNode
    {
        [SerializeField, Input(ShowBackingValue.Never)] IDamageTaker inTarget;
        [SerializeField, Input(ShowBackingValue.Never)] Vector3 inPosition;
        [SerializeField, Output(ShowBackingValue.Never)] IDamageTaker outTarget;
        [SerializeField, Output] Vector3 outPosition;


        IDamageTaker _cachedTarget;
        Vector3 _cachedPosition;

        protected override void PlayInternal()
        {
            base.PlayInternal();
            _cachedTarget = GetInputValue(nameof(inTarget), inTarget);
            _cachedPosition = GetInputValue(nameof(inPosition), inPosition);
            Completed();
        }

        override public object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(outTarget))
            {
                return _cachedTarget;
            }

            if (port.fieldName == nameof(outPosition))
            {
                return _cachedPosition;
            }
            
            return null;
        }
    }
}
