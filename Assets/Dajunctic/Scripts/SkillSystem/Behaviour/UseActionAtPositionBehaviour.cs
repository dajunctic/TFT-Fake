using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    [Serializable]
    public class UseActionAtPositionSetting
    {
        public List<Vector3> positions;
        public bool isLocalPosition;
        [SerializeReference] public List<SkillAction> actions;
    }

    public class UseActionAtPositionBehaviour: SkillBehaviour<UseActionAtPositionSetting>,
                                                PlayFxAction.ISubActionSource,   
                                                ExplodeAction.ISubActionSource
    {
        private Vector3 currentPosition;
        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            base.Execute(combatActor, context);

            if (combatActor == null || hasExecuted) return;

            var targetPositions = new List<Vector3>();
            
            if (Settings.positions != null)
            foreach (var position in Settings.positions)
            {
               if (Settings.isLocalPosition)
               {
                   targetPositions.Add(combatActor.CachedTransform.TransformPoint(position));
               }
               else
               {
                   targetPositions.Add(position);
               }
            }

            targetPositions.AddRange(context.TargetPositions);

            foreach (var targetPosition in targetPositions)
            {
                currentPosition = targetPosition;
                foreach (var action in Settings.actions)
                {
                    if (action == null) continue;
                    action.Execute(combatActor, context, this);
                }
            }
        }


        PlayFxAction.Data PlayFxAction.ISubActionSource.GetData()
        {
            return new PlayFxAction.Data()
            {
                position = currentPosition,
            };
        }

        ExplodeAction.Data ExplodeAction.ISubActionSource.GetData()
        {
            return new ExplodeAction.Data()
            {
                position = currentPosition,
            };
        }
    }
}