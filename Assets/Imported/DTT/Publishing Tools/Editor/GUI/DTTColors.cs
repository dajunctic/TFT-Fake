#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Contains pre-defined colors used for styling your GUI.
    /// </summary>
    public static class DTTColors
    {
        #region InnerClasses
        /// <summary>
        /// Holds colors used in the light theme of Unity.
        /// </summary>
        public class LightColors
        {
            /// <summary>
            /// The light dtt inspector color.
            /// </summary>
            public readonly Color inspector = new Color32(235, 235, 235, 255);

            /// <summary>
            /// The light line color.
            /// </summary>
            public readonly Color line = new Color32(127, 127, 127, 255);

            /// <summary>
            /// The light dtt red color.
            /// </summary>
            public readonly Color red = new Color32(235, 83, 64, 255);
        }

        /// <summary>
        /// Holds colors used for the dark theme of Unity.
        /// </summary>
        public class DarkColors
        {
            /// <summary>
            /// The dark dtt inspector color.
            /// </summary>
            public readonly Color inspector = new Color32(40, 40, 40, 255);

            /// <summary>
            /// The dark line color.
            /// </summary>
            public readonly Color line = new Color32(26, 26, 26, 255);

            /// <summary>
            /// The dark dtt red color.
            /// </summary>
            public readonly Color red = new Color32(208, 83, 64, 255);
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// A grey label color.
        /// </summary>
        public static readonly Color labelGrey;

        /// <summary>
        /// The DTT red color.
        /// </summary>
        public static Color DTTRed => EditorGUIUtility.isProSkin ? dark.red : light.red;

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
        static DTTColors()
        {
            labelGrey = new Color32(196, 196, 196, 255);

            dark = new DarkColors();
            light = new LightColors();
        }
        #endregion
    }
}

#endif