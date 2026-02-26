using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Systems/BenchSystemData", fileName = "BenchSystemData")]
    public class BenchSystemData : ScriptableObject
    {
        // Bench-specific config that doesn't depend on scene objects.
        // Scene refs (benchArea, fxGuid) are bound via BenchAreaBinder in the gameplay scene.
    }
}
