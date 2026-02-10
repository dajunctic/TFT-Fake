using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class TextureEditorTabDrawer : AssetWindowTabDrawer
	{
		private const string TAB_NAME = "Texture Editor";

		private TextureEditorTool textureEditorTool;
		private TextureEditorValuesDrawer textureEditorValuesDrawer;

		public TextureEditorTabDrawer(CommonStyles commonStyles, AllIn13DShaderWindow parentWindow) : base(commonStyles, parentWindow)
		{
			textureEditorTool = new TextureEditorTool();
			textureEditorValuesDrawer = new TextureEditorValuesDrawer();
			textureEditorValuesDrawer.Setup(textureEditorTool);
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
			EditorGUI.BeginChangeCheck();
			textureEditorTool.editorTexInput = EditorGUILayout.ObjectField("Image to Edit", textureEditorTool.editorTexInput, typeof(Texture2D), false, GUILayout.Width(300), GUILayout.Height(50)) as Texture2D;
			if (EditorGUI.EndChangeCheck())
			{
				if (textureEditorTool.editorTexInput != null)
				{
					textureEditorTool.Setup();
				}
			}

			EditorUtils.DrawThinLine();

			if (textureEditorTool.editorTex != null)
			{
				textureEditorValuesDrawer.Draw();
			}
			else
			{
				GUILayout.Label("Please select an Image to Edit above", EditorStyles.boldLabel);
			}
		}

		public override string GetTabName()
		{
			return TAB_NAME;
		}
	}
}