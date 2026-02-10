using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class EffectsProfileTabDrawer : AssetWindowTabDrawer
	{
		private const string TAB_NAME = "Active Effects List";

		private EffectsProfileCollection effectsProfileCollection;
		private EffectsProfile effectsProfile;
		private PropertiesConfig propertiesConfig;

		public EffectsProfileTabDrawer(PropertiesConfig propertiesConfig, GlobalConfiguration globalConfiguration, CommonStyles commonStyles, AllIn13DShaderWindow parentWindow) : base(commonStyles, parentWindow)
		{
			this.propertiesConfig = propertiesConfig;
			this.effectsProfileCollection = globalConfiguration.effectsProfileCollection;
			this.effectsProfile = effectsProfileCollection.generalProfile;
		}

		public override void Draw()
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Enable All"))
			{
				EnableAll();
			}

			if(GUILayout.Button("Disable All"))
			{
				DisableAll();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUI.BeginChangeCheck();
			for(int i = 0; i < effectsProfile.groups.Count; i++)
			{
				DrawGroup(effectsProfile.groups[i]);
			}
			bool hasChanges = EditorGUI.EndChangeCheck();
			if (hasChanges)
			{
				EditorUtility.SetDirty(effectsProfileCollection);
			}

			if (GUILayout.Button("Configure"))
			{
				Configure();
			}
		}

		public override void EnteredPlayMode()
		{

		}

		public override void Hide()
		{
			
		}

		public override void OnDisable()
		{
		
		}

		public override void OnEnable()
		{
		
		}

		public override void Show()
		{
		
		}

		private void Configure()
		{
			ShaderFeaturesFileCreator.CreateFile(effectsProfile);
		}

		private void DrawGroup(EffectsProfileGroup effectsProfileGroup)
		{
			EditorGUILayout.BeginVertical();
			GUILayout.Label(effectsProfileGroup.effectGroupConfig.displayName, commonStyles.bigLabel);

			for(int i = 0; i < effectsProfileGroup.entries.Count; i++)
			{
				DrawEntry(effectsProfileGroup.entries[i]);
			}

			GUILayout.Space(15f);

			EditorGUILayout.EndVertical();
		}

		private void DrawEntry(EffectsProfileEntry entry)
		{
			string label = $"{entry.GetDisplayIndex()}. {entry.displayName}";

			AllIn13DEffectConfig effectConfig = propertiesConfig.FindEffectConfigByID(entry.effectID);
			entry.BindEffectConfig(effectConfig);

			EditorGUILayout.BeginVertical();
			
			EditorGUILayout.BeginHorizontal();
			entry.isEnabled = GUILayout.Toggle(entry.isEnabled, string.Empty, GUILayout.Width(15));
			GUILayout.Label(label, GUILayout.Width(200f));
			
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}

		private void DrawSubKeywordToggle(SubkeywordEntryToggle subkeywordEntryToggle)
		{
			EditorGUILayout.BeginHorizontal(commonStyles.shaderPropertiesStyle);

			subkeywordEntryToggle.isEnabled = GUILayout.Toggle(subkeywordEntryToggle.isEnabled, string.Empty, GUILayout.Width(15));
			GUILayout.Label(subkeywordEntryToggle.displayName, GUILayout.Width(200f));

			EditorGUILayout.EndHorizontal();
		}

		private void DrawSubKeywordEnum(SubkeywordEntryEnum subkeywordEntryEnum)
		{
			EditorGUILayout.BeginHorizontal(commonStyles.shaderPropertiesStyle);

			subkeywordEntryEnum.kwIndexEnabled = EditorGUILayout.Popup(subkeywordEntryEnum.kwIndexEnabled, subkeywordEntryEnum.displayNames);

			EditorGUILayout.EndHorizontal();
		}

		private void EnableAll()
		{
			effectsProfile.EnableAllEffects();
			EditorUtility.SetDirty(effectsProfileCollection);
		}

		private void DisableAll()
		{
			effectsProfile.DisableAllEffects();
			EditorUtility.SetDirty(effectsProfileCollection);
		}

		public override string GetTabName()
		{
			return TAB_NAME;
		}
	}
}