#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides animated GUI functionalities. 
    /// </summary>
    public static class EditorAnimationExtensions
    {
        #region Variables
        #region Private
        /// <summary>
        /// The animated foldout instances used for providing the
        /// global animated foldout functionality. 
        /// </summary>
        private static readonly Dictionary<string, AnimatedFoldout> _animatedFoldouts;

        /// <summary>
        /// The animated toggle foldout instances used for providing the
        /// global animated toggle foldout functionality. 
        /// </summary>
        private static readonly Dictionary<string, AnimatedToggleFoldout> _animatedToggleFoldouts;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Listens for a selection change event to reset its state.
        /// </summary>
        static EditorAnimationExtensions()
        {
            _animatedFoldouts = new Dictionary<string, AnimatedFoldout>();
            _animatedToggleFoldouts = new Dictionary<string, AnimatedToggleFoldout>();

            Selection.selectionChanged -= RefreshState;
            Selection.selectionChanged += RefreshState;
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Draws a toggle foldout with inside it the GUI based.
        /// on the given draw action.
        /// </summary>
        /// <param name="editor">The editor to draw this foldout for.</param>
        /// <param name="headerName">The header name to display</param>
        /// <param name="drawAction">The method drawing the GUI inside the foldout.</param>
        public static void DrawAnimatedFoldout(this Editor editor, string headerName, Action drawAction)
        {
            // Don't draw anything if either the editor or header name is null.
            if (headerName == null || editor == null)
                return;

            // Cache an AnimatedFoldout for each separate editor instance its foldout.
            string key = headerName + editor.GetInstanceID().ToString();
            if (!_animatedFoldouts.ContainsKey(key))
                _animatedFoldouts.Add(key, new AnimatedFoldout(editor));

            _animatedFoldouts[key].OnGUI(headerName, drawAction);
        }

        /// <summary>
        /// Draws a toggle foldout with inside it the GUI based.
        /// on the given draw action.
        /// </summary>
        /// <param name="editor">The editor to draw this foldout for.</param>
        /// <param name="headerName">The header name to display</param>
        /// <param name="toggle">The toggle value.</param>
        /// <param name="drawAction">The method drawing the GUI inside the foldout.</param>
        /// <returns>Whether the toggle has been pressed.</returns>
        public static bool DrawAnimatedToggleFoldout(this Editor editor, string headerName, bool toggle, Action drawAction)
        {
            // Don't draw anything if either the editor or header name is null.
            if (headerName == null || editor == null)
                return false;

            // Cache an AnimatedFoldout for each separate editor instance its foldout.
            string key = headerName + editor.GetInstanceID().ToString();
            if (!_animatedToggleFoldouts.ContainsKey(key))
                _animatedToggleFoldouts.Add(key, new AnimatedToggleFoldout(editor));

            return _animatedToggleFoldouts[key].OnGUI(headerName, toggle, drawAction);
        }

        /// <summary>
        /// Draws a toggle foldout with inside it the GUI based.
        /// on the given draw action.
        /// </summary>
        /// <param name="window">The editor window to draw this foldout for.</param>
        /// <param name="headerName">The header name to display</param>
        /// <param name="drawAction">The method drawing the GUI inside the foldout.</param>
        public static void DrawAnimatedFoldout(this EditorWindow window, string headerName, Action drawAction)
        {
            // Don't draw anything if either the editor or header name is null.
            if (headerName == null || window == null)
                return;

            // Cache an AnimatedFoldout for each separate editor instance its foldout.
            string key = headerName + window.GetInstanceID().ToString();
            if (!_animatedFoldouts.ContainsKey(key))
                _animatedFoldouts.Add(key, new AnimatedFoldout(window));

            _animatedFoldouts[key].OnGUI(headerName, drawAction);
        }

        /// <summary>
        /// Draws a toggle foldout with inside it the GUI based.
        /// on the given draw action.
        /// </summary>
        /// <param name="window">The editor window to draw this foldout for.</param>
        /// <param name="headerName">The header name to display</param>
        /// <param name="toggle">The toggle value.</param>
        /// <param name="drawAction">The method drawing the GUI inside the foldout.</param>
        /// <returns>Whether the toggle has been pressed.</returns>
        public static bool DrawAnimatedToggleFoldout(this EditorWindow window, string headerName, bool toggle, Action drawAction)
        {
            // Don't draw anything if either the editor or header name is null.
            if (headerName == null || window == null)
                return false;

            // Cache an AnimatedFoldout for each separate editor instance its foldout.
            string key = headerName + window.GetInstanceID().ToString();
            if (!_animatedToggleFoldouts.ContainsKey(key))
                _animatedToggleFoldouts.Add(key, new AnimatedToggleFoldout(window));

            return _animatedToggleFoldouts[key].OnGUI(headerName, toggle, drawAction);
        }
        #endregion
        #region Private
        /// <summary>
        /// Resets the static state by removing animated foldouts with an invalid target.
        /// </summary>
        private static void RefreshState()
        {
            if (_animatedFoldouts.Count != 0)
            {
                // If there are animated foldouts, remove the ones with an invalid target.
                var invalidPairs = _animatedFoldouts.Where(p => p.Value.target == null).ToArray();

                for (int i = 0; i < invalidPairs.Length; i++)
                    _animatedFoldouts.Remove(invalidPairs[i].Key);
            }

            if (_animatedToggleFoldouts.Count != 0)
            {
                // If there are animated toggle foldouts, remove the ones with an invalid target.
                var invalidPairs = _animatedToggleFoldouts.Where(p => p.Value.target == null).ToArray();

                for (int i = 0; i < invalidPairs.Length; i++)
                    _animatedToggleFoldouts.Remove(invalidPairs[i].Key);
            }
        }
        #endregion
        #endregion
    }
}

#endif