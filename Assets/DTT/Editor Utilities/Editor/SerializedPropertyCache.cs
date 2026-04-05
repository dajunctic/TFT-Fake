#if UNITY_EDITOR

using DTT.Utils.Exceptions;
using DTT.Utils.Optimization;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides storage of <see cref="SerializedProperty"/>'s from a <see cref="SerializedObject"/> instance, 
    /// creating them when they are first used. It also provides access to them by property name 
    /// instead of by magic string.
    /// </summary>
    public class SerializedPropertyCache : LazyDictionaryBase<string, SerializedProperty>
    {
        #region Variables
        #region Protected
        /// <summary>
        /// The object used for initializing property values the first time.
        /// </summary>
        protected SerializedObject p_serializedObject;
        #endregion
        #region Private
        /// <summary>
        /// The stored properties, accessable by name.
        /// </summary>
        private Dictionary<string, SerializedProperty> _properties = new Dictionary<string, SerializedProperty>();

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
        public SerializedPropertyCache(SerializedObject serializedObject)
        {
            if (serializedObject == null)
                throw new LazyDictionaryException("Serialized object can't be null");

            p_serializedObject = serializedObject;
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Updates the serialized object representation. Call this before drawing
        /// your properties and eventuall changes.
        /// </summary>
        public void UpdateRepresentation() => p_serializedObject.Update();

        /// <summary>
        /// Applies modified changes of properties to the serialized object.
        /// Call this after a <see cref="EditorGUI.EndChangeCheck"/> has returned true.
        /// </summary>
        public void ApplyChanges() => p_serializedObject.ApplyModifiedProperties();
        #endregion
        #region Protected
        /// <summary>
        /// Tries retrieving a property value from the cache based on given name.
        /// </summary>
        /// <param name="nameOfProperty">The property name.</param>
        /// <returns>The property value.</returns>
        protected override SerializedProperty GetValue(string nameOfProperty)
        {
            // Null or empty property names aren't allowed.
            if (string.IsNullOrEmpty(nameOfProperty))
                throw new NullReferenceException($"Property name is null or empty.");

            if (!_properties.ContainsKey(nameOfProperty))
            {
                // Find a property using the given name.
                SerializedProperty property = p_serializedObject.FindProperty(nameOfProperty);
                if (property == null)
                {
                    // If the property wasn't found, try finding it might be using a private prefix.
                    string privatePrefixed = PRIVATE_PREFIX + nameOfProperty;
                    property = p_serializedObject.FindProperty(privatePrefixed);
                    if (property == null)
                        throw new InvalidOperationException($"{nameOfProperty} and {privatePrefixed} don't match a property.");
                }

                _properties.Add(nameOfProperty, property);
            }

            return _properties[nameOfProperty];
        }
        #endregion
        #endregion
    }
}

#endif