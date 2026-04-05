using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayFxNode : SkillNode
    {
        [XNode.Node.InputAttribute(connectionType = XNode.Node.ConnectionType.Multiple)] public bool @in;
        [XNode.Node.OutputAttribute(connectionType = XNode.Node.ConnectionType.Override)] public bool @out;

        public string fxId;
        public AnchorType spawnAnchor = AnchorType.FootPoint;
        public float duration = 2f;

        public override void Execute()
        {
            if (_context?.actor == null)
            {
                Complete();
                return;
            }

            Vector3 spawnPos = _context.actor.GetAnchorPosition(spawnAnchor);
            Quaternion spawnRot = Quaternion.LookRotation(_context.actor.Forward);

            var playFxEvent = new SpawnFxEvent
            {
                id = fxId,
                position = spawnPos,
                rotation = spawnRot,
                duration = duration
            };

            if (_context.Services != null && !string.IsNullOrEmpty(fxId))
            {
                _context.Services.SpawnFx(playFxEvent);
            }
            Complete();
        }
    }
}
