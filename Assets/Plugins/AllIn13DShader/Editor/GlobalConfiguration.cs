using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AllIn13DShader
{
	public class GlobalConfiguration : ScriptableObject
	{
		private static GlobalConfiguration _instance;
		public static GlobalConfiguration instance
		{
			get
			{
				return _instance;
			}
		}


		public enum ProjectType
		{
			[InspectorName("Standard PBR")]STANDARD_PBR = 2,
			[InspectorName("Standard Basic")]STANDARD_BASIC = 1,
			[InspectorName("AllIn13DShader Look")] ALLIN13DSHADERLOOK = 4,
			[InspectorName("Toon")] TOON = 0,
			[InspectorName("Custom")] CUSTOM = 3,
		}
		
		//Project type and default materials
		public ProjectType projectType;

		public Material standardBasicMaterial;
		public Material standardPBRMaterial;
		public Material toonMaterial;
		public Material allIn13dDShaderLookMaterial;

		public Material defaultPreset;


		//
		public const string MAIN_ASSEMBLY_NAME = "AllIn13DShaderAssembly";

		//
		public const string ALLIN13SHADER_CONFIG_DEFAULT_FOLDER_NAME = "AllIn3DShaderConfig";
		//public const string ALLIN13SHADER_CONFIG_DEFAULT_FOLDER = "Assets/AllIn3DShaderConfig";
		public const string GLOBAL_CONFIGURATION_ASSET_NAME = "GlobalConfiguration.asset";

		//Default Relative Paths
		
		public const string MATERIAL_SAVE_FOLDER_NAME		= "Materials";
		public const string RENDER_IMAGE_SAVE_FOLDER_NAME	= "Images";
		public const string NORMAL_MAP_SAVE_FOLDER_NAME		= "Normal Maps";
		public const string GRADIENT_SAVE_FOLDER_NAME		= "Gradients";
		public const string ATLASES_SAVE_FOLDER_NAME		= "Atlases";
		public const string NOISES_SAVE_FOLDER_NAME			= "Noises";

		//Default Root Plugin Path
		public const string GLOBAL_CONFIG_FOLDER_DEFAULT_PATH = "AllIn3DShaderConfig";
		public const string EXPORT_FOLDER_NAME_DEFAULT = "Export";

		//Paths
		[SerializeField] private string rootPluginPath;
		[SerializeField] private string globalConfigFolderPath;
		[SerializeField] private string exportFolderPath;
		[SerializeField] private string materialSavePath;
		[SerializeField] private string renderImageSavePath;
		[SerializeField] private string normalMapSavePath;
		[SerializeField] private string gradientSavePath;
		[SerializeField] private string atlasesSavePath;
		[SerializeField] private string noiseSavePath;

		//Render Image Scale
		[SerializeField] private float renderImageScale;

		//Project configured first time
		[SerializeField] private bool projectConfiguredFirstTime;

		//URP Configured first time
		[SerializeField] private bool urpConfiguredFirstTime;
		[SerializeField] private string lastPipelineConfiguredGUID;

		//Properties Config Collection
		//public PropertiesConfigCollection propertiesConfigCollection;

		//Shaders
		public Shader shStandard;
		public Shader shStandardNoShadowCaster;
		public Shader shOutline;
		public Shader shOutlineNoShadowCaster;

		//Effects Profile
		public EffectsProfileCollection effectsProfileCollection;

		//Shader Pass Collection
		public ShaderPassCollection shaderPassCollection;

		public string RootPluginPath
		{
			get
			{
				return rootPluginPath;
			}
			set
			{
				rootPluginPath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string GlobalConfigFolderPath
		{
			get
			{
				return globalConfigFolderPath;
			}
			set
			{
				globalConfigFolderPath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string ExportFolderPath
		{
			get
			{
				return exportFolderPath;
			}
			set
			{
				exportFolderPath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string MaterialSavePath
		{
			get
			{
				return materialSavePath;
			}
			set
			{
				materialSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string RenderImageSavePath
		{
			get
			{
				return renderImageSavePath;
			}
			set
			{
				renderImageSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string NormalMapSavePath
		{
			get
			{
				return normalMapSavePath;
			}
			set
			{
				normalMapSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string GradientSavePath
		{
			get
			{
				return gradientSavePath;
			}
			set
			{
				gradientSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string AtlasesSavePath
		{
			get
			{
				return atlasesSavePath;
			}
			set
			{
				atlasesSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public string NoiseSavePath
		{
			get
			{
				return noiseSavePath;
			}
			set
			{
				noiseSavePath = value;
				EditorUtility.SetDirty(this);
			}
		}

		public float RenderImageScale
		{
			get
			{
				return renderImageScale;
			}
			set
			{
				renderImageScale = value;
				EditorUtility.SetDirty(this);
			}
		}

		public bool ProjectConfiguredFirstTime
		{
			get
			{
				return projectConfiguredFirstTime;
			}
			set
			{
				projectConfiguredFirstTime = value;
				EditorUtility.SetDirty(this);
			}
		}

		public bool URPConfiguredFirstTime
		{
			get
			{
				return urpConfiguredFirstTime;
			}
			set
			{
				urpConfiguredFirstTime = value;
				EditorUtility.SetDirty(this);
			}
		}

		public void Init(string defaultGlobalConfigFolderPath)
		{
			if (!ProjectConfiguredFirstTime)
			{
				SetDefaultValues();
				ProjectConfiguredFirstTime = true;
			}

			globalConfigFolderPath	= InitPath(globalConfigFolderPath, defaultGlobalConfigFolderPath, GLOBAL_CONFIG_FOLDER_DEFAULT_PATH);

			exportFolderPath	= InitPath(exportFolderPath, globalConfigFolderPath, EXPORT_FOLDER_NAME_DEFAULT);

			materialSavePath	= InitPath(materialSavePath, exportFolderPath, MATERIAL_SAVE_FOLDER_NAME);
			renderImageSavePath = InitPath(renderImageSavePath, exportFolderPath, RENDER_IMAGE_SAVE_FOLDER_NAME);
			normalMapSavePath	= InitPath(normalMapSavePath, exportFolderPath, NORMAL_MAP_SAVE_FOLDER_NAME);
			gradientSavePath	= InitPath(gradientSavePath, exportFolderPath, GRADIENT_SAVE_FOLDER_NAME);
			atlasesSavePath		= InitPath(atlasesSavePath, exportFolderPath, ATLASES_SAVE_FOLDER_NAME);
			noiseSavePath		= InitPath(noiseSavePath, exportFolderPath, NOISES_SAVE_FOLDER_NAME); 

			bool foldersCreated = false;
			CreateDefaultExportFoldersIfNotExist(ref foldersCreated);

			if (foldersCreated)
			{
				AssetDatabase.Refresh();
			}
		}

		public void InitDefault()
		{
			
		}

		public string InitPath(string path, string parentFolder, string defaultValue, bool isRoot = false)
		{
			string res = path;
			if (!AssetDatabase.IsValidFolder(res))
			{
				string defaultPath = defaultValue;
				if (!isRoot)
				{
					defaultPath = Path.Combine(parentFolder, defaultValue);
				}
				res = defaultPath;

				EditorUtility.SetDirty(this);
			}

			return res;
		}

		public void CreateDefaultExportFoldersIfNotExist(ref bool foldersCreated)
		{
			CreateFolderIfNotExist(globalConfigFolderPath, ref foldersCreated);

			CreateFolderIfNotExist(exportFolderPath, ref foldersCreated);

			CreateFolderIfNotExist(materialSavePath, ref foldersCreated);
			CreateFolderIfNotExist(renderImageSavePath, ref foldersCreated);
			CreateFolderIfNotExist(normalMapSavePath, ref foldersCreated);
			CreateFolderIfNotExist(gradientSavePath, ref foldersCreated);
			CreateFolderIfNotExist(atlasesSavePath, ref foldersCreated);
			CreateFolderIfNotExist(noiseSavePath, ref foldersCreated);
		}

		private void CreateFolderIfNotExist(string absoluteFolderPath, ref bool foldersCreated)
		{
			if (!AssetDatabase.IsValidFolder(absoluteFolderPath))
			{
				AssetDatabase.CreateFolder(Path.GetDirectoryName(absoluteFolderPath), Path.GetFileName(absoluteFolderPath));
				foldersCreated = foldersCreated || true;
			}
		}

		public void RefreshDefaultMaterial()
		{
			switch (projectType)
			{
				case ProjectType.STANDARD_BASIC:
					this.defaultPreset = standardBasicMaterial;
					break;
				case ProjectType.STANDARD_PBR:
					this.defaultPreset = standardPBRMaterial;
					break;
				case ProjectType.TOON:
					this.defaultPreset = toonMaterial;
					break;
				case ProjectType.ALLIN13DSHADERLOOK:
					this.defaultPreset = allIn13dDShaderLookMaterial;
					break;
			}
		}

		public void RootFolderChanged(string oldPluginRootPath) 
		{
			string oldGlobalConfigFolder = Path.Combine(oldPluginRootPath, GLOBAL_CONFIG_FOLDER_DEFAULT_PATH);
			string oldExportFolder = GetExportFolderByParentFolder(oldGlobalConfigFolder);

			GlobalConfigFolderPath = UpdateRootFolders(oldPluginRootPath, GlobalConfigFolderPath, RootPluginPath, GLOBAL_CONFIG_FOLDER_DEFAULT_PATH);

			ExportFolderPath = UpdateRootFolders(oldGlobalConfigFolder, ExportFolderPath, GlobalConfigFolderPath, EXPORT_FOLDER_NAME_DEFAULT);

			MaterialSavePath	= UpdateRootFolders(oldExportFolder, MaterialSavePath, ExportFolderPath, MATERIAL_SAVE_FOLDER_NAME);
			RenderImageSavePath = UpdateRootFolders(oldExportFolder, RenderImageSavePath, ExportFolderPath, RENDER_IMAGE_SAVE_FOLDER_NAME);
			NormalMapSavePath	= UpdateRootFolders(oldExportFolder, NormalMapSavePath, ExportFolderPath, NORMAL_MAP_SAVE_FOLDER_NAME);
			GradientSavePath	= UpdateRootFolders(oldExportFolder, GradientSavePath, ExportFolderPath, GRADIENT_SAVE_FOLDER_NAME);
			AtlasesSavePath		= UpdateRootFolders(oldExportFolder, AtlasesSavePath, ExportFolderPath, ATLASES_SAVE_FOLDER_NAME);
			NoiseSavePath		= UpdateRootFolders(oldExportFolder, NoiseSavePath, ExportFolderPath, NOISES_SAVE_FOLDER_NAME);
		}

		private string UpdateRootFolders(string oldRootFolder, string pathToCheck, string parentFolder, string relativePath)
		{
			string res = pathToCheck; 

			string pathToCheckFull = Path.GetFullPath(pathToCheck);
			string pathWithOldRootFull = Path.GetFullPath(Path.Combine(oldRootFolder, relativePath)); 

			if (pathToCheckFull == pathWithOldRootFull)
			{
				res = Path.Combine(parentFolder, relativePath);
			}

			return res;
		}

		public static string GetRootPluginFolderPath() 
		{
			Object mainAssemblyAsset = EditorUtils.FindAsset<AssemblyDefinitionAsset>(MAIN_ASSEMBLY_NAME);
			string assemblyPath = AssetDatabase.GetAssetPath(mainAssemblyAsset);

			string res = Path.GetDirectoryName(assemblyPath); 
			return res;
		}

		public static bool CheckRootFolder(out string oldRootFolder)
		{
			bool res = false;
			string newRootPluginFolderPath = GetRootPluginFolderPath();
			oldRootFolder = instance.RootPluginPath;

			if (newRootPluginFolderPath != instance.RootPluginPath && newRootPluginFolderPath != null)
			{
				string oldRootPath = instance.RootPluginPath;
				instance.RootPluginPath = newRootPluginFolderPath;

				res = true;

				//instance.RootFolderChanged(oldRootPath);
			}

			res = res && !string.IsNullOrEmpty(oldRootFolder);

			return res;
		}

		public static void CheckGlobalConfigFolder()
		{
			string newGlobalConfigFolderPath = _instance.globalConfigFolderPath;

			if (newGlobalConfigFolderPath != instance.GlobalConfigFolderPath)
			{
				string oldGlobalConfigFolderPath = instance.GlobalConfigFolderPath;
				instance.GlobalConfigFolderPath = newGlobalConfigFolderPath;
				instance.RootFolderChanged(oldGlobalConfigFolderPath);
			}
		}

		public static void CheckMaterialReferences()
		{
			string allIn13DShaderLookMaterialPath = Path.Combine(_instance.RootPluginPath, "Editor/MaterialPresets/MAT_Preset_AllIn13DShaderLook.mat");
			string basicMaterialPath = Path.Combine(_instance.RootPluginPath, "Editor/MaterialPresets/MAT_Preset_Basic.mat");
			string standardPBRMaterialPath = Path.Combine(_instance.RootPluginPath, "Editor/MaterialPresets/MAT_Preset_StandardPBR.mat");
			string toonMaterialPath = Path.Combine(_instance.RootPluginPath, "Editor/MaterialPresets/MAT_Preset_Toon.mat");

			if(_instance.allIn13dDShaderLookMaterial == null)
			{
				_instance.allIn13dDShaderLookMaterial = AssetDatabase.LoadAssetAtPath<Material>(allIn13DShaderLookMaterialPath);
			}

			if(_instance.standardBasicMaterial == null)
			{
				_instance.standardBasicMaterial = AssetDatabase.LoadAssetAtPath<Material>(basicMaterialPath);
			}

			if(_instance.standardPBRMaterial == null)
			{
				_instance.standardPBRMaterial = AssetDatabase.LoadAssetAtPath<Material>(standardPBRMaterialPath);
			}

			if(_instance.toonMaterial == null)
			{
				_instance.toonMaterial = AssetDatabase.LoadAssetAtPath<Material>(toonMaterialPath);
			}

			if(_instance.defaultPreset == null)
			{
				_instance.RefreshDefaultMaterial();
			}
		}

		//private static string GetGlobalConfigFolderPath()
		//{
		//	string assetPath = AssetDatabase.GetAssetPath(instance);

		//	string res = Path.GetDirectoryName(assetPath);
		//	return res;
		//}

		public void SetDefaultValues()
		{
			projectType = ProjectType.STANDARD_PBR;
			defaultPreset = standardPBRMaterial;

			EditorUtility.SetDirty(this);
		}

		public void Save()
		{
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssetIfDirty(this);
		}

		public static string GetDefaultConfigFolderPath()
		{
			string res = string.Empty;
#if ALLIN13DSHADER_DEVELOP
			res = Path.Combine("Assets", ALLIN13SHADER_CONFIG_DEFAULT_FOLDER_NAME);
#else
			res = Path.Combine(_instance.RootPluginPath, ALLIN13SHADER_CONFIG_DEFAULT_FOLDER_NAME);
#endif
			return res;
		}

		public static string GetParentConfigFolderPath()
		{
			string res = string.Empty;
#if ALLIN13DSHADER_DEVELOP
			res = "Assets";
#else
			res = instance.RootPluginPath;
#endif
			return res;
		}

		public static string GetExportFolderByParentFolder(string rootFolder)
		{
			string res = string.Empty;

#if ALLIN13DSHADER_DEVELOP
			res = Path.Combine("Assets", EXPORT_FOLDER_NAME_DEFAULT);
#else
			res = Path.Combine(rootFolder, EXPORT_FOLDER_NAME_DEFAULT); 
#endif

			return res;
		}

		public static GlobalConfiguration SaveInstanceAsAsset()
		{
			string defaultConfigFolderPath = GetDefaultConfigFolderPath();
			string parentFolder = GetParentConfigFolderPath();
			string defaultGlobalConfigurationPath = Path.Combine(defaultConfigFolderPath, GLOBAL_CONFIGURATION_ASSET_NAME);

			if (!AssetDatabase.IsValidFolder(defaultConfigFolderPath))
			{
				AssetDatabase.CreateFolder(Path.GetDirectoryName(defaultConfigFolderPath), Path.GetFileName(defaultConfigFolderPath));
			}

			//GlobalConfiguration res = ScriptableObject.CreateInstance<GlobalConfiguration>();
			_instance.Init(parentFolder);
			AssetDatabase.CreateAsset(_instance, defaultGlobalConfigurationPath);
			
			return _instance;
		}

		public static GlobalConfiguration CreateInstanceIfNeeded(out bool globalConfigInstanceCreated)
		{
			GlobalConfiguration res = EditorUtils.FindAsset<GlobalConfiguration>("GlobalConfiguration");
			globalConfigInstanceCreated = false;

			if (res == null)
			{
				res = ScriptableObject.CreateInstance<GlobalConfiguration>();
				globalConfigInstanceCreated = true;
			}

			return res;
		}

		//public static GlobalConfiguration InitializeInstance()
		//{
		//	GlobalConfiguration res = EditorUtils.FindAsset<GlobalConfiguration>("GlobalConfiguration");

		//	if (res == null)
		//	{
		//		res = CreateInstanceInDefaultPath();
		//	}

		//	return res;
		//}

		public static void InitIfNeeded()
		{
			bool globalConfigInstanceCreated = false;
			bool needToCreateInstance = _instance == null || !AssetDatabase.IsMainAsset(_instance);

			if (needToCreateInstance)
			{
				//_instance = InitializeInstance();
				_instance = CreateInstanceIfNeeded(out globalConfigInstanceCreated);
			}

			string oldRootPath = string.Empty;
			bool rootFolderChanged = CheckRootFolder(out oldRootPath);
			if (globalConfigInstanceCreated)
			{
				SaveInstanceAsAsset();
			}

			if (rootFolderChanged)
			{
				instance.RootFolderChanged(oldRootPath);
			}

			//CheckGlobalConfigFolder(rootFolderChanged);

			CheckMaterialReferences();

			SessionState.SetString(ConstantsRuntime.SESSION_KEY_ROOT_PLUGIN_PATH, _instance.rootPluginPath);
		}

		//public static void SetPropertiesConfigCollection(PropertiesConfigCollection propertiesConfigCollection)
		//{
		//	if(propertiesConfigCollection != null)
		//	{
		//		if (_instance.propertiesConfigCollection != propertiesConfigCollection)
		//		{
		//			_instance.propertiesConfigCollection = propertiesConfigCollection;
		//			EditorUtility.SetDirty(_instance);
		//		}
		//	}

		//	if(_instance.propertiesConfigCollection == null)
		//	{
		//		_instance.propertiesConfigCollection = PropertiesConfigCreator.FindPropertiesCollection();
		//		EditorUtility.SetDirty(_instance);
		//	}
		//}

		public static void SetupShadersReferences()
		{
			if(_instance.shStandard == null)
			{
				_instance.shStandard = Shader.Find("AllIn13DShader/AllIn13DShader");
			}

			if(_instance.shStandardNoShadowCaster == null)
			{
				_instance.shStandardNoShadowCaster = Shader.Find("AllIn13DShader/AllIn13DShader_NoShadowCaster");
			}

			if(_instance.shOutline == null)
			{
				_instance.shOutline = Shader.Find("AllIn13DShader/AllIn13DShaderOutline");
			}

			if(_instance.shOutlineNoShadowCaster == null)
			{
				_instance.shOutlineNoShadowCaster = Shader.Find("AllIn13DShader/AllIn13DShaderOutline_NoShadowCaster");
			}
		}

		public EffectsProfileCollection GetEffectsProfileCollection()
		{
			return effectsProfileCollection;
		}

		public void SetEffectsProfileCollection(EffectsProfileCollection effectsProfileCollection)
		{
			this.effectsProfileCollection = effectsProfileCollection;
		}
	}
}