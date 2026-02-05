namespace Dajunctic
{
    public interface ILifeCycle
    {
        bool IsInitialized {get; }
        void Initialize();
        void Cleanup();
    }
}