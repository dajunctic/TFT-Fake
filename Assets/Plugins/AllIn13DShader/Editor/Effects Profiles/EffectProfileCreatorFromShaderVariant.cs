using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public static class EffectProfileCreatorFromShaderVariant
	{
		private const string REGEX_EFFECTS_ENABLED_IN_SHADER_VARIANT = @"\/\/<ALLIN1_EFFECTS>((?:\s+#define\s\w+)*\s+)\/\/<\/ALLIN1_EFFECTS>";

		public static EffectsProfile Create(Shader shaderVariant, string shaderVariantPath, PropertiesConfig propertiesConfig, EffectsProfileCollection effectsProfileCollection)
		{
			EffectsProfile res = effectsProfileCollection.CreateNewProfile(shaderVariant.name);
			res.InitFromOtherProfile(effectsProfileCollection.generalProfile);

			res.shaderGUID = AssetDatabase.AssetPathToGUID(shaderVariantPath);

			string shaderText = File.ReadAllText(shaderVariantPath);

			MatchCollection matchCollection = Regex.Matches(shaderText, REGEX_EFFECTS_ENABLED_IN_SHADER_VARIANT);


			List<string> enabledKeywordsList = new List<string>();
			if(matchCollection.Count > 0)
			{ 
				string matchText = matchCollection[0].Groups[1].Value;

				string[] linesSplitted = matchText.Split("\n");

				for (int i = 0; i < linesSplitted.Length; i++)
				{
					string lineProcessed = linesSplitted[i];
					lineProcessed = lineProcessed.Replace("#define", string.Empty);
					lineProcessed = lineProcessed.Trim();

					if (!string.IsNullOrEmpty(lineProcessed))
					{
						enabledKeywordsList.Add(lineProcessed);
					}
				}
			}

			string[] enabledKeywordsArray = enabledKeywordsList.ToArray();

			List<AllIn13DEffectConfig> allEffects = propertiesConfig.GetAllEffects();
			for(int i = 0; i < allEffects.Count; i++)
			{
				AllIn13DEffectConfig effect = allEffects[i];

				if (effect.ContainsSomeKeywordFromList(enabledKeywordsArray))
				{
					res.EnableEffect(effect, enabledKeywordsArray); 
				}
				else
				{
					res.DisableEffect(effect);
				}
			}

			return res;
		}
	}
}