using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace Dajunctic
{
    [Serializable]
    public class PlayFxSetting
    {
        [GuidReference("fx", typeof(IDummyId))] public string Id;
        public float duration = -1f;
        public bool targetEnemy;
        [ShowIf("@targetEnemy")]
        public AnchorType anchorType;
        public Vector3 offset;
        public bool isLocalPosition;
    }

    public class PlayFxBehaviour: SkillBehaviour<PlayFxSetting>
    {
        public override void Execute(CombatActor combatActor, SkillTrackContext context)
        {
            if (combatActor != null && !hasExecuted)
            {

                if (Settings.targetEnemy)
                {
                    foreach (var target in context.EnemyTargets)
                    {
                        foreach (var actor in context.EnemyTargets)
                        {
                            if (actor != null && actor.gameObject.activeInHierarchy)
                            {
                                var data = new PlayFxData();

                                data.Position = actor.GetAnchorPosition(Settings.anchorType) + Settings.offset;
                                data.duration = Settings.duration;
                                GameManager.Instance.PlayFx(Settings.Id, data);
                            }
                        }
                    }
                    hasExecuted = true;
                }
                else
                {
                    var data = new PlayFxData();
                    if (Settings.isLocalPosition)
                    {
                        data.Position = combatActor.CachedTransform.TransformPoint(Settings.offset);
                    }
                    else {
                        data.Position = Settings.offset;
                    }
                    GameManager.Instance.PlayFx(Settings.Id, data);
                    hasExecuted = true;
                }
            }
        }
    }
}