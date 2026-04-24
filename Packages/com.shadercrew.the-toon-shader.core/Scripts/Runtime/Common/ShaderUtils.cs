using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
#if USING_URP
using UnityEngine.Rendering.Universal;
#endif
namespace ShaderCrew.TheToonShader
{
    public static class ShaderUtils
    {

        public enum RenderPipelineOptions
        {
            HDRP,
            URP,
            BiRP,
            URP2D,
            NONE
        }




        public static RenderPipelineOptions getCurrentRenderPipeline()
        {
            RenderPipelineOptions rp;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
                    rp = RenderPipelineOptions.HDRP;
                }
                else
                {
                    rp = RenderPipelineOptions.URP;
                }

#if USING_URP2D
                UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                ScriptableRenderer renderer = urpAsset.scriptableRenderer;
                if (renderer.GetType().Name.Contains("Renderer2D"))
                {
                    rp = RenderPipelineOptions.URP2D;
                }

#endif

            }
            else
            {
                rp = RenderPipelineOptions.BiRP;
            }

            return rp;
        }



        public static readonly Dictionary<string, string> STS_BIRP_SHADER_DICTIONARY = new Dictionary<string, string>
        {

#if __I_AM_A_THETOONSHADER_DEV__
                { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "Dev/TheToonShaderDev_BiRP_WithOutline"},
                { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, "Dev/TheToonShaderDev_BiRP_WithOutline"},
                { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, "Dev/TheToonShaderDev_BiRP"},
#else
                { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, TheToonShaderConstants.TTS_SHADER_NAME_BIRP_WITH_OUTLINE},
                { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_BIRP_WITH_OUTLINE},
                { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_NAME_BIRP},
                { TheToonShaderConstants.TTS_SHADER_STS_KEY, TheToonShaderConstants.TTS_SHADER_NAME_BIRP_WITH_STS},
                { TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_BIRP_WITH_OUTLINE_AND_STS},
#endif

        };


        //#if USING_HDRP
        //        public static readonly Dictionary<string,string> TTS_HDRP_SHADER_DICTIONARY = new Dictionary<string, string>
        //        {

        //#if UNITY_2023 || UNITY_6000
        //            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2023/Unlit" },
        //            { "HDRP/Lit", "SeeThroughShader/HDRP/2023/Lit" },
        //            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2023/Lit" },
        //#elif UNITY_2022
        //            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2022/Unlit" },
        //            { "HDRP/Lit", "SeeThroughShader/HDRP/2022/Lit" },
        //            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2022/Lit" },
        //#elif UNITY_2021
        //            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2021/Unlit" },
        //            { "HDRP/Lit", "SeeThroughShader/HDRP/2021/Lit" },
        //            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2021/Lit" },
        //#elif UNITY_2020
        //            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2020/Unlit" },
        //            { "HDRP/Lit", "SeeThroughShader/HDRP/2020/Lit" },
        //            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2020/Lit" },
        //#else
        //            //{ "HDRP/Unlit", "SeeThroughShader/HDRP/2019/Unlit" },
        //            { "HDRP/Lit", "SeeThroughShader/HDRP/2019/Lit" },
        //            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "SeeThroughShader/HDRP/2019/Lit" },
        //#endif
        //        };
        //#endif


#if USING_URP
        public static readonly Dictionary<string, string> TTS_URP_SHADER_DICTIONARY = new Dictionary<string, string>
        {

#if __I_AM_A_THETOONSHADER_DEV__
            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, "Dev/TheToonShaderDev_URP_WithOutline"},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, "Dev/TheToonShaderDev_URP_WithOutline"},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, "Dev/TheToonShaderDev_URP"},
#else
#if UNITY_2023 || UNITY_6000
            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_UNITY6},
            { TheToonShaderConstants.TTS_SHADER_STS_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_UNITY6_WITH_STS},
            { TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_UNITY6_WITH_OUTLINE_AND_STS},
#elif UNITY_2022
            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2022_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2022_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2022},
            { TheToonShaderConstants.TTS_SHADER_STS_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2022_WITH_STS},
            { TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2022_WITH_OUTLINE_AND_STS},
#elif UNITY_2021
            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2021_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2021_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2021},
            { TheToonShaderConstants.TTS_SHADER_STS_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2021_WITH_STS},
            { TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2021_WITH_OUTLINE_AND_STS},
#elif UNITY_2020
            { TheToonShaderConstants.TTS_SHADER_DEFAULT_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2020_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2020_WITH_OUTLINE},
            { TheToonShaderConstants.TTS_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2020},
            { TheToonShaderConstants.TTS_SHADER_STS_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2020_WITH_STS},
            { TheToonShaderConstants.TTS_SHADER_STS_WITH_OUTLINE_KEY, TheToonShaderConstants.TTS_SHADER_NAME_URP_2020_WITH_OUTLINE_AND_STS},
#endif
#endif
        };



        public static readonly Dictionary<string, string> TTS_URP2D_SHADER_DICTIONARY = new Dictionary<string, string>
        {

#if __I_AM_A_THETOONSHADER_DEV__
            { TheToonShaderConstants.TTS_URP2D_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_FILENAME_2D_SHADERGRAPH}
#else
            { TheToonShaderConstants.TTS_URP2D_SHADER_STANDARD_KEY, TheToonShaderConstants.TTS_SHADER_FILENAME_2D_SHADERGRAPH}
#endif
        };


#endif






        public static Dictionary<string, string> getUnityToTTSShaderMapping()
        {
            Dictionary<string, string> UnityToTTSShaderMapping = null;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
//#if USING_HDRP
//                    UnityToTTSShaderMapping = TTS_HDRP_SHADER_DICTIONARY;
//#endif
                }
                else //UTP
                {
#if USING_URP
                    UniversalRenderPipelineAsset urpAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
                    ScriptableRenderer renderer = urpAsset.scriptableRenderer;
                    if (renderer.GetType().Name.Contains("Renderer2D"))
                    {
                        UnityToTTSShaderMapping = TTS_URP2D_SHADER_DICTIONARY;
                    }
                    else
                    {

                        UnityToTTSShaderMapping = TTS_URP_SHADER_DICTIONARY;

                    }
#endif

                }
            }
            else
            {
                UnityToTTSShaderMapping = STS_BIRP_SHADER_DICTIONARY;
            }

            return UnityToTTSShaderMapping;
        }









        public static bool IsNativeTTSShader(Shader shader)
        {
            if(shader != null)
            {
                return IsNativeTTSShader(shader.name);
            }
            return false;
        }

        //public static bool IsNativeTTSShader(string shaderName)
        //{
        //    if (shaderName != null && !shaderName.Equals(""))
        //    {
        //        if (THETOONSHADER_SHADER_LIST.Contains(shaderName))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static bool IsNativeTTSShader(string shaderName)
        {
            if (shaderName != null && !shaderName.Equals(""))
            {
                if (getUnityToTTSShaderMapping().ContainsValue(shaderName))
                {
                    return true;
                }
            }
            return false;
        }


        public class UnityVersionRenderPipelineShaderInfo
        {
            public string unityVersion;
            public string renderPipeline;
            public string versionAndRPCorrectedShader;
            public string shaderFolder;


            public UnityVersionRenderPipelineShaderInfo(string unityVersion, string renderPipeline, string shader, string shaderFolder)
            {
                this.unityVersion = unityVersion;
                this.renderPipeline = renderPipeline;
                this.versionAndRPCorrectedShader = shader;
                this.shaderFolder = shaderFolder;
            }
        }
        public static UnityVersionRenderPipelineShaderInfo getUnityVersionAndRenderPipelineCorrectedShaderString()
        {
            string unityVersion;
            string renderPipeline;
            string shaderString;
            string shaderFolder;
            unityVersion = Application.unityVersion;
            if (GraphicsSettings.currentRenderPipeline)
            {
                if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("HighDefinition"))
                {
                    renderPipeline = "HDRP";
                    if (unityVersion.Substring(0, 4).Equals("2019"))
                    {
                        shaderString = "TheToonShader/HDRP/TheToonShader_HDRP2019";
                        shaderFolder = "TheToonShader/HDRP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2020"))
                    {
                        shaderString = "TheToonShader/HDRP/TheToonShader_HDRP2020";
                        shaderFolder = "TheToonShader/HDRP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2021"))
                    {
                        shaderString = "TheToonShader/HDRP/TheToonShader_HDRP2021";
                        shaderFolder = "TheToonShader/HDRP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2022"))
                    {
                        shaderString = "TheToonShader/HDRP/TheToonShader_HDRP2022";
                        shaderFolder = "TheToonShader/HDRP/...";
                    }
                    else
                    {
                        shaderString = "TheToonShader/HDRP/TheToonShader_HDRP2023";
                        shaderFolder = "TheToonShader/HDRP/...";
                    }

                }
                else
                {
                    renderPipeline = "URP";
                    if (unityVersion.Substring(0, 4).Equals("2019"))
                    {
                        shaderString = "TheToonShader/URP/TheToonShader_URP2019";
                        shaderFolder = "TheToonShader/URP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2020"))
                    {
                        shaderString = "TheToonShader/URP/TheToonShader_URP2020";
                        //shaderString = "Dev/TheToonShaderBetterShaders";
                        shaderFolder = "TheToonShader/URP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2021"))
                    {
                        shaderString = "TheToonShader/URP/TheToonShader_URP2021";
                        shaderFolder = "TheToonShader/URP/...";
                    }
                    else if (unityVersion.Substring(0, 4).Equals("2022"))
                    {
                        shaderString = "TheToonShader/URP/TheToonShader_URP2022";
                        shaderFolder = "TheToonShader/URP/...";
                    }
                    else
                    {
                        shaderString = "TheToonShader/URP/TheToonShader_URP2023";
                        shaderFolder = "TheToonShader/URP/...";
                    }
                    shaderString = "Dev/TheToonShaderDev_URP";
#if __I_AM_A_THETOONSHADER_DEV_
                    shaderString = "Dev/TheToonShaderDev_URP";
#endif
                }
            }
            else
            {
                renderPipeline = "Built-in RP";
                shaderString = "TheToonShader/URP/TheToonShader_BiRP";
#if __I_AM_A_THETOONSHADER_DEV_
                shaderString = "Dev/TheToonShaderDev_BiRP";
#endif
                shaderFolder = "TheToonShader/BiRP/...";
            }

            return new UnityVersionRenderPipelineShaderInfo(unityVersion, renderPipeline, shaderString, shaderFolder);
        }


        public static List<string> SURFACEOPTIONS_PROPERTIES_LIST = new List<string>
        {
            "_Cutoff",
            "_Surface", "_Blend", "_Cull", "_AlphaClip", "_ReceiveShadows",
        };

        public static Dictionary<string, List<string>> SURFACEOPTIONSANDINPUTS_CATEGORIES = new Dictionary<string, List<string>>
        {
            { "Surface Options", SURFACEOPTIONS_PROPERTIES_LIST}, {"Surface Inputs", SURFACEOPTIONS_PROPERTIES_LIST}
        };



        //public static readonly List<string> ALL_PROPERTIES_LIST = SURFACEOPTIONSANDINPUTS_PROPERTIES_LIST.Concat(TOONSHADING_PROPERTIES_LIST)
        //                                                                                                .Concat(STYLING_PROPERTIES_LIST)
        //                                                                                                .Concat(OUTLINE_PROPERTIES_LIST).ToList();



        //public static readonly Dictionary<string, string> THETOONSHADER_PROPERTIES_LIST = new Dictionary<string, string> { };

        public static readonly List<string> ALL_PROPERITES = new List<string>
       // public static string[] SURFACEOPTIONSANDINPUTS_PROPERTIES_LIST =
        {
            "_ShadingMode", "_LightFunction", 
            
            "_UseAlphaOnlyFromBaseMap",

            //BiRP and URP:
            "_Cull", "_AlphaClip", "_Cutoff",
            "_BumpScale", "_BumpMap", "_EmissionColor", "_EmissionMap", "_SmoothnessTextureChannel",
            "_Metallic", "_MetallicGlossMap", "_Parallax", "_ParallaxMap", "_OcclusionStrength",
            "_OcclusionMap", "_DetailMask", "_DetailAlbedoMap", "_DetailNormalMapScale", "_DetailNormalMap",




#if USING_URP
            //URP only:
            "_BaseMap", "_BaseColor", 
            "_Surface", "_Blend", 
            "_ReceiveShadows", "_SpecularHighlights", "_SpecColor", "_SpecGlossMap",
            "_Smoothness", "_SmoothnessSource", "_WorkflowMode",  
            "_EnvironmentReflections",  "_DetailAlbedoMapScale", 
#else
            //BiRP only:
            "_Color","_MainTex",
            "_Glossiness","_GlossMapScale",
            "_UVSec","_Mode",
#endif


            "_EnableToonShading", "_ShadingFunction", "_GradientTex", "_GradientMode",
            "_GradientBlending", "_GradientBlendFactor", "_NumberOfCells", "_CellTransitionSmoothness", "_SumLightsBeforePosterization", "_ShadingUseLightColors", "_EnableShadows",
            "_CoreShadowColor", "_TerminatorPosition", "_TerminatorWidth", "_TerminatorSmoothness", "_FormShadowColor", "_EnableCastShadows",
            //"_CastShadowsStrength", "_CastShadowsSmoothness",
            "_CastShadowColor",
            "_CastShadowColorMode",

            "_ShadingAffectedByNormalMap",
            //Specular
            "_EnableSpecular", "_SpecularBlending", "_SpecularColor", "_SpecularSize", "_SpecularSmoothness","_SpecularOpacity", "_SpecularAffectedByNormalMap", "_SpecularUseLightColors",
            //Rim
            "_EnableRim", "_RimBlending", "_RimColor", "_RimSize", "_RimSmoothness", "_RimOpacity", "_RimAffectedArea", "_RimAffectedByNormalMap",
            //Styling
            "_EnableStyling", "_EnableStylingDistanceFade", "_StylingDFStartingDistance", "_StylingDFFalloff", "_StylingAdjustDistanceFadeValue", "_StylingDistanceFadeValue",
            "_EnableAntiAliasing",
            /////////////////////
            // SHADING STYLING //
            /////////////////////
            "_EnableShadingStyling", "_StylingShadingSyncWithOtherStyling", 
            "_StylingColor",
            //Style
            "_ShadingStyle", 
            //Partitioning
            "_SyncWithLightPartitioning", "_NumberOfCellsHatching", "_StylingTerminatorPosition", "_StylingOvermodelingFactor",
            //Blending
            "_StylingShadingBlending", "_StylingShadingIsInverted", 
            //DrawSpace
            "_DrawSpace", "_SSCameraDistanceScaled","_AnchorSSToObjectsOrigin", "_CoordinateSystem", "_PolarCenterMode", "_PolarCenter",
            //Density and Direction
            "_StylingShadingDensity", "_StylingShadingInitialDirection", "_StylingShadingRotationBetweenCells", "_StylingShadingHalftonesOffset",
            //Style Settings
            "_StylingShadingOpacity", "_StylingShadingOpacityFalloff", "_StylingShadingThicknessControl", "_StylingShadingThickness", "_StylingShadingThicknessFalloff",
            "_StylingShadingHardness", "_StylingShadingHalftonesRoundness", "_StylingShadingHalftonesRoundnessFalloff",
            //Randomizer
            "_EnableShadingRandomizer", "_ShadingNoise1Size", "_ShadingNoise1Seed", "_ShadingNoise2Seed",
            "_NoiseIntensity", "_SpacingRandomMode", "_SpacingRandomIntensity","_OpacityRandomMode", "_OpacityRandomIntensity", "_HardnessRandomMode", "_HardnessRandomIntensity", 
            "_LengthRandomMode", "_LengthRandomIntensity", "_ThicknessRandomMode", "_ThicknesshRandomIntensity", 

            //////////////////////////
            // Cast Shadows Styling //
            //////////////////////////
            "_EnableCastShadowsStyling",
            "_StylingCastShadowsColor",
            "_StylingCastShadowsSyncWithOtherStyling",
            //Style
            "_CastShadowsStyle", 
            //Partitioning
            "_CastShadowsNumberOfCellsHatching", "_StylingCastShadowsSmoothness",
            //Blending
            "_StylingCastShadowsBlending", "_StylingCastShadowsIsInverted", 
            //DrawSpace
            "_CastShadowsDrawSpace", "_CastShadowsSSCameraDistanceScaled","_CastShadowsAnchorSSToObjectsOrigin", "_CastShadowsCoordinateSystem", "_CastShadowsPolarCenterMode", "_CastShadowsPolarCenter",
            //Density and Direction
            "_StylingCastShadowsDensity", "_StylingCastShadowsInitialDirection", "_StylingCastShadowsRotationBetweenCells", "_StylingCastShadowsHalftonesOffset",
            //Style Settings
            "_StylingCastShadowsOpacity", "_StylingCastShadowsOpacityFalloff", "_StylingCastShadowsThicknessControl", "_StylingCastShadowsThickness", "_StylingCastShadowsThicknessFalloff",
            "_StylingCastShadowsHardness", "_StylingCastShadowsHalftonesRoundness", "_StylingCastShadowsHalftonesRoundnessFalloff",
            //Randomizer
            "_EnableCastShadowsRandomizer", "_CastShadowsNoise1Size", "_CastShadowsNoise1Seed", "_CastShadowsNoise2Seed",
            "_CastShadowsNoiseIntensity", "_CastShadowsSpacingRandomMode", "_CastShadowsSpacingRandomIntensity","_CastShadowsOpacityRandomMode", "_CastShadowsOpacityRandomIntensity",
            "_CastShadowsHardnessRandomMode", "_CastShadowsHardnessRandomIntensity", "_CastShadowsLengthRandomMode", "_CastShadowsLengthRandomIntensity", "_CastShadowsThicknessRandomMode", "_CastShadowsThicknesshRandomIntensity", 

            //////////////////////
            // Specular Styling //
            //////////////////////
            "_EnableSpecularStyling", "_StylingSpecularSyncWithOtherStyling",
            //Specular Settings
            "_SyncWithSpecular", "_StylingSpecularSize", "_StylingSpecularSmoothness",

            "_StylingSpecularColor", "_StylingSpecularUseLightColors",
            //Style
            "_SpecularStyle",
            //Blending
            "_StylingSpecularBlending", "_StylingSpecularIsInverted", 
            //DrawSpace
            "_SpecularDrawSpace", "_SpecularSSCameraDistanceScaled", "_SpecularAnchorSSToObjectsOrigin", "_SpecularCoordinateSystem", "_SpecularPolarCenterMode", "_SpecularPolarCenter",
            //Density and Direction
            "_StylingSpecularDensity", "_StylingSpecularRotation", "_StylingSpecularHalftonesOffset",
            //Style Settings
            "_StylingSpecularOpacity", "_StylingSpecularOpacityFalloff", "_StylingSpecularThicknessControl", "_StylingSpecularThickness", "_StylingSpecularThicknessFalloff",
            "_StylingSpecularHardness", "_StylingSpecularHalftonesRoundness", "_StylingSpecularHalftonesRoundnessFalloff", 
            //Randomizer
            "_EnableSpecularRandomizer", "_SpecularNoise1Size", "_SpecularNoise1Seed", "_SpecularNoise2Seed",
            "_SpecularNoiseIntensity", "_SpecularSpacingRandomMode", "_SpecularSpacingRandomIntensity", "_SpecularOpacityRandomMode", "_SpecularOpacityRandomIntensity",
            "_SpecularHardnessRandomMode", "_SpecularHardnessRandomIntensity", "_SpecularLengthRandomMode", "_SpecularLengthRandomIntensity", "_SpecularThicknessRandomMode",
            "_SpecularThicknesshRandomIntensity",


            /////////////////
            // Rim Styling //
            /////////////////
            "_EnableRimStyling", "_StylingRimSyncWithOtherStyling",
            //Rim Settings
             "_SyncWithRim", "_StylingRimSize", "_StylingRimSmoothness", "_StylingRimAffectedArea",

             "_StylingRimColor", 
             //Style
            "_RimStyle",
            //Blending
            "_StylingRimBlending", "_StylingRimIsInverted",
            //DrawSpace
            "_RimDrawSpace", "_RimSSCameraDistanceScaled", "_RimAnchorSSToObjectsOrigin", "_RimCoordinateSystem", "_RimPolarCenterMode", "_RimPolarCenter",
            //Density and Direction
             "_StylingRimDensity", "_StylingRimRotation", "_StylingRimHalftonesOffset", 
            //Style Settings
            "_StylingRimThicknessControl", "_StylingRimThickness", "_StylingRimThicknessFalloff", "_StylingRimOpacity", "_StylingRimOpacityFalloff", "_StylingRimHardness", 
            "_StylingRimHalftonesRoundness", "_StylingRimHalftonesRoundnessFalloff",
            //Randomizer
            "_EnableRimRandomizer", "_RimNoise1Size", "_RimNoise1Seed", "_RimNoise2Seed",
            "_RimNoiseIntensity", "_RimSpacingRandomMode", "_RimSpacingRandomIntensity", "_RimOpacityRandomMode", "_RimOpacityRandomIntensity", "_RimHardnessRandomMode",
            "_RimHardnessRandomIntensity", "_RimLengthRandomMode", "_RimLengthRandomIntensity", "_RimThicknessRandomMode", "_RimThicknesshRandomIntensity",

            "_HatchingAffectedByNormalMap", "_HatchingCameraDistanceFade", "_HalftonePatternCameraDistanceFade",
            "_EnableOutline", "_OutlineColor", "_OutlineWidth", "_OutlineDepthOffset",
            "_OutlineCameraDistanceImpact",
        };


        public static readonly List<string> THETOONSHADER_KEYWORDS_LIST = new List<string>
        {
            "_SHADING_COLOR",
            "_SHADING_BLINNPHONG",
            "_SHADING_PBR",
        };


        public static readonly List<string> TOONSHADING_PROPERTIES_LIST = new List<string>
        //public static string[] TOONSHADING_PROPERTIES_LIST =
        {
            "_EnableToonShading", "_ShadingFunction", "_GradientTex", "_GradientMode", "_GradientBlending",
            "_GradientBlendFactor", "_NumberOfCells", "_CellTransitionSmoothness", "_EnableShadows", "_CoreShadowColor",
            "_TerminatorWidth", "_TerminatorSmoothness", "_FormShadowColor", "_EnableCastShadows", "_CustomizeCastShadowsColor",
            "_CastShadowsStrength", "_CastShadowsSmoothness", "_CastShadowColorMode", "_CastShadowColor", "_ShadowsAffectByNormalMap",
            "_EnableSpecular", "_SpecularBlending", "_SpecularColor", "_SpecularSize", "_SpecularSmoothness",
            "_SpecularOpacity", "_SpecularAffectedByNormalMap", "_EnableRim", "_RimBlending", "_RimColor",
            "_RimSize", "_RimSmoothness", "_RimOpacity", "_RimAffectedArea", "_RimAffectedByNormalMap"
        };

        //public static readonly List<string> STYLING_PROPERTIES_LIST = new List<string>
        ////public static string[] STYLING_PROPERTIES_LIST =
        //{
        //    "_EnableStyling", "_EnableShadingStyling", "_ShadingStyle", "_StylingShadingBlending", "_DrawSpace",
        //    "_SSCameraDistanceScaled", "_CoordinateSystem", "_PolarCenterMode", "_PolarCenter", "_SyncWithLightPartitioning",
        //    "_NumberOfCellsHatching", "_StylingOvermodelingFactor", "_StylingColor", "_StylingShadingDensity", "_StylingShadingHalftonesOffset",
        //    "_StylingShadingThicknessControl", "_StylingShadingThickness", "_StylingShadingOpacity", "_StylingShadingOpacityFalloff", "_StylingShadingThicknessFalloff",
        //    "_StylingShadingHardness", "_StylingShadingInitialDirection", "_StylingShadingRotationBetweenCells", "_StylingShadingHalftonesRoundness", "_StylingShadingHalftonesRoundnessFalloff",
        //    "_EnableShadingRandomizer", "_PerlinNoiseSize", "_NoiseIntensity", "_SpacingRandomMode", "_SpacingRandomIntensity",
        //    "_OpacityRandomMode", "_OpacityRandomIntensity", "_HardnessRandomMode", "_HardnessRandomIntensity", "_LengthRandomMode",
        //    "_LengthRandomIntensity", "_ThicknessRandomMode", "_ThicknesshRandomIntensity", "_EnableSpecularStyling", "_SpecularStyle",
        //    "_StylingSpecularBlending", "_SpecularDrawSpace", "_SpecularSSCameraDistanceScaled", "_SpecularCoordinateSystem", "_SpecularPolarCenterMode",
        //    "_SpecularPolarCenter", "_SyncWithSpecular", "_StylingSpecularSize", "_StylingSpecularSmoothness", "_StylingSpecularColor",
        //    "_StylingSpecularDensity", "_StylingSpecularHalftonesOffset", "_StylingSpecularThicknessControl", "_StylingSpecularThickness", "_StylingSpecularThicknessFalloff",
        //    "_StylingSpecularOpacity", "_StylingSpecularOpacityFalloff", "_StylingSpecularHardness", "_StylingSpecularHalftonesRoundness", "_StylingSpecularHalftonesRoundnessFalloff",
        //    "_StylingSpecularRotation", "_EnableSpecularRandomizer", "_SpecularPerlinNoiseSize", "_SpecularPerlinNoiseSeed", "_SpecularWhiteNoiseSeed",
        //    "_SpecularNoiseIntensity", "_SpecularSpacingRandomMode", "_SpecularSpacingRandomIntensity", "_SpecularOpacityRandomMode", "_SpecularOpacityRandomIntensity",
        //    "_SpecularHardnessRandomMode", "_SpecularHardnessRandomIntensity", "_SpecularLengthRandomMode", "_SpecularLengthRandomIntensity", "_SpecularThicknessRandomMode",
        //    "_SpecularThicknesshRandomIntensity", "_EnableRimStyling", "_RimStyle", "_StylingRimBlending", "_RimDrawSpace",
        //    "_RimSSCameraDistanceScaled", "_RimCoordinateSystem", "_RimPolarCenterMode", "_RimPolarCenter", "_SyncWithRim",
        //    "_StylingRimSize", "_StylingRimSmoothness", "_StylingRimAffectedArea", "_StylingRimColor", "_StylingRimDensity",
        //    "_StylingRimHalftonesOffset", "_StylingRimThicknessControl", "_StylingRimThickness", "_StylingRimThicknessFalloff", "_StylingRimOpacity",
        //    "_StylingRimOpacityFalloff", "_StylingRimHardness", "_StylingRimHalftonesRoundness", "_StylingRimHalftonesRoundnessFalloff", "_StylingRimRotation",
        //    "_EnableRimRandomizer", "_RimPerlinNoiseSize", "_RimPerlinNoiseSeed", "_RimWhiteNoiseSeed", "_RimNoiseIntensity",
        //    "_RimSpacingRandomMode", "_RimSpacingRandomIntensity", "_RimOpacityRandomMode", "_RimOpacityRandomIntensity", "_RimHardnessRandomMode",
        //    "_RimHardnessRandomIntensity", "_RimLengthRandomMode", "_RimLengthRandomIntensity", "_RimThicknessRandomMode", "_RimThicknesshRandomIntensity",
        //    "_HatchingAffectedByNormalMap", "_NoiseMap1", "_NoiseMap2", "_HatchingCameraDistanceFade", "_HalftonePatternCameraDistanceFade"
        //};

        //public static readonly List<string> STYLING_GENERAL_PROPERTIES = new List<string>
        //{
        //    "_EnableStyling", "_EnableShadingStyling", "_ShadingStyle", "_StylingShadingBlending", "_DrawSpace",
        //    "_SSCameraDistanceScaled", "_CoordinateSystem", "_PolarCenterMode", "_PolarCenter", "_StylingColor", "_StylingShadingDensity", "_StylingShadingHalftonesOffset",
        //    "_StylingShadingThicknessControl", "_StylingShadingThickness", "_StylingShadingOpacity", "_StylingShadingOpacityFalloff", "_StylingShadingThicknessFalloff",
        //    "_StylingShadingHardness", "_StylingShadingInitialDirection", "_StylingShadingRotationBetweenCells", "_StylingShadingHalftonesRoundness", "_StylingShadingHalftonesRoundnessFalloff",
        //    "_EnableSpecularStyling", "_SpecularStyle",
        //    "_StylingSpecularBlending", "_SpecularDrawSpace", "_SpecularSSCameraDistanceScaled", "_SpecularCoordinateSystem", "_SpecularPolarCenterMode",
        //    "_SpecularPolarCenter", "_SyncWithSpecular", "_StylingSpecularSize", "_StylingSpecularSmoothness", "_StylingSpecularColor",
        //    "_StylingSpecularDensity", "_StylingSpecularHalftonesOffset", "_StylingSpecularThicknessControl", "_StylingSpecularThickness", "_StylingSpecularThicknessFalloff",
        //    "_StylingSpecularOpacity", "_StylingSpecularOpacityFalloff", "_StylingSpecularHardness", "_StylingSpecularHalftonesRoundness", "_StylingSpecularHalftonesRoundnessFalloff",
        //    "_StylingSpecularRotation", "_EnableSpecularRandomizer", "_SpecularPerlinNoiseSize", "_SpecularPerlinNoiseSeed", "_SpecularWhiteNoiseSeed",
        //    "_SpecularNoiseIntensity", "_SpecularSpacingRandomMode", "_SpecularSpacingRandomIntensity", "_SpecularOpacityRandomMode", "_SpecularOpacityRandomIntensity",
        //    "_SpecularHardnessRandomMode", "_SpecularHardnessRandomIntensity", "_SpecularLengthRandomMode", "_SpecularLengthRandomIntensity", "_SpecularThicknessRandomMode",
        //    "_SpecularThicknesshRandomIntensity", "_EnableRimStyling", "_RimStyle", "_StylingRimBlending", "_RimDrawSpace",
        //    "_RimSSCameraDistanceScaled", "_RimCoordinateSystem", "_RimPolarCenterMode", "_RimPolarCenter", "_SyncWithRim",
        //    "_StylingRimSize", "_StylingRimSmoothness", "_StylingRimAffectedArea", "_StylingRimColor", "_StylingRimDensity",
        //    "_StylingRimHalftonesOffset", "_StylingRimThicknessControl", "_StylingRimThickness", "_StylingRimThicknessFalloff", "_StylingRimOpacity",
        //    "_StylingRimOpacityFalloff", "_StylingRimHardness", "_StylingRimHalftonesRoundness", "_StylingRimHalftonesRoundnessFalloff", "_StylingRimRotation",
        //    "_EnableRimRandomizer", "_RimPerlinNoiseSize", "_RimPerlinNoiseSeed", "_RimWhiteNoiseSeed", "_RimNoiseIntensity",
        //    "_RimSpacingRandomMode", "_RimSpacingRandomIntensity", "_RimOpacityRandomMode", "_RimOpacityRandomIntensity", "_RimHardnessRandomMode",
        //    "_RimHardnessRandomIntensity", "_RimLengthRandomMode", "_RimLengthRandomIntensity", "_RimThicknessRandomMode", "_RimThicknesshRandomIntensity",
        //    "_HatchingAffectedByNormalMap", "_HatchingCameraDistanceFade", "_HalftonePatternCameraDistanceFade"
        //};

        public static readonly List<string> STYLING_SHADING_PARTITIONING_PROPERTIES = new List<string>
        {
             "_SyncWithLightPartitioning","_NumberOfCellsHatching", "_StylingOvermodelingFactor",
        };

        public static readonly List<string> STYLING_SHADING_RANDOMIZER_PROPERTIES = new List<string>
        {
            "_EnableShadingRandomizer", "_PerlinNoiseSize", "_NoiseIntensity", "_SpacingRandomMode", "_SpacingRandomIntensity",
            "_OpacityRandomMode", "_OpacityRandomIntensity", "_HardnessRandomMode", "_HardnessRandomIntensity", "_LengthRandomMode",
            "_LengthRandomIntensity", "_ThicknessRandomMode", "_ThicknesshRandomIntensity",
        };


        public static readonly List<string> OUTLINE_PROPERTIES_LIST = new List<string>
        //public static string[] OUTLINE_PROPERTIES_LIST =
        {
            "_EnableOutline", "_OutlineColor", "_OutlineWidth", "_OutlineDepthOffset", "_OutlineCameraDistanceImpact"
        };


        public static List<string> test = new List<string> { };

        //public static List<string> ALL_STYLING_PROPERTIES()
        //{
        //    return STYLING_GENERAL_PROPERTIES.Concat(STYLING_SHADING_PARTITIONING_PROPERTIES).Concat(STYLING_SHADING_RANDOMIZER_PROPERTIES).ToList();
        //}


        //public static List<string> ALL_PROPERTIES()
        //{
        //    return SURFACEOPTIONSANDINPUTS_PROPERTIES_LIST.Concat(TOONSHADING_PROPERTIES_LIST).Concat(ALL_STYLING_PROPERTIES()).Concat(OUTLINE_PROPERTIES_LIST).ToList();
        //}


    }
}