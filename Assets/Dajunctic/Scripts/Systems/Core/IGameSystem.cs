namespace Dajunctic
{
    /// <summary>
    /// Base interface for all game systems.
    /// Systems are initialized and managed by GameSystemManager.
    /// </summary>
    public interface IGameSystem
    {
        /// <summary>
        /// Called when the system is initialized by GameSystemManager.
        /// Use this to get references to other systems and set up dependencies.
        /// </summary>
        void Initialize(GameSystemManager manager);

        /// <summary>
        /// Called when the system is being shut down.
        /// Use this to clean up resources and unsubscribe from events.
        /// </summary>
        void Shutdown();
    }
}
