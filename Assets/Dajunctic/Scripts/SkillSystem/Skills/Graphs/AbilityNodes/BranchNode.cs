using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GraphProcessor;

namespace Dajunctic.SkillSystem.Logic
{
    [System.Serializable, NodeMenuItem("Ability/Branch")]
    public class BranchNode : AbilityNode
    {
        [GraphProcessor.Input(name = "condition")] public bool condition;
        [SerializeField] bool random;
        [GraphProcessor.Output] private AbilityNode trueBranch;
        [GraphProcessor.Output] private AbilityNode falseBranch;

        private List<AbilityNode> _trueNodes;
        private List<AbilityNode> _falseNodes;

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            var truePort = outputPorts.FirstOrDefault(p => p.fieldName == nameof(trueBranch));
            _trueNodes = truePort?.GetEdges().Select(e => e.inputNode as AbilityNode).Where(n => n != null).ToList() ?? new List<AbilityNode>();
            
            var falsePort = outputPorts.FirstOrDefault(p => p.fieldName == nameof(falseBranch));
            _falseNodes = falsePort?.GetEdges().Select(e => e.inputNode as AbilityNode).Where(n => n != null).ToList() ?? new List<AbilityNode>();
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

