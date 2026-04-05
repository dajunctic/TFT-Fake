#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides editor layout utilities like dropdown displays.
    /// </summary>
    public static class EditorLayoutUtility
    {
        /// <summary>
        /// Displays a contextual dropdown at mouse position using given context dropdown items.
        /// </summary>
        /// <param name="items">The items to display in the dropdown.</param>
        /// <param name="OnClick">
        /// The method to call when an item has been clicked, 
        /// returning the number of the clicked item.
        /// </param>
        public static void ContextDropdown(IEnumerable<ContextDropdownItem> items, Action<int> OnClick = null)
            => ContextDropdown(null, items, OnClick);

        /// <summary>
        /// Displays a contextual dropdown at given position using given context dropdown items.
        /// </summary>
        /// <param name="items">The items to display in the dropdown.</param>
        /// <param name="OnClick">
        /// The method to call when an item has been clicked, 
        /// returning the number of the clicked item.
        /// </param>
        public static void ContextDropdown(Rect? position, IEnumerable<ContextDropdownItem> items, Action<int> OnClick = null)
        {
            ContextDropdown dropdown = new ContextDropdown(position);

            foreach (ContextDropdownItem item in items)
                dropdown.AddItem(item);

            if (OnClick != null)
                dropdown.OnClick += OnClick;

            dropdown.Show();
        }

        /// <summary>
        /// Use to create a contextual dropdown display at the location of the mouse. 
        /// Concatenate methods (e.g. AddItem(args).AddItem(args).AddSeparator() to
        /// add features to the dropdown to be shown. 
        /// </summary>
        /// <returns>The builder object to create the contextual dropdown.</returns>
        public static ContextDropdownBuilder ContextDropdown() => ContextDropdown(null);

        /// <summary>
        /// Use to create a contextual dropdown display at the given position. 
        /// Concatenate methods (e.g. AddItem(args).AddItem(args).AddSeparator() to
        /// add features to the dropdown to be shown. 
        /// </summary>
        /// <param name="position">The position at which to show the dropdown.</param>
        /// <returns>The builder object to create the contextual dropdown.</returns>
        public static ContextDropdownBuilder ContextDropdown(Rect? position) => new ContextDropdownBuilder(position);

        /// <summary>
        /// Displays an extended dropdown at given position using given extended dropdown items.
        /// </summary>
        /// <param name="name">The name of the dropdown.</param>
        /// <param name="position">The position to display to dropdown at.</param>
        /// <param name="items">The items to display in the dropdown.</param>
        /// <param name="OnClick">
        /// The method to call when an item has been clicked, 
        /// returning the number of the clicked item.
        /// </param>
        public static void ExtendedDropdown(string name, Rect position, IEnumerable<ExtendedDropdownItem> items, Action<int> OnClick = null)
            => ExtendedDropdown(name, position, items, new AdvancedDropdownState(), OnClick);

        /// <summary>
        /// Displays an extended dropdown dislay at given position using given extended dropdown items.
        /// </summary>
        /// <param name="name">The name of the dropdown.</param>
        /// <param name="position">The position to display to dropdown at.</param>
        /// <param name="state">The state of the dropdown (This can be serialized to make it survive assembly reload).</param>
        /// <param name="items">The items to display in the dropdown.</param>
        /// <param name="OnClick">
        /// The method to call when an item has been clicked, 
        /// returning the number of the clicked item.
        /// </param>
        public static void ExtendedDropdown(
            string name,
            Rect position,
            IEnumerable<ExtendedDropdownItem> items,
            AdvancedDropdownState state,
            Action<int> OnClick = null)
        {
            ExtendedDropdown dropdown = new ExtendedDropdown(name, position, state);

            foreach (ExtendedDropdownItem item in items)
                dropdown.AddItem(item);

            if (OnClick != null)
                dropdown.OnClick += OnClick;

            dropdown.Show();
        }

        /// <summary>
        /// Use to create a extended dropdown display at the location of the mouse. 
        /// Concatenate methods (e.g. AddItem(args).AddItem(args).AddSeparator() to
        /// add features to the dropdown to be shown. 
        /// </summary>
        /// <param name="name">The name of the dropdown.</param>
        /// <param name="position">The position of the dropdown.</param>
        /// <param name="state">The state of the dropdown (This can be serialized to make it survive assembly reload).</param>
        /// <returns>The builder object to create the contextual dropdown.</returns>
        public static ExtendedDropdownBuilder ExtendedDropdown(string name, Rect position, AdvancedDropdownState state = null)
            => new ExtendedDropdownBuilder(name, position, state);
    }
}

#endif