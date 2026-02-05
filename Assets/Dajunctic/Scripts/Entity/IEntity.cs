
namespace Dajunctic {
    public interface IEntity: IAssetId
    {
        public void Tick();
        public void Initialize();
        public void DoEnable();
        public void DoDisable();
        public void ListenEvents();
        public void StopListenEvents();
    }
}