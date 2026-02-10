using System.IO;

namespace AllIn13DShader
{
	public static class Constants
	{
		public static string VERSION = "2.21";
		
		public const string EFFECT_ATTRIBUTE_PREFIX = "Effect(";
		public const string EFFECT_PROPERTY_ATTRIBUTE_PREFIX = "EffectProperty(";
		public const string HEADER_PREFIX = "Header(";
		public const string SINGLE_PROPERTY_ATTRIBUTE = "SingleProperty";
		public const string ADVANCED_PROPERTY_ATTRIBUTE = "AdvancedProperty";


		public const string SHADER_ROOT = "AllIn13DShader";

		public static string[] SHADERS_NAMES = new string[]
		{
			"AllIn13DShader",
			"AllIn13DShader_NoShadowCaster",
			"AllIn13DShaderOutline_NoShadowCaster",
			"AllIn13DShaderOutline",
		};

		public static string MAIN_SHADER_NAME
		{
			get
			{
				return SHADERS_NAMES[0];
			}
		}

		public static string SHADER_FULL_NAME_ALLIN13D
		{
			get
			{
				return SHADER_ROOT + "/" + MAIN_SHADER_NAME;
			}
		}

		public static string SHADER_FULL_NAME_ALLIN13D_OUTLINE
		{
			get
			{
				return SHADER_ROOT + "/" + SHADERS_NAMES[3];
			}
		}

		public static string SHADER_NAME_ALLIN13D_OUTLINE = "AllIn13DShaderOutline";



		//=========== Paths ===========
		public static string SHADERS_FOLDER_PATH = Path.Combine(GlobalConfiguration.instance.RootPluginPath, "Shaders");/*"Assets/AllIn13DShader/Shaders";*/
		public static string SHADERS_GENERIC_FOLDER_PATH = Path.Combine(SHADERS_FOLDER_PATH, "Generic Shaders");
		public static string SHADER_LIBRARY_FOLDER_PATH  = Path.Combine(SHADERS_FOLDER_PATH, "ShaderLibrary");
		//public static string SHADERS_PROPERTIES_FOLDER_PATH = /*"Assets/AllIn13DShader/Editor"*/Path.Combine(GlobalConfiguration.instance.RootPluginPath, "Editor");
		public static string TEMPLATES_FOLDER = Path.Combine(GlobalConfiguration.instance.RootPluginPath, "Editor/Templates");
		public const string STANDARD_EXAMPLES_MATERIALS_LOCAL_PATH = "Demo/Materials/StandardExamples";
		
		/* Shader Passes Paths */
		public const string MAIN_PASS_PATH			= "Shaders/ShaderLibrary/AllIn13DShader_BasePass.hlsl";
		public const string LIGHT_ADD_PASS_PATH		= "Shaders/ShaderLibrary/AllIn13DShaderLightAddPass.hlsl";
		public const string SHADOW_CASTER_PASS_PATH = "Shaders/ShaderLibrary/AllIn13DShader_ShadowCasterPass.hlsl";
		public const string DEPTH_ONLY_PASS_PATH	= "Shaders/ShaderLibrary/AllIn13DShader_URP_DepthOnlyPass.hlsl";
		public const string DEPTH_NORMALS_PASS_PATH = "Shaders/ShaderLibrary/AllIn13DShader_URP_DepthNormalsPass.hlsl";
		public const string META_PASS_PATH			= "Shaders/ShaderLibrary/AllIn13DShader_URP_MetaPass.hlsl";
		public const string OUTLINE_PASS_PATH		= "Shaders/ShaderLibrary/AllIn13DShader_OutlinePass.hlsl";

		public const string BIRP_HELPER_PATH		= "Shaders/ShaderLibrary/AllIn13DShaderHelper_BIRP.hlsl";
		public const string URP_HELPER_PATH			= "Shaders/ShaderLibrary/AllIn13DShaderHelper_URP.hlsl";

		//====================================

		public const string KEYWORD_NONE = "NONE";

		//Special Properties
		public const string MATPROPERTY_RENDERING_MODE = "_RenderPreset";
		public const string MATPROPERTY_BLEND_SRC = "_BlendSrc";
		public const string MATPROPERTY_BLEND_DST = "_BlendDst";
		public const string KEYWORD_ALLIN13D_SURFACE_TRANSPARENT = "_ALLIN13D_SURFACE_TRANSPARENT";

		//Drawers IDs
		public const string GENERAL_EFFECT_DRAWER_ID = "GENERAL_EFFECT_DRAWER";
		public const string TRIPLANAR_EFFECT_DRAWER_ID = "TRIPLANAR_EFFECT_DRAWER";
		public const string COLOR_RAMP_EFFECT_DRAWER_ID = "COLOR_RAMP_EFFECT_DRAWER";
		public const string OUTLINE_DRAWER_ID = "OUTLINE_DRAWER_ID";
		public const string TEXTURE_BLENDING_EFFECT_DRAWER_ID = "TEXTURE_BLENDING_EFFECT_DRAWER";
		public const string NORMAL_MAP_EFFECT_DRAWER_ID = "NORMAL_MAP_EFFECT_DRAWER";

		//Regex
		public const string REGEX_EFFECT = @"\(EffectID#([\w\s]+),.*GroupID#([\w\s]+)(?:,.*AllowReset#([\w\s]+))?(?:,.*DependentOn#([\w\s]+))?(?:,.*IncompatibleWith#([\w\s]+))?(?:,.*Doc#([\w\s\\\.]+))?(?:,.*CustomDrawer#([\w\s]+))?(?:,.*ExtraPasses#[\s]*\(([\w\s,]+)\))?\)";
		public const string REGEX_EFFECT_PROPERTY = @"EffectProperty\((.*)\)";
		//public const string REGEX_EFFECT_PROOPERTY_COMPLETE = @"EffectProperty\(ParentEffect# ([\w\s]+), Keywords\((.*)\)\)";
		public const string REGEX_EFFECT_PROOPERTY_COMPLETE = @"EffectProperty\(ParentEffect# ([\w\s]+)(?:,.*KeywordsOp# ([\w]+))?(?:,.*IncompatibleWithKws\(([\w]+)\))?, Keywords\((.*)\), AllowReset# ([\w]+)\)";
		public const string REGEX_PARENT_EFFECT_KEYWORDS = @".*\((.*)\)";
		public const string REGEX_KEYWORDS_ENUM = @"KeywordEnum\((.*)\)";
		public const string REGEX_TOGGLE = @"Toggle\((.*)\)";

		//Editor Prefs Keys
		public const string LAST_TIME_SHADER_PROPERTIES_REBUILT_KEY = "AllIn13DShader_RebuiltTime";
		public const string LAST_RENDER_PIPELINE_CHECKED_KEY = "AllIn13DShader_LastRenderPipeline";

		//Strings
		public const string HDR_STR = "HDR";
		public const string KEYWORD_ENUM_STR = "KeywordEnum";
		public const string ON_STR = "On";
		public const string OFF_STR = "Off";
		public const string DISABLED_ENUM_OPTION_STR = "None";

		//Default Names
		public const string DEFAULT_NAME_EFFECTS_PROFILE = "EffectsProfile";
	}
}