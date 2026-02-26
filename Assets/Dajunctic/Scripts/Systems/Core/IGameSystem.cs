using System.Threading.Tasks;

namespace Dajunctic
{
    /// <summary>
    /// Base interface for all game systems.
    /// Systems are created and managed by GameSystemManager.
    /// </summary>
    public interface IGameSystem
    {
        /// <summary>
        /// Load system-specific ScriptableObject data via Addressables.
        /// Called before Initialize() so data is ready for cross-system wiring.
        /// </summary>
        Task LoadDataAsync();

        /// <summary>
        /// Called after all data is loaded. Wire up cross-system dependencies here.
        /// </summary>
        void Initialize(GameSystemManager manager);

        /// <summary>
        /// Called when the system is being shut down. Clean up resources and events.
        /// </summary>
        void Shutdown();
    }
}
