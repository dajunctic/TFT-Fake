using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.ActionNodes
{
    public class PlayFxActionNode : ActionNode
    {
        public string fxId;
        public AnchorType spawnAnchor = AnchorType.MidPoint;
        public float duration = 2f;

        public override void Execute(object source)
        {
            if (_context?.Services == null || string.IsNullOrEmpty(fxId) || source == null) return;

            if (source is ISubActionSource actionSource)
            {
                var data = actionSource.GetData();
                if (data == null || data.damageTakers == null) return;

                foreach (var target in data.damageTakers)
                {
                    if (target == null) continue;

                    Vector3 spawnPos = target.AsCombatActor().GetAnchorPosition(spawnAnchor);
                    Quaternion spawnRot = Quaternion.LookRotation(target.AsTransform().Forward);

                    var playFxEvent = new SpawnFxEvent
                    {
                        id = fxId,
                        position = spawnPos,
                        rotation = spawnRot,
                        duration = duration
                    };

                    _context.Services.SpawnFx(playFxEvent);
                }
            }

            base.Execute(source);
        }
    }
}