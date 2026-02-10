using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace AllIn13DShader
{
	[System.Serializable]
	public class EffectProperty
	{
		public enum PropertyType
		{
			BASIC = 0,
			ENUM = 1,
			TOGGLE = 2
		}

		[SerializeReference] public AllIn13DEffectConfig parentEffect;

		public int propertyIndex;
		public string propertyName;
		public string displayName;
		
		public List<string> keywords;
		public List<string> incompatibleKeywords;
		public string[] propertyKeywords;
		public string[] fullKeywordNames;

		public KeywordsOp keywordsOp;
		public bool allowReset;

		public ShaderPropertyType shaderPropertyType;

		public bool isHDR;
		public bool hasTilingAndOffset;

		public EffectProperty(AllIn13DEffectConfig parentEffect, int propertyIndex, string propertyName, string displayName, 
			KeywordsOp keywordsOp, bool allowReset, ShaderPropertyType shaderPropertyType, ShaderPropertyFlags shaderPropertyFlags)
		{
			this.parentEffect = parentEffect;

			this.keywords = new List<string>();
			this.incompatibleKeywords = new List<string>();
			this.propertyKeywords = new string[0];
			this.fullKeywordNames = new string[0];

			this.propertyIndex = propertyIndex;
			this.propertyName = propertyName;
			this.displayName = displayName;

			this.keywordsOp = keywordsOp;
			this.allowReset = allowReset;

			this.shaderPropertyType = shaderPropertyType;

			this.isHDR = shaderPropertyFlags.HasFlag(ShaderPropertyFlags.HDR);
			this.hasTilingAndOffset = !shaderPropertyFlags.HasFlag(ShaderPropertyFlags.NoScaleOffset) && shaderPropertyType == ShaderPropertyType.Texture;
		}

		public void AddKeyword(string keyword)
		{
			this.keywords.Add(keyword);
		}

		public void AddIncompatibleKeyword(string keyword)
		{
			this.incompatibleKeywords.Add(keyword);
		}

		public void AddPropertyKeywords(List<string> propertyKeywordsToAdd)
		{
			bool isEnum = propertyKeywordsToAdd.Count > 1;

			for (int i = 0; i < propertyKeywordsToAdd.Count; i++)
			{
				string fullKeywordName;
				if (isEnum)
				{
					fullKeywordName = this.propertyName.ToUpperInvariant() + "_" + propertyKeywordsToAdd[i].ToUpperInvariant();
				}
				else
				{
					fullKeywordName = propertyKeywordsToAdd[i];
				}

				ArrayUtility.Add(ref this.propertyKeywords, propertyKeywordsToAdd[i]);
				ArrayUtility.Add(ref this.fullKeywordNames, fullKeywordName);
			}
		}

		public bool IsPropertyWithKeywords()
		{
			bool res = propertyKeywords.Length > 0;
			return res;
		}

		public bool IsToggleProperty()
		{
			bool res = propertyKeywords.Length == 1;
			return res;
		}

		public bool IsEnumProperty()
		{
			bool res = propertyKeywords.Length >= 2;
			return res;
		}

		public PropertyType GetPropertyType()
		{
			PropertyType res = PropertyType.BASIC;
			if (IsEnumProperty())
			{
				res = PropertyType.ENUM;
			}
			else if (IsToggleProperty())
			{
				res = PropertyType.TOGGLE;
			}

			return res;
		}

		public int GetEnabledKeywordIndex(string[] enabledKeywords)
		{
			int res = -1;

			if (IsEnumProperty() || IsToggleProperty())
			{
				for (int i = 0; i < enabledKeywords.Length; i++)
				{
					for (int j = 0; j < fullKeywordNames.Length; j++)
					{
						if (fullKeywordNames[j] == enabledKeywords[i])
						{
							res = j;
							break;
						}
					}

					if (res >= 0)
					{
						break;
					}
				}
			}

			return res;
		}
	}
}