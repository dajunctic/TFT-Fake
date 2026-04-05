using System.Collections.Generic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.ActionNodes
{
    public class PlayFxActionNode : ActionNode
    {
        [XNode.Node.InputAttribute] public ActionNode @in;

        [SerializeField, GuidReference("fx", typeof(IDummyId))] private string fxId;
        public AnchorType spawnAnchor = AnchorType.MidPoint;
        public float duration = 2f;

        public override void Execute(object source)
        {
            if (_context?.Services == null || string.IsNullOrEmpty(fxId))
            {
                base.Execute(source);
                return;
            }

            var data = GetFxData(source);
            if (data == null || data.targets == null)
            {
                base.Execute(source);
                return;
            }

            for (int i = 0; i < data.targets.Count; i++)
            {
                var target = data.targets[i];
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

            base.Execute(source);
        }
    }
}