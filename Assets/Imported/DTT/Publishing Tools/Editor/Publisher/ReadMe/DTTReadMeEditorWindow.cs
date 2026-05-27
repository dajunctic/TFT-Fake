#if UNITY_EDITOR

using DTT.Utils.EditorUtilities;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Draws the editor readme content.
    /// </summary>
    public class DTTReadMeEditorWindow : EditorWindow
    {
        /// <summary>
        /// The read me target to be edited.
        /// </summary>
        [SerializeField]
        private DTTReadMe _readMe;

        /// <summary>
        /// The asset json of the package the readme is for.
        /// </summary>
        [SerializeField]
        private AssetJson _assetJson;

        /// <summary>
        /// The styles to use for displaying the readme window.
        /// </summary>
        private DTTReadMeStyles _styles;

        /// <summary>
        /// The custom icon that can be used for the ReadMe header.
        /// </summary>
        private Texture _customIcon;

        /// <summary>
        /// The minimum size of this window.
        /// </summary>
        private readonly Vector2 _minSize = new Vector2(500, 450);

        /// <summary>
        /// The current scroll position off the readme.
        /// </summary>
        private Vector2 _scrollPosition = Vector2.zero;

        /// <summary>
        /// The regular expression used for matching content in readme sections.
        /// </summary>
        private readonly Regex _contentMatcher = new Regex(@"\[(.*?)\]");

        /// <summary>
        /// The character used to identify a link in readme content.
        /// </summary>
        private const string LINK_CHARACTER = "l";

        /// <summary>
        /// The character used to identify an image in readme content.
        /// </summary>
        private const string IMAGE_CHARACTER = "i";

        /// <summary>
        /// The message shown if no paragraphs were added to the text property in the readme section json.
        /// </summary>
        private const string MISSING_TEXT_MESSAGE = "<color=red>The readme section did not have any paragraphs. " +
                                                    "Make sure to surround your text with square brackets.</color>";
        
        /// <summary>
        /// The formatted missing property message.
        /// </summary>
        private const string MISSING_PROPERTY_MESSAGE = "<color=red>Missing required {0} property</color>";
        
        /// <summary>
        /// Opens the window to show the readme of given asset json, 
        /// docking it onto the inspector window if possible.
        /// </summary>
        /// <returns>The window instance.</returns>
        public static DTTReadMeEditorWindow Open(AssetJson assetJson)
        {
            Assembly editorAssembly = typeof(Editor).Assembly;
            Type inspectorType = editorAssembly.GetType("UnityEditor.InspectorWindow");

            string windowName = assetJson.displayName + " ReadMe";
            DTTReadMeEditorWindow window = GetWindow<DTTReadMeEditorWindow>(windowName, true, inspectorType);
            window.Initialize(assetJson);
            window.titleContent = new GUIContent(windowName);

            return window;
        }

        /// <summary>
        /// Initializes the window state.
        /// </summary>
        private void OnEnable()
        {
            if (_assetJson != null)
                _readMe.Initialize(_assetJson);

            _styles = new DTTReadMeStyles();

            minSize = _minSize;
        }

        /// <summary>
        /// Draws the ReadMe card.
        /// </summary>
        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            DTTGUILayout.CardHeader(DrawDTTHeaderContent);
            DTTGUILayout.CardBody(DrawSections);
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Initializes the content to be shown based on the given asset json.
        /// </summary>
        /// <param name="assetJson">The asset json of a package.</param>
        private void Initialize(AssetJson assetJson)
        {
            _assetJson = assetJson;
            _readMe = new DTTReadMe();
            _readMe.Initialize(_assetJson);

            if (_assetJson.customIconPath != null)
            {
                // If the asset json has a custom icon path defined, try loading the icon at that path.
                string iconLoadPath = Path.Combine(DTTEditorConfig.GetContentFolderPath(_assetJson), _assetJson.customIconPath);
                _customIcon = AssetDatabase.LoadAssetAtPath<Texture>(iconLoadPath);
            }
        }

        /// <summary>
        /// Draws the DTT header content.
        /// </summary>
        private void DrawDTTHeaderContent()
        {
            Texture icon;

            // Use the custom icon if it isn't null. Otherwise use the DTT logo based on theme.
            // Can't use null coalescing (??) operator because Unity will throw a missing reference
            // exception because of it.
            if (_customIcon != null)
                icon = _customIcon;
            else
                icon = EditorGUIUtility.isProSkin ? DTTTextures.dark.Logo : DTTTextures.light.Logo;

            Vector2 size = new Vector2(icon.width * 0.5f, icon.height * 0.5f);

            EditorGUILayout.BeginHorizontal(GUILayout.Width(size.x));
            DrawIcon(icon, size);
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws the given icon in given icon size.
        /// </summary>
        /// <param name="icon">The icon to draw.</param>
        /// <param name="iconSize">The size in which to draw the icon.</param>
        private void DrawIcon(Texture icon, Vector2 iconSize)
        {
            Rect iconRect = GUILayoutUtility.GetRect(iconSize.x, iconSize.y);
            GUI.DrawTexture(iconRect, icon);
        }

        /// <summary>
        /// Draws the different readme sections.
        /// </summary>
        private void DrawSections()
        {
            foreach (ReadMeSection section in _readMe.loadedSections)
            {
                DrawSectionHeader(section);
                DrawSectionContent(section);
            }
        }

        /// <summary>
        /// Draws the header for given section.
        /// </summary>
        /// <param name="section">The section to draw the header for.</param>
        private void DrawSectionHeader(ReadMeSection section)
        {
            string contentTitle = string.IsNullOrEmpty(section.content.title)
                ? string.Format(MISSING_PROPERTY_MESSAGE, nameof(section.content.title))
                : section.content.title;
            
            EditorGUILayout.Space();
            GUILayout.Label(contentTitle, _styles.ContentTitle);
            RectOffset margin = new RectOffset(2, 2, 0, 0);
            GUIDrawTools.Separator(EditorGUIUtility.isProSkin ? DTTColors.light.line : GUIColors.light.unityInspector, margin);
        }

        /// <summary>
        /// Draws the content for given section.
        /// </summary>
        /// <param name="section">The section to draw content for.</param>
        private void DrawSectionContent(ReadMeSection section)
        {
            if (string.IsNullOrEmpty(section.content.text))
            {
                GUILayout.Label(string.Format(MISSING_PROPERTY_MESSAGE, nameof(section.content.text)), _styles.Content);
                return;
            }
            
            MatchCollection matches = _contentMatcher.Matches(section.content.text);
            if (matches.Count == 0)
            {
                GUILayout.Label(MISSING_TEXT_MESSAGE, _styles.Content);
                return;
            }
            
            int linkCount = 0;
            int imageCount = 0;
            
            foreach (Match match in matches)
            {
                string content = match.Groups[1].Value;
                switch (content)
                {
                    case LINK_CHARACTER:
                        ReadMeSection.LinkContent link = section.GetLink(linkCount++);
                        DrawLinkContent(link);
                        break;

                    case IMAGE_CHARACTER:
                        Texture image = section.GetImage(imageCount++, _readMe.SectionsFolder);
                        if (image != null)
                            DrawImage(image);
                        break;

                    default:
                        GUILayout.Label(content, _styles.Content);
                        break;
                }
            }
        }

        /// <summary>
        /// Draws the GUI for given link content.
        /// </summary>
        /// <param name="link">The link content to draw the GUI of.</param>
        private void DrawLinkContent(ReadMeSection.LinkContent link)
        {
            if (link != null)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    DrawPrefix(link.prefix);
                    DrawLink(link);
                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                Debug.LogWarning("Link was null. Make sure the links match the [l] amount in your text.");
            }
        }

        /// <summary>
        /// Draws the given prefix of a link.
        /// </summary>
        /// <param name="prefix">The prefix of the link.</param>
        private void DrawPrefix(string prefix)
        {
            if (!string.IsNullOrEmpty(prefix))
                GUILayout.Label(prefix, _styles.Content);
        }

        /// <summary>
        /// Draws an image on screen using its texture width and height. 
        /// </summary>
        /// <param name="texture">The image to draw.</param>
        private void DrawImage(Texture texture)
        {
            Rect rect = GUILayoutUtility.GetRect(texture.width, texture.height);
            GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit);
        }

        /// <summary>
        /// Draws the link label of given link content.
        /// </summary>
        /// <param name="link">The link content.</param>
        private void DrawLink(ReadMeSection.LinkContent link)
        {
            GUIStyle style = link.inline ? _styles.InlineContentLink : _styles.ContentLink;
            if (GUIDrawTools.LinkLabel(link.text, style))
            {
                if (link.url.StartsWith("mailto:"))
                    Application.OpenURL(link.url);
                else
                    DTTEditorConfig.OpenPackageLink(_assetJson, link.url);
            }
        }
    }
}

#endif