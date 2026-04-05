#if UNITY_EDITOR

using System;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// A static class containing extension methods usable for creating GUI content.
    /// </summary>
    public static class GUIContentExtensions
    {
        #region Methods
        #region Public
        /// <summary>
        /// Returns the size in the GUI when drawn with given style.
        /// </summary>
        /// <param name="content">The content to be drawn.</param>
        /// <param name="style">The style the content is to be drawn with.</param>
        /// <returns>The size in the GUI.</returns>
        public static Vector2 GetGUISize(this string content, GUIStyle style)
        {
            if (style == null)
                throw new ArgumentNullException($"The style value to calculate {content}  was null.");
            if (content == null)
                throw new ArgumentNullException("The content was null when calculating size.");

            return style.CalcSize(new GUIContent(content));
        }

        /// <summary>
        /// Returns the size in the GUI when drawn with given style.
        /// </summary>
        /// <param name="content">The content to be drawn.</param>
        /// <param name="style">The style the content is to be drawn with.</param>
        /// <returns>The size in the GUI.</returns>
        public static Vector2 GetGUISize(this GUIContent content, GUIStyle style)
        {
            if (style == null)
                throw new ArgumentNullException($"The style value to calculate {content}  was null.");
            if (content == null)
                throw new ArgumentNullException("The content was null when calculating size.");

            return style.CalcSize(content);
        }
        #endregion
        #endregion
    }
}

#endif