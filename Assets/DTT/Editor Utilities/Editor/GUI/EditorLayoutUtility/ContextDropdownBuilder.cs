#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Can setup a context dropdown display using the builder pattern. 
    /// </summary>
    public class ContextDropdownBuilder
    {
        /// <summary>
        /// The amount of items added to the dropdown.
        /// </summary>
        public int ItemCount => _dropdown.ItemCount;
        
        /// <summary>
        /// The context dropdown instance used for showing the dropdown.
        /// </summary>
        private readonly ContextDropdown _dropdown;

        /// <summary>
        /// The indent text path used for adding depth to the 
        /// menu items.
        /// </summary>
        private string _indentTextPath = string.Empty;
        
        /// <summary>
        /// Creates the builder using the position at which the 
        /// dropdown should be shown.
        /// </summary>
        /// <param name="position">The position at which to show the dropdown.</param>
        public ContextDropdownBuilder(Rect? position = null) => _dropdown = new ContextDropdown(position);

        /// <summary>
        /// Starts a new indent using given path. If there already is an indent, this one will be 
        /// added to the current.
        /// <para>The path should be in the format 'Parent/Child'.</para>
        /// </summary>
        /// <param name="path">The path for the indent.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder StartIndent(string path)
        {
            _indentTextPath += path;
            return this;
        }

        /// <summary>
        /// Ends the current indent.
        /// </summary>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder EndIndent()
        {
            _indentTextPath = string.Empty;
            return this;
        }

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="name">The name of the item. This can be written as a path (e.g. Fruit/Apple).</param>
        /// <param name="clicked">The method to call when this item is clicked.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(string name, Action clicked = null) => AddItem(name, false, false, null, clicked);

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="name">The name of the item. This can be written as a path (e.g. Fruit/Apple).</param>
        /// <param name="disabled">Whether this item is disabled or not.</param>
        /// <param name="clicked">The method to call when this item is clicked.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(string name, bool disabled, Action clicked = null)
            => AddItem(name, disabled, false, null, clicked);

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="name">The name of the item. This can be written as a path (e.g. Fruit/Apple).</param>
        /// <param name="disabled">Whether this item is disabled or not.</param>
        /// <param name="activated">Whether this item is activated/is checkmarked.</param>
        /// <param name="clicked">The method to call when this item is clicked.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(string name, bool disabled, bool activated, Action clicked = null)
            => AddItem(name, disabled, activated, null, clicked);

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="name">The name of the item. This can be written as a path (e.g. Fruit/Apple).</param>
        /// <param name="activated">Whether this item is activated/is checkmarked.</param>
        /// <param name="tooltip">The tooltip to show when hovering over the item.</param>
        /// <param name="clicked">The method to call when this item is clicked.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(string name, bool activated, string tooltip, Action clicked = null)
            => AddItem(name, false, activated, tooltip, clicked);

        /// <summary>
        /// Adds a new item to the dropdown.
        /// </summary>
        /// <param name="name">The name of the item. This can be written as a path (e.g. Fruit/Apple).</param>
        /// <param name="disabled">Whether this item is disabled or not.</param>
        /// <param name="activated">Whether this item is activated/is checkmarked.</param>
        /// <param name="tooltip">The tooltip to show when hovering over the item.</param>
        /// <param name="clicked">The method to call when this item is clicked.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(string name, bool disabled, bool activated, string tooltip, Action clicked = null)
        {
            string path = string.IsNullOrEmpty(_indentTextPath) ? name : (_indentTextPath + "/" + name);
            return AddItem(new ContextDropdownItem
            {
                name = path,
                disabled = disabled,
                activated = activated,
                tooltip = tooltip,
                clicked = clicked
            });
        }

        /// <summary>
        /// Adds given item to the dropdown.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItem(ContextDropdownItem item)
        {
            _dropdown.AddItem(item);
            return this;
        }

        /// <summary>
        /// Adds given items to the dropdown.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AddItems(IEnumerable<ContextDropdownItem> items)
        {
            foreach (ContextDropdownItem item in items)
                _dropdown.AddItem(item);

            return this;
        }

        /// <summary>
        /// Allow duplicate names to show in the dropdown.
        /// </summary>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder AllowDuplicateNames()
        {
            _dropdown.AllowDuplicateNames();
            return this;
        }

        /// <summary>
        /// Adds a separator between the last and next item. 
        /// The path can be written as a path (e.g. Fruit/Apple/).
        /// </summary>
        /// <param name="path">The path to show the separator at.</param>
        /// <returns></returns>
        public ContextDropdownBuilder AddSeparator(string path = null)
        {
            _dropdown.AddSeparator(path ?? string.Empty);
            return this;
        }

        /// <summary>
        /// Adds a callback to when an item is clicked, returning the 
        /// number of the added item out of all items.
        /// </summary>
        /// <param name="callback">The method to call.</param>
        /// <returns>The builder.</returns>
        public ContextDropdownBuilder OnClick(Action<int> callback)
        {
            _dropdown.OnClick += callback;
            return this;
        }

        /// <summary>
        /// Returns the result of the dropdown build.
        /// </summary>
        /// <returns>The context dropdown instance.</returns>
        public ContextDropdown GetResult() => _dropdown;

        /// <summary>
        /// Converts the dropdown builder to the dropdown instance using the <see cref="GetResult"/> method.
        /// </summary>
        /// <param name="builder">The builder to convert.</param>
        public static implicit operator ContextDropdown(ContextDropdownBuilder builder) => builder.GetResult();
    }
}

#endif