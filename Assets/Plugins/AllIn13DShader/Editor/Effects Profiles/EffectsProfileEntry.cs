using System.Collections.Generic;
using UnityEditor;

namespace AllIn13DShader
{
	[System.Serializable]
	public class EffectsProfileEntry
	{	
		public string effectID;
		public string displayName;
		public bool isEnabled;
		public int kwEnabledIndex;

		public SubkeywordEntryEnum[] subkeywordEntriesEnum;
		public SubkeywordEntryToggle[] subkeywordEntriesToggle;

		private AllIn13DEffectConfig effectConfig;

		public bool IsToggleEffect
		{
			get
			{
				bool res = effectConfig.keywords.Count == 1;
				return res;
			}
		}

		public bool IsEnumEffect
		{
			get
			{
				bool res = effectConfig.keywords.Count > 1;
				return res;
			}
		}

		public string[] KeywordsDisplayNames
		{
			get
			{
				string[] res = effectConfig.keywordsDisplayNames;
				return res;
			}
		}

		public List<EffectKeywordData> ParentKeywords
		{
			get
			{
				return effectConfig.keywords;
			}
		}

		public List<EffectProperty> EffectProperties
		{
			get
			{
				return effectConfig.effectProperties;
			}
		}

		public EffectsProfileEntry(string effectID, string displayName, bool isEnabled)
		{
			this.effectID = effectID;
			this.displayName = displayName;
			this.isEnabled = isEnabled;

			subkeywordEntriesEnum = new SubkeywordEntryEnum[0];
			subkeywordEntriesToggle = new SubkeywordEntryToggle[0];
		}

		public EffectsProfileEntry(EffectsProfileEntry copyFrom)
		{
			this.effectConfig = copyFrom.effectConfig;

			this.effectID = copyFrom.effectID;
			this.displayName = copyFrom.displayName;
			this.isEnabled = copyFrom.isEnabled;
			this.kwEnabledIndex = copyFrom.kwEnabledIndex;

			this.subkeywordEntriesEnum = new SubkeywordEntryEnum[copyFrom.subkeywordEntriesEnum.Length];
			for(int i = 0; i < copyFrom.subkeywordEntriesEnum.Length; i++)
			{
				this.subkeywordEntriesEnum[i] = new SubkeywordEntryEnum(copyFrom.subkeywordEntriesEnum[i]);
			}

			this.subkeywordEntriesToggle = new SubkeywordEntryToggle[copyFrom.subkeywordEntriesToggle.Length];
			for (int i = 0; i < copyFrom.subkeywordEntriesToggle.Length; i++)
			{
				this.subkeywordEntriesToggle[i] = new SubkeywordEntryToggle(copyFrom.subkeywordEntriesToggle[i]);
			}
		}

		public void AddSubkeywordEntries(EffectProperty effectProperty)
		{
			if (effectProperty.IsEnumProperty())
			{
				SubkeywordEntryEnum subkeywordEntryEnum = new SubkeywordEntryEnum(0, effectProperty.propertyKeywords, effectProperty.fullKeywordNames, effectProperty.propertyName);
				ArrayUtility.Add(ref subkeywordEntriesEnum, subkeywordEntryEnum);
			}
			else if (effectProperty.IsToggleProperty())
			{
				SubkeywordEntryToggle subkeywordEntryToggle = new SubkeywordEntryToggle(false, effectProperty.fullKeywordNames[0], effectProperty.displayName);
				ArrayUtility.Add(ref subkeywordEntriesToggle, subkeywordEntryToggle);
			}
		}

		public bool HasSubkeywords()
		{
			bool res = subkeywordEntriesEnum.Length != 0 || subkeywordEntriesToggle.Length != 0;
			return res;
		}

		public void BindEffectConfig(AllIn13DEffectConfig effectConfig)
		{
			this.effectConfig = effectConfig;
		}

		public List<string> GetParentKeywordsEnabled()
		{
			List<string> res = new List<string>();

			if (IsToggleEffect)
			{
				if (isEnabled)
				{
					res.Add(effectConfig.keywords[0].keyword);
				}
			}
			else if(IsEnumEffect)
			{
				res.Add(effectConfig.keywords[kwEnabledIndex].keyword);
			}

			return res;
		}

		public AllIn13DPassType[] GetExtraPasses()
		{
			return effectConfig.extraPasses;
		}

		public bool IsKeywordEnabled(string keyword)
		{
			bool res = false;

			if (isEnabled)
			{
				if (IsToggleEffect)
				{
					res = true;
				}
				else if (IsEnumEffect)
				{
					res = effectConfig.keywords[kwEnabledIndex].keyword == keyword;
				}
			}
			return res;
		}

		public void CollectKeywords(List<string> res)
		{
			res.Add(effectConfig.keywords[kwEnabledIndex].keyword);

			for(int i = 0; i < subkeywordEntriesEnum.Length; i++)
			{
				res.Add(subkeywordEntriesEnum[i].GetKeywordEnabled());
			}

			for(int i = 0; i < subkeywordEntriesToggle.Length; i++)
			{
				if (subkeywordEntriesToggle[i].isEnabled)
				{
					res.Add(subkeywordEntriesToggle[i].keyword);
				}
			}
		}

		public int GetDisplayIndex()
		{
			return effectConfig.displayIndex;
		}

		public void Disable()
		{
			isEnabled = false;
			DisableSubEntries();
		}

		public void DisableSubEntries()
		{
			for(int i = 0; i < subkeywordEntriesEnum.Length; i++)
			{
				subkeywordEntriesEnum[i].kwIndexEnabled = 0;
			}

			for (int i = 0; i < subkeywordEntriesToggle.Length; i++)
			{
				subkeywordEntriesToggle[i].isEnabled = false;
			}
		}

		public SubkeywordEntryEnum FindEntryEnumByPropertyName(string propertyName)
		{
			SubkeywordEntryEnum res = null;

			for(int i = 0; i < subkeywordEntriesEnum.Length; i++)
			{
				if (subkeywordEntriesEnum[i].propertyName == propertyName)
				{
					res = subkeywordEntriesEnum[i];
					break;
				}
			}

			return res;
		}
	}
}