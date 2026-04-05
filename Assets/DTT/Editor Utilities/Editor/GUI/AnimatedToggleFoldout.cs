#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEditor.AnimatedValues;

using UnityObject = UnityEngine.Object;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides a simple fade group layout implementation for giving your
    /// conditional toggle drawers a nice foldout animation.
    /// </summary>
    public class AnimatedToggleFoldout
    {
        #region Variables
        #region Public
        /// <summary>
        /// The target for which this foldout element is drawn.
        /// </summary>
        public readonly UnityObject target;
        #endregion
        #region Private
        /// <summary>
        /// The foldout animation.
        /// </summary>
        private readonly AnimBool _animation;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="editor">The editor to draw the foldout for.</param>
        /// <param name="opened">Whether the foldout is opened or not.</param>
        public AnimatedToggleFoldout(Editor editor, bool opened = false)
        {
            _animation = new AnimBool(opened);
            _animation.valueChanged.AddListener(editor.Repaint);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="window">The editor window to draw the foldout for.</param>
        /// <param name="opened">Whether the foldout is opened or not.</param>
        public AnimatedToggleFoldout(EditorWindow window, bool opened = false)
        {
            _animation = new AnimBool(opened);
            _animation.valueChanged.AddListener(window.Repaint);
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Draws the toggle foldout with inside it the GUI based
        /// on the given draw action.
        /// </summary>
        /// <param name="headerName">The name of the foldout header.</param>
        /// <param name="toggle">The toggle value.</param>
        /// <param name="drawAction">The method drawing the GUI inside the foldout.</param>
        /// <returns>Whether the toggle has been pressed.</returns>
        public bool OnGUI(string headerName, bool toggle, Action drawAction)
        {
            toggle = EditorGUILayout.Toggle(headerName, toggle);
            _animation.target = toggle;
            if (EditorGUILayout.BeginFadeGroup(_animation.faded))
                drawAction?.Invoke();

            EditorGUILayout.EndFadeGroup();

            return toggle;
        }
        #endregion
        #endregion
    }
}

#endif