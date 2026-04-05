#if UNITY_EDITOR

using System;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Respresents data of an item to be shown in an extended dropdown display.
    /// </summary>
    public struct ExtendedDropdownItem
    {
        /// <summary>
        /// The name of the item.
        /// </summary>
        public string name;

        /// <summary>
        /// Whether or not it the item is disabled or not.
        /// </summary>
        public bool disabled;

        /// <summary>
        /// An optional icon to use for the display.
        /// </summary>
        public Texture2D icon;

        /// <summary>
        /// The method to call when this item is clicked.
        /// </summary>
        public Action clicked;
    }
}

#endif
