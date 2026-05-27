#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// A static class used for drawing the graphical user interface
    /// in DTT style.
    /// </summary>
    public static class DTTGUI
    {
        /// <summary>
        /// The skin used by DTT.
        /// </summary>
        public static GUISkin Skin
        {
            get
            {
                if (_skin == null)
                    _skin = AssetDatabase.LoadAssetAtPath<GUISkin>(DTTEditorConfig.SkinPath);

                return _skin;
            }
        }


        /// <summary>
        /// The title font used by DTT.
        /// </summary>
        public static Font TitleFont
        {
            get
            {
                if (_titleFont == null)
                    _titleFont = AssetDatabase.LoadAssetAtPath<Font>(Path.Combine(DTTEditorConfig.FontFolder, "Montserrat", "Montserrat-ExtraBold.ttf"));

                return _titleFont;
            }
        }

        /// <summary>
        /// Additional styles used for styling a GUI in DTT style. Styles
        /// will differ based on Unity's Dark/Light theme.
        /// </summary>
        public static readonly DTTGUIStyles styles;

        /// <summary>
        /// The skin used by DTT.
        /// </summary>
        private static GUISkin _skin;

        /// <summary>
        /// The title font used by DTT.
        /// </summary>
        private static Font _titleFont;

        /// <summary>
        /// Creates the static instance, initializing the static state.
        /// </summary>
        static DTTGUI() => styles = new DTTGUIStyles();
    }
}

#endif