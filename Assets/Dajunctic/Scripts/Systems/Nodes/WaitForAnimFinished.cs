using UnityEngine;

namespace Dajunctic
{
    public class WaitForAnimFinished : CustomYieldInstruction
    {
        private CombatActor _actor;

        public WaitForAnimFinished(CombatActor actor)
        {
            _actor = actor;
        }

        public override bool keepWaiting => !_actor.IsAnimFinished;
    }
}