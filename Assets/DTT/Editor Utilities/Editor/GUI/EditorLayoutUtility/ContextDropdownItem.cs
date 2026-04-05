#if UNITY_EDITOR

using System;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Respresents data of an item to be shown in a context dropdown display.
    /// </summary>
    public struct ContextDropdownItem
    {
        #region Variables
        #region Public
        /// <summary>
        /// The name of the item. This can be written as a path (e.g. Fruit/Apple).
        /// </summary>
        public string name;

        /// <summary>
        /// The tooltip to show when hovering over the item.
        /// </summary>
        public string tooltip;

        /// <summary>
        /// Whether this item is disabled or not.
        /// </summary>
        public bool disabled;

        /// <summary>
        /// Whether this item is activated/is checkmarked.
        /// </summary>
        public bool activated;

        /// <summary>
        /// The method to call when this item is clicked.
        /// </summary>
        public Action clicked;
        #endregion
        #endregion
    }
}

#endif