#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides utility methods regarding unity scenes and prefab scenes.
    /// </summary>
    public static class EditorSceneUtility
    {
        #region Methods
        #region Public
        /// <summary>
        /// Returns whether the given component is part of a prefab scene.
        /// </summary>
        /// <param name="component">The component to check.</param>
        /// <returns>Whether the component is part of a prefab scene.</returns>
        public static bool IsPartOfPrefabScene(Component component)
        {
            if (component == null)
                throw new ArgumentNullException(nameof(component), "Given component to check is null.");

            return IsPartOfPrefabScene(component.gameObject);
        }

        /// <summary>
        /// Returns whether the given game object is part of a prefab scene.
        /// </summary>
        /// <param name="component">The game object to check.</param>
        /// <returns>Whether the game object is part of a prefab scene.</returns>
        public static bool IsPartOfPrefabScene(GameObject gameObject)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject), "Given game object to check is null.");

#pragma warning disable 0618
            return gameObject.scene.name == PrefabUtility.FindPrefabRoot(gameObject).name;
#pragma warning restore
        }
        #endregion
        #endregion
    }
}

#endif