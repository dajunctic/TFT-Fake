namespace Dajunctic.SkillSystem.Logic
{
    public class OrNode : AbilityNode
    {
        bool _passed;

        protected override void ResetInternal()
        {
            base.ResetInternal();
            _passed = false;
        }

        protected override void PlayInternal()
        {
            Completed();
        }

        public override void OnInNodeCompleted(AbilityNode node)
        {
            if (_passed) return;
            _passed = true;
            Play();
        }
    }
}
