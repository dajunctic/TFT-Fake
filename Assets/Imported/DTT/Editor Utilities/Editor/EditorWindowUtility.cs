#if UNITY_EDITOR

using System;
using UnityEditor;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides utility methods and extensions for editor windows.
    /// </summary>
    public static class EditorWindowUtility
    {
        /// <summary>
        /// The inspector window type used for opening the inspector window.
        /// </summary>
        private static readonly Type _inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

        /// <summary>
        /// The type of the unity inspector.
        /// </summary>
        public static Type InspectorType => _inspectorType;

        /// <summary>
        /// Opens and focusses the Unity Inspector Window.
        /// </summary>
        public static void OpenInspectorWindow() => EditorWindow.GetWindow(_inspectorType).Focus();

        /// <summary>
        /// Get a window that will be docked next to the Unity Inspector Window.
        /// </summary>
        /// <typeparam name="T">The type of editor window to get.</typeparam>
        public static T GetInspectorWindow<T>() where T : EditorWindow => EditorWindow.GetWindow<T>(_inspectorType);

        /// <summary>
        /// Docks a window to the Unity Inspector Window.
        /// <para>Will fail if the inspector window is not opened.</para>
        /// </summary>
        /// <typeparam name="T">The type of window to dock.</typeparam>
        /// <param name="window">The window to dock.</param>
        /// <returns>Whether the docking succeeded.</returns>
        public static bool DockToInspector<T>(this T window) where T : EditorWindow => Dock(window, _inspectorType);

        /// <summary>
        /// Docks a window to another. Many prefered windows may be given.
        /// <para>Will fail if the prefered dock window is not opened.</para>
        /// </summary>
        /// <typeparam name="T">The type of the editor window.</typeparam>
        /// <param name="window">The window to dock.</param>
        /// <param name="preferedDockWindows">The prefered windows to dock to.</param>
        /// <returns>Whether the docking succeeded. This is only accurate on Unity 2020 or newer.</returns>
        public static bool Dock<T>(this T window, params Type[] preferedDockWindows) where T : EditorWindow
        {
            window.Close();

            T instance = EditorWindow.GetWindow<T>(window.titleContent.text, preferedDockWindows);
#if UNITY_2020_1_OR_NEWER
            return instance.docked;
#else
            return true;
#endif
        }
    }
}

#endif