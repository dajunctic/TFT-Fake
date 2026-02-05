using Dajunctic;

namespace Dajunctic
{
    public abstract class Tree: BaseView
    {
        private Node _root = null;

        public override void Initialize()
        {
            base.Initialize();
            _root = SetupTree();
        }

        public override void Tick()
        {
            base.Tick();

            if (_root != null)
            {
                _root.Evaluate();
            }


        }

        protected abstract Node SetupTree();
    }
}