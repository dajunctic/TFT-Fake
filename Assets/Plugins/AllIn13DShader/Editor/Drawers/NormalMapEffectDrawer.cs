using UnityEngine;
using UnityEditor;

namespace AllIn13DShader
{
    public class NormalMapEffectDrawer : AbstractEffectDrawer
    {
        private int mainTexPropertyIndex;
        private EffectProperty effectPropNormalMap;
        private EffectProperty effectPropNormalStrength;
        
        private bool invertNormalMapDirection = false;
        private float normalCreateStrength = 2.0f;
        private float normalCreateSmooth = 1.0f;
        
        public NormalMapEffectDrawer(AllIn13DShaderInspectorReferences references, 
			PropertiesConfig propertiesConfig) : base(references, propertiesConfig)
        {
            drawerID = Constants.NORMAL_MAP_EFFECT_DRAWER_ID;
            
            effectConfig = propertiesConfig.FindEffectConfigByID("NORMAL_MAP");
            
            mainTexPropertyIndex = FindPropertyIndex("_MainTex");
            effectPropNormalMap = effectConfig.FindEffectPropertyByName("_NormalMap");
            effectPropNormalStrength = effectConfig.FindEffectPropertyByName("_NormalStrength");
        }
        
        protected override void DrawProperties()
        {
            DrawProperty(effectPropNormalMap);
            DrawProperty(effectPropNormalStrength);
            
            EditorUtils.DrawLine(Color.grey, 1, 3);
            EditorGUILayout.LabelField("Normal Map Creation", EditorStyles.boldLabel);
            
            EditorUtils.TextureEditorFloatParameter("Strength", ref normalCreateStrength, 0.1f, 8.0f, 2.0f);
            EditorUtils.TextureEditorFloatParameter("Smooth", ref normalCreateSmooth, 0.0f, 5.0f, 1.0f);
            invertNormalMapDirection = EditorGUILayout.Toggle("Invert Normal Direction", invertNormalMapDirection);
            
            GUILayout.Space(2);
            if(GUILayout.Button("Create Normal Map Texture")) CreateNormalMap();   
        }

        private void CreateNormalMap()
        {
            Texture2D sourceTexture = references.matProperties[mainTexPropertyIndex].textureValue as Texture2D;
            if(sourceTexture == null)
            {
                EditorUtility.DisplayDialog("Error", "No Main Tex found. Please assign it first", "OK");
                return;
            }
            EditorUtils.SetTextureReadWrite(AssetDatabase.GetAssetPath(sourceTexture), true);
            NormalMapCreatorTool normalMapCreatorTool = new NormalMapCreatorTool
            {
                targetNormalImage = sourceTexture,
                normalStrength = normalCreateStrength,
                normalSmoothing = Mathf.RoundToInt(normalCreateSmooth),
                isComputingNormals = 1
            };
            Texture2D normalMapToSave = normalMapCreatorTool.CreateNormalMap(invertNormalMapDirection);
            Texture savedTex = EditorUtils.SaveTextureAsPNG(GlobalConfiguration.instance.NormalMapSavePath, "NormalMap", "Normal Map", normalMapToSave, FilterMode.Bilinear, TextureImporterType.NormalMap, TextureWrapMode.Repeat);
            normalMapCreatorTool.isComputingNormals = 0;

            references.matProperties[effectPropNormalMap.propertyIndex].textureValue = savedTex;
        }
    }
}