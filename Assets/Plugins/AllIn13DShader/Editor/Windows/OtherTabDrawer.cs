using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class OtherTabDrawer : AssetWindowTabDrawer
	{
		private const string TAB_NAME = "Other";

		private GlobalConfiguration globalConfiguration;

		public OtherTabDrawer(GlobalConfiguration globalConfiguration, CommonStyles commonStyles, AllIn13DShaderWindow parentWindow) : base(commonStyles, parentWindow)
		{
			this.globalConfiguration = globalConfiguration;

			if (globalConfiguration.defaultPreset == null)
			{
				globalConfiguration.projectType = GlobalConfiguration.ProjectType.STANDARD_BASIC;
				globalConfiguration.RefreshDefaultMaterial();
				globalConfiguration.Save();
			}
		}

		public override void Show()
		{

		}

		public override void Hide()
		{

		}

		public override void OnEnable()
		{

		}

		public override void OnDisable()
		{

		}

		public override void EnteredPlayMode()
		{

		}

		public override void Draw()
		{
			GUILayout.Label("Default Materials", commonStyles.bigLabel);
			GUILayout.Space(20);

			EditorGUI.BeginChangeCheck();
			globalConfiguration.projectType = (GlobalConfiguration.ProjectType)EditorGUILayout.EnumPopup("Project Type", globalConfiguration.projectType);
			bool projectTypeChanged = EditorGUI.EndChangeCheck(); 

			bool disabled = globalConfiguration.projectType != GlobalConfiguration.ProjectType.CUSTOM;
			EditorGUI.BeginDisabledGroup(disabled);
			EditorGUI.BeginChangeCheck();
			globalConfiguration.defaultPreset = (Material)EditorGUILayout.ObjectField("Default Material", globalConfiguration.defaultPreset, typeof(Material), false);
			bool defaultPresetChanged = EditorGUI.EndChangeCheck();
			EditorGUI.EndDisabledGroup();

			if (projectTypeChanged)
			{
				globalConfiguration.RefreshDefaultMaterial();
			}

			if(projectTypeChanged || defaultPresetChanged)
			{
				globalConfiguration.Save();
			}


			EditorGUILayout.Space();
			EditorUtils.DrawThinLine();
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			if (GUILayout.Button("Refresh the Material Inspector Properties Configuration"))
			{
				ShadersCreatorTool.BuildShaderFiles();
				PropertiesConfigCreator.CreateConfig();
			}

			EditorGUILayout.LabelField("The asset uses auto generated cached data to display the properties of the Material inspector\nYou should never need this button", commonStyles.wordWrappedStyle);
		}

		public override string GetTabName()
		{
			return TAB_NAME;
		}
	}
}