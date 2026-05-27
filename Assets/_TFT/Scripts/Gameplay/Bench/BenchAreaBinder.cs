using UnityEngine;

namespace Dajunctic
{

    public class BenchAreaBinder : MonoBehaviour
    {
        [SerializeField] private SquareAreaView benchArea;
        [SerializeField, GuidReference("fx", typeof(IDummyId))] private string fxGuid;

        private void Awake()
        {
            if (GameSystemManager.Instance == null)
            {
                Debug.LogError("BenchAreaBinder: GameSystemManager not found!");
                return;
            }

            Arena arena = GetComponentInParent<Arena>();
            if (arena != null)
            {
                GameSystemManager.Instance.Bench.RegisterArena(arena, fxGuid);
            }
            else
            {
                Debug.LogError("BenchAreaBinder: Arena component not found in parent hierarchy!");
            }
        }
    }
}
