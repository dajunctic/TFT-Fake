using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class TargetRandomEnemyNode : AbilityNode
    {
        [SerializeField] protected float radius = 10f;
        [SerializeField] protected int count = 1;
        
        [SerializeField, Output(ShowBackingValue.Never)] protected List<IDamageTaker> targets;
        [SerializeField, Output(ShowBackingValue.Never)] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets = new List<IDamageTaker>();
        protected IDamageTaker _cachedMainTarget;

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

        protected IDamageTaker GetMainTarget()
        {
            FindRandomTargets();
            if (_cachedTargets.Count > 0)
            {
                _cachedMainTarget = _cachedTargets[0];
            }
            else
            {
                _cachedMainTarget = null;
            }
            return _cachedMainTarget;
        }

        protected void FindRandomTargets()
        {
            _cachedTargets.Clear();
            
            List<IDamageTaker> allEnemies = new List<IDamageTaker>();
            SkillHelper.FindTargetsInRadius(
                Owner.AsTeamMember().EnemyTeam, 
                Owner.AsTransform().Position, 
                1.5f, 
                radius, 
                null, 
                allEnemies, 
                50 // grab up to 50
            );
            
            // Randomly select 'count' enemies
            for (int i = 0; i < count && allEnemies.Count > 0; i++)
            {
                int rnd = Random.Range(0, allEnemies.Count);
                _cachedTargets.Add(allEnemies[rnd]);
                allEnemies.RemoveAt(rnd);
            }
        }
    }
}
