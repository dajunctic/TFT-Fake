#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static AllIn1ShaderImporter;

[InitializeOnLoad]
public static class AllIn1ShaderImporter
{
	public enum UnityVersion
	{
		NONE = 0,
		UNITY_2019 = 1,
		UNITY_2020 = 2,
		UNITY_2021 = 3,
		UNITY_2022 = 4,
		UNITY_6 = 5,
	}

	public enum RenderPipeline
	{
		NONE = -1,
		BIRP = 0,
		URP = 1,
		HDRP = 2,
	}


	private const string SHADER_FOLDER_PATH = "../../Shaders/LitShaders";
	private const string FINAL_SHADERS_FOLDER_PATH = "../../Shaders";
	private const string SHADER_TEMPLATE_NAME = @"{0}_{1}.txt";

	private const string SPRITE_LIT_SHADER_NAME = "AllIn1SpriteShaderLit";
	private const string SPRITE_LIT_TRANSPARENT_SHADER_NAME = "AllIn1SpriteShaderLitTransparent";

	private const string PIPELINE_SUFFIX_URP_2019 = "BetterShader_URP2019";
	private const string PIPELINE_SUFFIX_HDRP_2019 = "BetterShader_HDRP2019";

	private const string PIPELINE_SUFFIX_URP_2020 = "BetterShader_URP2020";
	private const string PIPELINE_SUFFIX_HDRP_2020 = "BetterShader_HDRP2020";

	private const string PIPELINE_SUFFIX_URP_2021 = "BetterShader_URP2021";
	private const string PIPELINE_SUFFIX_HDRP_2021 = "BetterShader_HDRP2021";

	private const string PIPELINE_SUFFIX_URP_2022 = "BetterShader_URP2022";
	private const string PIPELINE_SUFFIX_HDRP_2022 = "BetterShader_HDRP2022";

	private const string PIPELINE_SUFFIX_URP_2023 = "BetterShader_URP2023";
	private const string PIPELINE_SUFFIX_HDRP_2023 = "BetterShader_HDRP2023";

	private const string PIPELINE_SUFFIX_STANDARD = "BetterShader_Standard";

	private const string LIT_SHADER_PIPELINE_KEY = "AllIn1SpriteShader_LitShader_RenderPipeline";
	private const string LIT_SHADER_UNITY_VERSION_KEY = "AllIn1SpriteShader_LitShader_UnityVersion";
	private const string LIT_SHADER_FIRST_TIME_PROJECT = "AllIn1SpriteShader_LitShader_FirstTimeProject";

	static AllIn1ShaderImporter()
	{
		EditorApplication.quitting += Quit;

		ConfigureShaders();
	}

	private static void Quit()
	{
		EditorPrefs.DeleteKey(LIT_SHADER_FIRST_TIME_PROJECT);
	}

	private static void ConfigureShaders()
	{
		RenderPipelineChecker.RefreshData();

		UnityVersion unityVersion = GetUnityVersion();
		RenderPipeline renderPipeline = GetRenderPipeline();

		RenderPipeline lastRenderPipeline = (RenderPipeline)EditorPrefs.GetInt(LIT_SHADER_PIPELINE_KEY, -1);
		UnityVersion lastUnityVersion = (UnityVersion)EditorPrefs.GetInt(LIT_SHADER_UNITY_VERSION_KEY, 0);
		int firstTimeProject = EditorPrefs.GetInt(LIT_SHADER_FIRST_TIME_PROJECT, -1);

		if (lastRenderPipeline != renderPipeline || lastUnityVersion != unityVersion || firstTimeProject != 1)
		{
			EditorPrefs.SetInt(LIT_SHADER_PIPELINE_KEY, (int)renderPipeline);
			EditorPrefs.SetInt(LIT_SHADER_UNITY_VERSION_KEY, (int)unityVersion);
			EditorPrefs.SetInt(LIT_SHADER_FIRST_TIME_PROJECT, 1);

			ConfigureShader(SPRITE_LIT_SHADER_NAME);
			ConfigureShader(SPRITE_LIT_TRANSPARENT_SHADER_NAME);
		}
	}

	private static void ConfigureShader(string shaderName)
	{
		string pipelineSufix = string.Empty;

		UnityVersion unityVersion = GetUnityVersion();
		RenderPipeline renderPipeline = GetRenderPipeline();

		if (renderPipeline == RenderPipeline.HDRP)
		{
			switch (unityVersion)
			{
				case UnityVersion.UNITY_2019:
					pipelineSufix = PIPELINE_SUFFIX_HDRP_2019;
					break;
				case UnityVersion.UNITY_2020:
					pipelineSufix = PIPELINE_SUFFIX_HDRP_2020;
					break;
				case UnityVersion.UNITY_2021:
					pipelineSufix = PIPELINE_SUFFIX_HDRP_2021;
					break;
				case UnityVersion.UNITY_2022:
					pipelineSufix = PIPELINE_SUFFIX_HDRP_2022;
					break;
				case UnityVersion.UNITY_6:
					pipelineSufix = PIPELINE_SUFFIX_HDRP_2023;
					break;

			}
		}
		else if (renderPipeline == RenderPipeline.URP)
		{
			switch (unityVersion)
			{
				case UnityVersion.UNITY_2019:
					pipelineSufix = PIPELINE_SUFFIX_URP_2019;
					break;
				case UnityVersion.UNITY_2020:
					pipelineSufix = PIPELINE_SUFFIX_URP_2020;
					break;
				case UnityVersion.UNITY_2021:
					pipelineSufix = PIPELINE_SUFFIX_URP_2021;
					break;
				case UnityVersion.UNITY_2022:
					pipelineSufix = PIPELINE_SUFFIX_URP_2022;
					break;
				case UnityVersion.UNITY_6:
					pipelineSufix = PIPELINE_SUFFIX_URP_2023;
					break;
			}
		}
		else
		{
			pipelineSufix = PIPELINE_SUFFIX_STANDARD;
		}

		string templateFileName = string.Format(SHADER_TEMPLATE_NAME, shaderName, pipelineSufix);

		string shaderTemplatePath = SHADER_FOLDER_PATH + "/" + templateFileName;
		string finalShaderPath = FINAL_SHADERS_FOLDER_PATH + "/" + $"{shaderName}.shader";

		try
		{
			var currentFileGUID = AssetDatabase.FindAssets($"t:Script {nameof(AllIn1ShaderImporter)}")[0];
			string currentFolder = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(currentFileGUID));

			string newShaderStr = File.ReadAllText(Path.Combine(currentFolder, shaderTemplatePath));
			newShaderStr = newShaderStr.Replace($"Shader \"AllIn1SpriteShader/{shaderName}_BetterShader\"", $"Shader \"AllIn1SpriteShader/{shaderName}\"");

			File.WriteAllText(Path.Combine(currentFolder, finalShaderPath), newShaderStr);

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		catch (Exception e)
		{
			Debug.LogError("Shader not found: " + e);
		}
	}

	private static UnityVersion GetUnityVersion()
	{
		UnityVersion res = UnityVersion.NONE;

		string unityVersion = Application.unityVersion;

		if (unityVersion.Contains("2019"))
		{
			res = UnityVersion.UNITY_2019;
		}
		else if (unityVersion.Contains("2020"))
		{
			res = UnityVersion.UNITY_2020;
		}
		else if (unityVersion.Contains("2021"))
		{
			res = UnityVersion.UNITY_2021;
		}
		else if (unityVersion.Contains("2022"))
		{
			res = UnityVersion.UNITY_2022;
		}
		else if (unityVersion.Contains("6000"))
		{
			res = UnityVersion.UNITY_6;
		}

		return res;
	}

	private static RenderPipeline GetRenderPipeline()
	{
		RenderPipeline res = RenderPipeline.BIRP;

		if (RenderPipelineChecker.IsURP)
		{
			res = RenderPipeline.URP;
		}
		else if (RenderPipelineChecker.IsHDRP)
		{
			res = RenderPipeline.HDRP;
		}

		return res;
	}

	public static void ForceReimport()
	{
		EditorPrefs.DeleteKey(LIT_SHADER_PIPELINE_KEY);
		EditorPrefs.DeleteKey(LIT_SHADER_UNITY_VERSION_KEY);
		EditorPrefs.DeleteKey(LIT_SHADER_FIRST_TIME_PROJECT);

		ConfigureShaders();
	}
}
#endif