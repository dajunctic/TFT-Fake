using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.Graphs;
using UnityEditorInternal;
using UnityEngine;

using static ShaderCrew.TheToonShader.ReplaceOrSynchronizeToonSettings;



namespace ShaderCrew.TheToonShader
{
    [CustomEditor(typeof(ReplaceOrSynchronizeToonSettings))]
    public class ReplaceOrSynchronizeToonSettingsEditor : Editor
    {



#region VARIABLES
        private ReplaceOrSynchronizeToonSettings replaceOrSynchronizeToonSettings;

        private SerializedProperty replaceOrSyncMode;
        private SerializedProperty methodMode;

        private SerializedProperty replaceOrSyncGroupType;


        private SerializedProperty keepMaterialsInSyncWithReference;


        private SerializedProperty isSyncingEditorMode;

        private SerializedProperty parentTransform;


        //private SerializedProperty materialList;

        private SerializedProperty materialList;
        private SerializedProperty materialListForParents;
        private ReorderableList materialsReorderableList;

        //private SerializedProperty replaceOrSyncMode;
        #region SerializedPropertyDeclaration
        private SerializedProperty surfaceOptionsAndInputs;
        private SerializedProperty surfaceOptionsAndInputsFoldout;
        private SerializedProperty surfaceOptions;
        private SerializedProperty surfaceOptionsFoldout;
        private SerializedProperty surfaceInputs;
        private SerializedProperty surfaceInputsFoldout;

        private SerializedProperty toonShading;
        private SerializedProperty toonShadingFoldout;
        private SerializedProperty toonShadingLightPartitioning;
        private SerializedProperty toonShadingLightPartitioningFoldout;
        private SerializedProperty toonShadingLight;
        private SerializedProperty toonShadingLightFoldout;
        private SerializedProperty toonShadingShading;
        private SerializedProperty toonShadingShadingFoldout;
        private SerializedProperty toonShadingCastShadows;
        private SerializedProperty toonShadingCastShadowsFoldout;
        private SerializedProperty toonShadingSpecular;
        private SerializedProperty toonShadingSpecularFoldout;
        private SerializedProperty toonShadingRim;
        private SerializedProperty toonShadingRimFoldout;

        private SerializedProperty styling;
        private SerializedProperty stylingFoldout;

        private SerializedProperty stylingDistanceFade;
        private SerializedProperty stylingDistanceFadeFoldout;

        private SerializedProperty stylingShading;
        private SerializedProperty stylingShadingFoldout;
        private SerializedProperty stylingShadingPartitioning;
        private SerializedProperty stylingShadingPartitioningFoldout;
        private SerializedProperty stylingShadingBlending;
        private SerializedProperty stylingShadingBlendingFoldout;
        private SerializedProperty stylingShadingDrawSpace;
        private SerializedProperty stylingShadingDrawSpaceFoldout;
        private SerializedProperty stylingShadingDensityAndRotation;
        private SerializedProperty stylingShadingDensityAndRotationFoldout;
        private SerializedProperty stylingShadingStyleSettings;
        private SerializedProperty stylingShadingStyleSettingsFoldout;
        private SerializedProperty stylingShadingRandomize;
        private SerializedProperty stylingShadingRandomizeFoldout;

        private SerializedProperty stylingCastShadows;
        private SerializedProperty stylingCastShadowsFoldout;
        private SerializedProperty stylingCastShadowsPartitioning;
        private SerializedProperty stylingCastShadowsPartitioningFoldout;
        private SerializedProperty stylingCastShadowsBlending;
        private SerializedProperty stylingCastShadowsBlendingFoldout;
        private SerializedProperty stylingCastShadowsDrawSpace;
        private SerializedProperty stylingCastShadowsDrawSpaceFoldout;
        private SerializedProperty stylingCastShadowsDensityAndRotation;
        private SerializedProperty stylingCastShadowsDensityAndRotationFoldout;
        private SerializedProperty stylingCastShadowsStyleSettings;
        private SerializedProperty stylingCastShadowsStyleSettingsFoldout;
        private SerializedProperty stylingCastShadowsRandomize;
        private SerializedProperty stylingCastShadowsRandomizeFoldout;

        private SerializedProperty stylingSpecular;
        private SerializedProperty stylingSpecularFoldout;
        private SerializedProperty stylingSpecularSettings;
        private SerializedProperty stylingSpecularSettingsFoldout;
        private SerializedProperty stylingSpecularBlending;
        private SerializedProperty stylingSpecularBlendingFoldout;
        private SerializedProperty stylingSpecularDrawSpace;
        private SerializedProperty stylingSpecularDrawSpaceFoldout;
        private SerializedProperty stylingSpecularDensityAndRotation;
        private SerializedProperty stylingSpecularDensityAndRotationFoldout;
        private SerializedProperty stylingSpecularStyleSettings;
        private SerializedProperty stylingSpecularStyleSettingsFoldout;
        private SerializedProperty stylingSpecularRandomize;
        private SerializedProperty stylingSpecularRandomizeFoldout;

        private SerializedProperty stylingRim;
        private SerializedProperty stylingRimFoldout;
        private SerializedProperty stylingRimSettings;
        private SerializedProperty stylingRimSettingsFoldout;
        private SerializedProperty stylingRimBlending;
        private SerializedProperty stylingRimBlendingFoldout;
        private SerializedProperty stylingRimDrawSpace;
        private SerializedProperty stylingRimDrawSpaceFoldout;
        private SerializedProperty stylingRimDensityAndRotation;
        private SerializedProperty stylingRimDensityAndRotationFoldout;
        private SerializedProperty stylingRimStyleSettings;
        private SerializedProperty stylingRimStyleSettingsFoldout;
        private SerializedProperty stylingRimRandomize;
        private SerializedProperty stylingRimRandomizeFoldout;


        private SerializedProperty outline;
        private SerializedProperty outlineFoldout;



        //PROPERTIES:

        // General  Shading:
        private SerializedProperty _ShadingMode;
        private SerializedProperty _LightFunction;

        private SerializedProperty _UseAlphaOnlyFromBaseMap;

        //BiRP and URP:
        private SerializedProperty _Cull;
        private SerializedProperty _AlphaClip;
        private SerializedProperty _Cutoff;
        private SerializedProperty _BumpScale;
        private SerializedProperty _BumpMap;
        private SerializedProperty _EmissionColor;
        private SerializedProperty _EmissionMap;
        private SerializedProperty _SmoothnessTextureChannel;
        private SerializedProperty _Metallic;
        private SerializedProperty _MetallicGlossMap;
        private SerializedProperty _Parallax;
        private SerializedProperty _ParallaxMap;
        private SerializedProperty _OcclusionStrength;
        private SerializedProperty _OcclusionMap;
        private SerializedProperty _DetailMask;
        private SerializedProperty _DetailAlbedoMap;
        private SerializedProperty _DetailNormalMapScale;
        private SerializedProperty _DetailNormalMap;

#if USING_URP
        //URP only:
        private SerializedProperty _BaseMap;
        private SerializedProperty _BaseColor;

        private SerializedProperty _Surface;
        private SerializedProperty _Blend;
#else
        //BiRP only:
        private SerializedProperty _Color;
        private SerializedProperty _MainTex;
        private SerializedProperty _Glossiness;
        private SerializedProperty _GlossMapScale;
        private SerializedProperty _UVSec;
        private SerializedProperty _Mode;
#endif




        private SerializedProperty _ReceiveShadows;
        // SimpleLit and Lit
        private SerializedProperty _SpecularHighlights;
        private SerializedProperty _SpecColor;
        private SerializedProperty _SpecGlossMap;
        private SerializedProperty _Smoothness;
        //SimpleLit
        private SerializedProperty _SmoothnessSource;
        //Lit
        private SerializedProperty _WorkflowMode;

        private SerializedProperty _EnvironmentReflections;

        private SerializedProperty _DetailAlbedoMapScale;



        //TOON SHADING:
        private SerializedProperty _EnableToonShading;
        private SerializedProperty _ShadingFunction;

        //Gradient:
        private SerializedProperty _GradientTex;
        private SerializedProperty _GradientMode;
        private SerializedProperty _GradientBlending;
        private SerializedProperty _GradientBlendFactor;

        //Cells:
        private SerializedProperty _NumberOfCells;
        private SerializedProperty _CellTransitionSmoothness;
        private SerializedProperty _SumLightsBeforePosterization;
        private SerializedProperty _ShadingUseLightColors;

        //Shadows:
        private SerializedProperty _EnableShadows;
        private SerializedProperty _CoreShadowColor;
        private SerializedProperty _TerminatorPosition;
        private SerializedProperty _TerminatorWidth;
        private SerializedProperty _TerminatorSmoothness;
        private SerializedProperty _FormShadowColor;
        //Cast Shadows:
        private SerializedProperty _EnableCastShadows;
        //private SerializedProperty _CustomizeCastShadowsColor;
        //private SerializedProperty _CastShadowsStrength;
        //private SerializedProperty _CastShadowsSmoothness;
        private SerializedProperty _CastShadowColorMode;
        private SerializedProperty _CastShadowColor;

        private SerializedProperty _ShadingAffectedByNormalMap;

        //Specular:
        private SerializedProperty _EnableSpecular;
        private SerializedProperty _SpecularBlending;
        private SerializedProperty _SpecularColor;
        private SerializedProperty _SpecularSize;
        private SerializedProperty _SpecularSmoothness;
        private SerializedProperty _SpecularOpacity;
        private SerializedProperty _SpecularAffectedByNormalMap;
        private SerializedProperty _SpecularUseLightColors;

        //Rim:
        private SerializedProperty _EnableRim;
        private SerializedProperty _RimBlending;
        private SerializedProperty _RimColor;
        private SerializedProperty _RimSize;
        private SerializedProperty _RimSmoothness;
        private SerializedProperty _RimOpacity;
        private SerializedProperty _RimAffectedArea;
        private SerializedProperty _RimAffectedByNormalMap;





        // STYLING:
        private SerializedProperty _EnableStyling;
        //Distance Fade:
        private SerializedProperty _EnableStylingDistanceFade;
        private SerializedProperty _StylingDFStartingDistance;
        private SerializedProperty _StylingDFFalloff;
        private SerializedProperty _StylingAdjustDistanceFadeValue;
        private SerializedProperty _StylingDistanceFadeValue;


        private SerializedProperty _EnableAntiAliasing;

        //SYTLING SHADING:
        private SerializedProperty _EnableShadingStyling;
        private SerializedProperty _StylingShadingSyncWithOtherStyling;

        private SerializedProperty _StylingColor;
        //Style
        private SerializedProperty _ShadingStyle;
        //Partitioning:
        private SerializedProperty _SyncWithLightPartitioning;
        private SerializedProperty _NumberOfCellsHatching;
        private SerializedProperty _StylingTerminatorPosition;
        private SerializedProperty _StylingOvermodelingFactor;
        //Blending:
        private SerializedProperty _StylingShadingBlending;
        private SerializedProperty _StylingShadingIsInverted;
        //UV SPACE:
        private SerializedProperty _DrawSpace;
        private SerializedProperty _SSCameraDistanceScaled;
        private SerializedProperty _AnchorSSToObjectsOrigin;
        private SerializedProperty _CoordinateSystem;
        private SerializedProperty _PolarCenterMode;
        private SerializedProperty _PolarCenter;

        //Density  and Rotation:
        private SerializedProperty _StylingShadingDensity;
        private SerializedProperty _StylingShadingInitialDirection;
        private SerializedProperty _StylingShadingRotationBetweenCells;
        private SerializedProperty _StylingShadingHalftonesOffset;

        //Style Settings:
        private SerializedProperty _StylingShadingThicknessControl;
        private SerializedProperty _StylingShadingThickness;
        private SerializedProperty _StylingShadingOpacity;
        private SerializedProperty _StylingShadingOpacityFalloff;
        private SerializedProperty _StylingShadingThicknessFalloff;
        private SerializedProperty _StylingShadingHardness;

        private SerializedProperty _StylingShadingHalftonesRoundness;
        private SerializedProperty _StylingShadingHalftonesRoundnessFalloff;

        //Randomizer:
        private SerializedProperty _EnableShadingRandomizer;
        private SerializedProperty _ShadingNoise1Size;
        private SerializedProperty _ShadingNoise1Seed;
        private SerializedProperty _ShadingNoise2Seed;
        private SerializedProperty _NoiseIntensity;
        private SerializedProperty _SpacingRandomMode;
        private SerializedProperty _SpacingRandomIntensity;
        private SerializedProperty _OpacityRandomMode;
        private SerializedProperty _OpacityRandomIntensity;
        private SerializedProperty _HardnessRandomMode;
        private SerializedProperty _HardnessRandomIntensity;
        private SerializedProperty _LengthRandomMode;
        private SerializedProperty _LengthRandomIntensity;
        private SerializedProperty _ThicknessRandomMode;
        private SerializedProperty _ThicknesshRandomIntensity;



        //STYLING CAST SHADOWS:
        private SerializedProperty _EnableCastShadowsStyling;
        private SerializedProperty _StylingCastShadowsSyncWithOtherStyling;

        private SerializedProperty _StylingCastShadowsColor;
        //Style:
        private SerializedProperty _CastShadowsStyle;
        //Partitioning:
        private SerializedProperty _CastShadowsNumberOfCellsHatching;
        private SerializedProperty _StylingCastShadowsSmoothness;

        // Blending:
        private SerializedProperty _StylingCastShadowsBlending;
        private SerializedProperty _StylingCastShadowsIsInverted;
        // UV Space:
        private SerializedProperty _CastShadowsDrawSpace;
        private SerializedProperty _CastShadowsSSCameraDistanceScaled;
        private SerializedProperty _CastShadowsAnchorSSToObjectsOrigin;
        private SerializedProperty _CastShadowsCoordinateSystem;
        private SerializedProperty _CastShadowsPolarCenterMode;
        private SerializedProperty _CastShadowsPolarCenter;
        //Density and Direction
        private SerializedProperty _StylingCastShadowsDensity;
        private SerializedProperty _StylingCastShadowsInitialDirection;
        private SerializedProperty _StylingCastShadowsRotationBetweenCells;
        private SerializedProperty _StylingCastShadowsHalftonesOffset;
        //Style Settngs:
        private SerializedProperty _StylingCastShadowsOpacity;
        private SerializedProperty _StylingCastShadowsOpacityFalloff;
        private SerializedProperty _StylingCastShadowsThicknessControl;
        private SerializedProperty _StylingCastShadowsThickness;
        private SerializedProperty _StylingCastShadowsThicknessFalloff;
        private SerializedProperty _StylingCastShadowsHardness;
        private SerializedProperty _StylingCastShadowsHalftonesRoundness;
        private SerializedProperty _StylingCastShadowsHalftonesRoundnessFalloff;
        //Randomizer:
        private SerializedProperty _EnableCastShadowsRandomizer;
        private SerializedProperty _CastShadowsNoise1Size;
        private SerializedProperty _CastShadowsNoise1Seed;
        private SerializedProperty _CastShadowsNoise2Seed;
        private SerializedProperty _CastShadowsNoiseIntensity;
        private SerializedProperty _CastShadowsSpacingRandomMode;
        private SerializedProperty _CastShadowsSpacingRandomIntensity;
        private SerializedProperty _CastShadowsOpacityRandomMode;
        private SerializedProperty _CastShadowsOpacityRandomIntensity;
        private SerializedProperty _CastShadowsHardnessRandomMode;
        private SerializedProperty _CastShadowsHardnessRandomIntensity;
        private SerializedProperty _CastShadowsLengthRandomMode;
        private SerializedProperty _CastShadowsLengthRandomIntensity;
        private SerializedProperty _CastShadowsThicknessRandomMode;
        private SerializedProperty _CastShadowsThicknesshRandomIntensity;


        //STYLING SPECULAR:
        private SerializedProperty _EnableSpecularStyling;
        private SerializedProperty _StylingSpecularSyncWithOtherStyling;
        //Specular Settings:
        private SerializedProperty _SyncWithSpecular;
        private SerializedProperty _StylingSpecularSize;
        private SerializedProperty _StylingSpecularSmoothness;

        private SerializedProperty _StylingSpecularColor;
        private SerializedProperty _StylingSpecularUseLightColors;
        //Style
        private SerializedProperty _SpecularStyle;
        // Blending:
        private SerializedProperty _StylingSpecularBlending;
        private SerializedProperty _StylingSpecularIsInverted;
        // UV Space:
        private SerializedProperty _SpecularDrawSpace;
        private SerializedProperty _SpecularSSCameraDistanceScaled;
        private SerializedProperty _SpecularAnchorSSToObjectsOrigin;
        private SerializedProperty _SpecularCoordinateSystem;
        private SerializedProperty _SpecularPolarCenterMode;
        private SerializedProperty _SpecularPolarCenter;
        //Density and Direction
        private SerializedProperty _StylingSpecularDensity;
        private SerializedProperty _StylingSpecularRotation;
        private SerializedProperty _StylingSpecularHalftonesOffset;
        //Style Settngs:
        private SerializedProperty _StylingSpecularOpacity;
        private SerializedProperty _StylingSpecularOpacityFalloff;
        private SerializedProperty _StylingSpecularThicknessControl;
        private SerializedProperty _StylingSpecularThickness;
        private SerializedProperty _StylingSpecularThicknessFalloff;
        private SerializedProperty _StylingSpecularHardness;
        private SerializedProperty _StylingSpecularHalftonesRoundness;
        private SerializedProperty _StylingSpecularHalftonesRoundnessFalloff;
        //Randomizer:
        private SerializedProperty _EnableSpecularRandomizer;
        private SerializedProperty _SpecularNoise1Size;
        private SerializedProperty _SpecularNoise1Seed;
        private SerializedProperty _SpecularNoise2Seed;
        private SerializedProperty _SpecularNoiseIntensity;
        private SerializedProperty _SpecularSpacingRandomMode;
        private SerializedProperty _SpecularSpacingRandomIntensity;
        private SerializedProperty _SpecularOpacityRandomMode;
        private SerializedProperty _SpecularOpacityRandomIntensity;
        private SerializedProperty _SpecularHardnessRandomMode;
        private SerializedProperty _SpecularHardnessRandomIntensity;
        private SerializedProperty _SpecularLengthRandomMode;
        private SerializedProperty _SpecularLengthRandomIntensity;
        private SerializedProperty _SpecularThicknessRandomMode;
        private SerializedProperty _SpecularThicknesshRandomIntensity;


        //STYLING RIM:
        private SerializedProperty _EnableRimStyling;
        private SerializedProperty _StylingRimSyncWithOtherStyling;
        //Style
        private SerializedProperty _RimStyle;
        //Rim Settings:
        private SerializedProperty _SyncWithRim;
        private SerializedProperty _StylingRimSize;
        private SerializedProperty _StylingRimSmoothness;
        private SerializedProperty _StylingRimAffectedArea;

        private SerializedProperty _StylingRimColor;
        // Blending:
        private SerializedProperty _StylingRimBlending;
        private SerializedProperty _StylingRimIsInverted;
        // UV Space:
        private SerializedProperty _RimDrawSpace;
        private SerializedProperty _RimSSCameraDistanceScaled;
        private SerializedProperty _RimAnchorSSToObjectsOrigin;
        private SerializedProperty _RimCoordinateSystem;
        private SerializedProperty _RimPolarCenterMode;
        private SerializedProperty _RimPolarCenter;
        //Density and Direction
        private SerializedProperty _StylingRimDensity;
        private SerializedProperty _StylingRimRotation;
        private SerializedProperty _StylingRimHalftonesOffset;
        //Style Settngs:
        private SerializedProperty _StylingRimThicknessControl;
        private SerializedProperty _StylingRimThickness;
        private SerializedProperty _StylingRimThicknessFalloff;
        private SerializedProperty _StylingRimOpacity;
        private SerializedProperty _StylingRimOpacityFalloff;
        private SerializedProperty _StylingRimHardness;
        private SerializedProperty _StylingRimHalftonesRoundness;
        private SerializedProperty _StylingRimHalftonesRoundnessFalloff;
        //Randomizer:
        private SerializedProperty _EnableRimRandomizer;
        private SerializedProperty _RimNoise1Size;
        private SerializedProperty _RimNoise1Seed;
        private SerializedProperty _RimNoise2Seed;
        private SerializedProperty _RimNoiseIntensity;
        private SerializedProperty _RimSpacingRandomMode;
        private SerializedProperty _RimSpacingRandomIntensity;
        private SerializedProperty _RimOpacityRandomMode;
        private SerializedProperty _RimOpacityRandomIntensity;
        private SerializedProperty _RimHardnessRandomMode;
        private SerializedProperty _RimHardnessRandomIntensity;
        private SerializedProperty _RimLengthRandomMode;
        private SerializedProperty _RimLengthRandomIntensity;
        private SerializedProperty _RimThicknessRandomMode;
        private SerializedProperty _RimThicknesshRandomIntensity;


        private SerializedProperty _HatchingAffectedByNormalMap;

        private SerializedProperty _HatchingCameraDistanceFade;
        private SerializedProperty _HalftonePatternCameraDistanceFade;




        private SerializedProperty _EnableOutline;
        private SerializedProperty _OutlineColor;
        private SerializedProperty _OutlineWidth;
        private SerializedProperty _OutlineDepthOffset;
        private SerializedProperty _OutlineCameraDistanceImpact;

#endregion

        AnimBool surfaceOptionsAndInputsAnimBool;
        AnimBool toonShadingAnimBool;
        AnimBool stylingAnimBool;
        AnimBool outlineAnimBool;


        AnimFloat animFloatTest;

        private SerializedProperty referenceMaterial;


        private Dictionary<string, SavedBool> propertiesSynchronizationMapping;

        private const string k_KeyPrefix = "TheToonShader:ReplaceOrSynchronizeToonSettingsEditor:UI_State:";
        private string m_HeaderStateKey = null;
        protected string headerStateKey { get { return m_HeaderStateKey; } }



        private List<Material> materialsWithWrongShader = new List<Material>();

        [SerializeField] private float scrollSpeed = 0.5f;
        private Texture2D stripeTexture;
        private float scrollOffset = 0f;
        private double lastTime;

        #endregion
        private void InitializeReorderableListForMaterials()
        {
            materialsWithWrongShader.Clear();
            materialsReorderableList = new ReorderableList(serializedObject, materialList)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = false,
                drawHeaderCallback = rect =>
                {
                    //GUI.color = Color.cyan;
                    //GUI.backgroundColor = Color.red;

                    GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
                    style.normal.textColor = Color.white;
                    style.alignment = TextAnchor.MiddleCenter;
                    EditorGUI.LabelField(rect, "List of Materials", style);

                },
                drawElementCallback = (rect, index, focused, active) =>
                {

                    var element = materialList.GetArrayElementAtIndex(index);

                    var backgroundColor = GUI.backgroundColor;
                    int count = 0;
                    for (int i = 0; i < materialList.arraySize; i++)
                    {
                        if (materialList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    if (element.objectReferenceValue == null || count > 1)
                    {
                        GUI.backgroundColor = Color.red;
                    }
                    else
                    {
                        GUI.backgroundColor = backgroundColor;
                    }




                    //GUI.color = Color.cyan;
                    GUIStyle insertVarNameHere = new GUIStyle();
                    insertVarNameHere.fontStyle = FontStyle.Bold;
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(element)), element, new GUIContent("Material " + (index + 1) + ": "));

                    GUI.backgroundColor = backgroundColor;


                    if (element.objectReferenceValue == null)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material  " + (index + 1) + "  may not be empty!", MessageType.Error);
                    }
                    else if (count > 1)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Duplicate! Material  " + (index + 1) + "  has to be unique!", MessageType.Error);
                    }
                    //else if ((ReplaceOrSync)replaceOrSyncMode.enumValueIndex == ReplaceOrSync.Sync && !((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                    //{
                    //    rect.y += EditorGUI.GetPropertyHeight(element);
                    //    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material " + (index + 1) + " has to be a \"The Toon Shader\" material!", MessageType.Error);
                    //}
                    //else if ((ReplaceOrSync)replaceOrSyncMode.enumValueIndex == ReplaceOrSync.Replace && ((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                    //{
                    //    rect.y += EditorGUI.GetPropertyHeight(element);
                    //    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material " + (index + 1) + " can NOT be a  \"The Toon Shader\"  material!", MessageType.Error);
                    //}
                    else if (!((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY) && (MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode)
                    {
                        rect.y += EditorGUI.GetPropertyHeight(element);
                        EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2), "Material " + (index + 1) + " has to be a \"The Toon Shader\" material!", MessageType.Error);
                        if(!materialsWithWrongShader.Contains((Material)element.objectReferenceValue)) {
                            materialsWithWrongShader.Add(((Material)element.objectReferenceValue));
                        }
                    }

                },

                elementHeightCallback = index =>
                {
                    var element = materialList.GetArrayElementAtIndex(index);
                    int count = 0;
                    for (int i = 0; i < materialList.arraySize; i++)
                    {
                        if (materialList.GetArrayElementAtIndex(i).objectReferenceValue == element.objectReferenceValue)
                        {
                            count++;
                        }
                    }
                    var height = EditorGUI.GetPropertyHeight(element);
                    if (element.objectReferenceValue == null || count > 1
                    //|| ((ReplaceOrSync)replaceOrSyncMode.enumValueIndex == ReplaceOrSync.Sync && !((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                    //|| ((ReplaceOrSync)replaceOrSyncMode.enumValueIndex == ReplaceOrSync.Replace && ((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
                    || (!((Material)element.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY) && (MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode) )
                    {
                        height += EditorGUIUtility.singleLineHeight * 2;
                    }
                    return height;
                },

                onAddCallback = list =>
                {
                    list.serializedProperty.arraySize++;

                    var newElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                    newElement.objectReferenceValue = null;
                },
                onRemoveCallback = list =>
                {
                    List<Material> temp = new List<Material>();
                    for (int i = 0; i < list.serializedProperty.arraySize; i++)
                    {
                        temp.Add((Material)list.serializedProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                    }
                    list.serializedProperty.arraySize--;
                    temp.RemoveAt(list.index);
                    for (int i = 0; i < temp.Count; i++)
                    {
                        var d = list.serializedProperty.GetArrayElementAtIndex(i);
                        d.objectReferenceValue = temp[i];
                    }
                    //ReorderableList.defaultBehaviours.DoRemoveButton(list);

                }
            };
        }

        private void OnEditorUpdate()
        {
            double currentTime = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(currentTime - lastTime);
            lastTime = currentTime;

            scrollOffset += scrollSpeed * deltaTime;
            scrollOffset %= 1f;
            Repaint();
        }

        private void OnEnable()
        {

            // Generate or assign your angled stripe texture here
            if (stripeTexture == null)
                stripeTexture = GenerateStripeTexture();

            // Initialize time
            lastTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += OnEditorUpdate;



            m_HeaderStateKey = k_KeyPrefix + target.name;// Create key string for editor prefs
            //Debug.Log(m_HeaderStateKey);


            replaceOrSynchronizeToonSettings = (ReplaceOrSynchronizeToonSettings)target;



            referenceMaterial = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.referenceMaterial));


            //replaceOrSyncMode = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.replaceOrSyncMode));


            foreach (string propertyName in ShaderUtils.ALL_PROPERITES)
            {

                //Debug.Log(this.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic));
                //Debug.Log(serializedObject.FindProperty(propertyName));
                this.GetType().GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, serializedObject.FindProperty(propertyName));
                //Debug.Log(this.GetType().GetField(propertyName));
            }

            //test = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.test));
            ////test.FindPropertyRelative("propertiesActive").intValue = 44;
            //Debug.Log(test.FindPropertyRelative("propertiesActive").intValue);


            //replaceOrSyncMode = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.replaceOrSyncMode));

            methodMode = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.methodMode));

            replaceOrSyncGroupType = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.replaceOrSyncGroupType));

            keepMaterialsInSyncWithReference = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.keepMaterialsInSyncWithReference));
            
            isSyncingEditorMode = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.isSyncingEditorMode));

            parentTransform = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.parentTransform));





            materialList = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.materialList));

            materialListForParents = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.materialListForParents));

            InitializeReorderableListForMaterials();

            surfaceOptionsAndInputs = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceOptionsAndInputs));
            surfaceOptionsAndInputsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceOptionsAndInputsFoldout));
            surfaceOptions = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceOptions));
            surfaceOptionsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceOptionsFoldout));
            surfaceInputs = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceInputs));
            surfaceInputsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.surfaceInputsFoldout));

            toonShading = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShading));
            toonShadingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingFoldout));
            toonShadingLightPartitioning = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingLightPartitioning));
            toonShadingLightPartitioningFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingLightPartitioningFoldout));
            toonShadingLight = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingLight));
            toonShadingLightFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingLightFoldout));
            toonShadingShading = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingShading));
            toonShadingShadingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingShadingFoldout));
            toonShadingCastShadows = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingCastShadows));
            toonShadingCastShadowsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingCastShadowsFoldout));
            toonShadingSpecular = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingSpecular));
            toonShadingSpecularFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingSpecularFoldout));
            toonShadingRim = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingRim));
            toonShadingRimFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.toonShadingRimFoldout));

            styling = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.styling));
            stylingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingFoldout));

            stylingDistanceFade= serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingDistanceFade));
            stylingDistanceFadeFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingDistanceFadeFoldout));

            stylingShading = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShading));
            stylingShadingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingFoldout));
            stylingShadingPartitioning = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingPartitioning));
            stylingShadingPartitioningFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingPartitioningFoldout));
            stylingShadingBlending = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingBlending));
            stylingShadingBlendingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingBlendingFoldout));
            stylingShadingDrawSpace = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingDrawSpace));
            stylingShadingDrawSpaceFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingDrawSpaceFoldout));
            stylingShadingDensityAndRotation = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingDensityAndRotation));
            stylingShadingDensityAndRotationFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingDensityAndRotationFoldout));
            stylingShadingStyleSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingStyleSettings));
            stylingShadingStyleSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingStyleSettingsFoldout));
            stylingShadingRandomize = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingRandomize));
            stylingShadingRandomizeFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingShadingRandomizeFoldout));

            stylingCastShadows = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadows));
            stylingCastShadowsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsFoldout));
            stylingCastShadowsPartitioning = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsPartitioning));
            stylingCastShadowsPartitioningFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsPartitioningFoldout));
            stylingCastShadowsBlending = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsBlending));
            stylingCastShadowsBlendingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsBlendingFoldout));
            stylingCastShadowsDrawSpace = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsDrawSpace));
            stylingCastShadowsDrawSpaceFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsDrawSpaceFoldout));
            stylingCastShadowsDensityAndRotation = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsDensityAndRotation));
            stylingCastShadowsDensityAndRotationFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsDensityAndRotationFoldout));
            stylingCastShadowsStyleSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsStyleSettings));
            stylingCastShadowsStyleSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsStyleSettingsFoldout));
            stylingCastShadowsRandomize = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsRandomize));
            stylingCastShadowsRandomizeFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingCastShadowsRandomizeFoldout));

            stylingSpecular = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecular));
            stylingSpecularFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularFoldout));
            stylingSpecularSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularSettings));
            stylingSpecularSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularSettingsFoldout));
            stylingSpecularBlending = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularBlending));
            stylingSpecularBlendingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularBlendingFoldout));
            stylingSpecularDrawSpace = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularDrawSpace));
            stylingSpecularDrawSpaceFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularDrawSpaceFoldout));
            stylingSpecularDensityAndRotation = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularDensityAndRotation));
            stylingSpecularDensityAndRotationFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularDensityAndRotationFoldout));
            stylingSpecularStyleSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularStyleSettings));
            stylingSpecularStyleSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularStyleSettingsFoldout));
            stylingSpecularRandomize = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularRandomize));
            stylingSpecularRandomizeFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingSpecularRandomizeFoldout));

            stylingRim = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRim));
            stylingRimFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimFoldout));
            stylingRimSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimSettings));
            stylingRimSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimSettingsFoldout));
            stylingRimBlending = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimBlending));
            stylingRimBlendingFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimBlendingFoldout));
            stylingRimDrawSpace = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimUVSpace));
            stylingRimDrawSpaceFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimUVSpaceFoldout));
            stylingRimDensityAndRotation = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimDensityAndRotation));
            stylingRimDensityAndRotationFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimDensityAndRotationFoldout));
            stylingRimStyleSettings = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimStyleSettings));
            stylingRimStyleSettingsFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimStyleSettingsFoldout));
            stylingRimRandomize = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimRandomize));
            stylingRimRandomizeFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.stylingRimRandomizeFoldout));


            outline = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.outline));
            outlineFoldout = serializedObject.FindProperty(nameof(ReplaceOrSynchronizeToonSettings.outlineFoldout));





            surfaceOptionsAndInputsAnimBool = new AnimBool(false);
            surfaceOptionsAndInputsAnimBool.valueChanged.AddListener(this.Repaint);

            toonShadingAnimBool = new AnimBool(false);
            toonShadingAnimBool.valueChanged.AddListener(this.Repaint);

            stylingAnimBool = new AnimBool(false);
            stylingAnimBool.valueChanged.AddListener(this.Repaint);

            outlineAnimBool = new AnimBool(false);
            outlineAnimBool.valueChanged.AddListener(this.Repaint);


            //animFloatTest = new AnimFloat(0);
            //animFloatTest.valueChanged.AddListener(this.Repaint);

#if USING_URP
            SetupSurfaceAndOptionsPropertiesArrays();
#else
            SetupSurfaceAndOptionsBiRPPropertiesArrays();
#endif

            SetupToonShadingPropertiesArrays();
            SetupStylingPropertiesArrays();
            SetupOutlinePropertiesArrays();
        }


        int originalIndent;

        private void OnDisable()
        {
            //replaceOrSynchronizeToonSettings.methodMode = MethodMode.Stopped;
            EditorApplication.update -= OnEditorUpdate;

        }
        Color lightBlue = new Color(1.0f, 0.5f, 0.4f, 1);
        private Texture2D GenerateStripeTexture()
        {
            const int width = 32;
            const int height = 8;
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                wrapMode = TextureWrapMode.Repeat,
                filterMode = FilterMode.Bilinear 
            };

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x < 16)
                        texture.SetPixel(x, y, Color.white);
                    else
                        texture.SetPixel(x, y, Color.gray);
                }
            }

            texture.Apply();
            return texture;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();


            Color oriCol = EditorStyles.label.normal.textColor;
            EditorStyles.label.normal.textColor = Color.white;


            originalIndent = EditorGUI.indentLevel;

            string modeText = "Replace or Synchronize";

            EditorUtils.LogoOnlyStart(modeText);



            GUIStyle replacementStyle = new GUIStyle();
            replacementStyle.normal.textColor = Color.white;
            replacementStyle.alignment = TextAnchor.MiddleCenter;
            replacementStyle.fontStyle = FontStyle.Bold;


            EditorUtils.Header("Choose The Method", replacementStyle);

            Rect rectt = EditorGUILayout.BeginVertical();
            rectt.x = 0;
            rectt.width = rectt.width + 30;
            GUI.Box(rectt, GUIContent.none);

            GUILayout.Space(5);

            if((MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode && Application.isPlaying)
            {
                GUI.enabled = false;
            }

            //replaceOrSyncMode.intValue = (int)(ReplaceOrSync)EditorGUILayout.EnumPopup("Replace or Sync Mode: ", (ReplaceOrSync)replaceOrSyncMode.intValue);

            //string[] replaceAndOrSyncTypes = System.Enum.GetNames(typeof(ReplaceOrSync));
            //EditorGUI.BeginChangeCheck();
            //replaceOrSyncMode.intValue = GUILayout.Toolbar(replaceOrSyncMode.intValue, replaceAndOrSyncTypes);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    InitializeReorderableListForMaterials(); //Refresh
            //}


            //EditorGUILayout.PropertyField(methodMode);
            string[] methodModeTypes = System.Enum.GetNames(typeof(MethodMode));


            bool guiState = GUI.enabled;
            if ((MethodMode)methodMode.enumValueIndex == MethodMode.PlayMode && Application.isPlaying)
            {
                GUI.enabled = false;
            }

            EditorGUI.BeginChangeCheck();
            methodMode.enumValueIndex = GUILayout.Toolbar(methodMode.enumValueIndex, methodModeTypes);
            if (EditorGUI.EndChangeCheck())
            {
                //if ((MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode)
                //{
                //    replaceOrSynchronizeToonSettings.CreateUndoMaterialsCopy();
                //}
                isSyncingEditorMode.boolValue = false;
            }
            GUI.enabled = guiState;

            guiState = GUI.enabled;
            if ((MethodMode)methodMode.enumValueIndex ==  MethodMode.EditorMode)
            {
                EditorGUILayout.HelpBox("Press the Button 'Start Syncing' to start this mode! All modifications are permanent, and your materials will be permanently altered with no option to revert to their original state. " 
                    + "If you want to play around without the changes being permanent, use the \"PlayMode\"."
                    //+ "If you want to undo the changes you made this session, you can do that pressing the \"Undo\" Button below. A session is started as soon as the components " +
                    //"runs and ends even if you just re-compile a C# file."
                    , MessageType.Info);

                string syncingText = "Start Syncing";
                if (isSyncingEditorMode.boolValue == true)
                {
                    syncingText = "Stop Syncing";
                }
                if (materialList.arraySize <= 0 && (ReplaceOrSyncGroupType)replaceOrSyncGroupType.enumValueIndex == ReplaceOrSyncGroupType.ListOfMaterials ||
                    referenceMaterial.objectReferenceValue == null)
                {
                    //EditorGUILayout.HelpBox("You haven't added any materials to the list! Without them, you can NOT sync.", MessageType.Error);
                    //EditorGUILayout.HelpBox("You have to set a reference material! Without it, you can NOT sync.", MessageType.Error);

                    GUI.enabled = false;
                }
                if (GUILayout.Button(syncingText))
                {
                    isSyncingEditorMode.boolValue = !isSyncingEditorMode.boolValue;

                    if (isSyncingEditorMode.boolValue == true)
                    {
                        List<Material> materialsFromSetup = replaceOrSynchronizeToonSettings.DoSetupForMaterialsEditorMode();
                        if((MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode && (ReplaceOrSyncGroupType)replaceOrSyncGroupType.enumValueIndex == ReplaceOrSyncGroupType.Parent)
                        {
                            materialListForParents.ClearArray();
                            int index = 0;
                            foreach (Material material in materialsFromSetup)
                            {
                                materialListForParents.InsertArrayElementAtIndex(index);
                                materialListForParents.GetArrayElementAtIndex(index).objectReferenceValue = material;
                                index++;
                            }
                        }

                        replaceOrSynchronizeToonSettings.SyncTheToonShaderProperties(true);
                    }
                }

                //if(isSyncingEditorMode.boolValue == true)
                //{
                //    EditorGUILayout.Space(4);
                //    Rect barRect = GUILayoutUtility.GetRect(1, 4, GUILayout.ExpandWidth(true));
                //    if (stripeTexture != null)
                //    {
                //        float tilingX = barRect.width / 64f;
                //        Rect textureRect = new Rect(-scrollOffset, 0f, tilingX, 1f);
                //        GUI.DrawTextureWithTexCoords(barRect, stripeTexture, textureRect);
                //    }
                //}

                GUI.enabled = guiState;


                //if (GUILayout.Button("Undo"))
                //{
                //    //methodMode.enumValueIndex = (int)MethodMode.Stopped;
                //    replaceOrSynchronizeToonSettings.UndoEditorModeMaterialChange();
                //}




            }
            else if((MethodMode)methodMode.enumValueIndex == MethodMode.PlayMode)
            {
                EditorGUILayout.HelpBox("Press PLAY to start this mode! All modifications are transient, and upon stop, your materials will promptly return to their original state.", MessageType.Info);
                keepMaterialsInSyncWithReference.boolValue = EditorGUILayout.ToggleLeft("Keep Materials in Sync with Reference", keepMaterialsInSyncWithReference.boolValue);

            }


            if ((MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode && isSyncingEditorMode.boolValue == true
               || ((MethodMode)methodMode.enumValueIndex == MethodMode.PlayMode && Application.isPlaying))
            {
                EditorGUILayout.Space(4);
                Rect barRect = GUILayoutUtility.GetRect(1, 4, GUILayout.ExpandWidth(true));
                if (stripeTexture != null)
                {
                    float tilingX = barRect.width / 64f;
                    Rect textureRect = new Rect(-scrollOffset, 0f, tilingX, 1f);
                    GUI.DrawTextureWithTexCoords(barRect, stripeTexture, textureRect);
                }
            }

            //else if ((MethodMode)methodMode.enumValueIndex == MethodMode.Stopped)
            //{
            //    EditorGUILayout.HelpBox("Currently this component is stopped, select one of the two method modes to activate it. Keep in mind that the modfications in Editor Mode are permanent, so only enable it, if you are ready for those changes! It's always good to test out your settings in Play Mode!", MessageType.Warning);
            //}



            //for (int i = 0; i < materialList.arraySize; i++)
            //{
            //    Material material = (Material) materialList.GetArrayElementAtIndex(i).objectReferenceValue;
            //    //Debug.Log(material.name);

            //}

            //EditorGUILayout.PropertyField(keepMaterialsInSyncWithReference);



            GUILayout.Space(5);


            EditorGUILayout.EndVertical();
            EditorUtils.DrawUILineFullWidth(Color.black, thickness: 1, padding: -5);

            GUILayout.Space(15);


            if(isSyncingEditorMode.boolValue == true || ((MethodMode)methodMode.enumValueIndex == MethodMode.PlayMode && Application.isPlaying) )
            {
                GUI.enabled = false;
            }

            EditorUtils.Header("Choose The Reference Material", replacementStyle);

            rectt = EditorGUILayout.BeginVertical();
            rectt.x = 0;
            rectt.width = rectt.width + 20;
            GUI.Box(rectt, GUIContent.none);

            GUILayout.Space(5);


            EditorGUILayout.PropertyField(referenceMaterial);
            if (referenceMaterial.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("You have to set a reference material, " +
                    " otherwise this component won't work"
                    //"otherwise the 'The Toon Shader' will be limited to the default shader settings and so won't work as expected!"
                    , MessageType.Error);

            }
            else if (!((Material)referenceMaterial.objectReferenceValue).HasProperty(TheToonShaderConstants.THETOONSHADER_IDENTIFIER_PROPERTY))
            {
                EditorGUILayout.HelpBox("The reference material is NOT a 'The Toon Shader' material! " + modeText + " will NOT work with a NON-TTS shader!", MessageType.Error);

            }

            GUILayout.Space(5);


            EditorGUILayout.EndVertical();
            EditorUtils.DrawUILineFullWidth(Color.black, thickness: 1, padding: -5);

            GUILayout.Space(15);



            EditorUtils.Header("Choose The Group Type", replacementStyle);

            rectt = EditorGUILayout.BeginVertical();
            rectt.x = 0;
            rectt.width = rectt.width + 20;
            GUI.Box(rectt, GUIContent.none);
            GUILayout.Space(5);

            EditorGUILayout.PropertyField(replaceOrSyncGroupType, new GUIContent("Group Type"));


            switch ((ReplaceOrSyncGroupType)replaceOrSyncGroupType.enumValueIndex)
            {
                case ReplaceOrSyncGroupType.Parent:
                    {
                        EditorGUILayout.PropertyField(parentTransform);
                        if (parentTransform.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("You didn't choose a parent transform, that means that this GameObject, '" +
                                this.target.name + "' will be the parent. Every child  of it that isn't in" +
                                " the exemption list will get the The Toon Shader assigned to its material during runtime.", MessageType.Info);
                        }
                        break;
                    }

                case ReplaceOrSyncGroupType.ListOfMaterials:
                    {
                        materialsReorderableList.DoLayoutList();
                        if (materialList.arraySize <= 0)
                        {
                            EditorGUILayout.HelpBox("You haven't added any Materials to the list, that means that the  " + this.target.GetType().Name + " script won't work!.", MessageType.Warning);
                        }
                        int materialsWithWrongShaderCount = materialsWithWrongShader.Count;
                        if (materialsWithWrongShader != null && materialsWithWrongShaderCount > 0 && (MethodMode)methodMode.enumValueIndex == MethodMode.EditorMode)
                        {
                            GUILayout.Space(5);

                            string numberOfMaterials;
                            if (materialsWithWrongShaderCount == 1)
                            {
                                numberOfMaterials = "is 1 material";
                            }
                            else
                            {
                                numberOfMaterials = "are " + materialsWithWrongShaderCount + " materials";
                            }
                            EditorGUILayout.HelpBox("There " + numberOfMaterials + " with non-TTS shaders. Syncing in the Editor Mode will only work with materials using a TTS shader!", MessageType.Warning);
                            EditorGUI.BeginChangeCheck();
                            if (GUILayout.Button("Apply \"The Toon Shader\""))
                            {
                                foreach (Material material in materialsWithWrongShader)
                                {
                                    material.shader = ((Material)referenceMaterial.objectReferenceValue).shader;
                                }
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                InitializeReorderableListForMaterials();
                            }
                        }
                        break;
                    }
                default: break;
            }


            //Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(1));
            ////r.x = -5;
            //r.width = r.width + 6;
            //r.x = r.x - 3;

            ////rectt.width = rectt.width + rectt.x;
            ////rectt.x = -10;


            //EditorGUI.DrawRect(r, new Color(0.2f, 0.2f, 0.2f, 1));



            GUILayout.Space(5);

            EditorGUILayout.EndVertical();
            EditorUtils.DrawUILineFullWidth(Color.black, thickness: 1, padding: -5);

            GUILayout.Space(15);


            EditorUtils.Header("The Toon Shader Properties", replacementStyle); // \n For " + modeText, replacementStyle);

            if (referenceMaterial!= null && referenceMaterial.objectReferenceValue != null)
            {
                EditorGUI.BeginChangeCheck();
                DrawReferenceMaterialPropertySelectionSection();
                if (EditorGUI.EndChangeCheck())
                {
                    bool test = serializedObject.ApplyModifiedProperties();
                    if(test)
                    {
                        replaceOrSynchronizeToonSettings.CalculatePropertyToogleBoolDictionary(); //Refresh
                    }

                    //replaceOrSynchronizeToonSettings.PrintStatusOfPropertiesAndToggles(); 
                    
                }
            }
            GUI.enabled = true;
            //flags = EditorGUILayout.MaskField("Player Flags", flags, options);

            EditorStyles.label.normal.textColor = oriCol;


            EditorUtils.LogoOnlyEnd();


            serializedObject.ApplyModifiedProperties();
        }



        private void DrawReferenceMaterialPropertySelectionSection()
        {
#if USING_URP
            DrawCategory(surfaceAndOptionsAllProperties.ToArray(), Position.Top, category: surfaceOptionsAndInputs, categoryName: null, categoryFoldout: surfaceOptionsAndInputsFoldout, animBoolFoldout: surfaceOptionsAndInputsAnimBool, SurfaceOptionAndInputs);
#else
            DrawCategory(surfaceAndOptionsAllProperties.ToArray(), Position.Top, category: surfaceOptionsAndInputs, categoryName: null, categoryFoldout: surfaceOptionsAndInputsFoldout, animBoolFoldout: surfaceOptionsAndInputsAnimBool, SurfaceOptionAndInputsBiRP);
#endif


            DrawCategory(toonShadingAllProperties.ToArray(), Position.Top, category: toonShading, categoryName: null, categoryFoldout: toonShadingFoldout, animBoolFoldout: toonShadingAnimBool, ToonShading);

            DrawCategory(stylingAllProperties.ToArray(), Position.Top, category: styling, categoryName: null, categoryFoldout: stylingFoldout, animBoolFoldout: stylingAnimBool, Styling);

            DrawCategory(outlineProperties.ToArray(), Position.Top, category: outline, categoryName: "Outline", categoryFoldout: outlineFoldout, animBoolFoldout: outlineAnimBool);
        }




        private float GetPropertyFloat(string propertyName)
        {
            return ((Material)referenceMaterial.objectReferenceValue).GetFloat(propertyName);
        }

        private void DisplayShaderPropertyBool(SerializedProperty serializedProperty, bool enabled = true, string displayName = null)
        {
            bool originalGUIEnabled = GUI.enabled;
            //EditorGUILayout.PropertyField(serializedProperty);
            if(!enabled)
                GUI.enabled = false;

            serializedProperty.boolValue = EditorGUILayout.ToggleLeft(displayName != null ? displayName : serializedProperty.displayName, serializedProperty.boolValue);

            if (!enabled)
                GUI.enabled = originalGUIEnabled;
        }

        private void DrawProperties(bool excludeEnabled = false, params SerializedProperty[] serializedProperties) {
            foreach (SerializedProperty serializedProperty in serializedProperties)
            {
                DisplayShaderPropertyBool(serializedProperty, excludeEnabled ? (!serializedProperty.displayName.Contains("Enable") && !serializedProperty.displayName.Contains("Sync With Other Styling") )  : true);
            }
        }



        private void DrawProperties(params SerializedProperty[] serializedProperties)
        {
            DrawProperties(excludeEnabled:false, serializedProperties);
        }


        //private delegate void PassableFunction(params SerializedProperty[] propertiesArray);
        //private delegate void PassableFunction(PassableFunction function = null, SerializedProperty category = null, string categoryName = null, params SerializedProperty[] propertiesArray);
        //private delegate void PassableFunction(SerializedProperty[] propertiesArray = null, SerializedProperty category = null, string categoryName = null, params PassableFunction[] functions);
        private delegate void PassableFunction();

        enum Position
        {
            Top,
            Sub,
            None,
        }

        private void DrawCategory(SerializedProperty[] propertiesArray, Position position, Color color, FontStyle fontStyle = FontStyle.Normal, int padding = 0, SerializedProperty category = null, string categoryName = null, SerializedProperty categoryFoldout = null, AnimBool animBoolFoldout = null, bool disableEnabler = true, params PassableFunction[] functions)
        {
            if (position == Position.Top)
            {
                bool showFoldout = ToonEditorUtils.MakeTheToonShaderSectionHeaderWithFoldoutWithToggle(new GUIContent(categoryName ?? category.displayName), categoryFoldout, category, propertiesArray);
                if (category.boolValue == false) //DONT KNOW WHY I HAVE TO DO THIS AGAIN LOL
                {
                    ToonEditorUtils.SetSerializedPropertiesBool(false, propertiesArray);
                }
                animBoolFoldout.target = showFoldout;
                if (EditorGUILayout.BeginFadeGroup(animBoolFoldout.faded))
                {
                    if (functions != null && functions.Length > 0)
                    {
                        foreach (PassableFunction function in functions)
                        {
                            function();
                        }
                    }
                    else
                    {
                        DrawProperties(disableEnabler, propertiesArray);
                    }

                }
                EditorGUILayout.EndFadeGroup();
            }
            else if (position == Position.Sub)
            {
                bool showFoldout = ToonEditorUtils.MakeTheToonShaderSubSectionHeaderWithFoldoutWithToggle(new GUIContent(categoryName ?? category.displayName),categoryFoldout, category, color, fontStyle, 0, padding, propertiesArray);
                if (category.boolValue == false)//DONT KNOW WHY I HAVE TO DO THIS AGAIN LOL
                { 
                    ToonEditorUtils.SetSerializedPropertiesBool(false, propertiesArray);
                }

                if (showFoldout)
                {
                    EditorGUI.indentLevel += 1;
                    if (functions != null && functions.Length > 0)
                    {

                        foreach (PassableFunction function in functions)
                        {
                            function();
                        }
                    }
                    else
                    {
                        DrawProperties(disableEnabler, propertiesArray);
                    }
                    EditorGUI.indentLevel -= 1;

                }        
            } 

        }
        private void DrawCategory(SerializedProperty[] propertiesArray, SerializedProperty category = null, string categoryName = null, SerializedProperty categoryFoldout = null, bool disableEnabler = true, params PassableFunction[] functions)
        {
            DrawCategory(propertiesArray, Position.Sub, Color.white, FontStyle.Normal, 10, category, categoryName, categoryFoldout, null, disableEnabler, functions);
        }

        private void DrawCategory(SerializedProperty[] propertiesArray, SerializedProperty category = null, string categoryName = null, SerializedProperty categoryFoldout = null, params PassableFunction[] functions)
        {
            DrawCategory(propertiesArray, Position.Sub, Color.white, FontStyle.Normal, 10,  category, categoryName, categoryFoldout, null, true, functions);
        }

        private void DrawCategory(SerializedProperty[] propertiesArray, Color color, SerializedProperty category = null, string categoryName = null, SerializedProperty categoryFoldout = null, params PassableFunction[] functions)
        {
            DrawCategory(propertiesArray, Position.Sub, color, FontStyle.BoldAndItalic, 0, category, categoryName, categoryFoldout, null, true, functions);
        }

        private void DrawCategory(SerializedProperty[] propertiesArray, Position position, SerializedProperty category = null, string categoryName = null, SerializedProperty categoryFoldout = null, AnimBool animBoolFoldout = null, params PassableFunction[] functions)
        {
            DrawCategory(propertiesArray, position, Color.gray, FontStyle.Italic, 0, category, categoryName, categoryFoldout, animBoolFoldout, true, functions);
        }



        private void SurfaceOptionAndInputsBiRP()
        {
            //DrawProperties(false, surfaceAndOptionsBiRPAllProperties.ToArray());

            bool originalGUIEnabled = GUI.enabled;
            GUI.enabled = false;
            DrawProperties(false, surfaceAndOptionsGeneralProperties.ToArray());
            GUI.enabled = originalGUIEnabled;

            EditorGUILayout.Space();


            DrawCategory(surfaceOptionsProperties.ToArray(), category: surfaceOptions, categoryName: "Surface Options", categoryFoldout: surfaceOptionsFoldout);

            string shadingModeLightFunction;
            if (GetPropertyFloat("_ShadingMode") == 1)
            {
                //if (GetPropertyFloat("_LightFunction") == 0)
                //{
                //    shadingModeLightFunction = " - Blinn Phong";
                //}
                //else
                //{
                    shadingModeLightFunction = " - (PBR)";
                //}

            }
            else
            {
                shadingModeLightFunction = " - (Color Based)";
            }

            DrawCategory(surfaceInputsLitProperties.ToArray(), category: surfaceInputs, categoryName: "Surface Inputs" + shadingModeLightFunction, categoryFoldout: surfaceInputsFoldout, SurfaceInputsBiRP);
            EditorGUILayout.Space();

        }

        private void SurfaceOptionAndInputs()
        {
            bool originalGUIEnabled = GUI.enabled;

            GUI.enabled = false;
            DrawProperties(false, surfaceAndOptionsGeneralProperties.ToArray());
            GUI.enabled = originalGUIEnabled;

            EditorGUILayout.Space();


            DrawCategory(surfaceOptionsProperties.ToArray(), category: surfaceOptions, categoryName: "Surface Options", categoryFoldout: surfaceOptionsFoldout);

            string shadingModeLightFunction;
            if (GetPropertyFloat("_ShadingMode") == 1)
            {
                if (GetPropertyFloat("_LightFunction") == 0)
                {
                    shadingModeLightFunction = " - Blinn Phong";
                }
                else
                {
                    shadingModeLightFunction = " - (PBR)";
                }

            }
            else
            {
                shadingModeLightFunction = " - (Color Based)";
            }

            DrawCategory(surfaceInputsLitProperties.ToArray(), category: surfaceInputs, categoryName: "Surface Inputs" + shadingModeLightFunction, categoryFoldout: surfaceInputsFoldout, SurfaceInputs);
            EditorGUILayout.Space();

        }


        private void SurfaceInputsBiRP()
        {
            bool originalGUIEnabled = GUI.enabled;

            if (GetPropertyFloat("_ShadingMode") == 1)
            {
                //if (GetPropertyFloat("_LightFunction") == 0)
                //{
                //    DrawProperties(false, surfaceInputsSimpleLitProperties.ToArray());
                //    GUI.enabled = false;
                //    DrawProperties(false, surfaceInputsOnlyLitProperties.ToArray());
                //    GUI.enabled = true;
                //}
                //else
                //{
                    DrawProperties(false, surfaceInputsLitProperties.ToArray());
                //}

            }
            else
            {
                DrawProperties(false, surfaceInputsBaseProperties.ToArray());
                GUI.enabled = false;
                DrawProperties(false, surfaceInputsOnlyLitProperties.ToArray());
                GUI.enabled = originalGUIEnabled;
            }
        }

        private void SurfaceInputs()
        {
            bool originalGUIEnabled = GUI.enabled;

            if (GetPropertyFloat("_ShadingMode") == 1)
            {
                if (GetPropertyFloat("_LightFunction") == 0)
                {
                    DrawProperties(false, surfaceInputsSimpleLitProperties.ToArray());
                    GUI.enabled = false;
                    DrawProperties(false, surfaceInputsOnlyLitProperties.ToArray());
                    GUI.enabled = originalGUIEnabled;
                }
                else
                {
                    DrawProperties(false, surfaceInputsLitProperties.ToArray());
                }

            }
            else
            {
                DrawProperties(false, surfaceInputsBaseProperties.ToArray());
                GUI.enabled = false;
                DrawProperties(false, surfaceInputsSimpleLitAndLitProperties.ToArray());
                GUI.enabled = originalGUIEnabled;
            }
        }

        private void ToonShading()
        {
            bool originalGUIEnabled = GUI.enabled;

            GUI.enabled = false;
            DrawProperties(false, toonShadingGeneralProperties.ToArray());
            GUI.enabled = originalGUIEnabled;
            EditorGUILayout.Space();

            string categoryNameToonShadingLight = GetPropertyFloat("_ShadingFunction") == 1 ? "Light Partitioning - Gradient" : "Light Partitioning - Cells";
            DrawCategory(toonShadingLightPartitioningProperties.ToArray(), category: toonShadingLightPartitioning, categoryName: categoryNameToonShadingLight,  categoryFoldout: toonShadingLightPartitioningFoldout, ToonShadingLightPartitioning);


            if (GetPropertyFloat("_ShadingMode") == 1)
            {
                DrawCategory(toonShadingLightProperties.ToArray(), category: toonShadingLight, categoryName: "Light", categoryFoldout: toonShadingLightFoldout, ToonShadingLight);


                GUI.enabled = false;
                toonShadingShadingFoldout.boolValue = false;
                toonShadingCastShadowsFoldout.boolValue = false;
                toonShadingSpecularFoldout.boolValue = false;
            }
            DrawCategory(toonShadingShadingProperties.ToArray(), category: toonShadingShading, categoryName: "Shading", categoryFoldout: toonShadingShadingFoldout, disableEnabler: GetPropertyFloat("_ShadingMode") == 0);

            DrawCategory(toonShadingCastShadowsProperties.ToArray(), category: toonShadingCastShadows, categoryName: "Cast Shadows", categoryFoldout: toonShadingCastShadowsFoldout, disableEnabler: GetPropertyFloat("_ShadingMode") == 0);

            DrawCategory(toonShadingSpecularProperties.ToArray(), category: toonShadingSpecular, categoryName: "Specular", categoryFoldout: toonShadingSpecularFoldout, disableEnabler: GetPropertyFloat("_ShadingMode") == 0);
            GUI.enabled = originalGUIEnabled;

            DrawCategory(toonShadingRimProperties.ToArray(), category: toonShadingRim, categoryName: "Rim", categoryFoldout: toonShadingRimFoldout);

        }
        //DisplayShaderPropertyBool(_ShadowsAffectByNormalMap, false);
        //DisplayShaderPropertyBool(_SpecularAffectedByNormalMap, false);
        //DisplayShaderPropertyBool(_SpecularSmoothness, false);
        private void ToonShadingLightPartitioning()
        {
            bool originalGUIEnabled = GUI.enabled;

            if (GetPropertyFloat("_ShadingFunction") == 1)  //Gradient:
            {
                DrawProperties(false, toonShadingLightGradientProperties.ToArray());
                GUI.enabled = false;
                DrawProperties(false, toonShadingLightCellProperties.ToArray());
                GUI.enabled = originalGUIEnabled;

            }
            else //Cells:
            {
                DrawProperties(false, toonShadingLightCellProperties.ToArray());
                GUI.enabled = false;
                DrawProperties(false, toonShadingLightGradientProperties.ToArray());
                GUI.enabled = originalGUIEnabled;

            }
        }

        private void ToonShadingLight()
        {

            DisplayShaderPropertyBool(_ShadingAffectedByNormalMap,true);
            DisplayShaderPropertyBool(_SpecularAffectedByNormalMap,true);
            DisplayShaderPropertyBool(_SpecularSmoothness,true, "Specular Edge Smoothness");
        }

        private void Styling()
        {
            DrawProperties(true, stylingGeneralProperties.ToArray());
            EditorGUILayout.Space();

            DrawCategory(stylingDistanceFadeProperties.ToArray(), Color.black, category: stylingDistanceFade, categoryName: "Distance Fade", categoryFoldout: stylingDistanceFadeFoldout);
            EditorGUILayout.Space();
            EditorUtils.DrawSubMenuSeparation();
            EditorGUILayout.Space();


            DrawCategory(stylingShadingAllProperties.ToArray(), Color.green, category: stylingShading, categoryName: "Shading", categoryFoldout: stylingShadingFoldout,  ShadingStyling);
            EditorGUILayout.Space();
            EditorUtils.DrawSubMenuSeparation();
            EditorGUILayout.Space();

            DrawCategory(stylingCastShadowsAllProperties.ToArray(), Color.black, category: stylingCastShadows, categoryName: "Cast Shadows", categoryFoldout: stylingCastShadowsFoldout, CastShadowsStyling);
            EditorGUILayout.Space();
            EditorUtils.DrawSubMenuSeparation();
            EditorGUILayout.Space();

            DrawCategory(stylingSpecularAllProperties.ToArray(), Color.cyan, category: stylingSpecular, categoryName: "Specular", categoryFoldout: stylingSpecularFoldout, SpecularStyling);
            EditorGUILayout.Space();
            EditorUtils.DrawSubMenuSeparation();
            EditorGUILayout.Space();


            DrawCategory(stylingRimAllProperties.ToArray(), Color.blue, category: stylingRim, categoryName: "Rim", categoryFoldout: stylingRimFoldout, RimStyling);
            EditorGUILayout.Space();

        }

        private void ShadingStyling()
        {

            DrawProperties(true, stylingShadingGeneralProperties.ToArray());
            EditorGUILayout.Space();
            //DrawCategory(stylingShadingGeneralProperties.ToArray(), stylingShadingPartitioning, "General");
            DrawCategory(stylingShadingPartitioningProperties.ToArray(), stylingShadingPartitioning, "Partitioning", stylingShadingPartitioningFoldout);
            DrawCategory(stylingShadingBlendingProperties.ToArray(), stylingShadingBlending, "Blending", stylingShadingBlendingFoldout);
            DrawCategory(stylingShadingDrawSpaceProperties.ToArray(), stylingShadingDrawSpace, "Draw Space", stylingShadingDrawSpaceFoldout);
            DrawCategory(stylingShadingDensityAndRotationProperties.ToArray(), stylingShadingDensityAndRotation, "Density and Rotation", stylingShadingDensityAndRotationFoldout);
            DrawCategory(stylingShadingStyleSettingsProperties.ToArray(), stylingShadingStyleSettings, "Style Settings", stylingShadingStyleSettingsFoldout);
            DrawCategory(stylingShadingRandomizeProperties.ToArray(), stylingShadingRandomize, "Randomize", stylingShadingRandomizeFoldout);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private void CastShadowsStyling()
        {
            DrawProperties(true, stylingCastShadowsGeneralProperties.ToArray());
            EditorGUILayout.Space();
            //DrawCategory(stylingCastShadowsGeneralProperties.ToArray(), stylingCastShadowsPartitioning, "General");
            DrawCategory(stylingCastShadowsPartitioningProperties.ToArray(), stylingCastShadowsPartitioning, "Partitioning", stylingCastShadowsPartitioningFoldout);
            DrawCategory(stylingCastShadowsBlendingProperties.ToArray(), stylingCastShadowsBlending, "Blending", stylingCastShadowsBlendingFoldout);
            DrawCategory(stylingCastShadowsDrawSpaceProperties.ToArray(), stylingCastShadowsDrawSpace, "Draw Space", stylingCastShadowsDrawSpaceFoldout);
            DrawCategory(stylingCastShadowsDensityAndRotationProperties.ToArray(), stylingCastShadowsDensityAndRotation, "Density and Rotation", stylingCastShadowsDensityAndRotationFoldout);
            DrawCategory(stylingCastShadowsStyleSettingsProperties.ToArray(), stylingCastShadowsStyleSettings, "Style Settings", stylingCastShadowsStyleSettingsFoldout);
            DrawCategory(stylingCastShadowsRandomizeProperties.ToArray(), stylingCastShadowsRandomize, "Randomize", stylingCastShadowsRandomizeFoldout);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private void SpecularStyling()
        {
            DrawProperties(true, stylingSpecularGeneralProperties.ToArray());
            EditorGUILayout.Space();
            //DrawCategory(stylingSpecularGeneralProperties.ToArray(), stylingSpecularPartitioning, "General");
            DrawCategory(stylingSpecularSettingsProperties.ToArray(), stylingSpecularSettings, "Specular Settings", stylingSpecularSettingsFoldout);
            DrawCategory(stylingSpecularBlendingProperties.ToArray(), stylingSpecularBlending, "Blending", stylingSpecularBlendingFoldout);
            DrawCategory(stylingSpecularUVSpaceProperties.ToArray(), stylingSpecularDrawSpace, "Draw Space", stylingSpecularDrawSpaceFoldout);
            DrawCategory(stylingSpecularDensityAndRotationProperties.ToArray(), stylingSpecularDensityAndRotation, "Density and Rotation", stylingSpecularDensityAndRotationFoldout);
            DrawCategory(stylingSpecularStyleSettingsProperties.ToArray(), stylingSpecularStyleSettings, "Style Settings", stylingSpecularStyleSettingsFoldout);
            DrawCategory(stylingSpecularRandomizeProperties.ToArray(), stylingSpecularRandomize, "Randomize", stylingSpecularRandomizeFoldout);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();


        }

        private void RimStyling()
        {
            DrawProperties(true, stylingRimGeneralProperties.ToArray());
            EditorGUILayout.Space();
            //DrawCategory(stylingRimGeneralProperties.ToArray(), stylingRimPartitioning, "General");
            DrawCategory(stylingRimSettingsProperties.ToArray(), stylingRimSettings, "Rim Settings", stylingRimSettingsFoldout);
            DrawCategory(stylingRimBlendingProperties.ToArray(), stylingRimBlending, "Blending", stylingRimBlendingFoldout);
            DrawCategory(stylingRimUVSpaceProperties.ToArray(), stylingRimDrawSpace, "UV Space", stylingRimDrawSpaceFoldout);
            DrawCategory(stylingRimDensityAndRotationProperties.ToArray(), stylingRimDensityAndRotation, "Density and Rotation", stylingRimDensityAndRotationFoldout);
            DrawCategory(stylingRimStyleSettingsProperties.ToArray(), stylingRimStyleSettings, "Style Settings", stylingRimStyleSettingsFoldout);
            DrawCategory(stylingRimRandomizeProperties.ToArray(), stylingRimRandomize, "Randomize", stylingRimRandomizeFoldout);

        }

        //SerializedProperty[] stylingShadingProperties;
        SerializedProperty[] stylingSpecularProperties;
        SerializedProperty[] stylingRimProperties;



        List<SerializedProperty> surfaceAndOptionsBiRPAllProperties;

        List<SerializedProperty> surfaceAndOptionsAllProperties;
        List<SerializedProperty> surfaceAndOptionsCategoriesProperties;
        List<SerializedProperty> surfaceAndOptionsGeneralProperties;
        List<SerializedProperty> surfaceOptionsProperties;
        List<SerializedProperty> surfaceInputsBaseProperties;
        List<SerializedProperty> surfaceInputsSimpleLitProperties;
        List<SerializedProperty> surfaceInputsLitProperties;

        List<SerializedProperty> surfaceInputsSimpleLitAndLitProperties;
        List<SerializedProperty> surfaceInputsOnlyLitProperties;
        //List<SerializedProperty> surfaceInputsAllProperties;



        List<SerializedProperty> outlineProperties;


        List<SerializedProperty> stylingAllProperties;
        List<SerializedProperty> stylingGeneralProperties;
        List<SerializedProperty> stylingDistanceFadeProperties;
        List<SerializedProperty> stylingShadingAllProperties;
        List<SerializedProperty> stylingCastShadowsAllProperties;
        List<SerializedProperty> stylingSpecularAllProperties;
        List<SerializedProperty> stylingRimAllProperties;

        List<SerializedProperty> stylingShadingCategoriesProperties;
        List<SerializedProperty> stylingShadingGeneralProperties;
        List<SerializedProperty> stylingShadingPartitioningProperties;
        List<SerializedProperty> stylingShadingBlendingProperties;
        List<SerializedProperty> stylingShadingDrawSpaceProperties;
        List<SerializedProperty> stylingShadingDensityAndRotationProperties;
        List<SerializedProperty> stylingShadingStyleSettingsProperties;
        List<SerializedProperty> stylingShadingRandomizeProperties;

        List<SerializedProperty> stylingCastShadowsCategoriesProperties;
        List<SerializedProperty> stylingCastShadowsGeneralProperties;
        List<SerializedProperty> stylingCastShadowsPartitioningProperties;
        List<SerializedProperty> stylingCastShadowsBlendingProperties;
        List<SerializedProperty> stylingCastShadowsDrawSpaceProperties;
        List<SerializedProperty> stylingCastShadowsDensityAndRotationProperties;
        List<SerializedProperty> stylingCastShadowsStyleSettingsProperties;
        List<SerializedProperty> stylingCastShadowsRandomizeProperties;


        List<SerializedProperty> stylingSpecularCategoriesProperties;
        List<SerializedProperty> stylingSpecularGeneralProperties;
        List<SerializedProperty> stylingSpecularSettingsProperties;
        List<SerializedProperty> stylingSpecularBlendingProperties;
        List<SerializedProperty> stylingSpecularUVSpaceProperties;
        List<SerializedProperty> stylingSpecularDensityAndRotationProperties;
        List<SerializedProperty> stylingSpecularStyleSettingsProperties;
        List<SerializedProperty> stylingSpecularRandomizeProperties;



        List<SerializedProperty> stylingRimCategoriesProperties;
        List<SerializedProperty> stylingRimGeneralProperties;
        List<SerializedProperty> stylingRimSettingsProperties;
        List<SerializedProperty> stylingRimBlendingProperties;
        List<SerializedProperty> stylingRimUVSpaceProperties;
        List<SerializedProperty> stylingRimDensityAndRotationProperties;
        List<SerializedProperty> stylingRimStyleSettingsProperties;
        List<SerializedProperty> stylingRimRandomizeProperties;



        List<SerializedProperty> toonShadingCategoriesProperties;
        List<SerializedProperty> toonShadingGeneralProperties;
        List<SerializedProperty> toonShadingLightGradientProperties;
        List<SerializedProperty> toonShadingLightCellProperties;
        List<SerializedProperty> toonShadingLightPartitioningProperties;
        List<SerializedProperty> toonShadingLightProperties;
        List<SerializedProperty> toonShadingShadingProperties;
        List<SerializedProperty> toonShadingCastShadowsProperties;
        List<SerializedProperty> toonShadingSpecularProperties;
        List<SerializedProperty> toonShadingRimProperties;
        List<SerializedProperty> toonShadingAllProperties;


        //List<SerializedProperty> stylingSpecularProperties;
        //List<SerializedProperty> stylingRimProperties;





        private void SetupToonShadingPropertiesArrays()
        {
            toonShadingCategoriesProperties = new List<SerializedProperty>() { toonShadingLight, toonShadingLightPartitioning, toonShadingShading, toonShadingSpecular, toonShadingRim };


            toonShadingGeneralProperties = new List<SerializedProperty>() { _EnableToonShading, _ShadingFunction, };

            toonShadingLightGradientProperties = new List<SerializedProperty>() {   _GradientTex,
                                                                                    _GradientMode,
                                                                                    _GradientBlending,
                                                                                    _GradientBlendFactor,};

            toonShadingLightCellProperties = new List<SerializedProperty>() {   _NumberOfCells,
                                                                                _CellTransitionSmoothness,
                                                                                _SumLightsBeforePosterization,
                                                                                _ShadingUseLightColors,
            };


            toonShadingLightPartitioningProperties = toonShadingLightGradientProperties.Concat(toonShadingLightCellProperties).ToList();

            toonShadingLightProperties = new List<SerializedProperty>() { _ShadingAffectedByNormalMap, _SpecularAffectedByNormalMap, _SpecularSmoothness };


            toonShadingShadingProperties = new List<SerializedProperty>() {  _EnableShadows,
                                                                            _CoreShadowColor,
                                                                            _TerminatorPosition,
                                                                            _TerminatorWidth,
                                                                            _TerminatorSmoothness,
                                                                            _FormShadowColor,
                                                                            _ShadingAffectedByNormalMap,
            };


            toonShadingCastShadowsProperties = new List<SerializedProperty>() { _EnableCastShadows,
                                                                            //_CastShadowsStrength,
                                                                            //_CastShadowsSmoothness,
                                                                            _CastShadowColorMode,
                                                                            _CastShadowColor,
            };

            toonShadingSpecularProperties = new List<SerializedProperty>() {    _EnableSpecular,
                                                                                _SpecularBlending,
                                                                                _SpecularColor,
                                                                                _SpecularSize,
                                                                                _SpecularSmoothness,
                                                                                _SpecularOpacity,
                                                                                _SpecularAffectedByNormalMap,
                                                                                _SpecularUseLightColors,
            };

            toonShadingRimProperties = new List<SerializedProperty>() {     _EnableRim,
                                                                            _RimBlending,
                                                                            _RimColor,
                                                                            _RimSize,
                                                                            _RimSmoothness,
                                                                            _RimOpacity,
                                                                            _RimAffectedArea,
                                                                            _RimAffectedByNormalMap};


            toonShadingAllProperties = toonShadingCategoriesProperties.Concat(toonShadingGeneralProperties)
                                                                        .Concat(toonShadingLightPartitioningProperties)
                                                                        .Concat(toonShadingLightProperties)
                                                                        .Concat(toonShadingShadingProperties)
                                                                        .Concat(toonShadingSpecularProperties)
                                                                        .Concat(toonShadingRimProperties).ToList();

        }
        private void SetupOutlinePropertiesArrays()
        {
            outlineProperties = new List<SerializedProperty>() { _EnableOutline, _OutlineColor, _OutlineWidth, _OutlineDepthOffset, _OutlineCameraDistanceImpact };
        }

#if !USING_URP
        private void SetupSurfaceAndOptionsBiRPPropertiesArrays()
        {



            //surfaceAndOptionsBiRPAllProperties = new List<SerializedProperty>() {
            //                                                                            _Mode,
            //                                                                            _Cull,
            //                                                                            _MainTex,
            //                                                                            _Color,
            //                                                                            _Cutoff,
            //                                                                            _AlphaClip,
            //                                                                            _UseAlphaOnlyFromBaseMap,
            //                                                                            _Metallic,
            //                                                                            _MetallicGlossMap,
            //                                                                            _Glossiness,
            //                                                                            _GlossMapScale,
            //                                                                            _SmoothnessTextureChannel,
            //                                                                            _BumpMap,
            //                                                                            _BumpScale,
            //                                                                            _ParallaxMap,
            //                                                                            _Parallax,
            //                                                                            _OcclusionMap,
            //                                                                            _OcclusionStrength,
            //                                                                            _DetailMask,
            //                                                                            _EmissionMap,
            //                                                                            _EmissionColor,
            //                                                                            _DetailAlbedoMap,
            //                                                                            _DetailNormalMap,
            //                                                                            _DetailNormalMapScale,
            //                                                                            _UVSec
            //                                                                         };


   



            surfaceAndOptionsGeneralProperties = new List<SerializedProperty>() { _ShadingMode, _LightFunction, };

            surfaceOptionsProperties = new List<SerializedProperty>() {    _Mode,   
                                                                            _Cull,
                                                                            _AlphaClip};

            surfaceInputsBaseProperties = new List<SerializedProperty>() {  _MainTex,
                                                                            _Color,
                                                                            _UseAlphaOnlyFromBaseMap,
                                                                            _Cutoff,
                                                                            _BumpScale,
                                                                            _BumpMap,
                                                                            _EmissionColor,
                                                                            _EmissionMap};




            //surfaceInputsSimpleLitAndLitProperties = new List<SerializedProperty>() {                           _SpecularHighlights,
            //                                                                                                    _SpecColor,
            //                                                                                                    _SpecGlossMap,
            //                                                                                                    _Smoothness,};


            //surfaceInputsSimpleLitProperties = surfaceInputsBaseProperties.Concat(surfaceInputsSimpleLitAndLitProperties).ToList();

            surfaceInputsOnlyLitProperties = new List<SerializedProperty>() {   _SmoothnessTextureChannel,
                                                                            _Metallic,
                                                                            _MetallicGlossMap,
                                                                            _Glossiness,
                                                                            _GlossMapScale,      
                                                                            _Parallax,
                                                                            _ParallaxMap,
                                                                            _OcclusionStrength,
                                                                            _OcclusionMap,
                                                                            _DetailMask,                                   
                                                                            _DetailAlbedoMap,
                                                                            _DetailNormalMapScale,
                                                                            _DetailNormalMap,
                                                                            _UVSec};

            //surfaceInputsSimpleLitAndLitProperties = surfaceInputsSimpleLitAndLitProperties.Concat(surfaceInputsOnlyLitProperties).ToList();

            surfaceInputsLitProperties = surfaceInputsBaseProperties.Concat(surfaceInputsOnlyLitProperties).ToList();




            surfaceAndOptionsAllProperties = new List<SerializedProperty>();
            surfaceAndOptionsAllProperties = surfaceAndOptionsAllProperties.Concat(surfaceAndOptionsGeneralProperties)
                                                                            .Concat(surfaceOptionsProperties)
                                                                            .Concat(surfaceInputsLitProperties).ToList();


        }
#else

        private void SetupSurfaceAndOptionsPropertiesArrays()
        {

            surfaceAndOptionsCategoriesProperties = new List<SerializedProperty>() {    surfaceOptions, surfaceInputs};


            surfaceAndOptionsGeneralProperties = new List<SerializedProperty>() {  _ShadingMode, _LightFunction,};       
            
            surfaceOptionsProperties = new List<SerializedProperty>() {    _WorkflowMode,
                                                                    _Surface,
                                                                    _Blend,
                                                                    _Cull,
                                                                    _AlphaClip,
                                                                    _ReceiveShadows};

            surfaceInputsBaseProperties = new List<SerializedProperty>() {  _BaseMap,
                                                                            _BaseColor,
                                                                            _UseAlphaOnlyFromBaseMap,
                                                                            _Cutoff,
                                                                            _BumpScale,
                                                                            _BumpMap,
                                                                            _EmissionColor,
                                                                            _EmissionMap};




            surfaceInputsSimpleLitAndLitProperties = new List<SerializedProperty>() {                           _SpecularHighlights,
                                                                                                                _SpecColor,
                                                                                                                _SpecGlossMap,
                                                                                                                _Smoothness,};


            surfaceInputsSimpleLitProperties = surfaceInputsBaseProperties.Concat(surfaceInputsSimpleLitAndLitProperties).ToList();

            surfaceInputsOnlyLitProperties = new List<SerializedProperty>() {   _SmoothnessTextureChannel,
                                                                            _Metallic,
                                                                            _MetallicGlossMap,
                                                                            _EnvironmentReflections,
                                                                            _Parallax,
                                                                            _ParallaxMap,
                                                                            _OcclusionStrength,
                                                                            _OcclusionMap,
                                                                            _DetailMask,
                                                                            _DetailAlbedoMapScale,
                                                                            _DetailAlbedoMap,
                                                                            _DetailNormalMapScale,
                                                                            _DetailNormalMap,
                                                                            _SmoothnessSource};

            surfaceInputsSimpleLitAndLitProperties = surfaceInputsSimpleLitAndLitProperties.Concat(surfaceInputsOnlyLitProperties).ToList();

            surfaceInputsLitProperties = surfaceInputsSimpleLitProperties.Concat(surfaceInputsOnlyLitProperties).ToList();




            surfaceAndOptionsAllProperties = new List<SerializedProperty>();
            surfaceAndOptionsAllProperties = surfaceAndOptionsAllProperties.Concat(surfaceAndOptionsCategoriesProperties)
                                                                            .Concat(surfaceAndOptionsGeneralProperties)
                                                                            .Concat(surfaceOptionsProperties)
                                                                            .Concat(surfaceInputsLitProperties).ToList();


        }

#endif
        private void SetupStylingPropertiesArrays()
        {
            stylingAllProperties = new List<SerializedProperty>() {stylingDistanceFade, stylingShading, stylingSpecular, stylingRim };
            stylingDistanceFadeProperties = new List<SerializedProperty>() { _EnableStylingDistanceFade, 
                                                                            _StylingDFStartingDistance, 
                                                                            _StylingDFFalloff,
                                                                            _StylingAdjustDistanceFadeValue,
                                                                            _StylingDistanceFadeValue };
            stylingGeneralProperties = new List<SerializedProperty>() { _EnableStyling, _HatchingAffectedByNormalMap, _EnableAntiAliasing};
            stylingAllProperties =  stylingAllProperties.Concat(stylingGeneralProperties)
                                                        .Concat(stylingDistanceFadeProperties)
                                                        .ToList();            
            SetupStyleShadingPropertiesArrays();
            SetupStyleCastShadowsPropertiesArrays();
            SetupStyleSpecularPropertiesArrays();
            SetupStyleRimPropertiesArrays();
            stylingAllProperties = stylingAllProperties.Concat(stylingShadingAllProperties)
                                                        .Concat(stylingCastShadowsAllProperties)
                                                        .Concat(stylingSpecularAllProperties)
                                                        .Concat(stylingRimAllProperties)
                                                        .ToList();
        }



        private void SetupStyleShadingPropertiesArrays()
        {

            stylingShadingCategoriesProperties = new List<SerializedProperty>() {   stylingShadingPartitioning,
                                                                                    stylingShadingBlending,
                                                                                    stylingShadingDrawSpace,
                                                                                    stylingShadingDensityAndRotation,
                                                                                    stylingShadingStyleSettings,
                                                                                    stylingShadingRandomize};

            stylingShadingGeneralProperties = new List<SerializedProperty>() {     _EnableShadingStyling,
                                                                                    _ShadingStyle};

            stylingShadingPartitioningProperties = new List<SerializedProperty>() {     _SyncWithLightPartitioning,
                                                                                        _NumberOfCellsHatching,
                                                                                        _StylingTerminatorPosition,
                                                                                        _StylingOvermodelingFactor};

            stylingShadingBlendingProperties = new List<SerializedProperty>() { _StylingShadingBlending};


            stylingShadingDrawSpaceProperties = new List<SerializedProperty>() {  _DrawSpace,
                                                                                _SSCameraDistanceScaled,
                                                                                _CoordinateSystem,
                                                                                _PolarCenterMode,
                                                                                _PolarCenter};


            stylingShadingDensityAndRotationProperties = new List<SerializedProperty>() {   _StylingShadingDensity,
                                                                                            _StylingShadingInitialDirection,
                                                                                            _StylingShadingRotationBetweenCells,
                                                                                            _StylingShadingHalftonesOffset};


            stylingShadingStyleSettingsProperties = new List<SerializedProperty>() {    _StylingColor,
                                                                                        _StylingShadingThicknessControl,
                                                                                        _StylingShadingThickness,
                                                                                        _StylingShadingOpacity,
                                                                                        _StylingShadingOpacityFalloff,
                                                                                        _StylingShadingThicknessFalloff,
                                                                                        _StylingShadingHardness,
                                                                                        _StylingShadingHalftonesRoundness,
                                                                                        _StylingShadingHalftonesRoundnessFalloff};

            stylingShadingRandomizeProperties = new List<SerializedProperty>() {     _EnableShadingRandomizer,
                                                                                    _ShadingNoise1Size,
                                                                                    _NoiseIntensity,
                                                                                    _SpacingRandomMode,
                                                                                    _SpacingRandomIntensity,
                                                                                    _OpacityRandomMode,
                                                                                    _OpacityRandomIntensity,
                                                                                    _HardnessRandomMode,
                                                                                    _HardnessRandomIntensity,
                                                                                    _LengthRandomMode,
                                                                                    _LengthRandomIntensity,
                                                                                    _ThicknessRandomMode,
                                                                                    _ThicknesshRandomIntensity};


            stylingShadingAllProperties = new List<SerializedProperty>();
            stylingShadingAllProperties = stylingShadingAllProperties.Concat(stylingShadingCategoriesProperties)
                                                                    .Concat(stylingShadingGeneralProperties)
                                                                    .Concat(stylingShadingPartitioningProperties)
                                                                    .Concat(stylingShadingBlendingProperties)
                                                                    .Concat(stylingShadingDrawSpaceProperties)
                                                                    .Concat(stylingShadingDensityAndRotationProperties)
                                                                    .Concat(stylingShadingStyleSettingsProperties)
                                                                    .Concat(stylingShadingRandomizeProperties).ToList();
        }



        private void SetupStyleCastShadowsPropertiesArrays()
        {

            stylingCastShadowsCategoriesProperties = new List<SerializedProperty>() {  stylingCastShadowsPartitioning,
                                                                                    stylingCastShadowsBlending,
                                                                                    stylingCastShadowsDrawSpace,
                                                                                    stylingCastShadowsDensityAndRotation,
                                                                                    stylingCastShadowsStyleSettings,
                                                                                    stylingCastShadowsRandomize};

            stylingCastShadowsGeneralProperties = new List<SerializedProperty>() {     _EnableCastShadowsStyling,
                                                                                        _StylingCastShadowsColor,
                                                                                        _StylingCastShadowsSyncWithOtherStyling,
                                                                                        _CastShadowsStyle};


            stylingCastShadowsPartitioningProperties = new List<SerializedProperty>() { _CastShadowsNumberOfCellsHatching,
                                                                                        _StylingCastShadowsSmoothness};


            stylingCastShadowsBlendingProperties = new List<SerializedProperty>() { _StylingCastShadowsBlending, _StylingCastShadowsIsInverted };


            stylingCastShadowsDrawSpaceProperties = new List<SerializedProperty>() { _CastShadowsDrawSpace,
                                                                                _CastShadowsSSCameraDistanceScaled,
                                                                                _CastShadowsAnchorSSToObjectsOrigin,
                                                                                _CastShadowsCoordinateSystem,
                                                                                _CastShadowsPolarCenterMode,
                                                                                _CastShadowsPolarCenter,};


            stylingCastShadowsDensityAndRotationProperties = new List<SerializedProperty>() {   _StylingCastShadowsDensity,
                                                                                            _StylingCastShadowsInitialDirection,
                                                                                            _StylingCastShadowsRotationBetweenCells,
                                                                                            _StylingCastShadowsHalftonesOffset,};


            stylingCastShadowsStyleSettingsProperties = new List<SerializedProperty>() { 
                                                                                        _StylingCastShadowsOpacity,
                                                                                        _StylingCastShadowsOpacityFalloff,
                                                                                        _StylingCastShadowsThicknessControl,
                                                                                        _StylingCastShadowsThickness,
                                                                                        _StylingCastShadowsThicknessFalloff,
                                                                                        _StylingCastShadowsHardness,
                                                                                        _StylingCastShadowsHalftonesRoundness,
                                                                                        _StylingCastShadowsHalftonesRoundnessFalloff};

            stylingCastShadowsRandomizeProperties = new List<SerializedProperty>() { _EnableCastShadowsRandomizer,
                                                                                    _CastShadowsNoise1Size,
                                                                                    _CastShadowsNoise1Seed,
                                                                                    _CastShadowsNoise2Seed,
                                                                                    _CastShadowsNoiseIntensity,
                                                                                    _CastShadowsSpacingRandomMode,
                                                                                    _CastShadowsSpacingRandomIntensity,
                                                                                    _CastShadowsOpacityRandomMode,
                                                                                    _CastShadowsOpacityRandomIntensity,
                                                                                    _CastShadowsHardnessRandomMode,
                                                                                    _CastShadowsHardnessRandomIntensity,
                                                                                    _CastShadowsLengthRandomMode,
                                                                                    _CastShadowsLengthRandomIntensity,
                                                                                    _CastShadowsThicknessRandomMode,
                                                                                    _CastShadowsThicknesshRandomIntensity};


            stylingCastShadowsAllProperties = new List<SerializedProperty>();
            stylingCastShadowsAllProperties = stylingCastShadowsAllProperties.Concat(stylingCastShadowsCategoriesProperties)
                                                                    .Concat(stylingCastShadowsGeneralProperties)
                                                                    .Concat(stylingCastShadowsPartitioningProperties)
                                                                    .Concat(stylingCastShadowsBlendingProperties)
                                                                    .Concat(stylingCastShadowsDrawSpaceProperties)
                                                                    .Concat(stylingCastShadowsDensityAndRotationProperties)
                                                                    .Concat(stylingCastShadowsStyleSettingsProperties)
                                                                    .Concat(stylingCastShadowsRandomizeProperties).ToList();

        }


        private void SetupStyleSpecularPropertiesArrays()
        {

            stylingSpecularCategoriesProperties = new List<SerializedProperty>() {  stylingSpecularSettings,
                                                                                    stylingSpecularBlending,
                                                                                    stylingSpecularDrawSpace,
                                                                                    stylingSpecularDensityAndRotation,
                                                                                    stylingSpecularStyleSettings,
                                                                                    stylingSpecularRandomize};

            stylingSpecularGeneralProperties = new List<SerializedProperty>() {     _EnableSpecularStyling,
                                                                                    _StylingSpecularColor,
                                                                                    _StylingSpecularSyncWithOtherStyling,
                                                                                    _StylingSpecularUseLightColors,
                                                                                    _SpecularStyle};

            stylingSpecularSettingsProperties = new List<SerializedProperty>() {    _SyncWithSpecular,
                                                                                    _StylingSpecularSize,
                                                                                    _StylingSpecularSmoothness};

            stylingSpecularBlendingProperties = new List<SerializedProperty>() { _StylingSpecularBlending, _StylingSpecularIsInverted };


            stylingSpecularUVSpaceProperties = new List<SerializedProperty>() { _SpecularDrawSpace,
                                                                                _SpecularSSCameraDistanceScaled,
                                                                                _SpecularAnchorSSToObjectsOrigin,
                                                                                _SpecularCoordinateSystem,
                                                                                _SpecularPolarCenterMode,
                                                                                _SpecularPolarCenter,};


            stylingSpecularDensityAndRotationProperties = new List<SerializedProperty>() {   _StylingSpecularDensity,
                                                                                            _StylingSpecularRotation,
                                                                                            _StylingSpecularHalftonesOffset,};


            stylingSpecularStyleSettingsProperties = new List<SerializedProperty>() {  
                                                                                        _StylingSpecularOpacity,
                                                                                        _StylingSpecularOpacityFalloff,
                                                                                        _StylingSpecularThicknessControl,
                                                                                        _StylingSpecularThickness,
                                                                                        _StylingSpecularThicknessFalloff,
                                                                                        _StylingSpecularHardness,
                                                                                        _StylingSpecularHalftonesRoundness,
                                                                                        _StylingSpecularHalftonesRoundnessFalloff};

            stylingSpecularRandomizeProperties = new List<SerializedProperty>() {   _EnableSpecularRandomizer,
                                                                                    _SpecularNoise1Size,
                                                                                    _SpecularNoise1Seed,
                                                                                    _SpecularNoise2Seed,
                                                                                    _SpecularNoiseIntensity,
                                                                                    _SpecularSpacingRandomMode,
                                                                                    _SpecularSpacingRandomIntensity,
                                                                                    _SpecularOpacityRandomMode,
                                                                                    _SpecularOpacityRandomIntensity,
                                                                                    _SpecularHardnessRandomMode,
                                                                                    _SpecularHardnessRandomIntensity,
                                                                                    _SpecularLengthRandomMode,
                                                                                    _SpecularLengthRandomIntensity,
                                                                                    _SpecularThicknessRandomMode,
                                                                                    _SpecularThicknesshRandomIntensity};


            stylingSpecularAllProperties = new List<SerializedProperty>();
            stylingSpecularAllProperties = stylingSpecularAllProperties.Concat(stylingSpecularCategoriesProperties)
                                                                    .Concat(stylingSpecularGeneralProperties)
                                                                    .Concat(stylingSpecularSettingsProperties)
                                                                    .Concat(stylingSpecularBlendingProperties)
                                                                    .Concat(stylingSpecularUVSpaceProperties)
                                                                    .Concat(stylingSpecularDensityAndRotationProperties)
                                                                    .Concat(stylingSpecularStyleSettingsProperties)
                                                                    .Concat(stylingSpecularRandomizeProperties).ToList();

        }

        private void SetupStyleRimPropertiesArrays()
        {

            stylingRimCategoriesProperties = new List<SerializedProperty>() {  stylingRimSettings,
                                                                                    stylingRimBlending,
                                                                                    stylingRimDrawSpace,
                                                                                    stylingRimDensityAndRotation,
                                                                                    stylingRimStyleSettings,
                                                                                    stylingRimRandomize};

            stylingRimGeneralProperties = new List<SerializedProperty>() {  _EnableRimStyling,
                                                                            _StylingRimColor,
                                                                            _StylingRimSyncWithOtherStyling,
                                                                            _RimStyle};

            stylingRimSettingsProperties = new List<SerializedProperty>() {     _SyncWithRim,
                                                                                _StylingRimSize,
                                                                                _StylingRimSmoothness,
                                                                                _StylingRimAffectedArea};

            stylingRimBlendingProperties = new List<SerializedProperty>() { _StylingRimBlending, _StylingRimIsInverted };


            stylingRimUVSpaceProperties = new List<SerializedProperty>() { _RimDrawSpace,
                                                                                _RimSSCameraDistanceScaled,
                                                                                _RimAnchorSSToObjectsOrigin,
                                                                                _RimCoordinateSystem,
                                                                                _RimPolarCenterMode,
                                                                                _RimPolarCenter};


            stylingRimDensityAndRotationProperties = new List<SerializedProperty>() {   _StylingRimDensity,
                                                                                            _StylingRimRotation,
                                                                                            _StylingRimHalftonesOffset};


            stylingRimStyleSettingsProperties = new List<SerializedProperty>() {  
                                                                                        _StylingRimOpacity,
                                                                                        _StylingRimOpacityFalloff,
                                                                                        _StylingRimThicknessControl,
                                                                                        _StylingRimThickness,
                                                                                        _StylingRimThicknessFalloff,
                                                                                        _StylingRimHardness,
                                                                                        _StylingRimHalftonesRoundness,
                                                                                        _StylingRimHalftonesRoundnessFalloff};

            stylingRimRandomizeProperties = new List<SerializedProperty>() {   _EnableRimRandomizer,
                                                                                    _RimNoise1Size,
                                                                                    _RimNoise1Seed,
                                                                                    _RimNoise2Seed,
                                                                                    _RimNoiseIntensity,
                                                                                    _RimSpacingRandomMode,
                                                                                    _RimSpacingRandomIntensity,
                                                                                    _RimOpacityRandomMode,
                                                                                    _RimOpacityRandomIntensity,
                                                                                    _RimHardnessRandomMode,
                                                                                    _RimHardnessRandomIntensity,
                                                                                    _RimLengthRandomMode,
                                                                                    _RimLengthRandomIntensity,
                                                                                    _RimThicknessRandomMode,
                                                                                    _RimThicknesshRandomIntensity};


            stylingRimAllProperties = new List<SerializedProperty>();
            stylingRimAllProperties = stylingRimAllProperties.Concat(stylingRimCategoriesProperties)
                                                                .Concat(stylingRimGeneralProperties)
                                                                .Concat(stylingRimSettingsProperties)
                                                                .Concat(stylingRimBlendingProperties)
                                                                .Concat(stylingRimUVSpaceProperties)
                                                                .Concat(stylingRimDensityAndRotationProperties)
                                                                .Concat(stylingRimStyleSettingsProperties)
                                                                .Concat(stylingRimRandomizeProperties).ToList();

        }


    }
}

