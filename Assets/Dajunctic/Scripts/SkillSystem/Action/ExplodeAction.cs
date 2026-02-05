using System.Collections.Generic;
using UnityEngine;

namespace Dajunctic
{
    public class ExplodeAction: SkillAction,
                                PlayFxAction.ISubActionSource
    {
        [SerializeField] public float radius;
        [SerializeField] public LayerMask targetLayer;
        [SerializeField] CombineDamage combineDamage;
        [SerializeReference] List<SkillAction> hitActions;


        private Vector3 currentPosition;
        private CombatActor currentTarget;

        public override void Execute(CombatActor combatActor, SkillTrackContext context, object source)
        {
            if (combatActor == null) return;

            var data = ((ISubActionSource)source).GetData();

            SkillHelper.ScanTargetInRadius(data.position, radius, targetLayer, out var foundActors);

            foreach (var actor in foundActors)
            {
                actor.TakeDamage(combineDamage);
                foreach (var action in hitActions)
                {
                    if (action == null) continue;
                    currentPosition = actor.Position;
                    currentTarget = actor;
                    action.Execute(combatActor, context, this);
                }
            }

        }

        public interface ISubActionSource
        {
            Data GetData();
        }

        public class Data
        {
            public Vector3 position = Vector3.zero;
        }

        PlayFxAction.Data PlayFxAction.ISubActionSource.GetData()
        {
            return new PlayFxAction.Data()
            {
                position = currentPosition,
                enemies = new List<CombatActor>() { currentTarget }
            };
        }
    }
}