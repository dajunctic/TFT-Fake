#if UNITY_EDITOR

using DTT.Utils.Optimization;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Contains the textures used as part of the DTT styling. 
    /// </summary>
    public static class DTTTextures
    {
        #region InnerClasses
        /// <summary>
        /// Holds the textures used for the Unity light theme.
        /// </summary>
        public class LightTextures : LazyTexture2DCache
        {
            /// <summary>
            /// The light theme logo texture.
            /// </summary>
            public Texture2D Logo => base[nameof(Logo)];

            /// <summary>
            /// The light theme card header texture.
            /// </summary>
            public Texture2D CardHeader => base[nameof(CardHeader)];

            /// <summary>
            /// The light theme card body texture.
            /// </summary>
            public Texture2D CardBody => base[nameof(CardBody)];

            /// <summary>
            /// Initializes the light theme textures.
            /// </summary>
            public LightTextures()
            {
                Add(nameof(Logo), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "logo_dark.png")));

                Add(nameof(CardHeader), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "card_header_light.png")));

                Add(nameof(CardBody), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "card_body_light.png")));
            }
        }

        /// <summary>
        /// Holds the textures used for the Unity dark theme.
        /// </summary>
        public class DarkTextures : LazyTexture2DCache
        {
            /// <summary>
            /// The dark theme logo texture.
            /// </summary>
            public Texture2D Logo => base[nameof(Logo)];

            /// <summary>
            /// The dark theme card header texture.
            /// </summary>
            public Texture2D CardHeader => base[nameof(CardHeader)];

            /// <summary>
            /// The dark theme card body texture.
            /// </summary>
            public Texture2D CardBody => base[nameof(CardBody)];

            /// <summary>
            /// Initializes the dark theme textures.
            /// </summary>
            public DarkTextures()
            {
                Add(nameof(Logo), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "logo_light.png")));

                Add(nameof(CardHeader), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "card_header_dark.png")));

                Add(nameof(CardBody), () => AssetDatabase.LoadAssetAtPath<Texture2D>(
                    Path.Combine(DTTEditorConfig.ArtFolder, "card_body_dark.png")));
            }
        }
        #endregion
        /// <summary>
        /// The DTT icon, without background.
        /// </summary>
        public static readonly Texture icon;

        /// <summary>
        /// Holds the textures used for the Unity light theme.
        /// </summary>
        public static readonly LightTextures light;

        /// <summary>
        /// Holds the textures used for the Unity dark theme.
        /// </summary>
        public static readonly DarkTextures dark;

        /// <summary>
        /// Loads the textures.
        /// </summary>
        static DTTTextures()
        {
            icon = AssetDatabase.LoadAssetAtPath<Texture>(Path.Combine(DTTEditorConfig.ArtFolder, "icon-large.png"));
            light = new LightTextures();
            dark = new DarkTextures();
        }
    }
}

#endif