#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides player settings related utility methods.
    /// </summary>
    public static class PlayerSettingsUtility
    {
        /// <summary>
        /// Adds a define symbol to the selected build target group its defined symbols.
        /// </summary>
        /// <param name="symbol">The symbol to add.</param>
        public static void AddScriptingDefineSymbol(string symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException("The symbol to add was null.");

            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            List<string> definesList = new List<string>(defines.Split(';'));

            if (!definesList.Contains(symbol))
                definesList.Add(symbol);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", definesList));
        }

        /// <summary>
        /// Removes the define symbol from the selected build target group its defined symbols.
        /// </summary>
        /// <param name="symbol">The symbol to remove.</param>
        public static void RemoveScriptingDefineSymbol(string symbol)
        {
            if (symbol == null)
                throw new ArgumentNullException("The symbol to remove was null.");

            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            List<string> definesList = new List<string>(defines.Split(';'));

            if (definesList.Contains(symbol))
                definesList.Remove(symbol);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", definesList));
        }
    }
}

#endif