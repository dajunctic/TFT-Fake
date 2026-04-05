#if UNITY_EDITOR

using UnityEditor;

namespace DTT.PublishingTools
{
    /// <summary>
    /// Enforces the DTT GUI skin and provides the option for drawing
    /// a DTT header at the top of your shader inspector.
    /// </summary>
    public abstract class DTTShaderEditor : ShaderGUI
    {
        /// <summary>
        /// Whether to display the header or not.
        /// </summary>
        private bool _canDisplay;

        /// <summary>
        /// The header instance used for drawing the DTT header.
        /// </summary>
        private DTTHeaderGUI _dttHeader;

        /// <summary>
        /// Whether this editor has been initialized.
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// Draws the DTT header on top of the shader settings.
        /// </summary>
        /// <param name="materialEditor"><inheritdoc/></param>
        /// <param name="properties"><inheritdoc/></param>
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            if (!_initialized)
                Initialize();

            if (_canDisplay)
                _dttHeader.OnGUI();

            base.OnGUI(materialEditor, properties);
        }

        /// <summary>
        /// Finds the attributes necessary for drawing the header.
        /// </summary>
        private void Initialize()
        {
            // Try retrieving the header attribute.
            DTTHeaderAttribute headerAttribute = GetHeaderAttribute();
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

            _initialized = true;
        }

        /// <summary>
        /// Tries retrieving the <see cref="DTTHeaderAttribute"/>.
        /// <para>Will return null if it isn't found.</para>
        /// </summary>
        /// <returns>The found attribute.</returns>
        private DTTHeaderAttribute GetHeaderAttribute()
        {
            object[] attributes = GetType().GetCustomAttributes(typeof(DTTHeaderAttribute), true);

            return attributes.Length != 0 ? (DTTHeaderAttribute)attributes[0] : null;
        }
    }
}

#endif