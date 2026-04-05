#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

using Object = UnityEngine.Object;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Provides utility features for tree view data structures in Unity.
    /// </summary>
    public static class TreeViewUtility
    {
        /// <summary>
        /// The id used for a default root.
        /// </summary>
        public const int DEFAULT_ROOT_ID = -1;

        /// <summary>
        /// The depth used for a default root.
        /// </summary>
        public const int DEFAULT_ROOT_DEPTH = -1;

        /// <summary>
        /// The name used for a default root.
        /// </summary>
        public const string DEFAULT_ROOT_NAME = "Root";

        /// <summary>
        /// The id used for an empty tree view item.
        /// </summary>
        public const int EMPTY_ID = 0;

        /// <summary>
        /// The depth used for an empty tree view item.
        /// </summary>
        public const int EMPTY_DEPTH = 0;

        /// <summary>
        /// The name used for an empty tree view item.
        /// </summary>
        public const string EMPTY_NAME = "Empty";

        /// <summary>
        /// The height used by a search field. Use this to calculate the height for a tree view.
        /// </summary>
        public const float SEARCH_BAR_HEIGHT = 22f;

        /// <summary>
        /// A default root tree view item.
        /// </summary>
        public static TreeViewItem DefaultRoot => new TreeViewItem
        {
            id = DEFAULT_ROOT_ID, 
            depth = DEFAULT_ROOT_DEPTH, 
            displayName = DEFAULT_ROOT_NAME
        };

        /// <summary>
        /// An empty tree view item.
        /// </summary>
        public static TreeViewItem EmptyItem => new TreeViewItem
        {
            id = EMPTY_ID, 
            depth = EMPTY_DEPTH, 
            displayName = EMPTY_NAME
        };

        /// <summary>
        /// The style used for drawing the labels in tree views.
        /// Modify with care as changing this value will have effect on all tree views in unity (e.g. scene view).
        /// <remarks>
        /// This value is not stored as a style as this would retrieve the style in the static
        /// constructor. We don't want this as that would mean using any of the other functionalities
        /// would result in an error as styles can only be retrieved in OnGUI methods.
        /// </remarks>
        /// </summary>
        private static string _labelStyleName = "TV line";

        /// <summary>
        /// Enables rich text for tree view labels in Unity.
        /// </summary>
        public static void EnableRichText()
        {
            GUIStyle labelStyle = _labelStyleName;
            labelStyle.richText = true;
        }

        /// <summary>
        /// Returns the unity object for a tree view item assuming the instance id has
        /// been used to set the id of the tree view item.
        /// </summary>
        /// <param name="treeViewItem">The tree view item to get the corresponding unity object for.</param>
        /// <returns>The unity object for the tree view item</returns>
        public static Object ToUnityObject(this TreeViewItem treeViewItem)
            => EditorUtility.InstanceIDToObject(treeViewItem.id);
        
        /// <summary>
        /// Returns the unity object for a tree view item assuming the instance id has
        /// been used to set the id of the tree view item.
        /// </summary>
        /// <typeparam name="T">The type of unity object.</typeparam>
        /// <param name="treeViewItem">The tree view item to get the corresponding unity object for.</param>
        /// <returns>The unity object for the tree view item</returns>
        public static T ToUnityObject<T>(this TreeViewItem treeViewItem) where T : Object
            => (T)EditorUtility.InstanceIDToObject(treeViewItem.id);
        
        /// <summary>
        /// Converts a tree of tree view items to a list.
        /// </summary>
        /// <param name="rootItem">The root tree view item.</param>
        /// <param name="result">The resulting list.</param>
        public static void TreeToList(TreeViewItem rootItem, IList<TreeViewItem> result)
        {
            result.Clear();

            // Create a stack of tree view items and add top level items to it.
            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
            for (int i = rootItem.children.Count - 1; i >= 0; i--)
                stack.Push(rootItem.children[i]);

            // While the stack is not empty, pop items and add them to the result.
            // If an item has children push them onto the stack.
            while (stack.Count > 0)
            {
                TreeViewItem current = stack.Pop();
                result.Add(current);

                if (current.hasChildren && current.children[0] != null)
                {
                    for (int i = current.children.Count - 1; i >= 0; i--)
                    {
                        stack.Push(current.children[i]);
                    }
                }
            }
        }
    }
}

#endif