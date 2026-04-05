#if UNITY_EDITOR

using System;
using System.Linq;
using System.Text.RegularExpressions;
using DTT.Utils.Extensions;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="SerializedProperty"/> instances used in 
    /// editor scripts.
    /// </summary>
    public static class SerializedPropertyExtensions
    {
        /// <summary>
        /// Represents a regular expression for checking whether a property name corresponds with
        /// an array element.
        /// </summary>
        private static readonly Regex _arrayElementRegex = new Regex(@"\w+\[\d\]", RegexOptions.Compiled);
        
        /// <summary>
        /// Returns a sibling property of a serialized object its property.
        /// <para>Doesn't work for nested properties.</para>
        /// </summary>
        /// <param name="property">The property to get the sibling of.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The sibling property.</returns>
        public static SerializedProperty GetSiblingProperty(this SerializedProperty property, string propertyName)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            return property.serializedObject.FindProperty(propertyName);
        }

        /// <summary>
        /// Returns whether a serialized property is an array element.
        /// </summary>
        /// <param name="property">The serialized property to check.</param>
        /// <returns>Whether it is an array element.</returns>
        public static bool IsArrayElement(this SerializedProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            string propertyName = property.propertyPath.Split('.').Last();
            return _arrayElementRegex.IsMatch(propertyName);
        }

        /// <summary>
        /// Removes an array element with a given value.
        /// </summary>
        /// <param name="arrayProperty">The array property to remove the element from.</param>
        /// <param name="value">The value of the element that should be removed.</param>
        /// <param name="compareContent">Whether to compare on content or on reference value.</param>
        public static void RemoveArrayElement(this SerializedProperty arrayProperty, SerializedProperty value, 
            bool compareContent = true)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            for (int i = arrayProperty.arraySize - 1; i >= 0 ; i--)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                bool condition = compareContent ? SerializedProperty.EqualContents(element, value) : element == value;
                if (condition)
                {
                    arrayProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, string value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).stringValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, int value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).intValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, float value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).floatValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, double value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).doubleValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Rect value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).rectValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, RectInt value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).rectIntValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Vector2 value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).vector2Value = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Vector2Int value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).vector2IntValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Vector3 value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).vector3Value = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Vector3Int value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).vector3IntValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Vector4 value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).vector4Value = value;
        }

        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, bool value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).boolValue = value;
        }

        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Bounds value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).boundsValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, BoundsInt value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).boundsIntValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Color value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).colorValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, long value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).longValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Quaternion value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).quaternionValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, AnimationCurve value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).animationCurveValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, UnityEngine.Object value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex);
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).objectReferenceValue = value;
        }
        
        /// <summary>
        /// Adds an array element to a serialized array property. Works only with enum value with
        /// an underlying type of integer.
        /// </summary>
        /// <param name="arrayProperty">The array property to add the element to.</param>
        /// <param name="value">The value to add.</param>
        /// <param name="insertIndex">The possible index to insert the element into.</param>
        public static void AddArrayElement(this SerializedProperty arrayProperty, Enum value, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            int newArrayElementIndex = arrayProperty.IncreaseArraySize(insertIndex); 
            arrayProperty.GetArrayElementAtIndex(newArrayElementIndex).enumValueIndex = value.ToInt();
        }
        
        /// <summary>
        /// Returns whether the serialized array property contains a given value.
        /// </summary>
        /// <param name="arrayProperty">The array property to check.</param>
        /// <param name="value">The value to search.</param>
        /// <returns>Whether the array property contains the given value.</returns>
        public static bool ContainsArrayElement(this SerializedProperty arrayProperty, SerializedProperty value,
            bool compareContent = true)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            for (int i = 0; i < arrayProperty.arraySize; i++)
            {
                SerializedProperty element = arrayProperty.GetArrayElementAtIndex(i);
                bool condition = compareContent ? SerializedProperty.EqualContents(element, value) : element == value;
                if (condition)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Finds the first index of the array property where the element matches a condition.
        /// <para>Will return -1 if no element was found or the properyt wasn't an array property.</para>
        /// </summary>
        /// <param name="arrayProperty">The array property.</param>
        /// <param name="condition">The condition to check on.</param>
        /// <returns></returns>
        public static int FindIndexInArray(this SerializedProperty arrayProperty, Func<SerializedProperty, bool> condition)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            for (int i = 0; i < arrayProperty.arraySize; i++)
                if (condition(arrayProperty.GetArrayElementAtIndex(i)))
                    return i;

            return -1;
        }

        /// <summary>
        /// Returns all the serialized property elements in an array property.
        /// </summary>
        /// <param name="arrayProperty">The array property.</param>
        /// <returns>The serialized property elements.</returns>
        public static SerializedProperty[] GetArrayValues(this SerializedProperty arrayProperty)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));

            SerializedProperty[] elements = new SerializedProperty[arrayProperty.arraySize];
            for (int i = 0; i < elements.Length; i++)
                elements[i] = arrayProperty.GetArrayElementAtIndex(i);
            
            return elements;
        }

        /// <summary>
        /// Increases the size of a serialized array property by one. If given an insert index,
        /// will insert an empty element at array index instead.
        /// </summary>
        /// <param name="arrayProperty">The array property of which to increase the size.</param>
        /// <param name="insertIndex">The index at which to insert the new element.</param>
        /// <returns>The index of the new empty element.</returns>
        public static int IncreaseArraySize(this SerializedProperty arrayProperty, int? insertIndex = null)
        {
            if (arrayProperty == null)
                throw new ArgumentNullException(nameof(arrayProperty));
            
            int newEmptyElementIndex = arrayProperty.arraySize;
            if (insertIndex.HasValue)
            {
                if (!insertIndex.Value.InRange(0, newEmptyElementIndex))
                    throw new ArgumentOutOfRangeException(nameof(insertIndex));
                
                newEmptyElementIndex = insertIndex.Value;
                arrayProperty.InsertArrayElementAtIndex(newEmptyElementIndex);
            }
            else
            {
                arrayProperty.arraySize++;
            }

            return newEmptyElementIndex;
        }
    }
}

#endif
