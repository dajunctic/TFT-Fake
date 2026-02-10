using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class AllIn13DShaderInspectorReferences
	{
		public MaterialProperty[] matProperties;
		public string[] oldKeyWords;

		public AbstractMaterialInfo[] targetMatInfos;

		public Material materialWithDefaultValues;

		public MaterialEditor editorMat;

		//Styles
		private const int bigFontSize = 16, smallFontSize = 11;
		public GUIStyle propertiesStyle, bigLabelStyle, smallLabelStyle, toggleButtonStyle, tabButtonStyle;

		//Outline Effect
		public AllIn13DEffectConfig outlineEffectConfig;

		//Cast Shadows Effect
		public AllIn13DEffectConfig castShadowsEffectConfig;

		//Effects Profile Collection
		public EffectsProfileCollection effectsProfileCollection;

		public AllIn13DShaderInspectorReferences()
		{
			propertiesStyle = new GUIStyle(EditorStyles.helpBox);
			propertiesStyle.margin = new RectOffset(0, 0, 0, 0);

			bigLabelStyle = new GUIStyle(EditorStyles.boldLabel);
			bigLabelStyle.fontSize = bigFontSize;

			smallLabelStyle = new GUIStyle(EditorStyles.boldLabel);
			smallLabelStyle.fontSize = smallFontSize;

			toggleButtonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, richText = true };

			tabButtonStyle = new GUIStyle(GUI.skin.button) { fontSize = 10 };
		}

		public void Setup(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			this.editorMat = materialEditor;

			if (this.targetMatInfos == null)
			{
				this.targetMatInfos = new AbstractMaterialInfo[materialEditor.targets.Length];
				for (int i = 0; i < materialEditor.targets.Length; i++)
				{
					Material mat = (Material)materialEditor.targets[i];
					targetMatInfos[i] = AbstractMaterialInfo.CreateInstance(mat);
				}

				materialWithDefaultValues = new Material(targetMatInfos[0].mat.shader);
			}

			this.effectsProfileCollection = GlobalConfiguration.instance.effectsProfileCollection;

			this.matProperties = properties;
		}

		public void SetOutlineEffect(PropertiesConfig propertiesConfig)
		{
			this.outlineEffectConfig = propertiesConfig.FindEffectConfigByID("OUTLINETYPE");
		}

		public void SetCastShadowsEffect(PropertiesConfig propertiesConfig)
		{
			this.castShadowsEffectConfig = propertiesConfig.FindEffectConfigByID("CAST_SHADOWS_ON");
		}

		public void SetMaterialsDirty()
		{
			for(int i = 0; i < targetMatInfos.Length; i++)
			{
				EditorUtility.SetDirty(targetMatInfos[i].mat);
			}
		}

		public Shader GetShader()
		{
			return targetMatInfos[0].mat.shader;
		}

		public bool IsKeywordEnabled(string keyword)
		{
			bool res = true;

			for(int i = 0; i < targetMatInfos.Length; i++)
			{
				res = res && targetMatInfos[i].IsKeywordEnabled(keyword);
			}

			return res;
		}

		public void RefreshMaterialKeywords()
		{
			for(int i = 0; i < this.targetMatInfos.Length; i++)
			{
				this.targetMatInfos[i].RefreshKeywords();
			}
		}

		public bool IsShaderVariant()
		{
			bool res = true;

			for(int i = 0; i < targetMatInfos.Length; i++)
			{
				res = res && targetMatInfos[i].IsShaderVariant();
			}

			return res;
		}

		public bool IsEffectEnabled(AllIn13DEffectConfig effectConfig, ref int selectedIndex)
		{
			bool res = true;

			for (int i = 0; i < targetMatInfos.Length; i++)
			{
				int enumIdx = -1;
				AbstractMaterialInfo matInfo = targetMatInfos[i];

				bool effectEnabled = AllIn13DEffectConfig.IsEffectEnabled(effectConfig, ref enumIdx, matInfo);

				if (i == 0)
				{
					selectedIndex = enumIdx;
				}
				else
				{
					res = res && (enumIdx == selectedIndex);
				}
			}

			return res;
		}

		public bool IsEffectPropertyEnabled(EffectProperty effectProperty, ref int selectedIndex)
		{
			bool res = true;

			for (int i = 0; i < targetMatInfos.Length; i++)
			{
				int enumIdx = 0;
				AbstractMaterialInfo matInfo = targetMatInfos[i];

				res = res && AllIn13DEffectConfig.IsEffectPropertyEnabled(effectProperty, ref enumIdx, matInfo);

				if (i == 0)
				{
					selectedIndex = enumIdx;
				}
				else
				{
					res = res && (enumIdx == selectedIndex);
				}
			}

			return res;
		}

		public bool AreAllMaterialsShaderVariant()
		{
			bool res = true;

			for(int i = 0; i < targetMatInfos.Length; i++)
			{
				res = res && targetMatInfos[i].IsShaderVariant();
			}

			return res;
		}

		public bool AreAllMaterialsShaderGeneric()
		{
			bool res = true;

			for (int i = 0; i < targetMatInfos.Length; i++)
			{
				res = res && !targetMatInfos[i].IsShaderVariant();
			}

			return res;
		}
	}
}