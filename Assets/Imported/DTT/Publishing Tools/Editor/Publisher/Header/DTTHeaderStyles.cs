#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Contains the styles used for drawing the header.
    /// </summary>
    internal class DTTHeaderStyles : GUIStyleCache
    {
        #region Variables
        #region Public
        /// <summary>
        /// Used for drawing a label inside the header.
        /// </summary>
        public GUIStyle Label => base[nameof(Label)];

        /// <summary>
        /// Used for drawing the company name inside the header.
        /// </summary>
        public GUIStyle TitleLabel => base[nameof(TitleLabel)];

        /// <summary>
        /// The link style.
        /// </summary>
        public GUIStyle Link => base[nameof(Link)];
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of this object, initializing
        /// the styles.
        /// </summary>
        public DTTHeaderStyles()
        {
            Add(nameof(Label), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.label);
                style.fontSize = 11;
                style.normal.textColor = EditorGUIUtility.isProSkin ? GUIColors.light.unityInspector : Color.black;
                return style;
            });

            Add(nameof(TitleLabel), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.label);
                style.fontSize = 13;
                style.font = DTTGUI.TitleFont;
                style.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : DTTColors.dark.inspector;
                return style;
            });

            Add(nameof(Link), () =>
            {
                GUIStyle style = new GUIStyle(GUIDrawTools.styles.MiniLinkLabel);
                style.fontSize = 10;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = DTTColors.DTTRed;
                style.active.textColor = DTTColors.DTTRed;
                style.hover.textColor = DTTColors.DTTRed;
                return style;
            });
        }
        #endregion
    }
}

#endif