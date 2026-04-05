#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DTT.PublishingTools
{
    /// <summary>
    /// A wrapper containing the object representation
    /// of a readme section json file.
    /// </summary>
    internal class ReadMeSection
    {
        #region InnerClasses
        /// <summary>
        /// The object representation of the readme section json file.
        /// </summary>
        [Serializable]
        public class JSONContent
        {
            /// <summary>
            /// The title of the section.
            /// </summary>
            public string title;

            /// <summary>
            /// The text of the section.
            /// </summary>
            public string text;

            /// <summary>
            /// The link content added. 
            /// </summary>
            public LinkContent[] links;

            /// <summary>
            /// The image content. Urls should
            /// be relative to the sections folder.
            /// </summary>
            public string[] imageUrls;
        }

        /// <summary>
        /// The link content added to a readme section.
        /// </summary>
        [Serializable]
        public class LinkContent
        {
            /// <summary>
            /// The prefix added before the link label.
            /// </summary>
            public string prefix;

            /// <summary>
            /// The link text.
            /// </summary>
            public string text;

            /// <summary>
            /// The url to open when clicked.
            /// </summary>
            public string url;

            /// <summary>
            /// Whether the link should be part of the
            /// <see cref="prefix"/> text.
            /// </summary>
            public bool inline = true;
        }
        #endregion

        #region Variables
        #region Public
        /// <summary>
        /// The object representation of the readme section json file.
        /// </summary>
        public readonly JSONContent content;
        #endregion
        #region Private
        /// <summary>
        /// A cache holding texture references retrieved from the asset 
        /// database relating to image content.
        /// </summary>
        private readonly Dictionary<string, Texture> _imageCache;
        #endregion
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance, creating the content based on given json string. 
        /// </summary>
        /// <param name="jsonString">The json string to create the content with.</param>
        public ReadMeSection(string jsonString)
        {
            _imageCache = new Dictionary<string, Texture>();

            content = new JSONContent();
            JsonUtility.FromJsonOverwrite(jsonString, content);
        }
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Retrieves link content based on given array index.
        /// If out of bounds, it will return null.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <returns>The link content.</returns>
        public LinkContent GetLink(int index)
        {
            LinkContent[] links = content.links;
            if (links == null || index > links.Length)
                return null;
            else
                return content.links[index];
        }

        /// <summary>
        /// Retrieves an image texture based on given array index.
        /// If out of bounds, it will return null.
        /// </summary>
        /// <param name="index">The array index.</param>
        /// <returns>The image texture.</returns>
        public Texture GetImage(int index, string folderPath)
        {
            string[] urls = content.imageUrls;
            if (urls == null || index > urls.Length)
            {
                return null;
            }
            else
            {
                // Cache the images loaded, so they will only be loaded once.
                string path = Path.Combine(folderPath, urls[index]);
                if (!_imageCache.ContainsKey(path))
                    _imageCache.Add(path, AssetDatabase.LoadAssetAtPath<Texture>(path));

                return _imageCache[path];
            }
        }
        #endregion
        #endregion
    }
}

#endif