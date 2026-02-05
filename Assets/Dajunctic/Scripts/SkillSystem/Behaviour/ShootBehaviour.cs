using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    [Serializable]
    public class ShootSetting
    {
        [SerializeField, GuidReference("missile", typeof(IDummyId))]public string missileId; 
        [SerializeField] public CombineDamage combineDamage;
        public Vector3 launcher;
        public Vector3 destination;
        public bool targetEnemy = true;

        [SerializeReference] public List<SkillAction> hitAction;

    }

    public class ShootBehaviour: SkillBehaviour<ShootSetting>,
                                PlayFxAction.ISubActionSource  
    {
        private MissileView missileView;
        private CombatActor target;

        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            base.Execute(combatActor, context);
            if (combatActor != null && !hasExecuted)
            {
                
                foreach (var target in context.EnemyTargets)
                {
                    var missileData = new MissileData();
                    missileData.launcher = combatActor.CachedTransform.TransformPoint(Settings.launcher);
                    missileData.destination = combatActor.CachedTransform.TransformPoint(Settings.destination);
                    missileData.targetActor = target;
                    missileData.combatActor = combatActor;
                    missileData.combineDamage = Settings.combineDamage;

                    missileView = GameManager.Instance.SpawnMissile(Settings.missileId, missileData);
                    missileView.OnHitTargetEvent += OnHitTarget;
                }
                hasExecuted = true;
            }
            
        }

        void OnHitTarget(CombatActor target)
        {
            if (Settings.hitAction == null) return;
            this.target = target;

            foreach (var action in Settings.hitAction)
            {
                if (action == null) continue;
                action.Execute(combatActor, context, this);
            }
        }

        PlayFxAction.Data PlayFxAction.ISubActionSource.GetData()
        {
            return new PlayFxAction.Data()
            {
                enemies = new List<CombatActor>() { target }
            };
        }
    }
}