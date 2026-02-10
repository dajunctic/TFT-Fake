using System;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class PropertySelectorAuxWindow : EditorWindow
	{
		public enum SelectionType
		{
			GLOBAL_PROPERTY,
			ADVANCED_PROPERTY,
			EFFECT_GROUP,
		}

		public enum TypeOfPropertyAdded
		{
			GLOBAL_PROPERTY,
			ADVANCED_PROPERTY,
			EFFECT_PROPERTY,
			EFFECT_MAIN,
		}

		private PropertiesConfigCollection propertiesConfigCollection;

		//private Shader allIn13DShader;
		private Shader allIn13DShaderOutline;

		//private PropertiesConfig propertiesConfigNormal;
		private PropertiesConfig propertiesConfigOutline;

		private int effectGroupIdx;
		private int effectIdx;
		private int globalPropertyIdx;
		private int advancedPropertyIdx;

		private EffectGroup selectedEffectGroup;
		private AllIn13DEffectConfig selectedEffectConfig;
		private EffectProperty selectedEffectProperty;

		private TypeOfPropertyAdded typeOfPropertyAdded;
		private SelectionType selectionType;

		private Action<AllIn13DEffectConfig, int, Shader, TypeOfPropertyAdded> propertyOverrideAddedCallback;

		private void OnEnable()
		{
			//this.allIn13DShader = Shader.Find(Constants.SHADER_FULL_NAME_ALLIN13D);
			this.allIn13DShaderOutline = Shader.Find(Constants.SHADER_FULL_NAME_ALLIN13D_OUTLINE);

			this.effectIdx = 0;
			this.globalPropertyIdx = 0;
			this.advancedPropertyIdx = 0;

			propertiesConfigCollection = EditorUtils.FindAsset<PropertiesConfigCollection>("PropertiesConfigCollection");
			propertiesConfigOutline = propertiesConfigCollection.FindPropertiesConfigByShader(allIn13DShaderOutline);
		}

		public void Setup(Action<AllIn13DEffectConfig, int, Shader, TypeOfPropertyAdded> propertyOverrideAddedCallback)
		{
			this.propertyOverrideAddedCallback = propertyOverrideAddedCallback;
		}

		private void OnGUI()
		{
			DrawEffectGroupSelector();

			if(selectionType == SelectionType.EFFECT_GROUP)
			{
				DrawEffectSelector();
			}
			else if(selectionType == SelectionType.GLOBAL_PROPERTY)
			{
				DrawGlobalPropertySelector();
			}
			else if(selectionType == SelectionType.ADVANCED_PROPERTY)
			{
				DrawAdvancedPropertySelector();
			}

			GUILayout.Space(20f);

			if (GUILayout.Button("Add"))
			{
				AddProperty();
			}
		}

		private void DrawEffectGroupSelector()
		{
			string[] effectsGroupsNames = propertiesConfigOutline.GetEffectGroupsDisplayNames();
			ArrayUtility.Insert(ref effectsGroupsNames, 0, "Advanced Properties");
			ArrayUtility.Insert(ref effectsGroupsNames, 0, "Global Properties");

			effectGroupIdx = EditorGUILayout.Popup("Group", effectGroupIdx, effectsGroupsNames);

			if(effectGroupIdx == 0)
			{
				selectionType = SelectionType.GLOBAL_PROPERTY;
				this.selectedEffectGroup = null;
			}
			else if(effectGroupIdx == 1)
			{
				selectionType = SelectionType.ADVANCED_PROPERTY;
				this.selectedEffectGroup = null;
			}
			else
			{
				selectionType = SelectionType.EFFECT_GROUP;
				this.selectedEffectGroup = propertiesConfigOutline.effectsGroups[effectGroupIdx - 2];
			}
		}

		private void DrawEffectSelector()
		{
			string[] effectsNames = selectedEffectGroup.GetEffectsNames();
			effectIdx = EditorGUILayout.Popup("Effect", effectIdx, effectsNames);

			selectedEffectConfig = selectedEffectGroup.effects[effectIdx];
		}

		private void DrawGlobalPropertySelector()
		{
			string[] globalPropertyNames = propertiesConfigOutline.GetGlobalPropertyNames();
			globalPropertyIdx = EditorGUILayout.Popup("Property", globalPropertyIdx, globalPropertyNames);
		}

		private void DrawAdvancedPropertySelector()
		{
			string[] globalPropertyNames = propertiesConfigOutline.GetAdvancedPropertyNames();
			advancedPropertyIdx = EditorGUILayout.Popup("Property", advancedPropertyIdx, globalPropertyNames);
		}

		private void AddProperty()
		{
			if(selectionType == SelectionType.GLOBAL_PROPERTY)
			{
				typeOfPropertyAdded = TypeOfPropertyAdded.GLOBAL_PROPERTY;
			}
			else if(selectionType == SelectionType.ADVANCED_PROPERTY)
			{
				typeOfPropertyAdded = TypeOfPropertyAdded.ADVANCED_PROPERTY;
			}
			else
			{
				if(selectedEffectProperty == null)
				{
					typeOfPropertyAdded = TypeOfPropertyAdded.EFFECT_MAIN;
				}
				else
				{
					typeOfPropertyAdded = TypeOfPropertyAdded.EFFECT_PROPERTY;
				}
			}

			if (propertyOverrideAddedCallback != null)
			{
				switch (typeOfPropertyAdded)
				{
					case TypeOfPropertyAdded.GLOBAL_PROPERTY:
						propertyOverrideAddedCallback(selectedEffectConfig, propertiesConfigOutline.singleProperties[globalPropertyIdx], allIn13DShaderOutline, typeOfPropertyAdded);
						break;
					case TypeOfPropertyAdded.ADVANCED_PROPERTY:
						propertyOverrideAddedCallback(selectedEffectConfig, propertiesConfigOutline.advancedProperties[advancedPropertyIdx + 1], allIn13DShaderOutline, typeOfPropertyAdded);
						break;
					case TypeOfPropertyAdded.EFFECT_MAIN:
						propertyOverrideAddedCallback(selectedEffectConfig, -1, allIn13DShaderOutline, typeOfPropertyAdded);
						break;
				}
			}
		
			Close();
		}
	}
}