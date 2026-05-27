#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides the interface for showing a extended dropdown display.
    /// <para>This class wraps Unity's <see cref="AdvancedDropdown"/> class.</para>
    /// </summary>
    public class ExtendedDropdown
    {
        /// <summary>
        /// The wrapped advanced dropdown class providing the actuall dropdown functionalities.
        /// </summary>
        private class Dropdown : AdvancedDropdown
        {
            /// <summary>
            /// Called when an item has been clicked, returning the number of the item
            /// in the list of extended dropdown items.
            /// </summary>
            public event Action<int> OnClick;

            /// <summary>
            /// The amount of items added to the dropdown.
            /// </summary>
            public int ItemCount => _items.Count;

            /// <summary>
            /// Holds the added extended dropdown items.
            /// </summary>
            private readonly Dictionary<int, ExtendedDropdownItem> _dropdownItems = new Dictionary<int, ExtendedDropdownItem>();

            /// <summary>
            /// Holds the advanced dropdown items that have been added.
            /// </summary>
            private readonly Dictionary<string, AdvancedDropdownItem> _items = new Dictionary<string, AdvancedDropdownItem>();

            /// <summary>
            /// The root of the advanced dropdown. 
            /// </summary>
            private readonly AdvancedDropdownItem _root;

            /// <summary>
            /// The id assignable to the next item to be created.
            /// </summary>
            private int _currentId = 0;

            /// <summary>
            /// The id assignable to the next dropdown item.
            /// </summary>
            private int _dropdownItemId = 0;

            /// <summary>
            /// Creates the root item.
            /// </summary>
            /// <param name="name">The name of the root item.</param>
            /// <param name="state">The state of the dropdown (This can be serialized).</param>
            public Dropdown(string name, AdvancedDropdownState state) : base(state)
                => _root = new AdvancedDropdownItem(name) { id = 0 };

            /// <summary>
            /// Adds given item to the dropdown.
            /// </summary>
            /// <param name="dropdownItem">The dropdown item to add.</param>
            public void AddItem(ExtendedDropdownItem dropdownItem)
            {
                AdvancedDropdownItem parent = _root;
                string itemName = dropdownItem.name;

                // Split up the name of the dropdown item to check whether ancestors are defined.
                string[] names = itemName.Split('/');
                if (names.Length != 0)
                {
                    for (int i = 0; i < names.Length - 1; i++)
                    {
                        AdvancedDropdownItem childItem = CreateItem(names[i], out bool isDuplicate);
                        if (!isDuplicate)
                        {
                            // Add non-duplicate items to the items list and assign them as child.
                            _items.Add(childItem.name, childItem);
                            parent.AddChild(childItem);
                        }

                        parent = childItem;
                    }

                    itemName = names[names.Length - 1];
                }

                // To avoid overlap between current id and dropdown item id
                // dropdown id is decremented and not incremented.
                _currentId++;
                _dropdownItemId--;

                AdvancedDropdownItem item = new AdvancedDropdownItem(itemName)
                {
                    id = _dropdownItemId,
                    enabled = !dropdownItem.disabled,
                    icon = dropdownItem.icon
                };
                parent.AddChild(item);

                _dropdownItems.Add(_dropdownItemId, dropdownItem);
            }

            /// <summary>
            /// Adds a minimum size constrained to the dropdown.
            /// </summary>
            /// <param name="minSize"></param>
            public void AddMinimumSize(Vector2 minSize) => minimumSize = minSize;

            /// <summary>
            /// Builds the dropdown from the root.
            /// </summary>
            /// <returns>The root item.</returns>
            protected override AdvancedDropdownItem BuildRoot() => _root;

            /// <summary>
            /// Called when an item has been selected to invoke events.
            /// </summary>
            /// <param name="item">The selected item.</param>
            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                int key = item.id;
                if (_dropdownItems.ContainsKey(key))
                {
                    ExtendedDropdownItem dropdownItem = _dropdownItems[key];
                    dropdownItem.clicked?.Invoke();

                    int number = key * -1;
                    OnClick?.Invoke(number);
                }
            }

            /// <summary>
            /// Creates a new item from given name outputting 
            /// whether or not it was a duplicate or not.
            /// </summary>
            /// <param name="name">The name of the item.</param>
            /// <param name="isDuplicate">Whether or not it was a duplicate item.</param>
            /// <returns>The item instance.</returns>
            private AdvancedDropdownItem CreateItem(string name, out bool isDuplicate)
            {
                if (_items.ContainsKey(name))
                {
                    isDuplicate = true;
                    return _items[name];
                }
                else
                {
                    _currentId++;
                    isDuplicate = false;
                    return new AdvancedDropdownItem(name) { id = _currentId };
                }
            }
        }
        
        /// <summary>
        /// Called when an item has been clicked, returning the number of the item
        /// in the list of extended dropdown items.
        /// </summary>
        public event Action<int> OnClick;
        
        
        /// <summary>
        /// The amount of items added to the dropdown.
        /// </summary>
        public int ItemCount => _dropdown.ItemCount;

        /// <summary>
        /// The wrapped advanced dropdown class providing the actuall dropdown functionalities.
        /// </summary>
        private Dropdown _dropdown;

        /// <summary>
        /// The position at which to display the dropdown.
        /// </summary>
        private Rect _position;
        
        /// <summary>
        /// Creates the dropdown instance with given name, storing the position
        /// and state for usage.
        /// </summary>
        /// <param name="name">The name of the dropdown.</param>
        /// <param name="position">The position at which to display the dropdown.</param>
        /// <param name="state">The dropdown state (This can be serialized).</param>
        public ExtendedDropdown(string name, Rect position, AdvancedDropdownState state)
        {
            _position = position;

            _dropdown = new Dropdown(name, state);
            _dropdown.OnClick += OnItemClicked;
        }

        /// <summary>
        /// Adds a minimum size constrained to the dropdown.
        /// </summary>
        /// <param name="minSize"></param>
        public void AddMinimumSize(Vector2 minSize) => _dropdown.AddMinimumSize(minSize);

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(ExtendedDropdownItem item) => _dropdown.AddItem(item);

        /// <summary>
        /// Called when an item in the dropdown has been clicked to invoke the OnClick event.
        /// </summary>
        /// <param name="numberOfItem"></param>
        private void OnItemClicked(int numberOfItem) => OnClick?.Invoke(numberOfItem);

        /// <summary>
        /// Shows the dropdown at stored position.
        /// </summary>
        public void Show() => _dropdown.Show(_position);
    }
}

#endif
