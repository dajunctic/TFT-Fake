using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static AllIn13DShader.EffectsExtraData;

namespace AllIn13DShader
{
	[System.Serializable]
	public class AllIn13DEffectConfig
	{
		public string effectName;
		public string displayName;
		public string group;
		
		public int keywordPropertyIndex;
		public string keywordPropertyName;

		public string effectDrawerID;
		public string dependentOnEffect;
		public string incompatibleWithEffectID;
		public string docURL;
		public MessageByKeywords[] customMessages;

		//public List<string> keywords;
		public List<EffectKeywordData> keywords;
		public string[] keywordsDisplayNames;
		public List<EffectProperty> effectProperties;

		public string disabledKeyword;

		public int displayIndex;

		public EffectConfigType effectConfigType;
		public AllIn13DPassType[] extraPasses;

		public AllIn13DEffectConfig(
			string displayName, string propertyName, int propertyIndex, EffectConfigType effectConfigType,
			EffectAttributeData data, EffectsExtraData effectsExtraData, int displayIndex)
		{
			this.displayName = displayName;
			this.keywordPropertyName = propertyName;
			this.keywordPropertyIndex = propertyIndex;
			this.effectConfigType = effectConfigType;

			this.effectName = data.effectID;
			this.group = data.groupID;
			this.effectDrawerID = data.drawerID;
			this.dependentOnEffect = data.dependentEffectID;
			this.incompatibleWithEffectID = data.incompatibleWithEffectID;

			EffectsExtraData.ExtraData extraData = effectsExtraData.GetExtraDataByEffectID(effectName);
			if(extraData != null)
			{
				this.docURL = extraData.docURL;
				this.customMessages = extraData.customMessages;
			}

			this.keywords = new List<EffectKeywordData>();
			this.effectProperties = new List<EffectProperty>();
			this.keywordsDisplayNames = new string[0];
			this.extraPasses = new AllIn13DPassType[0];

			for(int i = 0; i < data.extraPasses.Length; i++)
			{
				AllIn13DPassType passType;
				Enum.TryParse(data.extraPasses[i], out passType);

				ArrayUtility.Add(ref extraPasses, passType);
			}

			this.displayIndex = displayIndex;
		}

		public void AddKeyword(EffectKeywordData kw)
		{
			keywords.Add(kw);
		}

		public void AddKeywords(EffectKeywordData[] kws)
		{
			keywords.AddRange(kws);
		}

		public void Setup()
		{
			for(int i = 0; i < keywords.Count; i++)
			{
				ArrayUtility.Add(ref keywordsDisplayNames, keywords[i].displayName);
			}
		}

		public EffectProperty FindEffectPropertyByIndex(int propertyIndex)
		{
			EffectProperty res = null;

			for (int i = 0; i < effectProperties.Count; i++)
			{
				if (effectProperties[i].propertyIndex == propertyIndex)
				{
					res = effectProperties[i];
					break;
				}
			}

			return res;
		}

		public EffectProperty FindEffectPropertyByName(string propertyName)
		{
			EffectProperty res = null;

			for(int i = 0; i < effectProperties.Count; i++)
			{
				if (effectProperties[i].propertyName == propertyName)
				{
					res = effectProperties[i];
					break;
				}
			}

			return res;
		}

		public EffectProperty CreateEffectProperty(int propertyIndex, string propertyName, string displayName, 
			ShaderPropertyType shaderPropertyType, ShaderPropertyFlags shaderPropertyFlags,
			EffectPropertyAttributeData data)
		{
			EffectProperty res = new EffectProperty(this, propertyIndex, propertyName, displayName, 
				data.keywordsOp, data.allowReset, shaderPropertyType, shaderPropertyFlags);
			effectProperties.Add(res);

			for (int i = 0; i < data.keywords.Count; i++)
			{
				res.AddKeyword(data.keywords[i]);
			}

			for(int i = 0; i < data.incompatibleWithKws.Count; i++)
			{
				res.AddIncompatibleKeyword(data.incompatibleWithKws[i]);
			}

			res.AddPropertyKeywords(data.propertyKeywords);

			return res;
		}

		public string GetCustomMessage(AbstractMaterialInfo[] targetMatInfos)
		{
			string res = string.Empty;
			if(targetMatInfos.Length == 1)
			{
				res = GetCustomMessage(targetMatInfos[0]);
			}
			return res;
		}

		public string GetCustomMessage(AbstractMaterialInfo targetMatInfo)
		{
			string res = string.Empty;

			if(customMessages != null && customMessages.Length > 0)
			{	
				for(int i = 0; i < customMessages.Length; i++)
				{
					MessageByKeywords customMessage = customMessages[i];

					if (customMessage.IsMessageEnabled(targetMatInfo))
					{
						res = customMessage.message;
						break;
					}
				}
			}

			return res;
		}

		public static bool IsEffectAvailable(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references)
		{
			bool res = true;
			bool isShaderVariant = references.IsShaderVariant();

			if (isShaderVariant)
			{
				for (int i = 0; i < references.targetMatInfos.Length; i++)
				{
					AbstractMaterialInfo matInfo = references.targetMatInfos[i];
					res = res && AllIn13DEffectConfig.IsEffectEnabled(effectConfig, matInfo);
				}
			}

			return res;
		}

		public static bool IsEffectEnabled(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references)
		{
			int selectedIndex = 0;
			return IsEffectEnabled(effectConfig, ref selectedIndex, references);
		}

		public static bool IsEffectEnabled(AllIn13DEffectConfig effectConfig, AbstractMaterialInfo targetMatInfo)
		{
			int selectedIndex = 0;
			return IsEffectEnabled(effectConfig, ref selectedIndex, targetMatInfo);
		}

		public static bool IsEffectEnabled(AllIn13DEffectConfig effectConfig, ref int selectedIndex, AllIn13DShaderInspectorReferences references)
		{
			bool res = true;
			for (int i = 0; i < references.targetMatInfos.Length; i++)
			{
				res = res && IsEffectEnabled(effectConfig, ref selectedIndex, references.targetMatInfos[i]);
			}

			return res;
		}

		public bool AreDependenciesMet(PropertiesConfig propertiesConfig, AbstractMaterialInfo targetMatInfo)
		{
			bool res = true;

			if (!string.IsNullOrEmpty(dependentOnEffect))
			{
				AllIn13DEffectConfig dependentEffect = propertiesConfig.FindEffectConfigByID(dependentOnEffect);
				res = res && IsEffectEnabled(dependentEffect, targetMatInfo);
			}

			return res;
		}

		public static bool IsEffectEnabled(AllIn13DEffectConfig effectConfig, ref int selectedIndex, AbstractMaterialInfo targetMatInfo)
		{
			selectedIndex = 0;

			bool res = false;

			if (effectConfig.keywords.Count == 1)
			{
				if (targetMatInfo.IsKeywordEnabled(effectConfig.keywords[0].keyword))
				{
					res = true;
					selectedIndex = 0;
				}
			}
			else
			{
				for (int i = 0; i < effectConfig.keywords.Count; i++)
				{
					string keywordToCheck = effectConfig.keywords[i].keyword;

					bool isNoneOption = effectConfig.keywordsDisplayNames[i] == Constants.DISABLED_ENUM_OPTION_STR;
					if (targetMatInfo.IsKeywordEnabled(keywordToCheck) && !isNoneOption)
					{
						res = true;
						selectedIndex = i;
						break;
					}
				}
			}


			return res;
		}

		public static bool IsEffectPropertyEnabled(EffectProperty effectProperty, ref int selectedIndex, string[] enabledKeywords)
		{
			bool res = false;

			if (effectProperty.fullKeywordNames.Length == 1)
			{
				if (ArrayUtility.Contains(enabledKeywords, effectProperty.fullKeywordNames[0]))
				{
					res = true;
					selectedIndex = 0;
				}
			}
			else
			{
				for (int i = 0; i < effectProperty.fullKeywordNames.Length; i++)
				{
					if (ArrayUtility.Contains(enabledKeywords, effectProperty.fullKeywordNames[i]))
					{
						res = true;
						selectedIndex = i;
						break;
					}
				}
			}

			return res;
		}

		public static bool IsEffectPropertyEnabled(EffectProperty effectProperty, ref int selectedIndex, AbstractMaterialInfo targetMatInfo)
		{
			bool res = false;

			if(effectProperty.fullKeywordNames.Length == 1)
			{
				if (targetMatInfo.IsKeywordEnabled(effectProperty.fullKeywordNames[0]))
				{
					res = true;
					selectedIndex = 0;
				}
			}
			else
			{
				for(int i = 0; i < effectProperty.fullKeywordNames.Length; i++)
				{
					if (targetMatInfo.IsKeywordEnabled(effectProperty.fullKeywordNames[i]))
					{
						res = true;
						selectedIndex = i;
						break;
					}
				}
			}

			return res;
		}

		public static void ResetProperty(MaterialProperty targetProperty, AllIn13DShaderInspectorReferences references, AbstractMaterialInfo targetMatInfo)
		{
			Shader shader = targetMatInfo.mat.shader; 

			if (references.materialWithDefaultValues == null)
			{
				references.materialWithDefaultValues = new Material(shader);
			}

			AllIn1ShaderPropertyType targetPropertyType = EditorUtils.GetShaderTypeByMaterialProperty(targetProperty);
			int propertyIndex = shader.FindPropertyIndex(targetProperty.name);
			if (targetPropertyType == AllIn1ShaderPropertyType.Float || targetPropertyType == AllIn1ShaderPropertyType.Range)
			{
				targetProperty.floatValue = references.materialWithDefaultValues.GetFloat(targetProperty.name);
			}
			else if (targetPropertyType == AllIn1ShaderPropertyType.Vector)
			{
				targetProperty.vectorValue = references.materialWithDefaultValues.GetVector(targetProperty.name);
			}
			else if (targetPropertyType == AllIn1ShaderPropertyType.Color)
			{
				targetProperty.colorValue = references.materialWithDefaultValues.GetColor(targetProperty.name);
			}
			else if (targetPropertyType == AllIn1ShaderPropertyType.Texture)
			{
				targetProperty.textureValue = references.materialWithDefaultValues.GetTexture(targetProperty.name);
			}
		}

		public static void EnableEffect(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references, AbstractMaterialInfo targetMatInfo)
		{
			for (int i = 0; i < effectConfig.keywords.Count; i++)
			{
				string kw = effectConfig.keywords[i].keyword;
				targetMatInfo.EnableKeyword(kw);
			}
		}

		public static void EnableEffectToggle(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references, AbstractMaterialInfo targetMatInfo)
		{
			targetMatInfo.EnableKeyword(effectConfig.keywords[0].keyword);
			references.matProperties[effectConfig.keywordPropertyIndex].floatValue = 1f;
		}

		public static void DisableEffectToggle(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references, AbstractMaterialInfo targetMatInfo)
		{
			targetMatInfo.DisableKeyword(effectConfig.keywords[0].keyword);
			references.matProperties[effectConfig.keywordPropertyIndex].floatValue = 0f;
		}

		public static void EnableEffectByIndex(AllIn13DEffectConfig effectConfig, int index, AllIn13DShaderInspectorReferences references, AbstractMaterialInfo targetMatInfo)
		{
			DisableEffect(effectConfig, targetMatInfo);
			string kwToEnable = effectConfig.keywords[index].keyword;

			targetMatInfo.EnableKeyword(kwToEnable);
		}

		public static void DisableEffect(AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references)
		{
			for(int i = 0; i < references.targetMatInfos.Length; i++)
			{
				DisableEffect(effectConfig, references.targetMatInfos[i]);
			}
		}

		public static void DisableEffect(AllIn13DEffectConfig effectConfig, AbstractMaterialInfo targetMatInfo)
		{
			for (int i = 0; i < effectConfig.keywords.Count; i++)
			{
				string kw = effectConfig.keywords[i].keyword;

				targetMatInfo.DisableKeyword(kw);
			}
		}

		public static bool ContainsKeyword(Material mat, string kw, LocalKeyword[] enabledKeywords)
		{
			bool res = false;
			for (int i = 0; i < enabledKeywords.Length; i++)
			{
				if (enabledKeywords[i].name == kw)
				{
					res = true;
					break;
				}
			}

			return res;
		}



		public int GetKeywordIndex(string keyword)
		{
			int res = -1;
			for (int i = 0; i < keywords.Count; i++)
			{
				if (keywords[i].keyword == keyword)
				{
					res = i;
					break;
				}
			}

			return res;
		}

		public string[] GetPropertyNames()
		{
			string[] res = new string[effectProperties.Count];

			for(int i = 0; i < effectProperties.Count; i++)
			{
				res[i] = $"{effectProperties[i].displayName} ({effectProperties[i].propertyName})";
 			}

			return res;
		}

		public bool ContainsKeywordProperties()
		{
			bool res = false;

			for(int i = 0; i < effectProperties.Count; i++)
			{
				if (effectProperties[i].IsPropertyWithKeywords())
				{
					res = true;
					break;
				}
			}

			return res;
		}

		public GUIContent CreateGUIContent(int globalEffectIndex, int keywordSelectedIndex)
		{
			string label = $"{globalEffectIndex}. {displayName}";
			string tooltip = keywords[keywordSelectedIndex].keyword + " (C#)";
			
			GUIContent res = new GUIContent(label, tooltip);

			return res;
		}

		public bool ContainsKeyword(string keyword)
		{
			bool res = false;

			for(int i = 0; i < keywords.Count; i++)
			{
				if (keywords[i].keyword == keyword)
				{
					res = true;
					break;
				}
			}

			return res;
		}

		public bool ContainsSomeKeywordFromList(string[] keywords)
		{
			bool res = false;

			for(int i = 0; i < keywords.Length; i++)
			{
				if (ContainsKeyword(keywords[i]))
				{
					res = true;
					break;
				}
			}

			return res;
		}
	}
}