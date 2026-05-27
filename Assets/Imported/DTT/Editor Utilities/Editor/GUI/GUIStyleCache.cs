#if UNITY_EDITOR

using DTT.Utils.Optimization;
using UnityEngine;

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// Implement to Lazily store <see cref="GUIStyle"/> creating them when they are first needed.
    /// Create property members to refer to them by property name and not by magic string.
    /// <para>
    /// Use this to delay your style initialization in gui scripts like Editor and EditorWindow 
    /// as this can't be done in an OnEnable method.
    /// </para>
    /// </summary>
    public class GUIStyleCache : LazyDictionary<string, GUIStyle> { }
}

#endif