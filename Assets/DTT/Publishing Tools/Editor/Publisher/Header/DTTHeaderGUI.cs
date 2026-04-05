#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using DTT.Utils.Extensions;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Used for drawing the DTT header in the GUI.
    /// </summary>
    public class DTTHeaderGUI
    {
        #region Variables
        #region Public
        /// <summary>
        /// The height of the header.
        /// </summary>
        public const float HEADER_HEIGHT = 40f;
        #endregion
        #region Private
        /// <summary>
        /// The styles used when drawing the header.
        /// </summary>
        private readonly DTTHeaderStyles _styles;

        /// <summary>
        /// The content used for drawing the header.
        /// </summary>
        private readonly DTTHeaderContent _content;

        /// <summary>
        /// The custom icon that can be used in the header.
        /// </summary>
        private readonly Texture _customIcon;

        /// <summary>
        /// The asset json of the package displayed by the header.
        /// </summary>
        private AssetJson _assetJson;

        /// <summary>
        /// The margin between the icon and the labels.
        /// </summary>
        private const float LABEL_MARGIN = 5f;

        /// <summary>
        /// The total inspector margin (left + right) based on the
        /// default margin values used by unity. This is used to
        /// determine whether the inspector scroll bar is active.
        /// </summary>
        private const float TOTAL_HORIZONTAL_INSPECTOR_MARGIN = 23f;

        /// <summary>
        /// Whether the inspector scroll bar is active or not.
        /// </summary>
        private bool _activeInspectorScrollBar;

        /// <summary>
        /// The custom url towards the documentation.
        /// </summary>
        private readonly string _customDocumentationUrl;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes the header with the given asset json of the package, a custom display name
        /// and a custom documentation url.
        /// </summary>
        /// <param name="assetJson">The asset json of the package.</param>
        /// <param name="customDisplayName">The custom display name.</param>
        /// <param name="customDocumentationUrl">The custom documentation url.</param>
        /// <param name="customIconPath">
        /// The path towards a custom icon. 
        /// This value needs to be a local path relative to the package.
        /// </param>
        public DTTHeaderGUI(AssetJson assetJson, string customDisplayName, string customDocumentationUrl, string customIconPath)
        {
            _assetJson = assetJson;
            _styles = new DTTHeaderStyles();
            _customDocumentationUrl = customDocumentationUrl;

            // Use the custom display name if it isn't null. Otherwise use the asset json display name with a DTT prefix.
            string displayName = customDisplayName ?? "DTT " + assetJson.displayName.AddSpacesBeforeCapitals();
            string version = DTTEditorConfig.GetPackageVersion(assetJson.packageName);
            _content = new DTTHeaderContent(displayName, version);

            if (customIconPath != null)
            {
                // If the asset json has a custom icon path defined, try loading the icon at that path.
                string iconLoadPath = Path.Combine(DTTEditorConfig.GetContentFolderPath(_assetJson), customIconPath);
                _customIcon = AssetDatabase.LoadAssetAtPath<Texture>(iconLoadPath);
            }
        }

        /// <summary>
        /// Initializes the header with the given asset json of the package, a custom display name
        /// and a custom documentation url.
        /// </summary>
        /// <param name="assetJson">The asset json of the package.</param>
        /// <param name="customDisplayName">The custom display name.</param>
        public DTTHeaderGUI(AssetJson assetJson, string customDisplayName) : this(assetJson, customDisplayName, null, null)
        {
        }

        /// <summary>
        /// Initializes the header with the given asset json of the package, a custom display name
        /// and a custom documentation url.
        /// </summary>
        /// <param name="assetJson">The asset json of the package.</param>
        /// <param name="customDisplayName">The custom display name.</param>
        /// <param name="customDocumentationUrl">The custom documentation url.</param>
        public DTTHeaderGUI(AssetJson assetJson, string customDisplayName, string customDocumentationUrl)
            : this(assetJson, customDisplayName, customDocumentationUrl, null)
        {
        }

        /// <summary>
        /// Initializes the header with the given asset json of the package, a custom display name
        /// and a custom documentation url.
        /// </summary>
        /// <param name="assetJson">The asset json of the package.</param>
        public DTTHeaderGUI(AssetJson assetJson) : this(assetJson, null, null, null)
        {
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Draws the header at the top of the screen.
        /// </summary>
        public void OnGUI()
        {
            Rect rect = GetInitialRect();

            DrawBackground(rect);
            DrawContent(rect);
        }
        #endregion
        #region Private
        /// <summary>
        /// Returns the initial rectangle to draw the header in. Will also
        /// determine whether the inspector scroll bar is active or not.
        /// </summary>
        /// <returns>The initial rectangle to draw the header in.</returns>
        private Rect GetInitialRect()
        {
            float viewWidth = EditorGUIUtility.currentViewWidth;
            Rect rect = GUILayoutUtility.GetRect(viewWidth, HEADER_HEIGHT);

            // If the difference in width between the rectangle reserved for content
            // and the view is greater than the total horizontal inspector margin,
            // the inspector scroll bar is drawn.
            _activeInspectorScrollBar = viewWidth - rect.width >= TOTAL_HORIZONTAL_INSPECTOR_MARGIN;

            rect.x = rect.y = 0;
            rect.width = viewWidth;

            return rect;
        }

        /// <summary>
        /// Draws the background of the header.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawBackground(Rect initRect)
            => EditorGUI.DrawRect(initRect, EditorGUIUtility.isProSkin ? DTTColors.dark.inspector : DTTColors.light.inspector);

        /// <summary>
        /// Draws the header content inside the initial rectangle.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawContent(Rect initRect)
        {
            DrawIcon(initRect);

            DrawTitle(initRect);
            DrawVersion(initRect);

            DrawDocumentationLink(initRect);
        }

        /// <summary>
        /// Draws the DTT icon.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawIcon(Rect initRect)
        {
            Rect iconRect = new Rect(initRect);
            iconRect.width = initRect.height;

            if (_customIcon != null)
            {
                GUI.DrawTexture(iconRect, _customIcon);
            }
            else
            {
                EditorGUI.DrawRect(iconRect, DTTColors.DTTRed);
                GUI.DrawTexture(iconRect, DTTTextures.icon);
            }
        }

        /// <summary>
        /// Draws the title.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawTitle(Rect initRect)
        {
            Rect titleRect = new Rect(initRect);
            titleRect.x += HEADER_HEIGHT + LABEL_MARGIN;
            titleRect.y += LABEL_MARGIN;

            Vector2 size = _content.PackageNameLabel.GetGUISize(_styles.TitleLabel);
            titleRect.width = size.x;
            titleRect.height = size.y;

            GUI.Label(titleRect, _content.PackageNameLabel, _styles.TitleLabel);
        }

        /// <summary>
        /// Draws the documentation link.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawDocumentationLink(Rect initRect)
        {
            Rect linkRect = new Rect(initRect);
            linkRect.x += HEADER_HEIGHT + LABEL_MARGIN;
            linkRect.y += (HEADER_HEIGHT * 0.5f);
            linkRect.height = _content.DocumentationLabel.GetGUISize(_styles.Link).y;

            if (GUIDrawTools.LinkLabel(linkRect, _content.DocumentationLabel, _styles.Link))
            {
                if (_customDocumentationUrl != null)
                    DTTEditorConfig.OpenPackageLink(_assetJson, _customDocumentationUrl);
                else
                    DTTEditorConfig.OpenPackageDocumentation(_assetJson);
            }
        }

        /// <summary>
        /// Draws the package version.
        /// </summary>
        /// <param name="initRect">The initial rectangle the header is drawn in.</param>
        private void DrawVersion(Rect initRect)
        {
            float scrollBarOffset = _activeInspectorScrollBar ? 18f : 0f;
            Vector2 size = _content.VersionLabel.GetGUISize(_styles.Label);
            Rect versionRect = new Rect(initRect);
            versionRect.x = initRect.xMax - size.x - LABEL_MARGIN - scrollBarOffset;
            versionRect.width = size.x;
            versionRect.height = size.y;
            versionRect.y += LABEL_MARGIN;

            GUI.Label(versionRect, _content.VersionLabel, _styles.Label);
        }
        #endregion
        #endregion
    }
}

#endif
