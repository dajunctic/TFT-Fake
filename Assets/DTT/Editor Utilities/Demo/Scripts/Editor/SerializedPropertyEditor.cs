#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    [CustomEditor(typeof(SettingsScriptableObject))]
    public class SerializedPropertyEditor : Editor
    {
        /// <summary>
        /// Editor utilities allows you to create a cache of all serialized properties on a given serialized object.
        /// </summary>
        private class SettingsProperties : SerializedPropertyCache
        {
            public SerializedProperty setting => base[nameof(setting)];
            
            public SerializedProperty anotherSetting => base[nameof(anotherSetting)];
            
            public SerializedProperty andAnotherSetting => base[nameof(andAnotherSetting)];
            
            public SerializedProperty subSetting => base[nameof(subSetting)];
            
            
            public SettingsProperties(SerializedObject serializedObject) : base(serializedObject) { }
        }

        /// <summary>
        /// Editor utilities allows you to create a cache of all serialized properties on a given serialized property.
        /// </summary>
        private class SubSettingsProperties : RelativePropertyCache
        {
            
            public SerializedProperty name => base[nameof(name)];
            
            public SerializedProperty value => base[nameof(value)];

            
            public SubSettingsProperties(SerializedProperty serializedProperty) : base(serializedProperty) { }
        }

        /// <summary>
        /// Editor utilities allows you to create a cache of all GUIStyles you want to use in your editor.
        /// </summary>
        private class PropertyStyles : GUIStyleCache
        {
            public GUIStyle NameStyle => base[nameof(NameStyle)];

            public GUIStyle ValueStyle => base[nameof(ValueStyle)];
            
            
            public PropertyStyles()
            {
                Add(nameof(NameStyle), () =>
                {
                    GUIStyle style = new GUIStyle(EditorStyles.textField);
                    style.fontStyle = FontStyle.Bold;
                    return style;
                });
                
                Add(nameof(ValueStyle), () =>
                {
                    GUIStyle style = new GUIStyle(EditorStyles.numberField);
                    style.fontSize = 10;
                    return style;
                });
            }
        }

        private enum DrawMethod
        {
            [InspectorName("Using SerializedObject")]
            USING_SERIALIZED_OBJECT = 0,
            
            [InspectorName("Using Caches")]
            USING_CACHED = 1,
            
            [InspectorName("SubSetting Only")]
            SUB_SETTING_ONLY = 2,
            
            [InspectorName("Draw Tools")]
            DRAW_TOOLS = 3,
        }

        private SettingsProperties _settingsProperties;

        private SubSettingsProperties _subSettingsProperties;

        private PropertyStyles _styles;

        [SerializeField]
        private DrawMethod _drawMethod;

        private void OnEnable()
        {
            // Create instances of the property caches
            _styles = new PropertyStyles();
            _settingsProperties = new SettingsProperties(serializedObject);
            _subSettingsProperties = new SubSettingsProperties(_settingsProperties.subSetting);
        }

        public override void OnInspectorGUI()
        {
            _drawMethod = (DrawMethod)EditorGUILayout.EnumPopup("Draw Method", _drawMethod);
            switch (_drawMethod)
            {
                case DrawMethod.USING_CACHED:
                    DrawUsingCaches();
                    break;
                
                case DrawMethod.USING_SERIALIZED_OBJECT:
                    DrawUsingSerializedObject();
                    break;
                
                case DrawMethod.SUB_SETTING_ONLY:
                    DrawSubSetting();
                    break;
            }
        }

        /// <summary>
        /// Using the DrawProperties extension method, you can draw all the properties from
        /// an serialized object in your editor.
        /// </summary>
        private void DrawUsingSerializedObject()
        {
            serializedObject.DrawProperties();
        }

        /// <summary>
        /// Using the SerializedProperyCache, you can draw your properties without using magic string.
        /// </summary>
        private void DrawUsingCaches()
        {
            GUIDrawing.Colored(Color.blue, () =>
            {
                EditorGUILayout.PropertyField(_settingsProperties.anotherSetting, true);
            });
            
            EditorGUILayout.PropertyField(_settingsProperties.andAnotherSetting, true);
            EditorGUILayout.PropertyField(_settingsProperties.subSetting, true);
            EditorGUILayout.PropertyField(_settingsProperties.setting, true);
        }

        /// <summary>
        /// Using the RelativePropertyCache, you can draw your properties without using magic string.
        /// </summary>
        private void DrawSubSetting()
        {
            EditorGUILayout.PropertyField(_subSettingsProperties.value, true);
        }
    }
}

#endif