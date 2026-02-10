using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public static class EffectPropertyDrawer
	{
		public static void DrawMainProperty(int globalEffectIndex, 
			AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references, 
			bool isEffectAvailable, bool isEffectAvailableOnGlobalConfig)
		{
			EditorGUILayout.BeginHorizontal();

			bool isShaderVariant = references.IsShaderVariant();
			string label = $"{globalEffectIndex}. {effectConfig.displayName}";

			if (isEffectAvailableOnGlobalConfig || isShaderVariant)
			{
				switch (effectConfig.effectConfigType)
				{
					case EffectConfigType.EFFECT_TOGGLE:
						DrawMainPropertyToggle(label, effectConfig, references, isShaderVariant);
						break;
					case EffectConfigType.EFFECT_ENUM:
						DrawMainPropertyEnum(label, effectConfig, references, isEffectAvailable, isShaderVariant);
						break;
				}
			}
			else
			{
				int keywordSelectedIndex = 0;
				references.IsEffectEnabled(effectConfig, ref keywordSelectedIndex);

				GUIContent guiContent = effectConfig.CreateGUIContent(globalEffectIndex, keywordSelectedIndex);
				EditorGUILayout.LabelField(guiContent, GUILayout.Width(180f));
				EditorGUILayout.LabelField("Disabled in Active Effects List");
			}
			
			EditorGUILayout.EndHorizontal();
		}

		public static void DrawMainPropertyToggle(string label, 
			AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references,
			bool isShaderVariant)
		{
			bool isEffectEnabled = AllIn13DEffectConfig.IsEffectEnabled(effectConfig, references);
			
			EditorGUI.BeginChangeCheck();

			string tooltip = effectConfig.keywords[0].keyword + " (C#)";
			GUIContent guiContent = new GUIContent(label, tooltip);

			bool effectEnabledInAllMaterials = true;
			bool effectDisabledInAllMaterials = true;
			bool allMaterialsAreVariants = true;
			for (int i = 0; i < references.targetMatInfos.Length; i++)
			{
				bool effectEnabledThisMat = AllIn13DEffectConfig.IsEffectEnabled(effectConfig, references.targetMatInfos[i]);
				
				effectEnabledInAllMaterials = effectEnabledInAllMaterials && effectEnabledThisMat;
				effectDisabledInAllMaterials = effectDisabledInAllMaterials && !effectEnabledThisMat;
				allMaterialsAreVariants = allMaterialsAreVariants && references.targetMatInfos[i].IsShaderVariant();
			}

			if (allMaterialsAreVariants)
			{
				GUILayout.Label(guiContent);
			}
			else
			{
				bool mixedValue = (!(effectEnabledInAllMaterials || effectDisabledInAllMaterials)) && references.targetMatInfos.Length > 1;
				string style = mixedValue ? "ToggleMixed" : "Toggle";
				isEffectEnabled = GUILayout.Toggle(isEffectEnabled, guiContent, style);
			}
			if (EditorGUI.EndChangeCheck())
			{
				for(int i = 0; i < references.targetMatInfos.Length; i++)
				{
					AbstractMaterialInfo matInfo = references.targetMatInfos[i];

					if (isEffectEnabled)
					{
						AllIn13DEffectConfig.EnableEffect(effectConfig, references, matInfo);
					}
					else
					{
						AllIn13DEffectConfig.DisableEffect(effectConfig, matInfo);
					}
				}

				references.matProperties[effectConfig.keywordPropertyIndex].floatValue = isEffectEnabled ? 1f : 0f;

				EditorUtils.SetDirtyCurrentScene();

				references.SetMaterialsDirty();
			}
		}

		public static void DrawMainPropertyEnum(string label, 
			AllIn13DEffectConfig effectConfig, AllIn13DShaderInspectorReferences references,
			bool isEffectAvailable, bool isShaderVariant)
		{
			int selectedIndex = 0;
			//bool isEffectEnabled = AllIn13DEffectConfig.IsEffectEnabled(effectConfig, ref selectedIndex, references);


			bool sameEnumValueInAllMaterials = references.IsEffectEnabled(effectConfig, ref selectedIndex);
			//bool sameEnumValueInAllMaterials = true;
			//for(int i = 0; i < references.targetMatInfos.Length; i++)
			//{
			//	int enumIdx = -1;
			//	AbstractMaterialInfo matInfo = references.targetMatInfos[i];

			//	bool effectEnabled = AllIn13DEffectConfig.IsEffectEnabled(effectConfig, ref enumIdx, matInfo);

			//	if(i == 0)
			//	{
			//		selectedIndex = enumIdx;
			//	}
			//	else
			//	{
			//		sameEnumValueInAllMaterials = sameEnumValueInAllMaterials && (enumIdx == selectedIndex);
			//	}
			//}




			string tooltip = effectConfig.keywords[selectedIndex].keyword + " (C#)";
			GUIContent guiContent = new GUIContent(label, tooltip);


			if (isShaderVariant)
			{
				if (isEffectAvailable)
				{
					EditorGUILayout.BeginHorizontal(references.propertiesStyle);
					EditorGUILayout.LabelField(guiContent);
					EditorGUILayout.LabelField(effectConfig.keywordsDisplayNames[selectedIndex]);
					EditorGUILayout.EndHorizontal();
				}
				else
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(guiContent);
					EditorGUILayout.EndHorizontal();
				}
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.showMixedValue = !sameEnumValueInAllMaterials;
				selectedIndex = EditorGUILayout.Popup(guiContent, selectedIndex, effectConfig.keywordsDisplayNames);
				EditorGUI.showMixedValue = false;

				if (EditorGUI.EndChangeCheck())
				{
					for (int i = 0; i < references.targetMatInfos.Length; i++)
					{
						AbstractMaterialInfo matInfo = references.targetMatInfos[i];

						if (selectedIndex >= 0)
						{
							AllIn13DEffectConfig.EnableEffectByIndex(effectConfig, selectedIndex, references, matInfo);
						}
						else
						{
							AllIn13DEffectConfig.DisableEffect(effectConfig, matInfo);
						}
					}

					references.matProperties[effectConfig.keywordPropertyIndex].floatValue = selectedIndex;

					EditorUtils.SetDirtyCurrentScene();

					references.SetMaterialsDirty();
				}
			}
		}

		//public static void DrawProperty(int propertyIndex, string labelPrefix, bool allowReset, bool isKeywordProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	MaterialProperty targetProperty = references.matProperties[propertyIndex];
		//	DrawProperty(
		//		materialProperty: targetProperty,
		//		labelPrefix: labelPrefix,
		//		displayName: targetProperty.displayName,
		//		allowReset: allowReset,
		//		isKeywordProperty: isKeywordProperty,
		//		references: references);
		//}

		//public static void DrawProperty(EffectProperty effectProperty, string labelPrefix, bool allowReset, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(effectProperty.propertyIndex, labelPrefix, effectProperty.allowReset, effectProperty.IsPropertyWithKeywords(), references);
		//}

		//public static void DrawProperty(EffectProperty effectProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(propertyIndex: effectProperty.propertyIndex, isKeywordProperty: effectProperty.IsPropertyWithKeywords(),  references: references);
		//}

		//public static void DrawProperty(int propertyIndex, bool isKeywordProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(propertyIndex, string.Empty, true, isKeywordProperty, references);
		//}

		//public static void DrawProperty(MaterialProperty materialProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(materialProperty, false, references);
		//}

		//public static void DrawProperty(MaterialProperty materialProperty, bool isKeywordProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(
		//		materialProperty: materialProperty, 
		//		labelPrefix: string.Empty, 
		//		displayName: materialProperty.displayName,
		//		allowReset: true, 
		//		isKeywordProperty: isKeywordProperty, 
		//		references: references);
		//}

		//public static void DrawProperty(MaterialProperty materialProperty, bool allowReset, bool isKeywordProperty, AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(
		//		materialProperty: materialProperty, 
		//		labelPrefix: string.Empty, 
		//		displayName: materialProperty.displayName,
		//		allowReset: allowReset, 
		//		isKeywordProperty: isKeywordProperty, 
		//		references: references);
		//}

		//public static void DrawProperty(
		//	MaterialProperty materialProperty, 
		//	string labelPrefix, 
		//	bool allowReset, 
		//	bool isKeywordProperty, 
		//	AllIn13DShaderInspectorReferences references)
		//{
		//	DrawProperty(
		//		materialProperty: materialProperty, 
		//		labelPrefix: labelPrefix, 
		//		displayName: materialProperty.displayName,
		//		allowReset: allowReset, 
		//		isKeywordProperty: isKeywordProperty,
		//		references: references);
		//}

		//public static void DrawPropertyCustomValue(
		//	EffectProperty effectProperty,
		//	string labelPrefix,
		//	string displayName,
		//	bool allowReset,
		//	bool isKeywordProperty,
		//	AllIn13DShaderInspectorReferences references)
		//{
		//	MaterialProperty materialProperty = references.matProperties[effectProperty.propertyIndex];
		//	string label = $"{labelPrefix} {displayName}";
		//	string tooltip = materialProperty.name + "(C#)";

		//	EditorGUILayout.BeginHorizontal();

		//	DrawShaderProperty(materialProperty: materialProperty, label: label, tooltip: tooltip, isKeywordProperty: isKeywordProperty, references: references);
		//	if (allowReset)
		//	{
		//		DrawResetButton(materialProperty, references);
		//	}

		//	EditorGUILayout.EndHorizontal();
		//}

		public static void DrawProperty(
			MaterialProperty materialProperty, 
			string labelPrefix, 
			string displayName,
			string customValue,
			bool allowReset, 
			bool isKeywordProperty,
			EffectProperty.PropertyType propertyType,
			AllIn13DShaderInspectorReferences references)
		{
			string label = $"{labelPrefix} {displayName}";
			string tooltip = materialProperty.name + "(C#)";

			EditorGUILayout.BeginHorizontal();

			bool isShaderVariant = references.IsShaderVariant();

			if(isShaderVariant && isKeywordProperty)
			{
				DrawShaderPropertyCustomValue(label: label, tooltip: tooltip, customValue: customValue, isKeywordProperty: isKeywordProperty, propertyType: propertyType, references: references);
			}
			else
			{
				DrawShaderProperty(materialProperty: materialProperty, label: label, tooltip: tooltip, isKeywordProperty: isKeywordProperty, references: references);
				if (allowReset)
				{
					DrawResetButton(materialProperty, references);
				}
			}

			EditorGUILayout.EndHorizontal();
		}

		private static void DrawShaderProperty(
			MaterialProperty materialProperty, 
			string label, 
			string tooltip, 
			bool isKeywordProperty, 
			AllIn13DShaderInspectorReferences references)
		{
			GUIContent propertyLabel = new GUIContent();
			propertyLabel.text = label;
			propertyLabel.tooltip = tooltip;

			EditorGUI.BeginChangeCheck();
			references.editorMat.ShaderProperty(materialProperty, propertyLabel);
			if (EditorGUI.EndChangeCheck())
			{
				if (isKeywordProperty)
				{
					references.RefreshMaterialKeywords();
				}
			}
		}

		private static void DrawShaderPropertyCustomValue(
			string label,
			string tooltip,
			string customValue,
			bool isKeywordProperty,
			EffectProperty.PropertyType propertyType,
			AllIn13DShaderInspectorReferences references)
		{
			EditorGUILayout.BeginHorizontal();
			GUIContent propertyLabel = new GUIContent();
			propertyLabel.text = label;
			propertyLabel.tooltip = tooltip;

			if(propertyType == EffectProperty.PropertyType.TOGGLE)
			{
				EditorGUILayout.Toggle(propertyLabel, true);
			}
			else
			{
				EditorGUILayout.LabelField(propertyLabel);
				EditorGUILayout.LabelField(customValue);
			}

			EditorGUILayout.EndHorizontal();
		}

		public static void DrawResetButton(MaterialProperty targetProperty, AllIn13DShaderInspectorReferences references)
		{
			GUIContent resetButtonLabel = new GUIContent();
			resetButtonLabel.text = "R";
			resetButtonLabel.tooltip = "Resets to default value";
			if (GUILayout.Button(resetButtonLabel, GUILayout.Width(20)))
			{
				for (int i = 0; i < references.targetMatInfos.Length; i++)
				{
					AbstractMaterialInfo matInfo = references.targetMatInfos[i];
					AllIn13DEffectConfig.ResetProperty(targetProperty, references, matInfo);
				}
			}
		}
	}
}