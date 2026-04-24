using UnityEngine;
namespace ShaderCrew.TheToonShader
{
    public class GradientSO : ScriptableObject
    {
        [SerializeField]
        public Gradient gradient;

        [SerializeField]
        public bool isBakedToTexture = false;
    }
}