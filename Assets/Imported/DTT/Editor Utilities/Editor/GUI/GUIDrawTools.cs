#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides extended GUI drawing tools for editor and editor usage.
    /// </summary>
    public static class GUIDrawTools
    {
        /// <summary>
        /// Extended GUI styles.
        /// </summary>
        public static readonly ExtendedGUIStyles styles;

        /// <summary>
        /// Initializes the styles.
        /// </summary>
        static GUIDrawTools() => styles = new ExtendedGUIStyles();

        #region Methods
        #region Public
        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="rect">The position.</param>
        /// <param name="label">The label text.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(Rect rect, string label) => LinkLabel(rect, new GUIContent(label), styles.LinkLabel);

        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="rect">The position.</param>
        /// <param name="content">The text content.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(Rect rect, GUIContent content) => LinkLabel(rect, content, styles.LinkLabel);

        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="rect">The position.</param>
        /// <param name="label">The text content.</param>
        /// <param name="linkStyle">The link style.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(Rect rect, string label, GUIStyle linkStyle) => LinkLabel(rect, new GUIContent(label), linkStyle);

        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="rect">The position.</param>
        /// <param name="content">The text content.</param>
        /// <param name="linkStyle">The link style.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(Rect rect, GUIContent content, GUIStyle linkStyle)
        {
            float width = linkStyle.CalcSize(content).x;
            Rect textRect = new Rect(rect);
            textRect.xMax = rect.xMin + width;

            Vector2 offset = linkStyle.contentOffset;
            RectOffset padding = linkStyle.padding;

            Vector3 start = new Vector2(rect.xMin + padding.left + offset.x, rect.yMax + offset.y);
            Vector3 end = new Vector2(textRect.xMax - padding.right, rect.yMax + offset.y);

            Handles.BeginGUI();
            Handles.color = linkStyle.normal.textColor;
            Handles.DrawLine(start, end);
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(textRect, MouseCursor.Link);

            return GUI.Button(textRect, content, linkStyle);
        }

        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="label">The label text.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(string label) => LinkLabel(new GUIContent(label), styles.LinkLabel);

        /// <summary>
        /// Draws a clickable link label.
        /// </summary>
        /// <param name="content">The text content.</param>
        /// <returns>Whether the link label has been clicked.</returns>
        public static bool LinkLabel(GUIContent content) => LinkLabel(content, styles.LinkLabel);

        /// <summary>
        /// Draws a clickable label in given link style.
        /// </summary>
        /// <param name="label">The label text.</param>
        /// <param name="linkStyle">The link style.</param>
        /// <returns>Whether the link has been clicked.</returns>
        public static bool LinkLabel(string label, GUIStyle linkStyle) => LinkLabel(new GUIContent(label), linkStyle);

        /// <summary>
        /// Draws a clickable label in given link style.
        /// </summary>
        /// <param name="labelContent">The label content text.</param>
        /// <param name="linkStyle">The link style.</param>
        /// <returns>Whether the link has been clicked.</returns>
        public static bool LinkLabel(GUIContent labelContent, GUIStyle linkStyle)
        {
            Vector2 size = labelContent.GetGUISize(linkStyle);
            Rect rect = GUILayoutUtility.GetRect(size.x, size.y);
            rect.x += linkStyle.padding.left;

            return LinkLabel(rect, labelContent, linkStyle);
        }

        /// <summary>
        /// Draws a horizontal separating line in given color.
        /// </summary>
        /// <param name="color">The color in which to draw the line.</param>
        /// <param name="margin">The margin relative to the view it is in.</param>
        public static void Separator(Color color, RectOffset margin = null)
        {
            Rect rect = GUILayoutUtility.GetRect(GUILayoutUtility.GetLastRect().x, 1f);
            Separator(rect, color, margin);
        }

        /// <summary>
        /// Draws a horizontal separating line in the current editor theme color.
        /// </summary>
        /// <param name="margin">The margin relative to the view it is in.</param>
        public static void Separator(RectOffset margin = null)
        {
            Color color = EditorGUIUtility.isProSkin ? GUIColors.light.line : GUIColors.light.unityInspector;
            Separator(color, margin);
        }

        /// <summary>
        /// Draws a horizontal separating line in given color.
        /// </summary>
        /// <param name="rect">The rectangle view in which to draw the line.</param>
        /// <param name="margin">The margin relative to the view it is in.</param>
        public static void Separator(Rect rect, RectOffset margin = null)
        {
            Color color = EditorGUIUtility.isProSkin ? GUIColors.light.line : GUIColors.light.unityInspector;
            Separator(rect, color, margin);
        }
        
        /// <summary>
        /// Draws a horizontal separating line in given color.
        /// </summary>
        /// <param name="rect">The rectangle view in which to draw the line.</param>
        /// <param name="color">The color in which to draw the line.</param>
        /// <param name="margin">The margin relative to the view it is in.</param>
        public static void Separator(Rect rect, Color color, RectOffset margin = null)
        {
            RectOffset offset = margin ?? new RectOffset();

            Color originalColor = Handles.color;

            Handles.BeginGUI();
            Handles.color = color;

            float yPosition = rect.yMax - offset.top + offset.bottom;
            Vector3 left = new Vector3(rect.xMin + offset.left, yPosition);
            Vector3 right = new Vector3(rect.xMax - offset.right, yPosition);
            Handles.DrawLine(left, right);

            Handles.color = originalColor;
            Handles.EndGUI();
        }

        /// <summary>
        /// Draws a search field you can use to filter lists.
        /// </summary>
        /// <param name="searchText">The current search text.</param>
        /// <returns>The updated search text.</returns>
        public static string SearchField(string searchText) => EditorGUILayout.TextField(searchText, EditorStyles.toolbarSearchField);

        /// <summary>
        /// Draws a search field you can use to filter lists. It outputs whether
        /// the output string has been updated or not so operations based on a
        /// search string update are only executed once.
        /// </summary>
        /// <param name="searchText">The current search text.</param>
        /// <param name="updatedSearchText">Whether the output string has been updated.</param>
        /// <returns>The updated search text.</returns>
        public static string SearchField(string searchText, out bool updatedSearchText)
        {
            string enteredText = EditorGUILayout.TextField(searchText, EditorStyles.toolbarSearchField);
            updatedSearchText = enteredText != searchText;
            return enteredText;
        }
        #endregion
        #endregion
    }
}

#endif
