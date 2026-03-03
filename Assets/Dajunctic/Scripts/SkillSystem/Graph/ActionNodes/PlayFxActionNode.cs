using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.ActionNodes
{
    public class PlayFxActionNode : ActionNode
    {
        [SerializeField, GuidReference("fx", typeof(IDummyId))] private string fxId;
        public AnchorType spawnAnchor = AnchorType.MidPoint;
        public float duration = 2f;

        public override void Execute(object source)
        {
            if (_context?.Services == null || string.IsNullOrEmpty(fxId)) return;

            Debug.LogError("Ahihi");

            var data = GetFxData(source);
            if (data != null && data.targets != null)
            {
                foreach (var target in data.targets)
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