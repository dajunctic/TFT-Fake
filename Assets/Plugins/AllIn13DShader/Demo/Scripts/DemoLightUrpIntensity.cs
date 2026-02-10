using UnityEngine;

namespace AllIn13DShader
{
    public class DemoLightUrpIntensity : MonoBehaviour
    {
        [SerializeField] private Light thisLight;
        [SerializeField] private float urpIntensityMultiplier = 5f;


        private void Start()
        {
#if ALLIN13DSHADER_URP
            thisLight.intensity *= urpIntensityMultiplier;
#else
            float t = urpIntensityMultiplier; //This is preventing a compilation warning in the editor
#endif
        }

        private void Reset()
        {
            if(thisLight == null) 
            {
                thisLight = GetComponent<Light>();
            }
        }
    }
}