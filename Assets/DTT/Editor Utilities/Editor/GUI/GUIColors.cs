#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Contains pre-defined colors used for styling your GUI.
    /// </summary>
    public static class GUIColors
    {
        #region InnerClasses
        /// <summary>
        /// Holds colors used in the light theme of Unity.
        /// </summary>
        public class LightColors
        {
            /// <summary>
            /// The light unity inspector color.
            /// </summary>
            public readonly Color unityInspector = new Color32(203, 203, 203, 255);

            /// <summary>
            /// The light line color.
            /// </summary>
            public readonly Color line = new Color32(127, 127, 127, 255);
        }

        /// <summary>
        /// Holds colors used for the dark theme of Unity.
        /// </summary>
        public class DarkColors
        {
            /// <summary>
            /// The dark unity inspector color.
            /// </summary>
            public readonly Color unityInspector = new Color32(62, 62, 62, 255);

            /// <summary>
            /// The dark line color.
            /// </summary>
            public readonly Color line = new Color32(26, 26, 26, 255);
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// The color used for end lines in the inspector.
        /// </summary>
        public static Color LineColor => EditorGUIUtility.isProSkin ? light.line : dark.line;

        /// <summary>
        /// Holds colors used in the dark theme of Unity.
        /// </summary>
        public static readonly DarkColors dark;

        /// <summary>
        /// Holds colors used in the light theme of Unity.
        /// </summary>
        public static readonly LightColors light;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates the static instance, initializing the field color values.
        /// </summary>
        static GUIColors()
        {
            dark = new DarkColors();
            light = new LightColors();
        }
        #endregion
    }
}

#endif