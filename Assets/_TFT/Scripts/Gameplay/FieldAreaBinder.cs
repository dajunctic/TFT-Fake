using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Place this MonoBehaviour on a GameObject in the gameplay scene.
    /// It binds the scene's HexAreaView to the FieldSystem at Awake.
    /// </summary>
    public class FieldAreaBinder : MonoBehaviour
    {
        [SerializeField] private HexAreaView fieldArea;

        private void Awake()
        {
            if (GameSystemManager.Instance == null)
            {
                Debug.LogError("FieldAreaBinder: GameSystemManager not found!");
                return;
            }

            Arena arena = GetComponentInParent<Arena>();
            if (arena != null)
            {
                GameSystemManager.Instance.Field.RegisterArena(arena);
                Debug.Log("<color=cyan>FieldAreaBinder: Bound field area to FieldSystem via Arena</color>");
            }
            else
            {
                Debug.LogError("FieldAreaBinder: Arena component not found in parent hierarchy!");
            }
        }
    }
}
