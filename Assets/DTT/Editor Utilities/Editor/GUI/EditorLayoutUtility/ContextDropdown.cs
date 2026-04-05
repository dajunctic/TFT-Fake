#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides the interface for showing a contextual dropdown display.
    /// <para>This class wraps Unity's <see cref="GenericMenu"/> class.</para>
    /// </summary>
    public class ContextDropdown
    {
        /// <summary>
        /// Called when an item in the dropdown has been clicked, returning
        /// the number of the item in the item list.
        /// </summary>
        public event Action<int> OnClick;

        /// <summary>
        /// The amount of items added to the dropdown.
        /// </summary>
        public int ItemCount => _menu.GetItemCount();
        
        /// <summary>
        /// The wrapped generic menu instance.
        /// </summary>
        private GenericMenu _menu;

        /// <summary>
        /// The position at which to dropdown the display. This value is nullable
        /// to provide the option to not use it and show the dropdown at the current
        /// location of the mouse.
        /// </summary>
        private Rect? _position;

        /// <summary>
        /// Initializes the dropdown state.
        /// </summary>
        /// <param name="rect"></param>
        public ContextDropdown(Rect? rect)
        {
            _position = rect;
            _menu = new GenericMenu();
        }

        /// <summary>
        /// Adds given item to the wrapped generic menu.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(ContextDropdownItem item)
        {
            GUIContent content = new GUIContent(item.name);
            bool activated = item.activated;
            int numberOfItem = _menu.GetItemCount();

            if (item.disabled)
            {
                _menu.AddDisabledItem(content, activated);
            }
            else
            {
                _menu.AddItem(content, activated, () =>
                {
                    item.clicked?.Invoke();
                    OnMenuItemClick(numberOfItem);
                });
            }
        }

        /// <summary>
        /// Adds a separator to the generic menu. The path can be written as a path (e.g. Fruit/Apple/).
        /// </summary>
        /// <param name="path">The path used by the separator.</param>
        public void AddSeparator(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            _menu.AddSeparator(path);
        }

        /// <summary>
        /// Allows the generic menu to have duplicate names in the items list.
        /// </summary>
        public void AllowDuplicateNames() => _menu.allowDuplicateNames = true;

        /// <summary>
        /// Shows the dropdown display using the position if it has a value.
        /// </summary>
        public void Show()
        {
            if (_position.HasValue)
                _menu.DropDown(_position.Value);
            else
                _menu.ShowAsContext();
        }

        /// <summary>
        /// Called when an item is clicked in the display, providing the
        /// number of the item in the list to fire the OnClick event.
        /// </summary>
        /// <param name="numberOfItem">The number of the item in the list of items.</param>
        private void OnMenuItemClick(int numberOfItem) => OnClick?.Invoke(numberOfItem);
    }
}

#endif