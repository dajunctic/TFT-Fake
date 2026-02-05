using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace Dajunctic
{
    [Serializable]
    public class DamageSetting
    {
        public float damage = 10f;
        public DamageType damageType;
        [SerializeReference] public List<SkillAction> hitActions;

    }
    
    public class DamageBehaviour : SkillBehaviour<DamageSetting>,
                                    PlayFxAction.ISubActionSource
    {
        CombatActor currentTarget;

        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            if (combatActor != null && !hasExecuted)
            {
                var targets = context.EnemyTargets;
                var combineDamage = new CombineDamage(Settings.damageType, Settings.damage);

                foreach (var target in targets)
                {
                    if (target != null)
                    {
                        target.TakeDamage(combineDamage);
                        currentTarget = target;

                        foreach (var action in Settings.hitActions)
                        {
                            if (action == null) continue;
                            action.Execute(combatActor, context, this);
                        }
                    }
                }

                hasExecuted = true;
            }
        }

        PlayFxAction.Data PlayFxAction.ISubActionSource.GetData()
        {
            return new PlayFxAction.Data()
            {
                enemies = new List<CombatActor>() { currentTarget }
            };
        }
    }
}