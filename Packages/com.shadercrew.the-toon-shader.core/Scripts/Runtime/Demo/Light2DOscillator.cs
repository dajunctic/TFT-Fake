#if USING_URP
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace ShaderCrew.TheToonShader
{
    public class Light2DOscillator : MonoBehaviour
    {
        public float moveDistance = 3f;
        public float moveSpeed = 2f;

        private Vector3 startPos;

        void Start()
        {
            startPos = transform.position;
        }

        void Update()
        {
            float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
            transform.position = new Vector3(startPos.x + offset, startPos.y, startPos.z);
        }
    }
}
#endif