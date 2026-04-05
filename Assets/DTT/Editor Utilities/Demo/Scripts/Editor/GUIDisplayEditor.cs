#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Displays editor utility features in the demo scene on the GUIDisplayBehaviour component.
    /// </summary>
    [CustomEditor(typeof(GUIDisplayBehaviour))]
    internal class GUIDisplayEditor : Editor
    {
        /// <summary>
        /// The animated foldout used for the demo.
        /// </summary>
        private AnimatedFoldout _foldout;

        /// <summary>
        /// Initializes the foldout.
        /// </summary>
        private void OnEnable() => _foldout = new AnimatedFoldout(this, true, true);

        /// <summary>
        /// Draws the editor utility features used as part of the demo.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DisplayExtendedDropdown();
            DisplayAnimatedFoldout();
        }

        /// <summary>
        /// Displays the extended dropdown window when clicking a button.
        /// </summary>
        private void DisplayExtendedDropdown()
        {
            GUIContent content = new GUIContent("Open Extended Dropdown");
            Rect buttonRect = GUILayoutUtility.GetRect(content, GUI.skin.button);
            if (GUI.Button(buttonRect, content))
            {
                ExtendedDropdownBuilder builder = new ExtendedDropdownBuilder("Extended Dropdown", buttonRect, new AdvancedDropdownState());
                ExtendedDropdownItem[] items = new [] { "Monkey", "Giraffe", "Toad" }.Select(animalName => new ExtendedDropdownItem
                {
                    name = animalName
                }).ToArray();
                builder.StartIndent("Animals").AddItems(items).EndIndent();
                builder.GetResult().Show();
            }
        }

        /// <summary>
        /// Displays the animated foldout.
        /// </summary>
        private void DisplayAnimatedFoldout()
        {
            _foldout.OnGUI("Animated Foldout", () =>
            {
                GUILayout.Label("The first label");
                GUILayout.Label("The second label");
            }, DrawHeaderButton);

            void DrawHeaderButton(Rect foldoutRect)
            {
                float singleLineHeight = EditorGUIUtility.singleLineHeight;
                GUIContent content = new GUIContent(EditorGUIUtility.IconContent("_Help"));
                
                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.padding = new RectOffset();
                style.margin = new RectOffset();
                
                Rect buttonRect = foldoutRect;
                buttonRect.x = foldoutRect.xMax - singleLineHeight;
                buttonRect.width = singleLineHeight;
                if( GUI.Button(buttonRect, content, style))
                    Debug.Log("Info");
            }
        }
    }
}

#endif
