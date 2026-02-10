using UnityEditor;
using UnityEngine;

namespace AllIn13DShader 
{
	[CanEditMultipleObjects]
	public class AllIn13DShaderMaterialInspector : ShaderGUI
	{
		private PropertiesConfigCollection propertiesConfigCollection;
		
		private PropertiesConfig currentPropertiesConfig;

		private AllIn13DShaderInspectorReferences inspectorReferences;
		private AbstractEffectDrawer[] drawers;
		private GlobalPropertiesDrawer globalPropertiesDrawer;
		private AdvancedPropertiesDrawer advancedPropertiesDrawer;

		private MaterialPresetCollection blendingModeCollection;

		private MaterialProperty matPropertyRenderPreset;
		private MaterialProperty matPropertyBlendSrc;
		private MaterialProperty matPropertyBlendDst;
		private MaterialProperty matPropertyZWrite;
		
		private int lastRenderQueue;
		private float lasTimeRebuilt;

		private static CommonStyles commonStyles;

		private void RefreshReferences(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			if (inspectorReferences == null)
			{
				inspectorReferences = new AllIn13DShaderInspectorReferences();
				inspectorReferences.Setup(materialEditor, properties);

				if (lastRenderQueue > 0)
				{
					for(int i = 0; i < inspectorReferences.targetMatInfos.Length; i++)
					{
						inspectorReferences.targetMatInfos[i].mat.renderQueue = lastRenderQueue;
					}
				}
			}

			if (propertiesConfigCollection == null)
			{
				string[] guids = AssetDatabase.FindAssets("PropertiesConfigCollection t:PropertiesConfigCollection");
				if(guids.Length == 0)
				{
					Debug.LogWarning("PropertiesConfigCollection not found in the project. Configuring...");
					this.propertiesConfigCollection = PropertiesConfigCreator.CreateConfig();
					Debug.LogWarning("AllIn13DShader configured");
				}
				else
				{
					string path = AssetDatabase.GUIDToAssetPath(guids[0]);
					propertiesConfigCollection = AssetDatabase.LoadAssetAtPath<PropertiesConfigCollection>(path);
				}

				RefreshPropertiesConfig();

				lasTimeRebuilt = (float)EditorApplication.timeSinceStartup;
			}

			if (blendingModeCollection == null)
			{
				blendingModeCollection = (MaterialPresetCollection)EditorUtils.FindAsset<ScriptableObject>("BlendingModeCollection");
			}

			CreateDrawers();

			matPropertyRenderPreset = inspectorReferences.matProperties[currentPropertiesConfig.renderPreset];
			matPropertyBlendSrc = inspectorReferences.matProperties[currentPropertiesConfig.blendSrcIdx];
			matPropertyBlendDst = inspectorReferences.matProperties[currentPropertiesConfig.blendDstIdx];
			matPropertyZWrite = inspectorReferences.matProperties[currentPropertiesConfig.zWriteIndex];

			//We ensure that data is refreshed. Sometimes objects are not null but we need to refresh the references
			inspectorReferences.Setup(materialEditor, properties);

			if(commonStyles == null)
			{
				commonStyles = new CommonStyles();
			}

			RefreshDrawers();
		}

		private void ResetReferences()
		{
			this.propertiesConfigCollection = null;
			this.currentPropertiesConfig = null;

			inspectorReferences = null;
			drawers = null;

			globalPropertiesDrawer = null;
			advancedPropertiesDrawer = null;
		}

		private void CreateDrawers()
		{
			if (propertiesConfigCollection == null || 
				currentPropertiesConfig == null ||
				inspectorReferences == null ||
				drawers == null ||
				globalPropertiesDrawer == null ||
				advancedPropertiesDrawer == null)
			{
				drawers = new AbstractEffectDrawer[0];

				GeneralEffectDrawer generalEffectDrawer = new GeneralEffectDrawer(inspectorReferences, currentPropertiesConfig);
				
				EffectProperty mainNormalMapProperty = currentPropertiesConfig.FindEffectProperty("NORMAL_MAP", "_NormalMap");

				TriplanarEffectDrawer triplanarEffectDrawer = new TriplanarEffectDrawer(mainNormalMapProperty, inspectorReferences, currentPropertiesConfig);
				ColorRampEffectDrawer colorRampEffectDrawer = new ColorRampEffectDrawer(inspectorReferences, currentPropertiesConfig);
				OutlineEffectDrawer outlineEffectDrawer = new OutlineEffectDrawer(inspectorReferences, currentPropertiesConfig);
				TextureBlendingEffectDrawer vertexColorEffectDrawer = new TextureBlendingEffectDrawer(mainNormalMapProperty, inspectorReferences, currentPropertiesConfig);
				NormalMapEffectDrawer normalMapEffectDrawer = new NormalMapEffectDrawer(inspectorReferences, currentPropertiesConfig);

				drawers = new AbstractEffectDrawer[]
				{
					generalEffectDrawer,
					triplanarEffectDrawer,
					colorRampEffectDrawer, 
					outlineEffectDrawer,
					vertexColorEffectDrawer,
					normalMapEffectDrawer
				};

				advancedPropertiesDrawer = new AdvancedPropertiesDrawer(currentPropertiesConfig.advancedProperties, currentPropertiesConfig.blendSrcIdx, currentPropertiesConfig.blendDstIdx, inspectorReferences);
			}

			if (globalPropertiesDrawer == null)
			{
				globalPropertiesDrawer = new GlobalPropertiesDrawer();
			}
		}

		private void RefreshDrawers()
		{
			for(int i = 0; i < drawers.Length; i++)
			{
				drawers[i].Refresh(inspectorReferences);
			}
		}

		private AbstractEffectDrawer FindEffectDrawerByID(string drawerID)
		{
			AbstractEffectDrawer res = null;

			for (int i = 0; i < drawers.Length; i++)
			{
				if (drawers[i].ID == drawerID)
				{
					res = drawers[i];
					break;
				}
			}

			return res;
		}

		public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
		{
			base.AssignNewShaderToMaterial(material, oldShader, newShader);
		}

		private void RefreshPropertiesConfig()
		{
			Shader shader = inspectorReferences.GetShader();
			currentPropertiesConfig = propertiesConfigCollection.FindPropertiesConfigByShader(shader);

			inspectorReferences.SetOutlineEffect(currentPropertiesConfig);
			inspectorReferences.SetCastShadowsEffect(currentPropertiesConfig);
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			if (lasTimeRebuilt <= EditorPrefs.GetFloat(Constants.LAST_TIME_SHADER_PROPERTIES_REBUILT_KEY, float.MaxValue))
			{
				ResetReferences();
				lasTimeRebuilt = (float)EditorApplication.timeSinceStartup;
			}

			if (inspectorReferences != null && drawers != null)
			{
				inspectorReferences.Setup(materialEditor, properties);
			}

			RefreshReferences(materialEditor, properties);

			commonStyles.InitStyles();

#if ALLIN13DSHADER_URP
			bool urpCorrectlyConfigured = URPConfigurator.IsURPCorrectlyConfigured();
			if (!urpCorrectlyConfigured)
			{
				EditorGUILayout.LabelField(CommonMessages.URP_PIPELINE_NOT_ASSIGNED, commonStyles.warningLabel);
				EditorUtils.DrawButtonLink(CommonMessages.URP_PIPELINE_NOT_ASSIGNED_DOC_LINK);

				GUILayout.Space(50f);
			}

			EditorGUI.BeginDisabledGroup(!urpCorrectlyConfigured);
#endif

			DrawPresetsTabs();
			DrawAdvancedProperties();
			DrawGlobalProperties();
			DrawEffects();

#if ALLIN13DSHADER_URP
			EditorGUI.EndDisabledGroup();
#endif

			bool shaderChanged = false;
			for (int i = 0; i < inspectorReferences.targetMatInfos.Length; i++)
			{
				Material targetMat = inspectorReferences.targetMatInfos[i].mat;

				lastRenderQueue = targetMat.renderQueue;

				if (!inspectorReferences.targetMatInfos[i].IsShaderVariant())
				{
					shaderChanged = shaderChanged || MaterialUtils.CheckMaterialShader(targetMat);
				}
			}



			EditorGUILayout.Separator();
			EditorUtils.DrawLine(Color.grey, 1, 3);

			if (inspectorReferences.AreAllMaterialsShaderGeneric())
			{
				if (GUILayout.Button("Bake Shader Keywords"))
				{
					for (int i = 0; i < inspectorReferences.targetMatInfos.Length; i++)
					{
						string variantName = Selection.objects[i].name;
						Shader newShader = ShaderVariantCreator.CreateVariantByMaterialInfo(currentPropertiesConfig, inspectorReferences.targetMatInfos[i], variantName);
						inspectorReferences.targetMatInfos[i].mat.shader = newShader;
					}
					//Shader newShader = ShaderVariantCreator.CreateVariantByMaterialInfo(currentPropertiesConfig, inspectorReferences.targetMatInfos[0], variantName);
					//materialEditor.SetShader(newShader);

					shaderChanged = true;
				}
			}
			else
			{
				if (GUILayout.Button("Revert To Generic Shader"))
				{
					Shader currentShader = inspectorReferences.GetShader();
					GlobalConfiguration.instance.effectsProfileCollection.RemoveEffectsProfileByShader(currentShader);

					Shader newShader = Shader.Find(Constants.SHADER_FULL_NAME_ALLIN13D);
					materialEditor.SetShader(newShader);

					shaderChanged = true;
				}
			}

			if (shaderChanged)
			{
				ResetReferences();
			}
		}

		//private void CheckPasses(Material targetMat)
		//{
		//	if (targetMat.IsKeywordEnabled("_LIGHTMODEL_FASTLIGHTING") || targetMat.IsKeywordEnabled("_LIGHTMODEL_NONE"))
		//	{
		//		targetMat.SetShaderPassEnabled("ForwardAdd", false);
		//	}
		//	else
		//	{
		//		targetMat.SetShaderPassEnabled("ForwardAdd", true);
		//	}
		//}

		private void DrawPresetsTabs()
		{
			EditorGUI.BeginChangeCheck();

			string[] texts = blendingModeCollection.CreateStringsArray();


			int presetIndex = (int)matPropertyRenderPreset.floatValue;
			if (presetIndex >= blendingModeCollection.presets.Length)
			{
				presetIndex = 1;
				matPropertyRenderPreset.floatValue = presetIndex;
			}

			BlendingMode previousPreset = blendingModeCollection[presetIndex];
			if(previousPreset == null)
			{
				previousPreset = blendingModeCollection[0];
			}

			int newIndex = (int)matPropertyRenderPreset.floatValue;
			newIndex = GUILayout.SelectionGrid(newIndex, texts, 3, inspectorReferences.tabButtonStyle);
			matPropertyRenderPreset.floatValue = newIndex;
			if (EditorGUI.EndChangeCheck())
			{
				BlendingMode selectedPreset = blendingModeCollection[newIndex];
				for(int i = 0; i < inspectorReferences.targetMatInfos.Length; i++)
				{
					Material targetMat = inspectorReferences.targetMatInfos[i].mat;
					ApplyMaterialPreset(targetMat, previousPreset, selectedPreset);
				}
			}
		}

		private void DrawAdvancedProperties()
		{
			//Security check
			if (advancedPropertiesDrawer != null)
			{
				advancedPropertiesDrawer.Draw();
			}
		}

		private void DrawGlobalProperties()
		{
			globalPropertiesDrawer.Draw(currentPropertiesConfig.singleProperties, inspectorReferences);
		}

		private void DrawEffects()
		{
			int globalEffectIndex = 0;
			for (int groupIdx = 0; groupIdx < currentPropertiesConfig.effectsGroups.Length; groupIdx++)
			{
				EffectGroup effectGroup = currentPropertiesConfig.effectsGroups[groupIdx];
				if (effectGroup.effects.Length <= 0) { continue; }

				EditorGUILayout.Separator();
				EditorUtils.DrawLine(Color.grey, 1, 3);
				GUILayout.Label(effectGroup.DisplayName, inspectorReferences.bigLabelStyle);

				for (int effectIdx = 0; effectIdx < effectGroup.effects.Length; effectIdx++)
				{
					AllIn13DEffectConfig effectConfig = effectGroup.effects[effectIdx];

					globalEffectIndex++;

					AbstractEffectDrawer drawer = FindEffectDrawerByID(effectConfig.effectDrawerID);
					drawer.Draw(currentPropertiesConfig, effectConfig, globalEffectIndex);
				}
			}
		}

		private void ApplyMaterialPreset(Material targetMat, BlendingMode previousPresset, BlendingMode newPreset)
		{
			matPropertyBlendSrc.floatValue = (float)newPreset.blendSrc;
			matPropertyBlendDst.floatValue = (float)newPreset.blendDst;
			matPropertyZWrite.floatValue = newPreset.depthWrite ? 1.0f : 0.0f;


			lastRenderQueue = (int)newPreset.renderQueue;
			targetMat.renderQueue = lastRenderQueue;

			if (previousPresset != newPreset && previousPresset.defaultEnabledEffects != null)
			{
				for (int i = 0; i < previousPresset.defaultEnabledEffects.Length; i++)
				{
					string effectID = previousPresset.defaultEnabledEffects[i];
					AllIn13DEffectConfig effectConfig = currentPropertiesConfig.FindEffectConfigByID(effectID);

					for(int matIdx = 0; matIdx < inspectorReferences.targetMatInfos.Length; matIdx++)
					{
						AbstractMaterialInfo matInfo = inspectorReferences.targetMatInfos[matIdx];
						AllIn13DEffectConfig.DisableEffectToggle(effectConfig, inspectorReferences, matInfo);
					}
				}
			}

			if (newPreset.defaultEnabledEffects != null)
			{
				for (int matIdx = 0; matIdx < inspectorReferences.targetMatInfos.Length; matIdx++)
				{
					AbstractMaterialInfo matInfo = inspectorReferences.targetMatInfos[matIdx];

					if (newPreset.isTransparent)
					{
						matInfo.EnableKeyword(Constants.KEYWORD_ALLIN13D_SURFACE_TRANSPARENT);
					}
					else
					{
						matInfo.DisableKeyword(Constants.KEYWORD_ALLIN13D_SURFACE_TRANSPARENT);
					}

					for (int i = 0; i < newPreset.defaultEnabledEffects.Length; i++)
					{
						string effectID = newPreset.defaultEnabledEffects[i];
						AllIn13DEffectConfig effectConfig = currentPropertiesConfig.FindEffectConfigByID(effectID);
						AllIn13DEffectConfig.EnableEffectToggle(effectConfig, inspectorReferences, matInfo);
					}
				}
			}
		}
	}
}