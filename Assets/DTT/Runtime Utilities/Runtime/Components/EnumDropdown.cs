using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.Utils.Components
{
    /// <summary>
    /// Can be implemented to populate a dropdown ui element with enum values
    /// and provide callbacks on its changed values.
    /// </summary>
    /// <typeparam name="T">The type of enum.</typeparam>
    [RequireComponent(typeof(Dropdown))]
    public class EnumDropdown<T> : MonoBehaviour where T : Enum
    {
        /// <summary>
        /// Called when the dropdown value has changed.
        /// </summary>
        public event Action<T> ValueChanged;

        /// <summary>
        /// The dropdown reference.
        /// </summary>
        protected Dropdown p_dropdown;

        /// <summary>
        /// The names of the enum values.
        /// </summary>
        protected readonly string[] p_enumNames;

        /// <summary>
        /// The default dropdown option data used based on the enum names.
        /// </summary>
        protected readonly Dropdown.OptionData[] p_defaultOptions;

        /// <summary>
        /// Whether the dropdown already uses the default options.
        /// </summary>
        protected bool p_UsesDefaultOptions
        {
            get
            {
                List<Dropdown.OptionData> options = p_dropdown.options;
                if (options.Count != p_defaultOptions.Length)
                    return false;
                
                for(int i = 0; i < options.Count; i++)
                    if (options[i].text != p_defaultOptions[i].text)
                        return false;

                return true;
            }
        }

        /// <summary>
        /// Creates the enum dropdown instance initializing the read only fields.
        /// </summary>
        public EnumDropdown()
        {
            p_enumNames = Enum.GetNames(typeof(T));
            p_defaultOptions = p_enumNames.Select(enumName => new Dropdown.OptionData(enumName)).ToArray();
        }

        /// <summary>
        /// Populates the dropdown with enum name options. 
        /// </summary>
        private void Awake()
        {
            if(p_dropdown == null)
                p_dropdown = GetComponent<Dropdown>();
           
            if(!p_UsesDefaultOptions)
                ResetOptions();
        }

        /// <summary>
        /// Starts listening for value changes.
        /// </summary>
        private void OnEnable() => p_dropdown.onValueChanged.AddListener(OnValueChanged);
        
        /// <summary>
        /// Stops listening for value changes.
        /// </summary>
        private void OnDisable() => p_dropdown.onValueChanged.RemoveListener(OnValueChanged);

        /// <summary>
        /// Sets the current value displayed on the dropdown.
        /// </summary>
        /// <param name="newValue">The new value to display in the dropdown.</param>
        public void SetValue(T newValue)
        {
            string newEnumName = newValue.ToString();
            int index = Array.IndexOf(p_enumNames, newEnumName);
            if (index != -1)
                p_dropdown.value = index;
        }

        /// <summary>
        /// Called when the current value on the dropdown has changed to fire the
        /// ValueChanged event.
        /// </summary>
        /// <param name="newValue">The new index of the values stored in the dropdown.</param>
        private void OnValueChanged(int newValue)
        {
            if (newValue >= 0 && newValue <= p_enumNames.Length)
            {
                string enumName = p_enumNames[newValue];
                T enumValue = (T)Enum.Parse(typeof(T), enumName);
                ValueChanged?.Invoke(enumValue);
            }
        }

        /// <summary>
        /// Re-initializes the dropdown with default values.
        /// </summary>
        private void Reset()
        {
            p_dropdown = GetComponent<Dropdown>();
           
            if(!p_UsesDefaultOptions)
                ResetOptions();
            
            p_dropdown.SetValueWithoutNotify(0);
        }

        /// <summary>
        /// Re-initializes the dropdown options with default values.
        /// </summary>
        private void ResetOptions()
        {
            p_dropdown.ClearOptions();
            p_dropdown.AddOptions(new List<Dropdown.OptionData>(p_defaultOptions));
        }
    }
}


    