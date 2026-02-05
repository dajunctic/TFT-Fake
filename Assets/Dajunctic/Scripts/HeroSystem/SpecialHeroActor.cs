using System.Collections.Generic;
using Dajunctic;
using UnityEngine;

namespace Dajunctic
{
    public class SpecialHeroActor: HeroCombatActor
    {
        [SerializeField, GuidReference("tl", typeof(IDummyId))] List<string> testSkillIds;
        [SerializeField] bool testSkill;
        public override void ListenEvents()
        {
            base.ListenEvents();

            InputManager.OnTestFirstSkill += TestFirstSkill;
            InputManager.OnTestSecondSkill += TestSecondSkill;
            InputManager.OnTestThirdSkill += TestThirdSkill;
            InputManager.OnTestFourthSkill += TestForthSkill;
        
        }

        protected override void SetupTree()
        {
            base.SetupTree();

            List<Node> rootNodes = new List<Node>();

            if (isLeader)
            {
                rootNodes.Add(new InputMoveNode(this));
            }
            else
            {
                rootNodes.Add(new ForceFollowNode(this));
            }

            rootNodes.Add(CreateCombatBranch());

            if (!isLeader && !testSkill)
            {
                rootNodes.Add(new FollowLeaderNode(this));
            }

            root = new Selector(rootNodes);
        }

        public void TestFirstSkill()
        {
            if (testSkillIds.Count < 1) return;
            this.Raise(new PlayTimelineEvent(){ Id = testSkillIds[0]});
        }

        public void TestSecondSkill()
        {
            if (testSkillIds.Count < 2) return;
            this.Raise(new PlayTimelineEvent(){ Id = testSkillIds[1]});
        }

        public void TestThirdSkill()
        {
            if (testSkillIds.Count < 3) return;
            this.Raise(new PlayTimelineEvent(){ Id = testSkillIds[2]});
        }

        public void TestForthSkill()
        {
            if (testSkillIds.Count < 4) return;
            this.Raise(new PlayTimelineEvent(){ Id = testSkillIds[3]});
        }
    
    }
}