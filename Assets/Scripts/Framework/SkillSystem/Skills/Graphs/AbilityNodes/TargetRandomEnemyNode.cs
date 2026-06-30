using System.Collections.Generic;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/TargetRandomEnemy")]
    public class TargetRandomEnemyNode : AbilityNode, IHasRange
    {
        [SerializeField] protected float radius = 10f;
        [SerializeField] protected int count = 1;
        
        [GraphProcessor.Output] protected List<IDamageTaker> targets;
        [GraphProcessor.Output] protected IDamageTaker mainTarget;   

        protected List<IDamageTaker> _cachedTargets = new List<IDamageTaker>();
        protected IDamageTaker _cachedMainTarget;

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

        public float GetRange() => radius;

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
                50 
            );

            for (int i = 0; i < count && allEnemies.Count > 0; i++)
            {
                int rnd = Random.Range(0, allEnemies.Count);
                _cachedTargets.Add(allEnemies[rnd]);
                allEnemies.RemoveAt(rnd);
            }
        }
    }
}
