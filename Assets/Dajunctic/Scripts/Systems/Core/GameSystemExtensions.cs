using UnityEngine;

namespace Dajunctic
{
    public static class GameSystemExtensions
    {
        /// <summary>
        /// Extension method to easily retrieve a system from GameSystemManager.
        /// Usage: var economy = this.GetSystem<EconomySystem>();
        /// </summary>
        public static T GetSystem<T>(this Component component) where T : class, IGameSystem
        {
            if (GameSystemManager.Instance == null)
            {
                Debug.LogError("GameSystemManager Instance is null! Cannot get system.");
                return null;
            }
            return GameSystemManager.Instance.GetSystem<T>();
        }

        public static T GetSystem<T>(this object obj) where T : class, IGameSystem
        {
            if (GameSystemManager.Instance == null)
            {
                Debug.LogError("GameSystemManager Instance is null! Cannot get system.");
                return null;
            }
            return GameSystemManager.Instance.GetSystem<T>();
        }
    }
}
