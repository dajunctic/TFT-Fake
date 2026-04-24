


    using System.Collections;
using UnityEngine;
namespace ShaderCrew.TheToonShader
{
    public class LightRotation : MonoBehaviour
    {
        public float startAngle = -30f;
        public float endAngle = 30f;

        public float rotationSpeed = 1f;

        public int pauseDuration = 500;

        public int rotationAxis = 1;

        private float t = 0f;
        private bool reversing = false;
        private bool isPaused = false;

        void Update()
        {
            if (isPaused) return;

            t += (reversing ? -1 : 1) * rotationSpeed * Time.deltaTime;

            if (t >= 1f)
            {
                t = 1f;
                reversing = true;
                StartCoroutine(PauseAtMaxPoint());
            }
            else if (t <= 0f)
            {
                t = 0f;
                reversing = false;
                StartCoroutine(PauseAtMaxPoint());
            }

            float easedT = Mathf.SmoothStep(0f, 1f, t); 
            float angle = Mathf.Lerp(startAngle, endAngle, easedT);

            Vector3 rotation = transform.eulerAngles;
            if (rotationAxis == 0)
                rotation.x = angle;
            else if (rotationAxis == 1)
                rotation.y = angle;
            else if (rotationAxis == 2)
                rotation.z = angle;

            transform.eulerAngles = rotation;
        }

        private IEnumerator PauseAtMaxPoint()
        {
            isPaused = true;
            yield return new WaitForSeconds(pauseDuration / 1000f); 
            isPaused = false;
        }
    }
}