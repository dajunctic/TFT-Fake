using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class PropertiesConfigCreator
	{
		private const string KEY_PROPERTIES_CONFIG_CREATED_FIRST_TIME = "AllIn13DShader_PropertiesConfigCreatedFirstTime";
		
		private const string ASSET_NAME = "PropertiesConfigCollection.asset";

		private static PropertiesConfigCollection propertiesCollection;

		private static EffectGroupGlobalConfigCollection effectGroupGlobalConfigCollection;
		private static EffectsExtraData effectsExtraData;


		public static PropertiesConfigCollection CreateConfig()
		{
			propertiesCollection = CreatePropertiesCollection();
			effectGroupGlobalConfigCollection = GetEffectGroupGlobalConfigCollection();
			effectsExtraData = GetEffectsExtraData();

			CreatePropertiesConfig(propertiesCollection, Constants.SHADER_NAME_ALLIN13D_OUTLINE);
			
			EditorUtility.SetDirty(propertiesCollection);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			EditorPrefs.SetFloat(Constants.LAST_TIME_SHADER_PROPERTIES_REBUILT_KEY, (float)EditorApplication.timeSinceStartup);

			Debug.LogWarning("Creating data...");

			return propertiesCollection;
		}

		private static PropertiesConfigCollection CreatePropertiesCollection()
		{
			PropertiesConfigCollection res = FindPropertiesCollection();
			string path = AssetDatabase.GetAssetPath(res);

			if (!File.Exists(path))
			{
				string propertiesCollectionPath = Path.Combine(GlobalConfiguration.instance.GlobalConfigFolderPath, ASSET_NAME);
				res = ScriptableObject.CreateInstance<PropertiesConfigCollection>();
				AssetDatabase.CreateAsset(res, propertiesCollectionPath);
				AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
			}

			return res;
		}

		private static EffectGroupGlobalConfigCollection GetEffectGroupGlobalConfigCollection()
		{
			string[] guids = AssetDatabase.FindAssets("t:EffectGroupGlobalConfigCollection");
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);

			EffectGroupGlobalConfigCollection res = AssetDatabase.LoadAssetAtPath<EffectGroupGlobalConfigCollection>(path);
			return res;
		}

		private static EffectsExtraData GetEffectsExtraData()
		{
			EffectsExtraData res = null;

			res = EditorUtils.FindAsset<EffectsExtraData>("EffectsExtraData");

			return res;
		}

		private static void CreatePropertiesConfig(PropertiesConfigCollection propertiesCollection, string shaderName)
		{
			string shaderNameWithExtension = shaderName + ".shader";
			string assetPath = Path.Combine(Constants.SHADERS_GENERIC_FOLDER_PATH, shaderNameWithExtension);

			Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);

			PropertiesConfig propertiesConfig = new PropertiesConfig();
			propertiesConfig.shader = shader;

			propertiesConfig.CreateConfig(effectGroupGlobalConfigCollection, effectsExtraData);

			propertiesCollection.AddConfig(propertiesConfig);
		}

		public static PropertiesConfigCollection FindPropertiesCollection()
		{
			PropertiesConfigCollection res = null;

			string[] guids = AssetDatabase.FindAssets("t:PropertiesConfigCollection");
			if (guids.Length > 0)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
				res = AssetDatabase.LoadAssetAtPath<PropertiesConfigCollection>(assetPath);
			}

			return res;
		}

		public static PropertiesConfigCollection InitIfNeeded(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
		{
			PropertiesConfigCollection res = FindPropertiesCollection();

			bool assetDeleted = false;
			assetDeleted = EditorUtils.ConstainsFileName(deletedAssets, ASSET_NAME);
			
			bool configCreatedFirstTime = SessionState.GetBool(KEY_PROPERTIES_CONFIG_CREATED_FIRST_TIME, false);

			if (!configCreatedFirstTime)
			{
#if UNITY_WEBGL
				URPSettingsController.DisableFeature("ALLIN1_DOTS_INSTANCING_SUPPORT");
#endif
			}

#if !ALLIN13DSHADER_DEVELOP
			configCreatedFirstTime = true;
#endif

			bool shadersDeleted = EditorUtils.ContainsAnyFolowingFileNames(deletedAssets, Constants.SHADERS_NAMES, "shader");
			bool shadersImported = false;
			if (!shadersDeleted)
			{
				shadersImported = EditorUtils.ContainsAnyFolowingFileNames(importedAssets, Constants.SHADERS_NAMES, "shader");
			}

			if (res == null || !configCreatedFirstTime || assetDeleted || shadersDeleted /*|| shadersImported*/)
			{
				SessionState.SetBool(KEY_PROPERTIES_CONFIG_CREATED_FIRST_TIME, true);
				ShadersCreatorTool.BuildShaderFiles();
				res = CreateConfig();
			}


			SessionState.SetBool(KEY_PROPERTIES_CONFIG_CREATED_FIRST_TIME, true);

			return res;
		}
	}
}