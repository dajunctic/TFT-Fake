using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public static class URPSettingsController
	{
		public static void DisableFeature(string featureToDisable)
		{
			// Find the shader features file
			string[] guids = AssetDatabase.FindAssets("AllIn13DShader_FeaturesURP_Defines");
			if (guids.Length == 0)
			{
				Debug.LogWarning("AllIn13DShader_FeaturesURP file not found");
				return;
			}

			string shaderFeaturesFilePath = AssetDatabase.GUIDToAssetPath(guids[0]);
			if (string.IsNullOrEmpty(shaderFeaturesFilePath))
			{
				Debug.LogWarning("Could not get path for AllIn13DShader_FeaturesURP file");
				return;
			}

			// Read the file content
			string fileContent = File.ReadAllText(shaderFeaturesFilePath);
			string[] lines = fileContent.Split("\n");

			string correctedFile = string.Empty;
			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];
				if (line.StartsWith($"#define {featureToDisable}"))
				{
					line = "//" + line;
				}

				correctedFile += line;
			}

			File.WriteAllText(shaderFeaturesFilePath, correctedFile);
		}
	}
}