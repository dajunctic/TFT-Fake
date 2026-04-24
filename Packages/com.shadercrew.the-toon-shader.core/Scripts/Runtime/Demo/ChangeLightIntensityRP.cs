using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShaderCrew.TheToonShader.ShaderUtils;
namespace ShaderCrew.TheToonShader
{
    [ExecuteInEditMode]
    public class ChangeLightIntensityRP : MonoBehaviour
    {
        // Start is called before the first frame update

        private Light light;
        private float originalIntensity;
        public float intensityBiRP = 1f;
        public float intensityURP = 60f;



        protected RenderPipelineOptions rp = RenderPipelineOptions.NONE;
        void Start()
        {
            light = GetComponent<Light>();
            if (light != null)
            {
                if (rp == RenderPipelineOptions.NONE)
                {
                    rp = getCurrentRenderPipeline();
                }
                if (rp == RenderPipelineOptions.URP)
                {
                    light.intensity = intensityURP;
                }
                else
                {
                    light.intensity = intensityBiRP;
                }
            }

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}