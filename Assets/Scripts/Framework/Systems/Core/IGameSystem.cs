using System.Threading.Tasks;

namespace Dajunctic
{

    public interface IGameSystem
    {

        Task LoadDataAsync();

        void Initialize(GameSystemManager manager);

        void Shutdown();
    }
}
