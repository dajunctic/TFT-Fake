using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Place this MonoBehaviour on a GameObject in the gameplay scene.
    /// It binds the scene's SquareAreaView to the BenchSystem at Awake.
    /// </summary>
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

            GameSystemManager.Instance.Bench.BindArea(benchArea, fxGuid);
            Debug.Log("<color=cyan>BenchAreaBinder: Bound bench area to BenchSystem</color>");
        }
    }
}
