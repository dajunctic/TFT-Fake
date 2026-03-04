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
            if (_context == null)
            {
                Debug.LogWarning("[PlayFxActionNode] Execute skipped: _context is NULL");
                return;
            }

            if (_context.Services == null)
            {
                Debug.LogWarning($"[PlayFxActionNode] Execute skipped: Services is NULL. sourceType={(source == null ? "<null>" : source.GetType().FullName)}");
                return;
            }

            if (string.IsNullOrEmpty(fxId))
            {
                Debug.LogWarning($"[PlayFxActionNode] Execute skipped: fxId is EMPTY. sourceType={(source == null ? "<null>" : source.GetType().FullName)}");
                return;
            }

            if (_context.Services.IsDebug)
            {
                Debug.Log($"<color=#e599f7>[PlayFxActionNode]</color> Execute: fxId='{fxId}', anchor={spawnAnchor}, duration={duration}, sourceType={(source == null ? "<null>" : source.GetType().FullName)}");
            }

            var data = GetFxData(source);
            if (data == null)
            {
                Debug.LogWarning($"[PlayFxActionNode] GetFxData returned NULL. sourceType={(source == null ? "<null>" : source.GetType().FullName)}. Does source implement IFxDataProvider?");
                base.Execute(source);
                return;
            }

            if (data.targets == null)
            {
                Debug.LogWarning("[PlayFxActionNode] FxData.targets is NULL");
                base.Execute(source);
                return;
            }

            if (_context.Services.IsDebug)
            {
                Debug.Log($"<color=#e599f7>[PlayFxActionNode]</color> targets={data.targets.Count}");
            }

            for (int i = 0; i < data.targets.Count; i++)
            {
                var target = data.targets[i];
                if (target == null)
                {
                    Debug.LogWarning($"[PlayFxActionNode] target[{i}] is NULL");
                    continue;
                }

                Vector3 spawnPos = target.AsCombatActor().GetAnchorPosition(spawnAnchor);
                Quaternion spawnRot = Quaternion.LookRotation(target.AsTransform().Forward);

                if (_context.Services.IsDebug)
                {
                    var targetName = target != null ? target.GetType().Name : "<null>";
                    Debug.Log($"<color=#e599f7>[PlayFxActionNode]</color> SpawnFx: idx={i}, fxId='{fxId}', pos={spawnPos}, rot={spawnRot.eulerAngles}, targetType={targetName}");
                }

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