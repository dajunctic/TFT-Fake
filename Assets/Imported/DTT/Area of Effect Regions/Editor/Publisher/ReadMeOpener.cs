#if UNITY_EDITOR

using DTT.PublishingTools;
using UnityEditor;

namespace DTT.AreaOfEffectRegions.Editor
{
    /// <summary>
    /// Class that handles opening the editor window for the Area of Effect Regions package.
    /// </summary>
    internal static class ReadMeOpener
    {
        /// <summary>
        /// Opens the readme for this package.
        /// </summary>
        [MenuItem("Tools/DTT/Area Of Effect Regions/ReadMe")]
        private static void OpenReadMe() => DTTEditorConfig.OpenReadMe("dtt.area-of-effect-regions");
    }
}
#endif