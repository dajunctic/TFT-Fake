#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Additional styles used for styling a GUI in DTT style. Styles
    /// will differ based on Unity's Dark/Light theme.
    /// </summary>
    public class DTTGUIStyles : GUIStyleCache
    {
        #region Variables
        #region Public
        /// <summary>
        /// A style for a normal label.
        /// </summary>
        public GUIStyle Label => base[nameof(Label)];

        /// <summary>
        /// A style for a label that uses the dtt title font.
        /// </summary>
        public GUIStyle TitleLabel => base[nameof(TitleLabel)];

        /// <summary>
        /// The style for a card header.
        /// </summary>
        public GUIStyle CardHeader => base[nameof(CardHeader)];

        /// <summary>
        /// The style for a card body.
        /// </summary>
        public GUIStyle CardBody => base[nameof(CardBody)];

        /// <summary>
        /// The style for a button.
        /// </summary>
        public GUIStyle Button => base[nameof(Button)];
        #endregion
        #region Private
        /// <summary>
        /// The normal color of link text when not in pro mode.
        /// </summary>
        private readonly Color _lightBlueLinkColor = new Color32(0, 82, 255, 255);
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance, initializing the styles.
        /// </summary>
        public DTTGUIStyles()
        {
            Add(nameof(Label), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.label);
                style.normal.textColor = EditorStyles.label.normal.textColor;
                style.fontSize = 12;
                return style;
            });

            Add(nameof(TitleLabel), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.label);
                style.font = DTTGUI.TitleFont;
                style.normal.textColor = EditorStyles.label.normal.textColor;
                style.fontSize = 12;
                return style;
            });

            Add(nameof(CardHeader), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.box);

                // No scaled backgrounds should be used, only use the card background texture.
                style.normal.scaledBackgrounds = null;
                style.normal.background = EditorGUIUtility.isProSkin ? DTTTextures.dark.CardHeader : DTTTextures.light.CardHeader;

                // The border property ensures the rounding of the edges persists with stretch.
                style.border = new RectOffset(16, 16, 16, 16);

                // Set the margin and padding to work with the card body.
                style.margin.top = 8;
                style.margin.left = 8;
                style.margin.right = 8;
                style.margin.bottom = 0;
                style.padding = new RectOffset(15, 15, 15, 15);
                return style;
            });

            Add(nameof(CardBody), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.box);

                // No scaled backgrounds should be used, only use the card background texture.
                style.normal.scaledBackgrounds = null;
                style.normal.background = EditorGUIUtility.isProSkin ? DTTTextures.dark.CardBody : DTTTextures.light.CardBody;

                // The border property ensures the rounding of the edges persists with stretch.
                style.border = new RectOffset(16, 16, 16, 16);

                // Set the margin and padding to work with the card header.
                style.margin.top = 0;
                style.margin.left = 8;
                style.margin.right = 8;
                style.padding = new RectOffset(15, 15, 15, 15);
                return style;
            });

            Add(nameof(Button), () =>
            {
                GUIStyle style = new GUIStyle(DTTGUI.Skin.button);
                style.normal = EditorStyles.miniButton.normal;
                style.hover = EditorStyles.miniButton.hover;
                style.active = EditorStyles.miniButton.active;
                return style;
            });
        }
        #endregion
    }
}

#endif