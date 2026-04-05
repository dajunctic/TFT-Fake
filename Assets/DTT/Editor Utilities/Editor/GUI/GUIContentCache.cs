#if UNITY_EDITOR

using DTT.Utils.Optimization;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Implement to Lazily store <see cref="GUIContent"/> creating them when they are first needed.
    /// Create property members to refer to them by property name and not by magic string.
    /// </summary>
    public class GUIContentCache : LazyDictionary<string, GUIContent> { }
}

#endif