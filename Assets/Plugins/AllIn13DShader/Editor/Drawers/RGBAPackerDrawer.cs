using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class RGBAPackerDrawer
	{
		private const float LABEL_WIDTH = 70f;
		private const float TEXTURE_FIELD_WIDTH = 200f;
		private const float SPACING_BETWEEN_FIELDS = 20f;
		private const float BOOLEAN_LABEL_WIDTH = 100f;
		private const float SETTINGS_FIELD_WIDTH = 200f;

		private RGBAPackerTool rgbaPackerTool;

		public RGBAPackerValues ToolValues => rgbaPackerTool.values;

		private CommonStyles commonStyles;

		public RGBAPackerDrawer(RGBAPackerTool rgbaPackerTool, CommonStyles commonStyles)
		{
			this.rgbaPackerTool = rgbaPackerTool;
			this.commonStyles = commonStyles;
		}

		public void Draw()
		{
			GUILayout.Label("RGBA Channel Packer", commonStyles.bigLabel);
			GUILayout.Space(20);
			GUILayout.Label("Pack red channels from 4 textures into RGBA channels", EditorStyles.boldLabel);
			GUILayout.Space(10);

			// Channel texture slots
			EditorGUILayout.BeginVertical();
			{
				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("R Channel", GUILayout.Width(LABEL_WIDTH));
					ToolValues.rChannelTexture = (Texture2D)EditorGUILayout.ObjectField(ToolValues.rChannelTexture, typeof(Texture2D), false, GUILayout.Width(TEXTURE_FIELD_WIDTH));
					GUILayout.Space(SPACING_BETWEEN_FIELDS);
					GUILayout.Label("Default Is White?", GUILayout.Width(BOOLEAN_LABEL_WIDTH));
					ToolValues.rChannelDefaultWhite = EditorGUILayout.Toggle(ToolValues.rChannelDefaultWhite);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("G Channel", GUILayout.Width(LABEL_WIDTH));
					ToolValues.gChannelTexture = (Texture2D)EditorGUILayout.ObjectField(ToolValues.gChannelTexture, typeof(Texture2D), false, GUILayout.Width(TEXTURE_FIELD_WIDTH));
					GUILayout.Space(SPACING_BETWEEN_FIELDS);
					GUILayout.Label("Default Is White?", GUILayout.Width(BOOLEAN_LABEL_WIDTH));
					ToolValues.gChannelDefaultWhite = EditorGUILayout.Toggle(ToolValues.gChannelDefaultWhite);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("B Channel", GUILayout.Width(LABEL_WIDTH));
					ToolValues.bChannelTexture = (Texture2D)EditorGUILayout.ObjectField(ToolValues.bChannelTexture, typeof(Texture2D), false, GUILayout.Width(TEXTURE_FIELD_WIDTH));
					GUILayout.Space(SPACING_BETWEEN_FIELDS);
					GUILayout.Label("Default Is White?", GUILayout.Width(BOOLEAN_LABEL_WIDTH));
					ToolValues.bChannelDefaultWhite = EditorGUILayout.Toggle(ToolValues.bChannelDefaultWhite);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				{
					GUILayout.Label("A Channel", GUILayout.Width(LABEL_WIDTH));
					ToolValues.aChannelTexture = (Texture2D)EditorGUILayout.ObjectField(ToolValues.aChannelTexture, typeof(Texture2D), false, GUILayout.Width(TEXTURE_FIELD_WIDTH));
					GUILayout.Space(SPACING_BETWEEN_FIELDS);
					GUILayout.Label("Default Is White?", GUILayout.Width(BOOLEAN_LABEL_WIDTH));
					ToolValues.aChannelDefaultWhite = EditorGUILayout.Toggle(ToolValues.aChannelDefaultWhite);
				}
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndVertical();

			GUILayout.Space(15);

			// Texture settings
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label("Texture Size:", GUILayout.MaxWidth(100));
				ToolValues.textureSizes = (TextureSizes)EditorGUILayout.EnumPopup(ToolValues.textureSizes, GUILayout.MaxWidth(SETTINGS_FIELD_WIDTH));
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Label("Filtering:", GUILayout.MaxWidth(100));
				ToolValues.filtering = (FilterMode)EditorGUILayout.EnumPopup(ToolValues.filtering, GUILayout.MaxWidth(SETTINGS_FIELD_WIDTH));
			}
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);

			// Info about input channels
			int channelCount = 0;
			if(ToolValues.rChannelTexture != null) channelCount++;
			if(ToolValues.gChannelTexture != null) channelCount++;
			if(ToolValues.bChannelTexture != null) channelCount++;
			if(ToolValues.aChannelTexture != null) channelCount++;

			int textureSize = (int)ToolValues.textureSizes;
			GUILayout.Label($"Output will be a {textureSize}x{textureSize} RGBA texture with {channelCount}/4 channels assigned", EditorStyles.boldLabel);

			if(channelCount == 0)
			{
				GUILayout.Label("*No input textures assigned - output will use default values", EditorStyles.boldLabel);
			}
			else if(channelCount < 4)
			{
				GUILayout.Label($"*{4 - channelCount} channels will use default values (black or white based on toggles)", EditorStyles.boldLabel);
			}

			GUILayout.Space(20);
			GUILayout.Label("Select the folder where RGBA textures will be saved", EditorStyles.boldLabel);
			GlobalConfiguration.instance.AtlasesSavePath = EditorUtils.DrawSelectorFolder(GlobalConfiguration.instance.AtlasesSavePath, "RGBA");

			if(Directory.Exists(GlobalConfiguration.instance.AtlasesSavePath))
			{
				if(GUILayout.Button("Create And Save RGBA Texture", GUILayout.MaxWidth(CommonStyles.BUTTON_WIDTH)))
				{
					rgbaPackerTool.CreateRGBATexture();
					EditorUtils.SaveTextureAsPNG(GlobalConfiguration.instance.AtlasesSavePath, "RGBA_Packed", "RGBA Channel Packed Texture", ToolValues.createdRGBATexture, ToolValues.filtering, TextureImporterType.Default, TextureWrapMode.Clamp);
				}
			}
		}
	}
}