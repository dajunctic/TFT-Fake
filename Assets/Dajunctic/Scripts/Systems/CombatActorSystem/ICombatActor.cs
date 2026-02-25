using Dajunctic;
using Dajunctic.SkillSystem.Graph;
using UnityEngine;

namespace Dajunctic
{
    public interface ICombatActor: IDamageTaker, ISkillOwner, ITransform, IAnimator, IDamageOwner, IGameObject
    {
        public string DataId {get;}
        public Team Team{ get; }
        public bool IsTargetable {get; }

        // Animation
        public void PlayAnim(string animName, float transitionDuration = 0.1f);
        public void ResetAnim();
        public bool IsAnimFinished { get; }

        // Anchor
        public Vector3 GetAnchorPosition(AnchorType anchorType);

        // Transform
        public Transform CachedTransform { get; }

        // SkillGraph
        public SkillGraphRunner GetSkillGraphRunner();
    }
}
