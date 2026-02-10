using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	[System.Serializable]
	public class EffectsProfile
	{
		public string id;
		public string profileName;

		public string shaderGUID;
		public List<EffectsProfileGroup> groups;

		public EffectsProfile(string id, string profileName)
		{
			this.id = id;

			this.profileName = profileName;
			FormatProfileName();

			this.shaderGUID = string.Empty;
			this.groups = new List<EffectsProfileGroup>();
		}

		private void FormatProfileName()
		{
			this.profileName = this.profileName.Replace("\\", "_");
			this.profileName = this.profileName.Replace("/", "_");
		}

		public void InitFromOtherProfile(EffectsProfile copyFrom)
		{
			this.groups = new List<EffectsProfileGroup>(copyFrom.groups.Count);
			for (int i = 0; i < copyFrom.groups.Count; i++)
			{
				this.groups.Add(new EffectsProfileGroup(copyFrom.groups[i])); 
			}
		}

		public bool IsEffectEnabled(string effectID)
		{
			EffectsProfileEntry entry = GetEntryByEffectID(effectID);

			bool res = entry.isEnabled;
			return res;
		}

		public bool IsKeywordEnabled(string effectID, string keyword)
		{
			EffectsProfileEntry entry = GetEntryByEffectID(effectID);

			bool res = entry.IsKeywordEnabled(keyword);
			return res;
		}

		public EffectsProfileEntry GetEntryByEffectID(string effectID)
		{
			EffectsProfileEntry res = null;

			for(int i = 0; i < groups.Count; i++)
			{
				EffectsProfileGroup group = groups[i];
				for (int j = 0; j < group.entries.Count; j++)
				{
					if (group.entries[j].effectID == effectID)
					{
						res = group.entries[j];
						break;
					}
				}
			}

			return res;
		}

		public void CreateDefault(PropertiesConfigCollection propertiesConfigCollection)
		{
			groups = new List<EffectsProfileGroup>();

			PropertiesConfig propertiesConfig = propertiesConfigCollection.FindPropertiesConfigByShader(Shader.Find("AllIn13DShader/AllIn13DShader"));

			for (int i = 0; i < propertiesConfig.effectsGroups.Length; i++)
			{
				EffectGroup effectGroup = propertiesConfig.effectsGroups[i];

				EffectsProfileGroup effectsProfileGroup = new EffectsProfileGroup(effectGroup);
				groups.Add(effectsProfileGroup);
			}
		}

		public List<EffectsProfileEntry> GetEnabledEntriesFlatList()
		{
			List<EffectsProfileEntry> res = new List<EffectsProfileEntry>();

			for(int i = 0; i < groups.Count; i++)
			{
				for(int j = 0; j < groups[i].entries.Count; j++)
				{
					if (groups[i].entries[j].isEnabled)
					{
						res.Add(groups[i].entries[j]);
					}
				}
			}

			return res;
		}

		public List<ShaderPassConfig> GetShaderPasses(ShaderPassCollection shaderPassCollection, RenderPipelineEnum renderPipeline)
		{
			List<EffectsProfileEntry> enabledEntries = GetEnabledEntriesFlatList();
			List<AllIn13DPassType> passesTypeList = new List<AllIn13DPassType>();

			List<ShaderPassConfig> res = new List<ShaderPassConfig>();
			res.Add(shaderPassCollection.GetShaderPassConfig(AllIn13DPassType.MAIN));

			for (int i = 0; i < enabledEntries.Count; i++)
			{
				AllIn13DPassType[] extraPasses = enabledEntries[i].GetExtraPasses();
				for (int j = 0; j < extraPasses.Length; j++)
				{
					if (extraPasses[j] == AllIn13DPassType.FORWARD_ADD && renderPipeline == RenderPipelineEnum.URP) { continue; }

					ShaderPassConfig shaderPassConfig = shaderPassCollection.GetShaderPassConfig(extraPasses[j]);
					if (!res.Contains(shaderPassConfig))
					{
						res.Add(shaderPassConfig);
					}
				}
			}

			if (renderPipeline == RenderPipelineEnum.URP)
			{
				res.Add(shaderPassCollection.GetShaderPassConfig(AllIn13DPassType.DEPTH_NORMALS));
				res.Add(shaderPassCollection.GetShaderPassConfig(AllIn13DPassType.DEPTH_ONLY));
				res.Add(shaderPassCollection.GetShaderPassConfig(AllIn13DPassType.META));
			}

			return res;
		}

		public void SetEnableAllEffects(bool enabled)
		{
			for (int i = 0; i < groups.Count; i++)
			{
				EffectsProfileGroup group = groups[i];
				for (int j = 0; j < group.entries.Count; j++)
				{
					EffectsProfileEntry entry = group.entries[j];
					entry.isEnabled = enabled;
				}
			}
		}

		public void EnableAllEffects()
		{
			SetEnableAllEffects(true);
		}

		public void DisableAllEffects()
		{
			SetEnableAllEffects(false);
		}

		public List<string> GetKeywordsEnabled()
		{
			List<string> res = new List<string>();

			for(int i = 0; i < groups.Count; i++)
			{
				EffectsProfileGroup group = groups[i];
				group.CollectKeywords(res);
			}

			return res;
		}

		public void EnableEffect(AllIn13DEffectConfig effectConfig, string[] enabledKeywords)
		{
			EffectsProfileEntry effectProfileEntry = GetEntryByEffectID(effectConfig.effectName);

			effectProfileEntry.isEnabled = true;

			if (effectConfig.keywords.Count > 1)
			{
				int enabledKeywordIndex = -1;
				for (int i = 0; i < enabledKeywords.Length; i++)
				{
					enabledKeywordIndex = effectConfig.GetKeywordIndex(enabledKeywords[i]);
					if (enabledKeywordIndex >= 0)
					{
						break;
					}
				}

				effectProfileEntry.kwEnabledIndex = enabledKeywordIndex;
			}

			for (int i = 0; i < effectConfig.effectProperties.Count; i++)
			{
				EffectProperty effectProperty = effectConfig.effectProperties[i];

				if (effectProperty.IsPropertyWithKeywords())
				{
					int enabledIndex = 0;
					bool isPropertyEnabled = AllIn13DEffectConfig.IsEffectPropertyEnabled(effectProperty, ref enabledIndex, enabledKeywords);

					if (isPropertyEnabled)
					{
						if (effectProperty.IsEnumProperty())
						{
							SubkeywordEntryEnum subkeywordEntryEnum = effectProfileEntry.FindEntryEnumByPropertyName(effectProperty.propertyName);
							subkeywordEntryEnum.kwIndexEnabled = effectProperty.GetEnabledKeywordIndex(enabledKeywords);
						}
						else if (effectProperty.IsToggleProperty())
						{
							effectProfileEntry.subkeywordEntriesToggle[0].isEnabled = true;
						}
					}
					else
					{
						effectProfileEntry.DisableSubEntries();
					}
				}
			}
		}

		public void EnableEffect(AllIn13DEffectConfig effectConfig, AbstractMaterialInfo matInfo)
		{
			EffectsProfileEntry effectProfileEntry = GetEntryByEffectID(effectConfig.effectName);

			effectProfileEntry.isEnabled = true;

			if(effectConfig.keywords.Count > 1)
			{
				effectProfileEntry.kwEnabledIndex = matInfo.GetEnabledKeywordIndexByEffect(effectConfig);
			}

			for(int i = 0; i < effectConfig.effectProperties.Count; i++)
			{
				EffectProperty effectProperty = effectConfig.effectProperties[i];

				if (effectProperty.IsPropertyWithKeywords())
				{
					int enabledIndex = 0;
					bool isPropertyEnabled = AllIn13DEffectConfig.IsEffectPropertyEnabled(effectProperty, ref enabledIndex, matInfo);

					if (isPropertyEnabled)
					{
						if (effectProperty.IsEnumProperty())
						{
							SubkeywordEntryEnum entryEnum = effectProfileEntry.FindEntryEnumByPropertyName(effectProperty.propertyName);
							entryEnum.kwIndexEnabled = matInfo.GetEnabledKeywordIndexByEffectProperty(effectProperty);
						}
						else if (effectProperty.IsToggleProperty())
						{
							effectProfileEntry.subkeywordEntriesToggle[0].isEnabled = true;
						}
					}
					else
					{
						effectProfileEntry.DisableSubEntries();
					}
				}
			}
		}

		public Shader FindShader()
		{
			string shaderPath = AssetDatabase.GUIDToAssetPath(shaderGUID);
			Shader res = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);

			return res;
		}

		public void DisableEffect(AllIn13DEffectConfig effectConfig)
		{
			EffectsProfileEntry effectProfileEntry = GetEntryByEffectID(effectConfig.effectName);
			effectProfileEntry.Disable();
		}

		public void BindEffectConfigs(PropertiesConfig propertiesConfig)
		{
			for(int i = 0; i < groups.Count; i++)
			{
				for(int j = 0; j < groups[i].entries.Count; j++)
				{
					EffectsProfileEntry effectProfileEntry = groups[i].entries[j];
					AllIn13DEffectConfig effectConfig = propertiesConfig.FindEffectConfigByID(effectProfileEntry.effectID);

					effectProfileEntry.BindEffectConfig(effectConfig);
				}
			}
		}

		public List<EffectsProfileGroup> GetEnabledEffectsGroups()
		{
			List<EffectsProfileGroup> res = new List<EffectsProfileGroup>();

			for(int i = 0; i < groups.Count; i++)
			{
				if (groups[i].HasEffectsEnabled())
				{
					res.Add(groups[i]);
				}
			}

			return res;
		}
	}
}