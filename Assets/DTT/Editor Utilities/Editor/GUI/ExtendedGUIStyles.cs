#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Holds extra gui styles.
    /// </summary>
    public class ExtendedGUIStyles : GUIStyleCache
    {
        /// <summary>
        /// The style of a normal link label.
        /// </summary>
        public GUIStyle LinkLabel => base[nameof(LinkLabel)];

        /// <summary>
        /// The style of a small link label.
        /// </summary>
        public GUIStyle MiniLinkLabel => base[nameof(MiniLinkLabel)];

        /// <summary>
        /// The normal color of link text when not in pro mode.
        /// </summary>
        private readonly Color _lightBlueLinkColor = new Color32(0, 82, 255, 255);

        /// <summary>
        /// Initializes the styles.
        /// </summary>
        public ExtendedGUIStyles()
        {
            Add(nameof(LinkLabel), () =>
            {
                GUIStyle style = new GUIStyle(EditorStyles.linkLabel);
                style.contentOffset = new Vector2(0, -2f);
                style.clipping = TextClipping.Overflow;

                if (!EditorGUIUtility.isProSkin)
                {
                    style.normal.textColor = _lightBlueLinkColor;
                    style.hover.textColor = _lightBlueLinkColor;
                }

                return style;
            });

            Add(nameof(MiniLinkLabel), () =>
            {
                GUIStyle style = new GUIStyle(LinkLabel);
                style.fontSize = EditorStyles.miniLabel.fontSize;
                return style;
            });
        }
    }
}

#endif