#if UNITY_EDITOR

namespace DTT.Utils.EditorUtilities
{
    /// <summary>
    /// An interface for classes drawable in the Editor GUI.
    /// </summary>
    public interface IDrawable
    {
        /// <summary>
        /// Draws the Editor GUI.
        /// </summary>
        void OnGUI();
    }
}

#endif