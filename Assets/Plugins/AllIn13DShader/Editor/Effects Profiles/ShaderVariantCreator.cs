using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public static class ShaderVariantCreator
	{
		private const string INCLUDE_LINE_FORMAT = @"#include ""{0}""";
		
		private const string TAG_VARIANT_NAME = @"<VARIANT_NAME>";
		private const string TAG_SHADER_PROPERTIES = @"<SHADER_PROPERTIES>";

		private const string TAG_SHADER_PASSES_URP	= @"<SHADER_PASSES_URP>";
		private const string TAG_SHADER_PASSES_BIRP = @"<SHADER_PASSES_BIRP>";

		//
		private const string TAG_PASS_NAME					= @"<PASS_NAME>";
		private const string TAG_LIGHT_MODE					= @"<LIGHT_MODE>";
		private const string TAG_BLEND_COMMAND				= @"<BLEND_COMMAND>";
		private const string TAG_CULL_COMMAND				= @"<CULL_COMMAND>";
		

		private const string TAG_Z_WRITE_COMMAND			= @"<Z_WRITE_COMMAND>";

		private const string TAG_Z_TEST_COMMAND				= @"<Z_TEST_COMMAND>";

		private const string TAG_COLOR_MASK_COMMAND			= @"<COLOR_MASK_COMMAND>";


		private const string TAG_STENCIL_BLOCK				= @"<STENCIL_BLOCK>";
		private const string TAG_FEATURES_URP_DEFINES		= @"<FEATURES_URP_DEFINES>";
		private const string TAG_FEATURES_URP_LIBRARY		= @"<FEATURES_URP_LIBRARY>";
		private const string TAG_VERTEX_PROGRAM_NAME		= @"<VERTEX_PROGRAM_NAME>";
		private const string TAG_FRAGMENT_PROGRAM_NAME		= @"<FRAGMENT_PROGRAM_NAME>";
		private const string TAG_ALL_IN_1_FEATURES			= @"<ALLIN1_FEATURES>";
		private const string TAG_PIPELINE_PASS_SYMBOL		= @"<PIPELINE_PASS_SYMBOL>";
		private const string TAG_THIS_PASS_SYMBOL			= @"<THIS_PASS_SYMBOL>";
		private const string TAG_INCLUDE_PIPELINE_HELPER	= @"<INCLUDE_PIPELINE_HELPER>";
		private const string TAG_INCLUDE_EFFECT_LIBRARIES	= @"<INCLUDE_EFFECT_LIBRARIES>";
		private const string TAG_INCLUDE_PASS				= @"<INCLUDE_PASS>";
		private const string TAG_EXTRA_PRAGMA_LINES			= @"<EXTRA_PRAGMA_LINES>";

		private const string TAG_INCLUDE_ALL_IN_SHADER_FEATURES = @"<INCLUDE_ALL_IN_1_SHADER_FEATURES>";
		private const string TAG_INCLUDE_ALL_IN_1_SHADER_LIGHT	= @"<INCLUDE_ALL_IN_1_SHADER_LIGHT>";
		private const string TAG_INCLUDE_CORE_LIBRARY			= @"<INCLUDE_CORE_LIBRARY>";
		private const string TAG_INCLUDE_COMMON_FUNCTIONS		= @"<INCLUDE_COMMON_FUNCTIONS>";
		private const string TAG_INCLUDE_COMMON_STRUCTS			= @"<INCLUDE_COMMON_STRUCTS>";
		//

		private static string TEMPLATE_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Editor/Effects Profiles/Templates/ShaderTemplate.allin1template";
		private static string SHADER_PROPERTIES_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Editor/Templates/AllIn13DShader_ShaderProperties.allIn13DData";
		private static string SHADER_PASS_URP_TAMPLATE_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Editor/Effects Profiles/Templates/PassTemplate_URP.allin1template";
		private static string SHADER_PASS_BIRP_TAMPLATE_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Editor/Effects Profiles/Templates/PassTemplate_BIRP.allin1template";


		public static string SHADER_VARIANTS_FOLDER_NAME = "Baked Variants";
		private static string SHADER_VARIANT_FILE_NAME = "AllIn13D_BakedEffects_{0}.shader";
		
		public static Shader CreateVariantByMaterialInfo(PropertiesConfig propertiesConfig, AbstractMaterialInfo matInfo, string profileName)
		{
			EffectsProfileCollection effectsProfileCollection = GlobalConfiguration.instance.effectsProfileCollection;
			
			EffectsProfile effectsProfile = effectsProfileCollection.CreateNewProfile(profileName);

			effectsProfile.InitFromOtherProfile(effectsProfileCollection.generalProfile);
			effectsProfileCollection.ConfigureEffectProfileByMaterialInfo(
				target: effectsProfile, 
				activeEffectsList: effectsProfileCollection.generalProfile, 
				matInfo: matInfo, 
				propertiesConfig: propertiesConfig);

			string folderPath = Path.Combine(Constants.SHADERS_FOLDER_PATH, SHADER_VARIANTS_FOLDER_NAME);
			Shader res = CreateVariant(effectsProfile, effectsProfileCollection, folderPath, false);
			
			return res;
		}

		public static Shader CreateVariant(EffectsProfileCollection effectsProfileCollection, string profileName)
		{
			EffectsProfile effectProfile = effectsProfileCollection.CreateNewProfile(profileName);
			effectProfile.InitFromOtherProfile(effectsProfileCollection.generalProfile);

			string folderPath = Path.Combine(Constants.SHADERS_FOLDER_PATH, SHADER_VARIANTS_FOLDER_NAME);
			Shader res = CreateVariant(effectProfile, effectsProfileCollection, folderPath, false);
			return res;
		}

		public static Shader CreateVariant(EffectsProfile effectsProfile, EffectsProfileCollection effectsProfileCollection, string folderPath, bool overwrite)
		{
			if (!AssetDatabase.IsValidFolder(folderPath))
			{
				AssetDatabase.CreateFolder(Constants.SHADERS_FOLDER_PATH, SHADER_VARIANTS_FOLDER_NAME);
			}

			string txtShaderVariant = File.ReadAllText(TEMPLATE_PATH);

			txtShaderVariant = ConfigureVariantName(txtShaderVariant, effectsProfile.profileName);
			txtShaderVariant = ConfigureShaderProperties(txtShaderVariant);

			ShaderPassCollection shaderPassCollection = GlobalConfiguration.instance.shaderPassCollection;

			string urpPassesEntry = GetShaderPassesEntry(effectsProfile, shaderPassCollection, RenderPipelineEnum.URP, folderPath);
			txtShaderVariant = txtShaderVariant.Replace(TAG_SHADER_PASSES_URP, urpPassesEntry);

			string birpPassesEntry = GetShaderPassesEntry(effectsProfile, shaderPassCollection, RenderPipelineEnum.BIRP, folderPath);
			txtShaderVariant = txtShaderVariant.Replace(TAG_SHADER_PASSES_BIRP, birpPassesEntry);

			
			string filePath;
			if (overwrite)
			{
				Shader shader = effectsProfile.FindShader();
				filePath = AssetDatabase.GetAssetPath(shader);
			}
			else
			{
				string fileName = string.Format(SHADER_VARIANT_FILE_NAME, effectsProfile.profileName);
				filePath = Path.Combine(folderPath, fileName);

				filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
			}

			effectsProfile.shaderGUID = AssetDatabase.AssetPathToGUID(filePath);

			SaveFile(filePath, txtShaderVariant);
			AssetDatabase.Refresh();

			Shader res = AssetDatabase.LoadAssetAtPath(filePath, typeof(Shader)) as Shader;
			EditorUtility.SetDirty(effectsProfileCollection);

			return res;
		}

		private static void SaveFile(string filePath, string txtFile)
		{
			File.WriteAllText(filePath, txtFile);
		}

		private static string ConfigureVariantName(string input, string variantName)
		{
			string res = input.Replace(TAG_VARIANT_NAME, variantName);
			return res;
		}

		private static string ConfigureShaderProperties(string input)
		{
			string res = input;

			string shaderPropertiesTxt = EditorUtils.ReadFileTextWithTabs(SHADER_PROPERTIES_PATH, 2);
			res = res.Replace(TAG_SHADER_PROPERTIES, shaderPropertiesTxt);

			return res;
		}

		private static string GetShaderPassesEntry(EffectsProfile effectsProfile, ShaderPassCollection shaderPassCollection, RenderPipelineEnum renderPipeline, string shaderFolderPath)
		{
			string shaderPassTemplateTxt = EditorUtils.ReadFileTextWithTabs(GetTemplatePathByRenderPipeline(renderPipeline), 2);

			List<ShaderPassConfig> shaderPasses = effectsProfile.GetShaderPasses(shaderPassCollection, renderPipeline);

			bool hasStencilBlock = false;
			for(int i = 0; i < shaderPasses.Count; i++)
			{
				if (shaderPasses[i].passType == AllIn13DPassType.OUTLINE)
				{
					hasStencilBlock = true;
					break;
				}
			}

			string res = string.Empty;
			for (int i = 0; i < shaderPasses.Count; i++)
			{
				string currentPass = shaderPassTemplateTxt;
				currentPass = ConfigureShaderPass(effectsProfile, currentPass, shaderPasses[i], renderPipeline, hasStencilBlock, shaderFolderPath);

				res += currentPass;
				res += "\n";
			}

			return res;
		}

		private static string ConfigureShaderPass(EffectsProfile effectsProfile, 
			string templatePass, ShaderPassConfig shaderPass, RenderPipelineEnum renderPipeline, bool hasStencilBlock, 
			string shaderFolderPath)
		{
			string res = templatePass;

			res = res.Replace(TAG_PASS_NAME, shaderPass.GetPassName(renderPipeline));

			res = res.Replace(TAG_LIGHT_MODE, shaderPass.GetLightModeShaderEntry(renderPipeline));

			res = res.Replace(TAG_BLEND_COMMAND, shaderPass.blendCommand.GetShaderEntry());
			
			res = res.Replace(TAG_CULL_COMMAND, shaderPass.cullCommand.GetShaderEntry());
			res = res.Replace(TAG_Z_WRITE_COMMAND, shaderPass.zWriteCommand.GetShaderEntry());
			res = res.Replace(TAG_Z_TEST_COMMAND, shaderPass.zTestCommand.GetShaderEntry());
			res = res.Replace(TAG_COLOR_MASK_COMMAND, shaderPass.colorMaskCommand.GetShaderEntry());

			if (hasStencilBlock)
			{
				res = res.Replace(TAG_STENCIL_BLOCK, shaderPass.GetStencilBlockShaderEntry(effectsProfile));
			}
			else
			{
				res = res.Replace(TAG_STENCIL_BLOCK, string.Empty);
			}

			res = res.Replace(TAG_FEATURES_URP_DEFINES, shaderPass.GetPipelineFeaturesDefinesShaderEntry(renderPipeline, shaderFolderPath));
			res = res.Replace(TAG_FEATURES_URP_LIBRARY, shaderPass.GetPipelineFeaturesLibraryShaderEntry(renderPipeline, shaderFolderPath));

			res = res.Replace(TAG_VERTEX_PROGRAM_NAME, shaderPass.vertexProgramName);
			res = res.Replace(TAG_FRAGMENT_PROGRAM_NAME, shaderPass.fragmentProgramName);

			res = res.Replace(TAG_INCLUDE_PIPELINE_HELPER, string.Format(INCLUDE_LINE_FORMAT, shaderPass.GetHelperLibraryPath(renderPipeline, shaderFolderPath)));

			string effectsLibraries = shaderPass.GetEffectsLibrariesShaderEntry(effectsProfile, shaderFolderPath);
			res = res.Replace(TAG_INCLUDE_EFFECT_LIBRARIES, effectsLibraries);

			res = res.Replace(TAG_INCLUDE_PASS, string.Format(INCLUDE_LINE_FORMAT, shaderPass.GetPassFilePath(shaderFolderPath)));

			List<EffectsProfileEntry> enabledEntries = effectsProfile.GetEnabledEntriesFlatList();
			string entryFeaturesTxt = ShaderPassConfig.GetAllIn1FeaturesEntries(enabledEntries);
			
			res = res.Replace(TAG_ALL_IN_1_FEATURES, entryFeaturesTxt);

			res = res.Replace(TAG_PIPELINE_PASS_SYMBOL, shaderPass.GetPipelinePassSymbolShaderEntry(renderPipeline));
			res = res.Replace(TAG_THIS_PASS_SYMBOL, shaderPass.GetPassSymbolShaderEntry());

			res = res.Replace(TAG_EXTRA_PRAGMA_LINES, shaderPass.GetExtraPragmaLines(renderPipeline));

			res = res.Replace(TAG_INCLUDE_ALL_IN_SHADER_FEATURES, shaderPass.GetShaderFeaturesLibraryShaderEntry(shaderFolderPath));
			res = res.Replace(TAG_INCLUDE_ALL_IN_1_SHADER_LIGHT, shaderPass.GetLightLibraryShaderEntry(shaderFolderPath));
			res = res.Replace(TAG_INCLUDE_CORE_LIBRARY, shaderPass.GetCoreLibraryShaderEntry(shaderFolderPath));
			res = res.Replace(TAG_INCLUDE_COMMON_STRUCTS, shaderPass.GetCommonStructsShaderEntry(shaderFolderPath));
			res = res.Replace(TAG_INCLUDE_COMMON_FUNCTIONS, shaderPass.GetCommonFunctionsShaderEntry(shaderFolderPath));

			return res;
		}

		private static string GetTemplatePathByRenderPipeline(RenderPipelineEnum renderPipeline)
		{
			string res = string.Empty;
			switch (renderPipeline)
			{
				case RenderPipelineEnum.BIRP:
					res = SHADER_PASS_BIRP_TAMPLATE_PATH;
					break;

				case RenderPipelineEnum.URP:
					res = SHADER_PASS_URP_TAMPLATE_PATH;
					break;
			}

			return res;
		}

		public static string GetShaderVariantsFolderPath()
		{
			string res = Path.Combine(Constants.SHADERS_FOLDER_PATH, SHADER_VARIANTS_FOLDER_NAME);
			return res;
		}
	}
}