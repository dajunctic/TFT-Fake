#if UNITY_EDITOR

using System;
using UnityEditor;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Draws an editor window with a DTT header at the top.
    /// </summary>
    public class DTTEditorWindow : EditorWindow
    {
        #region Variables
        #region Private
        /// <summary>
        /// Whether to display the header or not.
        /// </summary>
        private bool _canDisplay;

        /// <summary>
        /// The header instance used for drawing the DTT header.
        /// </summary>
        private DTTHeaderGUI _dttHeader;
        #endregion
        #endregion

        #region Methods
        #region Protected
        /// <summary>
        /// Finds the attributes and creates the header based on found info.
        /// </summary>
        protected virtual void OnEnable() => Initialize();

        /// <summary>
        /// Draws the header if it can be displayed and sets the GUI skin to the 
        /// DTT GUI skin.
        /// </summary>
        protected virtual void OnGUI()
        {
            if (_canDisplay)
                _dttHeader.OnGUI();
        }
        #endregion
        #region Private
        /// <summary>
        /// Finds the attributes necessary for drawing the header.
        /// </summary>
        private void Initialize()
        {
            object[] attributes = GetType().GetCustomAttributes(true);
            if (attributes.Length != 0)
            {
                DTTHeaderAttribute headerAttribute = GetAttribute<DTTHeaderAttribute>(attributes);
                if (headerAttribute != null && headerAttribute.fullPackageName != null)
                {
                    AssetJson assetJson = DTTEditorConfig.GetAssetJson(headerAttribute.fullPackageName);
                    if (assetJson != null)
                    {
                        _canDisplay = true;
                        _dttHeader = new DTTHeaderGUI(
                            assetJson,
                            headerAttribute.displayName,
                            headerAttribute.documentationUrl,
                            headerAttribute.customIconPath);
                    }
                }
            }
        }

        /// <summary>
        /// Tries retrieving an attribute of type <typeparamref name="T"/> in 
        /// given array of attributes.
        /// <para>Will return null if it isn't found.</para>
        /// </summary>
        /// <typeparam name="T">The type to search for.</typeparam>
        /// <param name="attributes">The attributes to search through.</param>
        /// <returns>The found attribute.</returns>
        private T GetAttribute<T>(object[] attributes) where T : Attribute
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                object attribute = attributes[i];
                if (attribute is T)
                    return (T)attribute;
            }

            return null;
        }
        #endregion
        #endregion      
    }
}

#endif