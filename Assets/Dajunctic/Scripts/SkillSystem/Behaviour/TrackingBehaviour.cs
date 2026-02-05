using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dajunctic
{
    [Serializable]
    public class TrackingSetting
    {
        public bool immediately;
        public bool isManual;
        public Vector3 manualDirection;
    }

    public class TrackingBehaviour: SkillBehaviour<TrackingSetting>
    {
        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            base.Execute(combatActor, context);
           
           if (combatActor == null) return;
            var rotateSpeed = combatActor.RotateSpeed;
            if (Settings.isManual)
            {
                var dir = Settings.manualDirection == Vector3.zero 
                        ? combatActor.CachedTransform.forward
                        : Settings.manualDirection;
                combatActor.RotateRotation(dir, rotateSpeed, Time.deltaTime, true);
            }
            else
            {
                var target = (context.EnemyTargets != null && context.EnemyTargets.Count > 0) 
                         ? context.EnemyTargets[0] : null;

                if (target != null && target != combatActor)
                {
                    combatActor.RotatePosition(target.MidPoint.Position, rotateSpeed, Time.deltaTime, Settings.immediately);
                }
            }
        }
    }
}
        