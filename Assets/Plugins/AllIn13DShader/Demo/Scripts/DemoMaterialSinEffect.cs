using UnityEngine;

namespace AllIn13DShader
{
    public class DemoMaterialSinEffect : MonoBehaviour
    {
        [SerializeField] private Renderer targetRenderer;
        [SerializeField] private string propertyName = "_AlphaCutoffValue";
        [SerializeField] private float minValue = 0f;
        [SerializeField] private float maxValue = 1f;
        [SerializeField] private float speed = 1f;
    
        private Material currMaterial;
        private int propertyId;

        private void Start()
        {
            currMaterial = new Material(targetRenderer.material);
            targetRenderer.material = currMaterial;
            propertyId = Shader.PropertyToID(propertyName);
        }

        private void Update()
        {
            float sinValue = Mathf.Sin(Time.unscaledTime * speed);
            float mappedValue = Mathf.Lerp(minValue, maxValue, (sinValue + 1f) * 0.5f);
            currMaterial.SetFloat(propertyId, mappedValue);
        }
    }
}