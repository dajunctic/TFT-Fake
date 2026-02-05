#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Dajunctic
{
    [CustomPropertyDrawer(typeof(GuidReferenceAttribute))]
    public class GuidReferenceAttributeDrawer: PropertyDrawer
    {
        GuidReferenceAttribute TargetAttribute => attribute as GuidReferenceAttribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position = EditorGUI.PrefixLabel(position, label);

            var currentId = string.IsNullOrEmpty(property.stringValue) ? "None" : property.stringValue;

            if (EditorGUI.DropdownButton(position, new GUIContent(currentId), FocusType.Keyboard))
            {
                var entityTypes = TargetAttribute.Types;
                var prefix = TargetAttribute.Prefix;

                if (entityTypes.Length <= 0)
                {
                    entityTypes = GuidReferenceHelper.GetTypes();
                }

                GuidReferenceWindow.Open(property, prefix, entityTypes);
            }

            HandleContextClick(position, property);
        }

        private void HandleContextClick(Rect position, SerializedProperty property)
        {
            var evt = Event.current;
            if (evt.type == EventType.ContextClick && position.Contains(evt.mousePosition))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Clear"), false, () =>
                {
                    property.stringValue = "";
                    property.serializedObject.ApplyModifiedProperties();
                });
                menu.AddItem(new GUIContent("Copy"), false, () =>
                {
                    EditorGUIUtility.systemCopyBuffer = property.stringValue;
                });
                menu.ShowAsContext();
                evt.Use();
            }
        }
    }
}
#endif