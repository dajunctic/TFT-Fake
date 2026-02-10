using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor.Rendering;
using System.IO;


#if ALLIN13DSHADER_URP

using UnityEditor.Rendering.Universal;
#if UNITY_6000_0_OR_NEWER
using UnityEngine.Rendering.Universal;
#else
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
#endif

#endif

namespace AllIn13DShader
{
	public static class URPConfigurator
	{
#if ALLIN13DSHADER_URP

		private const string OUTLINE_PASS_NAME = "OutlinePass";
		private const string OUTLINE_OPAQUE_RENDER_FEATURE_NAME = "Render Feature - Outline Opaque";
		private const string OUTLINE_TRANSPARENT_RENDER_FEATURE_NAME = "Render Feature - Outline Transparent";

		private const string KEY_PIPELINE_PACKAGE_REMOVED = "ALLIN13DSHADER_PIPELINE_PACKAGE_REMOVED_KEY";

		public static bool IsURPCorrectlyConfigured()
		{
			bool res = true;
			if(GraphicsSettings.currentRenderPipeline == null)
			{
				res = false;
			}

			return res;
		}

		public static void Configure(bool forceConfigure = false)
		{
			RenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline;

			if(pipeline != null)
			{
				string guid;
				long localID;
				AssetDatabase.TryGetGUIDAndLocalFileIdentifier(pipeline, out guid, out localID);

				if (forceConfigure)
				{
					ConfigureURPAccepted(pipeline);

					GlobalConfiguration.instance.URPConfiguredFirstTime = true;
				}
				else
				{
					if (!GlobalConfiguration.instance.URPConfiguredFirstTime)
					{
						bool configureAccepted = EditorUtility.DisplayDialog("AllIn13DShader automatic configuration", "Do you want to automatically configure this URP project to work with all 3D-Shader?\n" +
		                                                                                               "If you Cancel please check the Documentation:\n" +
		                                                                                               "https://seasidestudios.gitbook.io/seaside-studios/3d-shader/urp-and-post-processing-setup", "Configure", "Cancel");
						if (configureAccepted)
						{
							ConfigureURPAccepted(pipeline);
						}

						GlobalConfiguration.instance.URPConfiguredFirstTime = true;
					}
				}
			}
		}

		private static void ConfigureURPAccepted(RenderPipelineAsset pipeline)
		{
			Debug.Log("Configuring plugin to work with URP...");

			ConfigureRenderPipeline(pipeline);

			string demoMaterialsFolder = Path.Combine(GlobalConfiguration.instance.RootPluginPath, Constants.STANDARD_EXAMPLES_MATERIALS_LOCAL_PATH);
			if (AssetDatabase.IsValidFolder(demoMaterialsFolder))
			{
				ConvertMaterialsFolder(demoMaterialsFolder);
			}
		}

		private static void CreateRenderFeatureOutline(UniversalRendererData universalRendererData, 
			string renderFeatureName,
			RenderQueueType renderQueueType, RenderPassEvent renderPassEvent)
		{
			RenderObjects outlineRenderFeature = null;
			bool outlineRenderFeatureFound = false;

			List<ScriptableRendererFeature> rendererFeatures = universalRendererData.rendererFeatures;
			foreach (ScriptableRendererFeature scriptableRendererFeature in rendererFeatures)
			{
				if (scriptableRendererFeature is RenderObjects)
				{
					RenderObjects renderObjects = (RenderObjects)scriptableRendererFeature;
					string[] passNames = renderObjects.settings.filterSettings.PassNames;

					for (int i = 0; i < passNames.Length; i++)
					{
						if (passNames[i] == OUTLINE_PASS_NAME && renderObjects.settings.filterSettings.RenderQueueType == renderQueueType)
						{
							outlineRenderFeature = renderObjects;
							outlineRenderFeatureFound = true;
						}
					}

					if (outlineRenderFeatureFound)
					{
						break;
					}
				}
			}

			if (!outlineRenderFeatureFound)
			{
				outlineRenderFeature = RenderObjects.CreateInstance<RenderObjects>();
				outlineRenderFeature.name = renderFeatureName;

				rendererFeatures.Add(outlineRenderFeature);

				FieldInfo fieldInfoRenderFeaturesMap = typeof(UniversalRendererData).GetField("m_RendererFeatureMap", BindingFlags.Instance | BindingFlags.NonPublic);
				List<long> renderFeaturesMapList = fieldInfoRenderFeaturesMap.GetValue(universalRendererData) as List<long>;

				string guid;
				long localID;
				AssetDatabase.TryGetGUIDAndLocalFileIdentifier(outlineRenderFeature, out guid, out localID);
				renderFeaturesMapList.Add(localID);

				AssetDatabase.AddObjectToAsset(outlineRenderFeature, universalRendererData);
				EditorUtility.SetDirty(outlineRenderFeature);
			}

			outlineRenderFeature.name = renderFeatureName;
			outlineRenderFeature.settings.filterSettings.RenderQueueType = renderQueueType;
			outlineRenderFeature.settings.filterSettings.LayerMask = ~0;
			outlineRenderFeature.settings.Event = renderPassEvent;
			outlineRenderFeature.settings.filterSettings.PassNames = new string[] { OUTLINE_PASS_NAME };
			outlineRenderFeature.SetActive(true);



			EditorUtility.SetDirty(outlineRenderFeature);
			EditorUtility.SetDirty(universalRendererData);

			FieldInfo fieldInfoRendererFeatures = typeof(UniversalRendererData).GetField("m_RendererFeatures", BindingFlags.Instance | BindingFlags.NonPublic);
			fieldInfoRendererFeatures.SetValue(universalRendererData, rendererFeatures);
		}

		public static void ConfigureRenderPipeline(RenderPipelineAsset renderPipelineAsset)
		{
			UniversalRenderPipelineAsset pipeline = renderPipelineAsset as UniversalRenderPipelineAsset;
			ScriptableRenderer scriptableRenderer = pipeline.GetRenderer(0);

			FieldInfo rendererDataListFieldInfo = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			ScriptableRendererData[] scriptableRendererDatas = rendererDataListFieldInfo.GetValue(pipeline) as ScriptableRendererData[];

			UniversalRendererData universalRendererData = scriptableRendererDatas[0] as UniversalRendererData;

			if(universalRendererData != null)
			{
				Undo.RecordObject(universalRendererData, "");

				universalRendererData.depthPrimingMode = DepthPrimingMode.Disabled;


				CreateRenderFeatureOutline(universalRendererData: universalRendererData, renderFeatureName: OUTLINE_OPAQUE_RENDER_FEATURE_NAME, 
					renderQueueType: RenderQueueType.Opaque, renderPassEvent: RenderPassEvent.AfterRenderingOpaques);

				CreateRenderFeatureOutline(universalRendererData: universalRendererData, renderFeatureName: OUTLINE_TRANSPARENT_RENDER_FEATURE_NAME,
					renderQueueType: RenderQueueType.Transparent, renderPassEvent: RenderPassEvent.AfterRenderingTransparents);

				EditorUtility.SetDirty(universalRendererData);
				EditorUtility.SetDirty(pipeline);
				AssetDatabase.SaveAssets();
			}
		}

		public static void ConvertMaterialsFolder(string dirPath)
		{
			StandardUpgrader standardUpgrader = new StandardUpgrader("Standard");

			DirectoryInfo dir = new DirectoryInfo(dirPath);

			List<string> materialsPathsToConvert = new List<string>();
			FileInfo[] files = dir.GetFiles("*.mat");
			for (int i = 0; i < files.Length; i++)
			{
				string materialPath = Path.Combine(dirPath, files[i].Name);
				materialsPathsToConvert.Add(materialPath);
			}

			for (int i = 0; i < materialsPathsToConvert.Count; i++)
			{
				Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialsPathsToConvert[i]);
				standardUpgrader.Upgrade(mat, MaterialUpgrader.UpgradeFlags.None);
			}
		}

		public static void CheckURPRemoved(string[] deletedAssets, bool didDomainReload)
		{
			bool removeUniversalForward = false;
			for (int i = 0; i < deletedAssets.Length; i++)
			{
				removeUniversalForward = deletedAssets[i].StartsWith("Packages/com.unity.render-pipelines");
			}

			SessionState.SetBool(KEY_PIPELINE_PACKAGE_REMOVED, removeUniversalForward);


			if (!didDomainReload && SessionState.GetBool(KEY_PIPELINE_PACKAGE_REMOVED, false))
			{
				RenderPipelineChecker.RemovePipelineSymbols();
				SessionState.SetBool(KEY_PIPELINE_PACKAGE_REMOVED, false);

				Debug.LogWarning("Render pipeline package removed. Configuring AllIn13DShader...");
			}
		}

		public static void AllAssetProcessed()
		{
			Configure();
		}


		public static bool IsRenderPipelineAssigned()
		{
			bool res = GraphicsSettings.currentRenderPipeline != null;
			return res;
		}

		public static bool IsOutlinePassConfigured()
		{
			bool res = false;

			UniversalRenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
			ScriptableRenderer scriptableRenderer = pipeline.GetRenderer(0);

			FieldInfo rendererDataListFieldInfo = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			ScriptableRendererData[] scriptableRendererDatas = rendererDataListFieldInfo.GetValue(pipeline) as ScriptableRendererData[];

			UniversalRendererData universalRendererData = scriptableRendererDatas[0] as UniversalRendererData;

			List<ScriptableRendererFeature> rendererFeatures = universalRendererData.rendererFeatures;
			foreach (ScriptableRendererFeature scriptableRendererFeature in rendererFeatures)
			{
				RenderObjects renderObjects = (RenderObjects)scriptableRendererFeature;

				string[] passNames = renderObjects.settings.filterSettings.PassNames;
				foreach (string passName in passNames)
				{
					if (passName == OUTLINE_PASS_NAME)
					{
						res = true;
						break;
					}
				}
			}

			return res;
		}

		public static bool IsDemoMaterialsConfigured()
		{
			bool res = false;

			string demoMaterialsFolder = Path.Combine(GlobalConfiguration.instance.RootPluginPath, Constants.STANDARD_EXAMPLES_MATERIALS_LOCAL_PATH);
			if (AssetDatabase.IsValidFolder(demoMaterialsFolder))
			{
				ConvertMaterialsFolder(demoMaterialsFolder);
			}

			return res;
		}

		public static bool IsURPMissconfigured()
		{
			bool res = false;

			bool renderPipelineAssigned = GraphicsSettings.currentRenderPipeline != null;

			return res;
		}
#endif
	}
}