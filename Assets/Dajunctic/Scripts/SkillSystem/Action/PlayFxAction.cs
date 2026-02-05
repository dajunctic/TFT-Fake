using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Dajunctic
{
    public class PlayFxAction: SkillAction
    {
        [GuidReference("fx", typeof(IDummyId))] public string Id;
        public float duration = -1f;
        public bool targetEnemy;
        [ShowIf("@targetEnemy")]
        public AnchorType anchorType;
        public Vector3 offset;
        public bool isLocalPosition;

        private Vector3 inOffset;

        public override void Execute(CombatActor combatActor, SkillTrackContext context, object source)
        {
            if (combatActor != null)
            {
                var data = ((ISubActionSource)source).GetData();

                var inEnemies = new List<CombatActor>();

                if (data != null)
                {
                    inEnemies.AddRange(data.enemies);
                    inOffset = data.position;
                }
                else
                {
                    inEnemies.AddRange(context.EnemyTargets);
                    inOffset = offset;
                }


                if (targetEnemy)
                {
                    foreach (var target in context.EnemyTargets)
                    {
                        foreach (var actor in context.EnemyTargets)
                        {
                            if (actor != null && actor.gameObject.activeInHierarchy)
                            {
                                var viewData = new PlayFxData();

                                viewData.Position = actor.GetAnchorPosition(anchorType) + offset;
                                viewData.duration = duration;
                                GameManager.Instance.PlayFx(Id, viewData);
                            }
                        }
                    }
                }
                else
                {
                    var viewData = new PlayFxData();
                    if (isLocalPosition)
                    {
                        viewData.Position = combatActor.CachedTransform.TransformPoint(inOffset);
                    }
                    else {
                        viewData.Position = inOffset;
                    }
                    GameManager.Instance.PlayFx(Id, viewData);
                }
            }
        }

        public class Data
        {
            public List<CombatActor> enemies = new List<CombatActor>();
            public Vector3 position = Vector3.zero;
        }
        
        public interface ISubActionSource
        {
            Data GetData();
        }
    }

}