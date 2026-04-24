using UnityEditor;
using UnityEngine;
namespace ShaderCrew.TheToonShader
{
    [System.Serializable]
    public class PropertyCategorySO
    {
        [SerializeField]
        public bool boolValue;

        [SerializeField]
        public int propertiesActive;

        public PropertyCategorySO()
        {
            propertiesActive = 4;
        }

    }
}