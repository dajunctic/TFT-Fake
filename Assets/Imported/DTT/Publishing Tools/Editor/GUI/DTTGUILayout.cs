#if UNITY_EDITOR

using System;
using UnityEditor;

namespace DTT.PublishingTools
{
    /// <summary>
    /// A static class used for drawing in the graphical user interface
    /// in DTT style without use of rectangles.
    /// </summary>
    public static class DTTGUILayout
    {
        #region Methods
        #region Public
        /// <summary>
        /// Draws a card header with the content drawn by given action inside it.
        /// </summary>
        /// <param name="action">The action that draws the content inside.</param>
        public static void CardHeader(Action action)
        {
            EditorGUILayout.BeginHorizontal(DTTGUI.styles.CardHeader);
            EditorGUILayout.BeginVertical();
            action.Invoke();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a card body with the content drawn by given action inside it.
        /// </summary>
        /// <param name="action">The action that draws the content inside.</param>
        public static void CardBody(Action action)
        {
            EditorGUILayout.BeginHorizontal(DTTGUI.styles.CardBody);
            EditorGUILayout.BeginVertical();
            action.Invoke();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        #endregion
        #endregion
    }
}

#endif
