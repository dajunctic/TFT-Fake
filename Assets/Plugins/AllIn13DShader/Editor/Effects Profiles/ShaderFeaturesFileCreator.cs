using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace AllIn13DShader
{
	public static class ShaderFeaturesFileCreator
	{
		private const string SHADER_FEATURE_FILE_ENTRY = @"#pragma shader_feature_local {0}";
		private static string TEMPLATE_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Editor/Effects Profiles/Templates/ShaderFeaturesTemplate.allin1template";
		private static string DST_PATH = GlobalConfiguration.GetRootPluginFolderPath() + "/Shaders/ShaderLibrary/AllIn13DShader_ShaderFeatures.hlsl";

		public static void CreateFile(EffectsProfile effectsProfile)
		{
			string txtGeneratedFile = File.ReadAllText(TEMPLATE_PATH);
			string txtContent = string.Empty; 
			for (int i = 0; i < effectsProfile.groups.Count; i++)
			{
				EffectsProfileGroup group = effectsProfile.groups[i];
				for (int j = 0; j < group.entries.Count; j++)
				{
					EffectsProfileEntry effectProfileEntry = group.entries[j];
					if (!effectProfileEntry.isEnabled) { continue; }
					
					txtContent = ProcessEffectConfig(txtContent, effectProfileEntry);
				}
			}

			txtGeneratedFile = txtGeneratedFile.Replace("<Content>", txtContent);
			txtGeneratedFile = txtGeneratedFile.Replace("<EffectsProfileID>", effectsProfile.id);

			File.WriteAllText(DST_PATH, txtGeneratedFile);

			AssetDatabase.Refresh();
		}

		private static string ProcessEffectConfig(string input, EffectsProfileEntry entry)
		{
			string res = input;

			List<EffectKeywordData> parentKeywords = entry.ParentKeywords;
			List<EffectProperty> effectProperties = entry.EffectProperties;

			for (int i = 0; i < parentKeywords.Count; i++)
			{
				string fileEntry = string.Format(SHADER_FEATURE_FILE_ENTRY, parentKeywords[i].keyword);
				res += fileEntry;
				res += Environment.NewLine;
			}

			for (int i = 0; i < effectProperties.Count; i++)
			{
				EffectProperty effectProperty = effectProperties[i];

				for (int j = 0; j < effectProperty.propertyKeywords.Length; j++)
				{
					if (effectProperty.IsEnumProperty())
					{
						res += string.Format(SHADER_FEATURE_FILE_ENTRY, effectProperty.fullKeywordNames[j]);
					}
					else
					{
						res += string.Format(SHADER_FEATURE_FILE_ENTRY, effectProperty.propertyKeywords[j]);
					}
						
					res += Environment.NewLine;
				}
			}

			return res;
		}
	}
}