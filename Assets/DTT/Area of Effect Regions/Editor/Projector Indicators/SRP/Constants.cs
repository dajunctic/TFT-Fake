namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Define the constants use in custom editor.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The universal plugin symbol.
        /// </summary>
        public static string UNIVERSAL_PLUGIN_SYMBOL = "UNIVERSAL_PACKAGE";
        
        /// <summary>
        /// The universal pluggin missing error text message.
        /// </summary>
        public const string UNIVERSAL_PLUGIN_ERROR = "The Universal renderer package (minimum version 12) is " +
                                                     "necessary for SRP projector, but the plugin was not " +
                                                     "found in the Assets!\n(Needed: com.unity.render-pipelines.universal, com.unity.render-pipelines.core ";
        
        /// <summary>
        /// The url to the universal repo.
        /// </summary>
        public const string UNIVERSAL_PACKAGE_URL
            = "https://github.com/Unity-Technologies/Graphics";
        
    }
}