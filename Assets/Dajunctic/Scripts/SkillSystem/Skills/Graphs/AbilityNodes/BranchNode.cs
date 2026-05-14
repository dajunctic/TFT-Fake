using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;

namespace Dajunctic.SkillSystem.Logic
{
    public class BranchNode : AbilityNode
    {
        [SerializeField, Input] bool condition;
        [SerializeField] bool random;
        [SerializeReference, Output] private AbilityNode trueBranch;
        [SerializeReference, Output] private AbilityNode falseBranch;

        private List<AbilityNode> _trueNodes;
        private List<AbilityNode> _falseNodes;

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            _trueNodes = GetOutputPort(nameof(trueBranch)).GetConnections().Select(port => port.node).OfType<AbilityNode>().ToList();
            _falseNodes = GetOutputPort(nameof(falseBranch)).GetConnections().Select(port => port.node).OfType<AbilityNode>().ToList();
        }

        protected override void CleanupInternal()
        {
            _trueNodes = null;
            _falseNodes = null;
            base.CleanupInternal();
        }

        protected override void PlayInternal()
        {
            var inCondition = GetInputValue(nameof(condition), condition);

            if (random) inCondition = CryptoRandom.value < 0.5f;

            Stop();

            var nodesToTrigger = inCondition ? _trueNodes : _falseNodes;

            foreach (var node in nodesToTrigger)
            {
                node.OnInNodeCompleted(this);
            }

            Completed();
        }
    }
}

