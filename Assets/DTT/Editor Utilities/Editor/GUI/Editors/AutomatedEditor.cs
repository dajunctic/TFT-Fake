#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides functionalities to automate your editor. All basis custom editor functionalities
    /// can be initialized by override the <see cref="OnEnable"/> and using the "Add" functions.
    /// </summary>
    [CanEditMultipleObjects]
    public class AutomatedEditor : Editor
    {
        #region InnerClasses
        /// <summary>
        /// Container of data related to the display of help box content underneath a
        /// <see cref="SerializedProperty"/> in the inspector.
        /// </summary>
        protected struct HelpBoxContent
        {
            /// <summary>
            /// Creates a new instance of this object.
            /// </summary>
            /// <param name="message">The message to write inside the help box.</param>
            /// <param name="type">The way in which you want to display this message.</param>
            /// <param name="condition">The condition for the message to be shown.</param>
            public HelpBoxContent(string message, MessageType type, Func<bool> condition)
            {
                this.message = message;
                this.messageType = type;
                this.condition = condition;
            }

            /// <summary>
            /// The message to write inside the help box.
            /// </summary>
            public string message;

            /// <summary>
            /// The way in which you want to display this message.
            /// </summary>
            public MessageType messageType;

            /// <summary>
            /// The condition for the message to be shown.
            /// </summary>
            public Func<bool> condition;

            /// <summary>
            /// Whether or not this helpbox struct has valid data.
            /// </summary>
            public bool HasValidCondition
            {
                get
                {
                    try
                    {
                        condition.Invoke();
                        return true;
                    }

                    catch
                    {
                        return false;
                    }
                }
            }
        }
        #endregion

        #region Variables
        #region Private
        /// <summary>
        /// Contains the <see cref="HelpBoxContent"/> to be shown underneath 
        /// <see cref="SerializedProperty"/>'s.
        /// </summary>
        private readonly Dictionary<string, HelpBoxContent> _helpBoxContent =
            new Dictionary<string, HelpBoxContent>();

        /// <summary>
        /// Contains the conditions for <see cref="SerializedProperty"/>'s to be shown.
        /// </summary>
        private readonly Dictionary<string, Func<bool>> _conditionalProperties
            = new Dictionary<string, Func<bool>>();

        /// <summary>
        /// Contains HelpBoxContent to be shown at the bottom of the inspector window.
        /// </summary>
        private readonly List<HelpBoxContent> _trailingHelpBoxContent = new List<HelpBoxContent>();

        /// <summary>
        /// Contains names of <see cref="SerializedProperty"/>'s to be hidden.
        /// </summary>
        private readonly List<string> _hiddenProperties = new List<string>();

        /// <summary>
        /// Contains names of <see cref="SerializedProperty"/>'s to be disabled.
        /// </summary>
        private readonly List<string> _disabledProperties = new List<string>();
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Draws properties of editor target based on stored settings on each one.
        /// </summary>
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();

            DrawProperties();

            bool changed = EditorGUI.EndChangeCheck();
            OnEndChangeCheck(changed);
            if (changed)
                ApplyChanges();

            DrawTrailingHelpBoxContent();
        }
        #endregion
        #region Protected
        /// <summary>
        /// Adds the script property to be disabled like in default inspector gui's.
        /// </summary>
        protected virtual void OnEnable() => AddDisabledProperty("m_Script");


        /// <summary>
        /// Adds name of <see cref="SerializedProperty"/> to conditional properties
        /// to be hidden based on given condition.
        /// </summary>
        /// <param name="name">The name of the <see cref="SerializedProperty"/>.</param>
        /// <param name="condition">
        /// The condition on which to show the <see cref="SerializedProperty"/>.
        /// </param>
        protected void AddConditionalProperty(string name, Func<bool> condition)
        {
            try
            {
                // Invoke the condition to test for exceptions.
                condition.Invoke();

                _conditionalProperties.Add(name, condition);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Condition of {name} threw an exception.", e);
            }
        }

        /// <summary>
        /// Adds name of <see cref="SerializedProperty"/> to a list of properties
        /// to be hidden.
        /// </summary>
        /// <param name="name">The name of the <see cref="SerializedProperty"/>.</param>
        protected void AddHiddenProperty(string name) => _hiddenProperties.Add(name);

        /// <summary>
        /// Adds name of <see cref="SerializedProperty"/> to a list of properties
        /// to be disabled. 
        /// </summary>
        /// <param name="name">The name of the <see cref="SerializedProperty"/>.</param>
        protected void AddDisabledProperty(string name) => _disabledProperties.Add(name);

        /// <summary>
        /// Adds name of <see cref="SerializedProperty"/> to the <see cref="HelpBoxContent"/> 
        /// to be shown underneath it.
        /// </summary>
        /// <param name="name">The name of the <see cref="SerializedProperty"/>.</param>
        /// <param name="content">The content to show.</param>
        protected void AddHelpBoxContent(string name, HelpBoxContent content)
        {
            if (!content.HasValidCondition)
                throw new ArgumentException("Helpbox content has invalid condition.");

            _helpBoxContent.Add(name, content);
        }

        /// <summary>
        /// Adds trailing help box content underneath all drawn properties.
        /// </summary>
        /// <param name="content">The content to show.</param>
        protected void AddHelpBoxContent(HelpBoxContent content)
        {
            if (!content.HasValidCondition)
                throw new ArgumentException("Helpbox content has invalid condition.");

            _trailingHelpBoxContent.Add(content);
        }

        /// <summary>
        /// Applies modified properties to serialized object.
        /// </summary>
        protected void ApplyChanges() => serializedObject.ApplyModifiedProperties();

        /// <summary>
        /// Override this method to execute functionality to know if 
        /// changes have been made in the inspector. 
        /// <para>Modified properties are automatically applied after this call is made.</para>
        /// </summary>
        /// <param name="changed">Whether the the inspector changed.</param>
        protected virtual void OnEndChangeCheck(bool changed)
        {

        }
        #endregion
        #region Private
        /// <summary>
        /// Draws all visible properties that are not child properties using the
        /// <see cref="SerializedObject.GetIterator"/> method.
        /// </summary>
        private void DrawProperties()
        {
            SerializedProperty property = serializedObject.GetIterator();
            bool enterChildren = true;
            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;
                DrawProperty(property);
            }
        }

        /// <summary>
        /// Tries drawing property based on stored data on it.
        /// </summary>
        /// <param name="property">The property to draw.</param>
        private void DrawProperty(SerializedProperty property)
        {
            string nameOfProperty = property.name;

            if (!IsHidden(nameOfProperty))
            {
                if (IsDisabled(nameOfProperty))
                    DrawDisabled(property);
                else
                    EditorGUILayout.PropertyField(property);
            }

            if (_helpBoxContent.ContainsKey(nameOfProperty))
            {
                HelpBoxContent content = _helpBoxContent[nameOfProperty];
                if (content.condition())
                    EditorGUILayout.HelpBox(content.message, content.messageType);

            }
        }

        /// <summary>
        /// Draws the given in disabled form in the inspector.
        /// </summary>
        /// <param name="property">The property to draw in disabled form.</param>
        private void DrawDisabled(SerializedProperty property)
        {
            GUI.enabled = false;

            EditorGUILayout.PropertyField(property);

            GUI.enabled = true;
        }

        /// <summary>
        /// Returns whether the given name corresponds with a <see cref="SerializedProperty"/>
        /// that is stored as disabled.
        /// </summary>
        /// <param name="nameOfProperty">The name of the property to check.</param>
        /// <returns>
        /// whether the given name corresponds with a <see cref="SerializedProperty"/>
        /// that is stored as disabled.
        /// </returns>
        private bool IsDisabled(string nameOfProperty) => _disabledProperties.Contains(nameOfProperty);

        /// <summary>
        /// Returns whether the given name corresponds with a <see cref="SerializedProperty"/>
        /// that is stored as hidden.
        /// </summary>
        /// <param name="nameOfProperty">The name of the property to check.</param>
        /// <returns>
        /// whether the given name corresponds with a <see cref="SerializedProperty"/>
        /// that is stored as hidden.
        /// </returns>
        private bool IsHidden(string nameOfProperty)
        {
            bool forceHide = _hiddenProperties.Contains(nameOfProperty);
            bool conditionalHide = _conditionalProperties.ContainsKey(nameOfProperty)
                && !_conditionalProperties[nameOfProperty]();

            return forceHide || conditionalHide;
        }

        /// <summary>
        /// Draws <see cref="HelpBoxContent"/> in <see cref="_trailingHelpBoxContent"/> if their
        /// condition is met.
        /// </summary>
        private void DrawTrailingHelpBoxContent()
        {
            for (int i = 0; i < _trailingHelpBoxContent.Count; i++)
            {
                HelpBoxContent content = _trailingHelpBoxContent[i];
                if (content.condition())
                    EditorGUILayout.HelpBox(content.message, content.messageType);
            }
        }
        #endregion
        #endregion
    }
}

#endif
