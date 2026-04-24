using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ShaderCrew.TheToonShader
{
    [AddComponentMenu(Strings.COMPONENTMENU_REPLACEORSYNCHRONIZE)]
    [ExecuteInEditMode]
    public class ReplaceOrSynchronizeToonSettings : MonoBehaviour
    {
        //public enum ReplaceOrSync
        //{
        //    Both,
        //    Replace,
        //    Sync,            
        //}

        public enum MethodMode
        {
            //Stopped,
            EditorMode,
            PlayMode,
        }

        public enum ReplaceOrSyncGroupType  
        {   
            Parent,
        //    Box, 
          //  Id,
            //ListOfGameObjects, 
            ListOfMaterials
        }


        public Material referenceMaterial;


        public Transform parentTransform;
        public Transform[] transformsWithTheToonShader;


        public List<Material> materialList;
        public List<Material> materialListForParents;

        private Dictionary<Material, Material> undoMaterialCache = new Dictionary<Material, Material>();

        private Dictionary<Material, Material> materialCache = new Dictionary<Material, Material>();

        //protected Dictionary<string, Shader> UnityToSTSShaderMapping;
        public LayerMask layerMaskToAdd = ~0;


        //public ReplaceOrSync replaceOrSyncMode;
        public MethodMode methodMode = MethodMode.PlayMode;

        public bool keepMaterialsInSyncWithReference = true;
        public bool isSyncingEditorMode = false;


        public ReplaceOrSyncGroupType replaceOrSyncGroupType = ReplaceOrSyncGroupType.ListOfMaterials;

        public bool surfaceOptionsAndInputs = false;
        public bool surfaceOptionsAndInputsFoldout = false;
        public bool surfaceOptions = false;
        public bool surfaceOptionsFoldout = false;
        public bool surfaceInputs = false;
        public bool surfaceInputsFoldout = false;
        public bool advanced = false;

        public bool toonShading = true;
        public bool toonShadingFoldout = false;
        public bool toonShadingLightPartitioning = true;
        public bool toonShadingLightPartitioningFoldout = false;
        public bool toonShadingLight = true;
        public bool toonShadingLightFoldout = false;
        public bool toonShadingShading = true;
        public bool toonShadingShadingFoldout = false;
        public bool toonShadingCastShadows = true;
        public bool toonShadingCastShadowsFoldout = false;
        public bool toonShadingSpecular = true;
        public bool toonShadingSpecularFoldout = false;
        public bool toonShadingRim = true;
        public bool toonShadingRimFoldout = false;

        public bool styling = true;
        public bool stylingFoldout = false;

        public bool stylingDistanceFade = true;
        public bool stylingDistanceFadeFoldout = false;


        public bool stylingShadingFoldout = true;
        public bool stylingShading = true;
        public bool stylingShadingPartitioning = true;
        public bool stylingShadingPartitioningFoldout = false;
        public bool stylingShadingBlending = true;
        public bool stylingShadingBlendingFoldout = false;
        public bool stylingShadingDrawSpace = true;
        public bool stylingShadingDrawSpaceFoldout = false;
        public bool stylingShadingDensityAndRotation = true;
        public bool stylingShadingDensityAndRotationFoldout = false;
        public bool stylingShadingStyleSettings = true;
        public bool stylingShadingStyleSettingsFoldout = false;
        public bool stylingShadingRandomize = true;
        public bool stylingShadingRandomizeFoldout = false;

        public bool stylingCastShadows = true;
        public bool stylingCastShadowsFoldout = false;
        public bool stylingCastShadowsPartitioning = true;
        public bool stylingCastShadowsPartitioningFoldout = false;
        public bool stylingCastShadowsBlending = true;
        public bool stylingCastShadowsBlendingFoldout = false;
        public bool stylingCastShadowsDrawSpace = true;
        public bool stylingCastShadowsDrawSpaceFoldout = false;
        public bool stylingCastShadowsDensityAndRotation = true;
        public bool stylingCastShadowsDensityAndRotationFoldout = false;
        public bool stylingCastShadowsStyleSettings = true;
        public bool stylingCastShadowsStyleSettingsFoldout = false;
        public bool stylingCastShadowsRandomize = true;
        public bool stylingCastShadowsRandomizeFoldout = false;

        public bool stylingSpecular = true;
        public bool stylingSpecularFoldout = false;
        public bool stylingSpecularSettings = true;
        public bool stylingSpecularSettingsFoldout = false;
        public bool stylingSpecularBlending = true;
        public bool stylingSpecularBlendingFoldout = false;
        public bool stylingSpecularDrawSpace = true;
        public bool stylingSpecularDrawSpaceFoldout = false;
        public bool stylingSpecularDensityAndRotation = true;
        public bool stylingSpecularDensityAndRotationFoldout = false;
        public bool stylingSpecularStyleSettings = true;
        public bool stylingSpecularStyleSettingsFoldout = false;
        public bool stylingSpecularRandomize = true;
        public bool stylingSpecularRandomizeFoldout = false;

        public bool stylingRim = true;
        public bool stylingRimFoldout = false;
        public bool stylingRimSettings = true;
        public bool stylingRimSettingsFoldout = false;
        public bool stylingRimBlending = true;
        public bool stylingRimBlendingFoldout = false;
        public bool stylingRimUVSpace = true;
        public bool stylingRimUVSpaceFoldout = false;
        public bool stylingRimDensityAndRotation = true;
        public bool stylingRimDensityAndRotationFoldout = false;
        public bool stylingRimStyleSettings = true;
        public bool stylingRimStyleSettingsFoldout = false;
        public bool stylingRimRandomize = true;
        public bool stylingRimRandomizeFoldout = false;

        public bool outline = true;
        public bool outlineFoldout = true;


        //public  PropertyCategorySO test;


        private Dictionary<string,List<string>> categoryToPropertiesMapping;




        // General  Shading:
        public bool _ShadingMode = true;
        public bool _LightFunction = true;

        public bool _UseAlphaOnlyFromBaseMap = true;


        //BiRP and URP:
        public bool _Cull = true;
        public bool _AlphaClip = true;
        public bool _Cutoff = true;

        public bool _BumpScale = true;
        public bool _BumpMap = true;
        public bool _EmissionColor = true;
        public bool _EmissionMap = true;

        //Lit and BiRP:
        public bool _SmoothnessTextureChannel = true;
        public bool _Metallic = true;
        public bool _MetallicGlossMap = true;
        public bool _Parallax = true;
        public bool _ParallaxMap = true;
        public bool _OcclusionStrength = true;
        public bool _OcclusionMap = true;
        public bool _DetailMask = true;
        public bool _DetailAlbedoMap = true;
        public bool _DetailNormalMapScale = true;
        public bool _DetailNormalMap = true;

//#if USING_URP
        //URP only:
        public bool _BaseMap = true;
        public bool _BaseColor = true;

        public bool _Surface = true;
        public bool _Blend = true;
//#else
        //BiRP only:
        public bool _Color = true;
        public bool _MainTex = true;
        public bool _Glossiness = true;
        public bool _GlossMapScale = true;
        public bool _UVSec = true;
        public bool _Mode = true;

//#endif
        public bool _ReceiveShadows = true;
        // SimpleLit and Lit
        public bool _SpecularHighlights = true;
        public bool _SpecColor = true;
        public bool _SpecGlossMap = true;
        public bool _Smoothness = true;
        //SimpleLit
        public bool _SmoothnessSource = true;
        //Lit
        public bool _WorkflowMode = true;

        public bool _EnvironmentReflections = true;

        public bool _DetailAlbedoMapScale = true;



//TOON SHADING:
        public bool _EnableToonShading = true;
        public bool _ShadingFunction = true;

        //Gradient:
        public bool _GradientTex = true;
        public bool _GradientMode = true;
        public bool _GradientBlending = true;
        public bool _GradientBlendFactor = true;

        //Cells:
        public bool _NumberOfCells = true;
        public bool _CellTransitionSmoothness = true;
        public bool _SumLightsBeforePosterization = true;
        public bool _ShadingUseLightColors = true;

        //Shadows:
        public bool _EnableShadows = true;
        public bool _CoreShadowColor = true;
        public bool _TerminatorPosition = true;
        public bool _TerminatorWidth = true;
        public bool _TerminatorSmoothness = true;
        public bool _FormShadowColor = true;
        public bool _ShadingAffectedByNormalMap = true;

        //Cast Shadows:
        public bool _EnableCastShadows = true;
        //public bool _CustomizeCastShadowsColor = true;
        //public bool _CastShadowsStrength = true;
        //public bool _CastShadowsSmoothness = true;
        public bool _CastShadowColorMode = true;
        public bool _CastShadowColor = true;


        //Specular:
        public bool _EnableSpecular = true;
        public bool _SpecularBlending = true;
        public bool _SpecularColor = true;
        public bool _SpecularSize = true;
        public bool _SpecularSmoothness = true;
        public bool _SpecularOpacity = true;
        public bool _SpecularAffectedByNormalMap = true;
        public bool _SpecularUseLightColors = true;

        //Rim:
        public bool _EnableRim = true;
        public bool _RimBlending = true;
        public bool _RimColor = true;
        public bool _RimSize = true;
        public bool _RimSmoothness = true;
        public bool _RimOpacity = true;
        public bool _RimAffectedArea = true;
        public bool _RimAffectedByNormalMap = true;





        // STYLING:
        public bool _EnableStyling = true;
        //Distance Fade:
        public bool _EnableStylingDistanceFade = true;
        public bool _StylingDFStartingDistance = true;
        public bool _StylingDFFalloff = true;
        public bool _StylingAdjustDistanceFadeValue = true;
        public bool _StylingDistanceFadeValue = true;


        public bool _EnableAntiAliasing = true;

    //SYTLING SHADING:
        public bool _EnableShadingStyling = true;
        public bool _StylingShadingSyncWithOtherStyling = true;

        public bool _StylingColor = true;
    //Style:
        public bool _ShadingStyle = true;
    //Partitioning:
        public bool _SyncWithLightPartitioning = true;
        public bool _NumberOfCellsHatching = true;
        public bool _StylingTerminatorPosition = true;
        public bool _StylingOvermodelingFactor = true;
    //Blending:
        public bool _StylingShadingBlending = true;
        public bool _StylingShadingIsInverted = true;
    //DrawSpace:
        public bool _DrawSpace = true;
        public bool _SSCameraDistanceScaled = true;
        public bool _AnchorSSToObjectsOrigin = true;
        public bool _CoordinateSystem = true;
        public bool _PolarCenterMode = true;
        public bool _PolarCenter = true;

    //Density  and Rotation:
        public bool _StylingShadingDensity = true;
        public bool _StylingShadingInitialDirection = true;
        public bool _StylingShadingRotationBetweenCells = true;
        public bool _StylingShadingHalftonesOffset = true;

    //Style Settings:
        public bool _StylingShadingThicknessControl = true;
        public bool _StylingShadingThickness = true;
        public bool _StylingShadingOpacity = true;
        public bool _StylingShadingOpacityFalloff = true;
        public bool _StylingShadingThicknessFalloff = true;
        public bool _StylingShadingHardness = true;

        public bool _StylingShadingHalftonesRoundness = true;
        public bool _StylingShadingHalftonesRoundnessFalloff = true;

    //Randomizer:
        public bool _EnableShadingRandomizer = true;
        public bool _ShadingNoise1Size = true;
        public bool _ShadingNoise1Seed = true;
        public bool _ShadingNoise2Seed = true;
        public bool _NoiseIntensity = true;
        public bool _SpacingRandomMode = true;
        public bool _SpacingRandomIntensity = true;
        public bool _OpacityRandomMode = true;
        public bool _OpacityRandomIntensity = true;
        public bool _HardnessRandomMode = true;
        public bool _HardnessRandomIntensity = true;
        public bool _LengthRandomMode = true;
        public bool _LengthRandomIntensity = true;
        public bool _ThicknessRandomMode = true;
        public bool _ThicknesshRandomIntensity = true;



    //STYLING CastShadows:
        public bool _EnableCastShadowsStyling = true;
        public bool _StylingCastShadowsSyncWithOtherStyling = true;

        public bool _StylingCastShadowsColor = true;
    //Style
        public bool _CastShadowsStyle = true;
    //Partitioning:
        public bool _CastShadowsNumberOfCellsHatching = true;
        public bool _StylingCastShadowsSmoothness = true;
    //Blending:
        public bool _StylingCastShadowsBlending = true;
        public bool _StylingCastShadowsIsInverted = true;
     //DrawSpace:
        public bool _CastShadowsDrawSpace = true;
        public bool _CastShadowsSSCameraDistanceScaled = true;
        public bool _CastShadowsAnchorSSToObjectsOrigin = true;
        public bool _CastShadowsCoordinateSystem = true;
        public bool _CastShadowsPolarCenterMode = true;
        public bool _CastShadowsPolarCenter = true;
    //Density and Direction
        public bool _StylingCastShadowsDensity = true;
        public bool _StylingCastShadowsInitialDirection = true;
        public bool _StylingCastShadowsRotationBetweenCells = true;
        public bool _StylingCastShadowsHalftonesOffset = true;
    //Style Settngs:
        public bool _StylingCastShadowsOpacity = true;
        public bool _StylingCastShadowsOpacityFalloff = true;
        public bool _StylingCastShadowsThicknessControl = true;
        public bool _StylingCastShadowsThickness = true;
        public bool _StylingCastShadowsThicknessFalloff = true;
        public bool _StylingCastShadowsHardness = true;
        public bool _StylingCastShadowsHalftonesRoundness = true;
        public bool _StylingCastShadowsHalftonesRoundnessFalloff = true;
    //Randomizer:
        public bool _EnableCastShadowsRandomizer = true;
        public bool _CastShadowsNoise1Size = true;
        public bool _CastShadowsNoise1Seed = true;
        public bool _CastShadowsNoise2Seed = true;
        public bool _CastShadowsNoiseIntensity = true;
        public bool _CastShadowsSpacingRandomMode = true;
        public bool _CastShadowsSpacingRandomIntensity = true;
        public bool _CastShadowsOpacityRandomMode = true;
        public bool _CastShadowsOpacityRandomIntensity = true;
        public bool _CastShadowsHardnessRandomMode = true;
        public bool _CastShadowsHardnessRandomIntensity = true;
        public bool _CastShadowsLengthRandomMode = true;
        public bool _CastShadowsLengthRandomIntensity = true;
        public bool _CastShadowsThicknessRandomMode = true;
        public bool _CastShadowsThicknesshRandomIntensity = true;


    //STYLING SPECULAR:
        public bool _EnableSpecularStyling = true;
        public bool _StylingSpecularSyncWithOtherStyling = true;

        public bool _StylingSpecularUseLightColors = true;
        public bool _StylingSpecularColor = true;
    //Style
        public bool _SpecularStyle = true;
    //Specular Settings:
        public bool _SyncWithSpecular = true;
        public bool _StylingSpecularSize = true;
        public bool _StylingSpecularSmoothness = true;
    //Blending:
        public bool _StylingSpecularBlending = true;
        public bool _StylingSpecularIsInverted = true;
    //DrawSpace:
        public bool _SpecularDrawSpace = true;
        public bool _SpecularSSCameraDistanceScaled = true;
        public bool _SpecularAnchorSSToObjectsOrigin = true;
        public bool _SpecularCoordinateSystem = true;
        public bool _SpecularPolarCenterMode = true;
        public bool _SpecularPolarCenter = true;
    //Density and Direction
        public bool _StylingSpecularDensity = true;
        public bool _StylingSpecularRotation = true;
        public bool _StylingSpecularHalftonesOffset = true;
    //Style Settngs:
        public bool _StylingSpecularOpacity = true;
        public bool _StylingSpecularOpacityFalloff = true;
        public bool _StylingSpecularThicknessControl = true;
        public bool _StylingSpecularThickness = true;
        public bool _StylingSpecularThicknessFalloff = true;
        public bool _StylingSpecularHardness = true;
        public bool _StylingSpecularHalftonesRoundness = true;
        public bool _StylingSpecularHalftonesRoundnessFalloff = true;
    //Randomizer:
        public bool _EnableSpecularRandomizer = true;
        public bool _SpecularNoise1Size = true;
        public bool _SpecularNoise1Seed = true;
        public bool _SpecularNoise2Seed = true;
        public bool _SpecularNoiseIntensity = true;
        public bool _SpecularSpacingRandomMode = true;
        public bool _SpecularSpacingRandomIntensity = true;
        public bool _SpecularOpacityRandomMode = true;
        public bool _SpecularOpacityRandomIntensity = true;
        public bool _SpecularHardnessRandomMode = true;
        public bool _SpecularHardnessRandomIntensity = true;
        public bool _SpecularLengthRandomMode = true;
        public bool _SpecularLengthRandomIntensity = true;
        public bool _SpecularThicknessRandomMode = true;
        public bool _SpecularThicknesshRandomIntensity = true;


//STYLING RIM:
        public bool _EnableRimStyling = true;

        public bool _StylingRimSyncWithOtherStyling = true;

        public bool _StylingRimColor = true;
    //Style
        public bool _RimStyle = true;
    //Rim Settings:
        public bool _SyncWithRim = true;
        public bool _StylingRimSize = true;
        public bool _StylingRimSmoothness = true;
        public bool _StylingRimAffectedArea = true;
    // Blending:
        public bool _StylingRimBlending = true;
        public bool _StylingRimIsInverted = true;
    // UV Space:
        public bool _RimDrawSpace = true;
        public bool _RimSSCameraDistanceScaled = true;
        public bool _RimAnchorSSToObjectsOrigin = true;
        public bool _RimCoordinateSystem = true;
        public bool _RimPolarCenterMode = true;
        public bool _RimPolarCenter = true;
    //Density and Direction
        public bool _StylingRimDensity = true;
        public bool _StylingRimRotation = true;
        public bool _StylingRimHalftonesOffset = true;
    //Style Settngs:
        public bool _StylingRimThicknessControl = true;
        public bool _StylingRimThickness = true;
        public bool _StylingRimThicknessFalloff = true;
        public bool _StylingRimOpacity = true;
        public bool _StylingRimOpacityFalloff = true;
        public bool _StylingRimHardness = true;
        public bool _StylingRimHalftonesRoundness = true;
        public bool _StylingRimHalftonesRoundnessFalloff = true;
    //Randomizer:
        public bool _EnableRimRandomizer = true;
        public bool _RimNoise1Size = true;
        public bool _RimNoise1Seed = true;
        public bool _RimNoise2Seed = true;
        public bool _RimNoiseIntensity = true;
        public bool _RimSpacingRandomMode = true;
        public bool _RimSpacingRandomIntensity = true;
        public bool _RimOpacityRandomMode = true;
        public bool _RimOpacityRandomIntensity = true;
        public bool _RimHardnessRandomMode = true;
        public bool _RimHardnessRandomIntensity = true;
        public bool _RimLengthRandomMode = true;
        public bool _RimLengthRandomIntensity = true;
        public bool _RimThicknessRandomMode = true;
        public bool _RimThicknesshRandomIntensity = true;




        public bool _HatchingAffectedByNormalMap = true;

        public bool _HatchingCameraDistanceFade = true;
        public bool _HalftonePatternCameraDistanceFade = true;

        public bool _EnableOutline = true;
        public bool _OutlineColor = true;
        public bool _OutlineWidth = true;
        public bool _OutlineDepthOffset = true;
        public bool _OutlineCameraDistanceImpact = true;


        private Dictionary<string, bool> propertyNameToBoolDictionary; 
        public void CalculatePropertyToogleBoolDictionary()
        {


            //if (propertyNameToBoolDictionary == null)
            //{
                propertyNameToBoolDictionary = new Dictionary<string, bool>();
            //}


            Component component = this;
            Type type = component.GetType();
            foreach (string propertyName in ShaderUtils.ALL_PROPERITES)
            {
                //Debug.Log("propertyName: " + propertyName);
                if(type.GetField(propertyName) != null)
                {
                    bool? boolValue = (bool)type.GetField(propertyName).GetValue(component);
                    if (boolValue != null)
                    {
                        if (propertyNameToBoolDictionary.ContainsKey(propertyName))
                        {
                            propertyNameToBoolDictionary[propertyName] = boolValue.Value;
                        }
                        else
                        {
                            propertyNameToBoolDictionary.Add(propertyName, boolValue.Value);

                        }
                    }
                    else
                    {
                        //Debug.Log(propertyName + " doesn't exist");
                    }
                } else
                {
                    //Debug.Log(propertyName + " doesn't exist");
                }

            }

        }

        public void PrintStatusOfPropertiesAndToggles()
        {
            if(propertyNameToBoolDictionary!=null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (KeyValuePair<string, bool> entry in propertyNameToBoolDictionary)
                {
                    sb.Append(entry.Key + " - " + entry.Value + "\n");
                }

                Debug.Log(sb.ToString());

            }

        }

        public void SyncTheToonShaderProperties(bool forceUpdateOfAllPropertiesAndKeywords = false)
        {
            if (referenceMaterial != null && referenceMaterial.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
            {
                if (propertyNameToBoolDictionary == null)
                {
                    CalculatePropertyToogleBoolDictionary();
                }
                if (methodMode == MethodMode.PlayMode)
                {
                    if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.ListOfMaterials)
                    {
                        if (materialList != null && materialList.Count > 0)
                        {
                            ReplaceAndSyncUtils.UpdateTheToonShaderMaterialPropertiesAndKeywords(materialList, referenceMaterial, propertyNameToBoolDictionary, forceUpdateOfAllPropertiesAndKeywords);
                        }
                    }
                    else
                    {
                        if (transformsWithTheToonShader != null && transformsWithTheToonShader.Length > 0)
                        {
                            ReplaceAndSyncUtils.UpdateTheToonShaderMaterialPropertiesAndKeywords(transformsWithTheToonShader, referenceMaterial, propertyNameToBoolDictionary, forceUpdateOfAllPropertiesAndKeywords);
                        }
                    }

                }
                else
                {

                    if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.ListOfMaterials)
                    {
                        if (materialList != null && materialList.Count > 0)
                        {
                            ReplaceAndSyncUtils.UpdateTheToonShaderMaterialPropertiesAndKeywords(materialList, referenceMaterial, propertyNameToBoolDictionary, forceUpdateOfAllPropertiesAndKeywords);
                        }
                    }
                    else
                    {
                        if (materialListForParents != null && materialListForParents.Count > 0)
                        {
                            ReplaceAndSyncUtils.UpdateTheToonShaderMaterialPropertiesAndKeywords(materialListForParents, referenceMaterial, propertyNameToBoolDictionary, forceUpdateOfAllPropertiesAndKeywords);
                        }
                    }

                }

            }
           
        }


        public List<Material> DoSetupForMaterialsEditorMode()
        {
            if (referenceMaterial != null && referenceMaterial.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
            {
                CalculatePropertyToogleBoolDictionary();
                if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.ListOfMaterials)
                {
                    //undoMaterialCache.Clear();
                    //CreateUndoMaterialsCopy();

                    foreach (Material material in materialList)
                    {
                        if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                        {
                            material.shader = referenceMaterial.shader;
                        }
                    }
                    return materialList;
                }
                else //if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.Parent)
                {
                    if (parentTransform == null)
                    {
                        parentTransform = this.transform;
                    }
                    return DoSetupOfAllMaterialsGameObjectBasedEditorMode();
                }
            }
            return null;
        }

        void Awake()
        {

            if (this.isActiveAndEnabled && Application.isPlaying)
            {
                if (methodMode == MethodMode.PlayMode)
                {


                    if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.ListOfMaterials)
                    {
                        if (referenceMaterial != null && referenceMaterial.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                        {
                            if (materialList != null && materialList.Count > 0)
                            {
                                materialCache.Clear();
                                foreach (Material material in materialList)
                                {
                                    if (material != null)
                                    {
                                        Material savedCopy = new Material(material);
                                        ReplaceAndSyncUtils.AddTheToonShaderInstancePrefix(material);
                                        string name = material.name;
                                        if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                                        {
                                            if (!name.Contains(" - Replaced by " + referenceMaterial.name))
                                            {
                                                name += " - Replaced by " + referenceMaterial.name;
                                            }
                                            //material.shader = UnityToSTSShaderMapping.TryGetValue(material.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];
                                            material.shader = referenceMaterial.shader;
                                        }
                                        else
                                        {
                                            if (!name.Contains(" - Synced with " + referenceMaterial.name))
                                            {
                                                name += " - Synced with " + referenceMaterial.name;
                                            }
                                        }
                                        material.name = name;
                                        materialCache.Add(material, savedCopy);
                                        //Debug.Log("AWAKE: " + material.shader.name + " - " + materialCache[material].shader.name);

                                        InitializeNoiseTextures(material);
                                    }
                                }

                                //SyncTheToonShaderProperties();


                            }
                        }

                    }
                    else //if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.Parent)
                    {
                        if (parentTransform == null)
                        {
                            parentTransform = this.transform;
                        }

                        DoSetupOfAllMaterialsGameObjectBasedPlayMode();
                    }

                }
            }
        }


        private void InitializeNoiseTextures(Material material)
        {
            material.SetTexture("_NoiseMap1", referenceMaterial.GetTexture("_NoiseMap1"));
            material.SetTexture("_NoiseMap2", referenceMaterial.GetTexture("_NoiseMap2"));
        }

        void OnDestroy()
        {
            //Debug.Log("OnDestroy");

            //if(Application.isPlaying)
            //{
            if (methodMode == MethodMode.PlayMode)
            {

                if (replaceOrSyncGroupType == ReplaceOrSyncGroupType.ListOfMaterials && materialList.Count > 0 && materialCache.Count > 0)
                {
                    for (int i = 0; i < materialList.Count; i++)
                    {
                        if (materialList[i] != null)
                        {
                            //Debug.Log("Destroy: " + materialList[i].shader.name + " - " + materialCache[materialList[i]].shader.name);

                            materialList[i].CopyPropertiesFromMaterial(materialCache[materialList[i]]);
                            materialList[i].name = materialCache[materialList[i]].name;
                            materialList[i].shader = materialCache[materialList[i]].shader;


                        }
                    }
                }
            } else if (methodMode == MethodMode.EditorMode)
            {
                //methodMode = MethodMode.Stopped;
            }
            //}

        }
        void OnDisable()
        {
            if (methodMode == MethodMode.EditorMode)
            {
                //methodMode = MethodMode.Stopped;
            }
        }

        //public void CreateUndoMaterialsCopy()
        //{
        //    if(materialList != null)
        //    {

        //        foreach (Material material in materialList)
        //        {
        //            if (material != null)
        //            {
        //                if (!undoMaterialCache.ContainsKey(material))
        //                {
        //                    undoMaterialCache[material] = new Material(material);
        //                }
        //            }
        //        }
        //    }
        //}

        //public void UndoEditorModeMaterialChange()
        //{
        //    for (int i = 0;i < materialList.Count;i++)
        //    {
        //        if (materialList[i] != null)
        //        {
        //            if (undoMaterialCache[materialList[i]] != null)
        //            {
        //                //materialList[i] = undoMaterialCache[materialList[i]];


        //                materialList[i].CopyPropertiesFromMaterial(undoMaterialCache[materialList[i]]);
        //                materialList[i].name = undoMaterialCache[materialList[i]].name;
        //                materialList[i].shader = undoMaterialCache[materialList[i]].shader;
        //            }
        //        }
        //    }
        //    //methodMode = MethodMode.Stopped;

        //    undoMaterialCache.Clear();
        //}

        void Start()
        {
            if (this.isActiveAndEnabled)
            {
                    if (referenceMaterial != null && referenceMaterial.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY) )
                    {

                        if (
#if (UNITY_EDITOR)
                        (methodMode == MethodMode.EditorMode && !EditorApplication.isPlaying)
                        ||
#endif

                        (methodMode == MethodMode.PlayMode  && Application.isPlaying))
                        {
                            SyncTheToonShaderProperties(true);
                        }

                    }
            }
        }


        //void Update()
        void LateUpdate()
        {

            if (this.isActiveAndEnabled)
            {
                if (referenceMaterial != null && referenceMaterial.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                {

#if (UNITY_EDITOR)
                    if ((methodMode == MethodMode.EditorMode && isSyncingEditorMode && !EditorApplication.isPlaying)
                    || (methodMode == MethodMode.PlayMode && keepMaterialsInSyncWithReference && EditorApplication.isPlaying))
                    {
                        SyncTheToonShaderProperties();
                    }
#endif
                }
            }
        }


        protected List<Material> DoSetupOfAllMaterialsGameObjectBasedEditorMode()
        {
            Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();
            List<GameObject> gameObjects = getGameObjectsDependingOnReplaceOrSyncGroupType();
            List<Transform> tmpTransforms = new List<Transform>();


            List<Material> materials = new List<Material>();

            foreach (GameObject go in gameObjects)
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null && renderers[i].sharedMaterials.Length > 0)
                    {
                        Material[] updatedMaterials = renderers[i].sharedMaterials;
                        for (int j = 0; j < renderers[i].sharedMaterials.Length; j++)
                        {
                            Material material = renderers[i].sharedMaterials[j];
                            if (material != null)
                            {
                                if (((1 << renderers[i].gameObject.layer) & layerMaskToAdd) != 0)
                                {
                                    //if (!materialTracker.ContainsKey(material.name))
                                    //{
                                    //    Material mat = new Material(material);       
                                    //    if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                                    //    {
                                    //        mat.shader = referenceMaterial.shader;
                                    //    }
                                    //    InitializeNoiseTextures(mat);

                                    //    materialTracker.Add(material.name, mat);
                                    //}

                                    //material = new Material(material);
                                    if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                                    {
                                        material.shader = referenceMaterial.shader;
                                    }
                                    InitializeNoiseTextures(material);
                                  

                                    updatedMaterials[j] = material;

                                    //if (!tmpTransforms.Contains(renderers[i].gameObject.transform))
                                    //{
                                    //    tmpTransforms.Add(renderers[i].gameObject.transform);
                                    //}


                                    if(!materials.Contains(material)) 
                                    {
                                        materials.Add(material);

                                    }
                                }
                            }
                        }
                        renderers[i].sharedMaterials = updatedMaterials;
                    }
                }
            }
            transformsWithTheToonShader = tmpTransforms.ToArray();
            materialListForParents = materials;
            return materialListForParents;
        }

        protected void DoSetupOfAllMaterialsGameObjectBasedPlayMode()
        {
            Dictionary<string, Material> materialTracker = new Dictionary<string, Material>();
            List<GameObject> gameObjects = getGameObjectsDependingOnReplaceOrSyncGroupType();
            List<Transform> tmpTransforms = new List<Transform>();


            foreach (GameObject go in gameObjects)
            {
                Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < renderers.Length; i++)
                {
                    if (renderers[i] != null && renderers[i].materials.Length > 0)
                    {
                        Material[] updatedMaterials = renderers[i].materials;
                        for (int j = 0; j < renderers[i].materials.Length; j++)
                        {
                            Material material = renderers[i].materials[j];
                            if (material != null)
                            {
                                if (((1 << renderers[i].gameObject.layer) & layerMaskToAdd) != 0)
                                {
                                    ReplaceAndSyncUtils.AddTheToonShaderInstancePrefix(material);
                                    string name = material.name.Replace(" (Instance)", "");
                                    if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                                    {
                                        if (!name.Contains(" - Replaced by " + referenceMaterial.name))
                                        {
                                            name += " - Replaced by " + referenceMaterial.name;
                                        }
                                    }
                                    else
                                    {
                                        if (!name.Contains(" - Synced with " + referenceMaterial.name))
                                        {
                                            name += " - Synced with " + referenceMaterial.name;
                                        }
                                    }

                                    material.name = name;

                                    if (!materialTracker.ContainsKey(material.name))
                                    {
                                        Material mat = new Material(material);
                                        if (!material.HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                                        {
                                            mat.shader = referenceMaterial.shader;
                                        }
                                        InitializeNoiseTextures(mat);

                                        materialTracker.Add(material.name, mat);
                                    }
                                    updatedMaterials[j] = materialTracker[material.name];
              
                                    if (!tmpTransforms.Contains(renderers[i].gameObject.transform))
                                    {
                                        tmpTransforms.Add(renderers[i].gameObject.transform);
                                    }
                                }                                
                            }
                        }
                        renderers[i].materials = updatedMaterials;
                    }
                }
            }
            transformsWithTheToonShader = tmpTransforms.ToArray();
        }

        protected List<GameObject> getGameObjectsDependingOnReplaceOrSyncGroupType()
        {
            List<GameObject> tempGameObjectList = new List<GameObject>();
            switch (replaceOrSyncGroupType)
            {
                case ReplaceOrSyncGroupType.Parent:
                    {
                        Transform[] transforms = parentTransform.GetComponentsInChildren<Transform>();

                        foreach (Transform transform in transforms)
                        {

                            Renderer renderer = transform.gameObject.GetComponent<Renderer>();
                            //Debug.Log(renderer != null);

                            if (renderer != null && renderer.sharedMaterials.Length > 0)
                            {

                                tempGameObjectList.Add(transform.gameObject);
                            }
                        }

                        break;
                    }
                //case ReplaceOrSyncGroupType.ListOfGameObjects:
                //    {
                //        foreach (GameObject gameObject in gameObjectList)
                //        {
                //            if (gameObject.GetComponent<Renderer>() != null)
                //            {
                //                //gameObjectList.Add(gO);
                //                AddFilteredGameObject(gameObject, tempGameObjectList);
                //            }
                //        }

                //        break;
                //    }
                case ReplaceOrSyncGroupType.ListOfMaterials:
                    {
                        break;
                    }
                default: break;
            }
            return tempGameObjectList;
        }


    }
}