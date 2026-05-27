namespace Dajunctic
{
    public interface ILifeCycle
    {
        bool Initialized {get; }
        void Initialize();
        void Cleanup();
    }
}
