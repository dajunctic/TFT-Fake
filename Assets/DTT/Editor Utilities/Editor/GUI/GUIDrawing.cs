#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides simplified drawing operations using draw actions.
    /// </summary>
    public static class GUIDrawing
    {
        /// <summary>
        /// The name of the script property.
        /// </summary>
        private const string SCRIPT_PROPERTY_NAME = "m_Script";

        /// <summary>
        /// Draws the properties of the serialized object. Will update the serialized object if 
        /// any property changes have occured.
        /// </summary>
        /// <param name="serializedObject">The serialized object of which to draw the properties.</param>
        /// <param name="propertiesToIgnore">The names of property to not draw.</param>
        /// <returns>A value has been changed when drawing the properties.</returns>
        public static bool DrawProperties(this SerializedObject serializedObject, params string[] propertiesToIgnore)
            => DrawProperties(serializedObject, true, propertiesToIgnore);

        /// <summary>
        /// Draws the properties of the serialized object. Will update the serialized object if 
        /// any property changes have occured.
        /// </summary>
        /// <param name="serializedObject">The serialized object of which to draw the properties.</param>
        /// <param name="disableScriptProperty">Whether the script property should be drawn.</param>
        /// <returns>A value has been changed when drawing the properties.</returns>
        public static bool DrawProperties(this SerializedObject serializedObject, bool disableScriptProperty = true)
            => DrawProperties(serializedObject, disableScriptProperty, Array.Empty<string>());

        /// <summary>
        /// Draws the properties of the serialized object. Will update the serialized object if 
        /// any property changes have occured.
        /// </summary>
        /// <param name="serializedObject">The serialized object of which to draw the properties.</param>
        /// <param name="disableScriptProperty">Whether the script property should be drawn.</param>
        /// <param name="propertiesToIgnore">The names of property to not draw.</param>
        /// <returns>A value has been changed when drawing the properties.</returns>
        public static bool DrawProperties(this SerializedObject serializedObject, bool disableScriptProperty, 
            params string[] propertiesToIgnore)
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            SerializedProperty property = serializedObject.GetIterator();
            bool enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (propertiesToIgnore.Contains(property.name))
                    continue;
                
                if (disableScriptProperty)
                {
                    EditorGUI.BeginDisabledGroup(property.name == SCRIPT_PROPERTY_NAME);
                    EditorGUILayout.PropertyField(property);
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    EditorGUILayout.PropertyField(property);
                }
            }

            bool changed = EditorGUI.EndChangeCheck();
            if (changed)
                serializedObject.ApplyModifiedProperties();

            return changed;
        }
        
        /// <summary>
        /// Draws everything in your draw action using the given gui color. 
        /// <para>This modifies <see cref="GUI.color"/> and resets.</para>
        /// </summary>
        /// <param name="guiColor">The color in which to draw the gui.</param>
        /// <param name="drawAction">The draw action.</param>
        public static void Colored(Color guiColor, Action drawAction)
        {
            Color color = GUI.color;
            GUI.color = guiColor;

            drawAction.Invoke();

            GUI.color = color;
        }

        /// <summary>
        /// Draws something inside a <see cref="GUI.BeginGroup(Rect)"/>.
        /// </summary>
        /// <param name="rect">The position of the group.</param>
        /// <param name="drawAction">The drawing action.</param>
        public static void Grouped(Rect rect, Action drawAction)
        {
            GUI.BeginGroup(rect);
            drawAction.Invoke();
            GUI.EndGroup();
        }

        /// <summary>
        /// Draws something inside a <see cref="GUI.BeginGroup(Rect)"/>.
        /// </summary>
        /// <param name="rect">The position of the group.</param>
        /// <param name="style">The style of the group.</param>
        /// <param name="drawAction">The drawing action.</param>
        public static void Grouped(Rect rect, GUIStyle style, Action drawAction)
        {
            GUI.BeginGroup(rect, style);
            drawAction.Invoke();
            GUI.EndGroup();
        }

        /// <summary>
        /// Draws something inside a <see cref="EditorGUILayout.BeginHorizontal(GUILayoutOption[])"/>
        /// </summary>
        /// <param name="style">The style to draw in.</param>
        /// <param name="drawAction">The drawing action.</param>
        public static void Horizontal(GUIStyle style, Action drawAction)
        {
            EditorGUILayout.BeginHorizontal(style);
            drawAction.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws something inside a <see cref="EditorGUILayout.BeginVertical(GUILayoutOption[])(GUILayoutOption[])"/>
        /// </summary>
        /// <param name="style">The style to draw in.</param>
        /// <param name="drawAction">The drawing action.</param>
        public static void Vertical(GUIStyle style, Action action)
        {
            EditorGUILayout.BeginVertical(style);
            action.Invoke();
            EditorGUILayout.EndVertical();
        }
    }
}

#endif
