using Dajunctic;

namespace Dajunctic
{
    public class WorldSystemView: BaseView
    {
        WorldSystem _system;
        public override void Initialize()
        {
            base.Initialize();
            _system = new WorldSystem();
            _system.LoadHero();
        }

        
    }
}