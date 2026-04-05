#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides methods for validating sprites.
    /// </summary>
    public static class SpriteUtility
    {
        #region Methods
        #region Public
        /// <summary>
        /// Returns the <see cref="TextureImporter"/> for the given object.
        /// <para>Will return null if the sprite is a unity buildin sprite.</para>
        /// </summary>
        /// <param name="sprite">The sprite to get the importer of.</param>
        /// <returns>The importer of the given sprite.</returns>
        public static TextureImporter GetImporterOfSprite(this Sprite sprite)
        {
            if (sprite == null)
                throw new ArgumentNullException(nameof(sprite));

            string path = AssetDatabase.GetAssetPath(sprite);
            return path != PathNames.BUILDIN_RESOURCES ? (TextureImporter)TextureImporter.GetAtPath(path) : null;
        }

        /// <summary>
        /// Determines whether the given sprite is a build in Unity sprite.
        /// </summary>
        /// <param name="sprite">The sprite to be checked on whether it's a build in Unity sprite.</param>
        /// <returns>Whether the given sprite is a build in Unity sprite.</returns>
        public static bool IsSpriteFromUnity(this Sprite sprite)
        {
            if (sprite == null)
                throw new ArgumentNullException(nameof(sprite));

            string path = AssetDatabase.GetAssetPath(sprite);
            return path == PathNames.BUILDIN_RESOURCES;
        }

        /// <summary>
        /// Returns whether a sprite is imported with a sprite mode of <see cref="SpriteImportMode.Multiple"/>.
        /// <para>Will return false on internal Unity resources sprites.</para>
        /// </summary>
        /// <param name="sprite">The sprite to check.</param>
        /// <returns>Whether the sprite is imported with the multiple sprite mode.</returns>
        public static bool IsImportedWithMultipleSpriteMode(this Sprite sprite)
        {
            if (sprite == null)
                throw new ArgumentNullException(nameof(sprite));

            TextureImporter importer = GetImporterOfSprite(sprite);
            return importer != null ? importer.spriteImportMode == SpriteImportMode.Multiple : false;
        }

        /// <summary>
        /// Returns whether a sprite is part of an atlas in the project. 
        /// <para>This is a performance heavy call and should thus only be called in initialization methods.</para>
        /// </summary>
        /// <param name="sprite">The sprite to check.</param>
        /// <returns>Whether the sprite is part of an atlas in the project.</returns>
        public static bool IsAtlasPacked(this Sprite sprite)
        {
            if (sprite == null)
                throw new ArgumentNullException(nameof(sprite));

            string[] guids = AssetDatabase.FindAssets("t:spriteatlas");
            foreach (string guid in guids)
            {
                SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(guid));
                if (atlas.CanBindTo(sprite))
                    return true;
            }

            return false;
        }
        #endregion
        #endregion
    }
}

#endif