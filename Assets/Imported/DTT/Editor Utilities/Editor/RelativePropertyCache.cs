#if UNITY_EDITOR

using DTT.Utils.Exceptions;
using DTT.Utils.Optimization;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides storage of <see cref="SerializedProperty"/>'s from a <see cref="SerializedProperty"/> instance, 
    /// creating them when they are first used. It also provides implementations access to them by property name 
    /// instead of by magic string.
    /// </summary>
    public class RelativePropertyCache : LazyDictionaryBase<string, SerializedProperty>
    {
        #region Variables
        #region Protected
        /// <summary>
        /// The property used for initializing relative property values the first time.
        /// </summary>
        protected SerializedProperty p_serializedProperty;
        #endregion
        #region Private
        /// <summary>
        /// The stored properties, accessable by name.
        /// </summary>
        private Dictionary<string, SerializedProperty> _relativeProperties = new Dictionary<string, SerializedProperty>();

        /// <summary>
        /// The prefix used for private variables.
        /// </summary>
        private const string PRIVATE_PREFIX = "_";
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance, initializing the stored properties
        /// of the given <see cref="SerializedObject"/>.
        /// </summary>
        /// <param name="serializedObject">The object of which to store the properties.</param>
        public RelativePropertyCache(SerializedProperty serializedProperty)
        {
            if (serializedProperty == null)
                throw new LazyDictionaryException("Serialized property can't be null");

            p_serializedProperty = serializedProperty;
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Updates the serialized object representation. Call this before drawing
        /// your properties and eventuall changes.
        /// </summary>
        public void UpdateObjectRepresentation() => p_serializedProperty.serializedObject.Update();

        /// <summary>
        /// Applies modified changes of properties to the serialized object.
        /// Call this after a <see cref="EditorGUI.EndChangeCheck"/> has returned true.
        /// </summary>
        public void ApplyChangesToObject() => p_serializedProperty.serializedObject.ApplyModifiedProperties();
        #endregion
        #region Protected
        /// <summary>
        /// Tries retrieving a relative property value from the cache based on given name.
        /// </summary>
        /// <param name="nameOfProperty">The property name.</param>
        /// <returns>The property value.</returns>
        protected override SerializedProperty GetValue(string nameOfProperty)
        {
            // Null or empty property names aren't allowed.
            if (string.IsNullOrEmpty(nameOfProperty))
                throw new NullReferenceException($"Property name is null or empty.");

            if (!_relativeProperties.ContainsKey(nameOfProperty))
            {
                // Find a property using the given name.
                SerializedProperty property = p_serializedProperty.FindPropertyRelative(nameOfProperty);
                if (property == null)
                {
                    // If the property wasn't found, try finding it might be using a private prefix.
                    string privatePrefixed = PRIVATE_PREFIX + nameOfProperty;
                    property = p_serializedProperty.FindPropertyRelative(privatePrefixed);
                    if (property == null)
                        throw new InvalidOperationException($"{nameOfProperty} and {privatePrefixed} don't match a property.");
                }

                _relativeProperties.Add(nameOfProperty, property);
            }

            return _relativeProperties[nameOfProperty];
        }
        #endregion       
        #endregion      
    }
}

#endif