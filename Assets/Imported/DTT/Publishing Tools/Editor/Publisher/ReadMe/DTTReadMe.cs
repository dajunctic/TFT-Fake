#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// The read me which is shown to the  
    /// user when he imports this package for this first time.
    /// </summary>
    [Serializable]
    internal class DTTReadMe
    {
        #region Variables
        #region Public
        /// <summary>
        /// The loaded sections since the last <see cref="ReloadSections"/> call.
        /// </summary>
        public readonly List<ReadMeSection> loadedSections = new List<ReadMeSection>();

        /// <summary>
        /// The folder in which the sections to be loaded reside.
        /// </summary>
        public string SectionsFolder { get; private set; }
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Loads the section data from the sections folder and returns
        /// their <see cref="ReadMeSection"/> wrappers.
        /// </summary>
        /// <returns>The readme section wrappers.</returns>
        public void ReloadSections()
        {
            if (SectionsFolder == null)
            {
                Debug.LogWarning("A reload of the sections was triggered but no sections folder was set. " +
                    "Make sure the readme is initialized.");

                return;
            }

            if (!Directory.Exists(SectionsFolder))
            {
                Debug.LogWarning("A reload of the sections was triggered but the sections folder didn't exist. " +
                    $"Make sure it exists at {DTTEditorConfig.DTT_README_SECTIONS_FOLDER_RELATIVE} relative to " +
                    "your package its folder.");

                return;
            }

            loadedSections.Clear();

            IEnumerable<string> files = Directory.EnumerateFiles(SectionsFolder, "*.json");
            foreach (string file in files)
                loadedSections.Add(new ReadMeSection(File.ReadAllText(file)));
        }

        /// <summary>
        /// Initializes the readme with a package its asset json.
        /// It does this by updating the sections folder and reloading the sections.
        /// </summary>
        /// <param name="assetJson">The asset json of the package.</param>
        public void Initialize(AssetJson assetJson)
        {
            if (assetJson != null)
            {
                // After assembly reload the asset json can be null. 
                // This is why the sections folder is only updated if
                // the asset json has a value.
                SectionsFolder = Path.Combine(
                    DTTEditorConfig.GetContentFolderPath(assetJson),
                    DTTEditorConfig.DTT_README_SECTIONS_FOLDER_RELATIVE);
            }

            ReloadSections();
        }
        #endregion
        #endregion
    }
}

#endif