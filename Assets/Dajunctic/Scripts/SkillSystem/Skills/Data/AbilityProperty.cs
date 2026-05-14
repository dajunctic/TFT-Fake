using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Dajunctic.SkillSystem.Data
{
    [Serializable]
    public class AbilityProperty
    {
        [SerializeField] public string propertyName;
        [SerializeField] public AbilityPropertyValue[] value;

        public AbilityProperty()
        {
        }

        public AbilityProperty(string n)
        {
            propertyName = n;
            value = Array.Empty<AbilityPropertyValue>();
        }

        public AbilityProperty CreateCopy()
        {
            return new AbilityProperty()
            {
                propertyName = propertyName,
                value = value.Select(v => v.CreateCopy()).ToArray()
            };
        }
    }

    [Serializable]
    public class AbilityPropertyValue
    {
        [SerializeField, SerializeReference] public IAbilityProperty value;

        public AbilityPropertyValue CreateCopy()
        {
            return new AbilityPropertyValue()
            {
                value = value.CreateCopy() 
            };
        }
    }

    public interface IAbilityProperty
    {
        IAbilityProperty CreateCopy();
    }

    public interface IAbilityProperty<T> : IAbilityProperty
    {
        T GetData();
    }

    public class AbilityPropertyModifyAttribute : Attribute
    {
        public AbilityPropertyModifyAttribute()
        {
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AbilityProperty))]
    public class AbilityPropertyDrawer : PropertyDrawer
    {
        struct EntryTypePair
        {
            public Type EntryType;
            public string EntryName;
        }
        
        List<string> _typeNames = new();
        static List<EntryTypePair> _typesAndNames = new();
        static string[] _typeDisplays;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            var (r1, r2) = position.HFixedSplit(position.width - 25);
            r2.height = 25;

            EditorGUI.PropertyField(r1, property, label, true);
            if (GUI.Button(r2, "+"))
            {
                PrepareEntryTypeList();
                var menu = new GenericMenu();
                for (var i = 0; i < _typeDisplays.Length; i++)
                {
                    var index = i;
                    menu.AddItem(new GUIContent(_typeDisplays[i]), false, () =>
                    {
                        var values = property.FindPropertyRelative("value");
                        values.InsertArrayElementAtIndex(values.arraySize);

                        var newElement = values.GetArrayElementAtIndex(values.arraySize - 1);
                        var value = newElement.FindPropertyRelative("value");
                        value.managedReferenceValue = (IAbilityProperty)Activator.CreateInstance(_typesAndNames[index].EntryType);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }

        void PrepareEntryTypeList()
        {
            if (_typeDisplays != null && _typeDisplays.Length > 1)
            {
                return;
            }

            var types = ReflectionUtils.GetAllTypes<IAbilityProperty>().ToList();

            _typeNames.Clear();
            foreach (var type in types)
            {
                var newType = new EntryTypePair();
                newType.EntryType = type;
                newType.EntryName = type.Name;
                _typesAndNames.Add(newType);
            }

            _typesAndNames = _typesAndNames.OrderBy(t => t.EntryName).ToList();
            
            for (var i = 0; i < _typesAndNames.Count; i++)
            {
                _typeNames.Add(_typesAndNames[i].EntryName);
            }

            _typeDisplays = _typeNames.ToArray();
        }
    }
#endif
}


