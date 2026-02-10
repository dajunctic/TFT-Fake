using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace AllIn13DShader
{
	public class URPSettingsDrawer : AssetWindowTabDrawer
	{
		private const string TAB_NAME = "URP Settings";

		private bool[] featureToggles = new bool[12]; // Array to track toggle states
		private string shaderFeaturesFilePath;

		private readonly string[] featureDefineNames = {
			"ALLIN1_GPU_INSTANCING_SUPPORT",
			"ALLIN1_DOTS_INSTANCING_SUPPORT",
			"ALLIN1_FOG_SUPPORT",
			"ALLIN1_LIGHTMAPS_SUPPORT",
			"ALLIN1_ADDITIONAL_LIGHTS_SUPPORT",
			"ALLIN1_CAST_SHADOWS_SUPPORT",
			"ALLIN1_SHADOW_MASK_SUPPORT",
			"ALLIN1_FORWARD_PLUS_SUPPORT_UNITY6",
			"ALLIN1_REFLECTIONS_PROBES_SUPPORT_UNITY6",
			"ALLIN1_ADAPTATIVE_PROBE_VOLUMES_UNITY6",
			"ALLIN1_SSO_SUPPORT",
			"ALLIN1_LIGHT_LAYERS_SUPPORT"
		};

		private readonly string[] featureNames = {
			"GPU Instancing Support",
			"Entities Graphics Instancing Support (and GPU Resident Drawer)", 
			"Fog Support",
			"Lightmaps Support",
			"Additional Lights Support",
			"Cast Shadows Support",
			"Shadow Mask Support",
			"Forward+ Support (Unity 6+)",
			"Reflection Probes Blending Support (Unity 6+)",
			"Adaptative Probe Volumes (Unity 6+)",
			"Screen Space Ambient Occlusion",
			"Light Layers Support"
		};

		private readonly string[] featureTooltips = {
			"Enables GPU Instancing for better performance when rendering many identical objects",
			"Supports Unity Entities Graphics instancing for DOTS-based projects and GPU Resident Drawer",
			"Unity fog system integration for atmospheric effects",
			"Enables baked lightmap support for static lighting",
			"Support for additional real-time lights beyond main directional light",
			"Enables shadow casting from materials using this shader",
			"Mixed lighting shadowmask support for hybrid lighting setups",
			"Forward+ rendering path support (Unity 6+ only)",
			"Probe blending support (Unity 6+ only, if you aren't using more than 1 probe you can disable this)",
			"Adaptative Probe Volumes support (Unity 6+ only)",
			"Screen Space Ambient Occlusion integration",
			"Unity light layer system support (for selective lighting)"
		};

		public URPSettingsDrawer(CommonStyles commonStyles, AllIn13DShaderWindow parentWindow) : base(commonStyles, parentWindow)
		{
		}

		public override void Draw()
		{
			GUILayout.Label("Shader Feature Configuration", commonStyles.bigLabel);
			EditorGUILayout.HelpBox("Enable/disable shader features to optimize compilation time and editor performance. \n" +
			                        "Hover each feature to get a more detailed tooltip.", MessageType.Info);
			
			GUILayout.Space(5);
			
			for(int i = 0; i < featureNames.Length; i++)
			{
				EditorGUILayout.BeginHorizontal();
				
				GUIContent toggleContent = new GUIContent(featureNames[i], featureTooltips[i]);
				featureToggles[i] = EditorGUILayout.ToggleLeft(toggleContent, featureToggles[i]);
				
				EditorGUILayout.EndHorizontal();
			}
			
			GUILayout.Space(10);
			
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Apply Changes"))
			{
				ApplyFeatureChanges();
			}
			
			if(GUILayout.Button("Reset to Defaults"))
			{
				ResetToDefaults();
			}
			EditorGUILayout.EndHorizontal();
			
			GUILayout.Space(20);
			EditorUtils.DrawThinLine();
			
			GUILayout.Label("Configure AllIn13D to work correctly with URP", commonStyles.bigLabel);
			if (GUILayout.Button("Configure"))
			{
#if ALLIN13DSHADER_URP
				URPConfigurator.Configure(forceConfigure: true);
#endif
			}
		}

		public override void Hide()
		{

		}

		public override void Show()
		{
			LoadCurrentFeatureStates();
		}

		public override void OnDisable()
		{
		
		}

		public override void OnEnable()
		{
			LoadCurrentFeatureStates();
		}

		public override void EnteredPlayMode()
		{
		
		}

		private void LoadCurrentFeatureStates()
		{
			// Find the shader features file
			string[] guids = AssetDatabase.FindAssets("AllIn13DShader_FeaturesURP_Defines");
			if(guids.Length == 0)
			{
				Debug.LogWarning("AllIn13DShader_FeaturesURP file not found");
				return;
			}

			shaderFeaturesFilePath = AssetDatabase.GUIDToAssetPath(guids[0]);
			if(string.IsNullOrEmpty(shaderFeaturesFilePath))
			{
				Debug.LogWarning("Could not get path for AllIn13DShader_FeaturesURP file");
				return;
			}

			// Read the file content
			string fileContent = File.ReadAllText(shaderFeaturesFilePath);
			
			// Check each feature state
			for(int i = 0; i < featureDefineNames.Length; i++)
			{
				string pattern = $@"^(\s*)(\/\/)?(\s*#define\s+{featureDefineNames[i]})";
				Match match = Regex.Match(fileContent, pattern, RegexOptions.Multiline);
				
				if(match.Success)
				{
					// Feature is enabled if there's no "//" comment at the start
					featureToggles[i] = string.IsNullOrEmpty(match.Groups[2].Value);
				}
			}
		}

		private void ApplyFeatureChanges()
		{
			if(string.IsNullOrEmpty(shaderFeaturesFilePath))
			{
				Debug.LogError("Shader features file path not found");
				return;
			}

			string fileContent = File.ReadAllText(shaderFeaturesFilePath);
			
			for(int i = 0; i < featureDefineNames.Length; i++)
			{
				string pattern = $@"^(\s*)(\/\/)?(\s*#define\s+{featureDefineNames[i]}.*)$";
				
				fileContent = Regex.Replace(fileContent, pattern, (Match match) =>
				{
					string indent = match.Groups[1].Value;
					string defineLine = match.Groups[3].Value;
					
					if(featureToggles[i])
					{
						// Feature enabled - remove comment
						return indent + defineLine;
					}
					else
					{
						// Feature disabled - add comment
						return indent + "//" + defineLine;
					}
				}, RegexOptions.Multiline);
			}

			// Save the file
			File.WriteAllText(shaderFeaturesFilePath, fileContent);
			AssetDatabase.Refresh();
			
			Debug.Log("Shader feature configuration applied successfully!");
		}

		private void ResetToDefaults()
		{
			// Set default values (most features enabled, some disabled)
			featureToggles[0]	= true;		// GPU_INSTANCING_SUPPORT
			featureToggles[1]	= false;	// DOTS_INSTANCING_SUPPORT
			featureToggles[2]	= true;		// FOG_SUPPORT
			featureToggles[3]	= true;		// LIGHTMAPS_SUPPORT
			featureToggles[4]	= true;		// ADDITIONAL_LIGHTS_SUPPORT
			featureToggles[5]	= true;		// CAST_SHADOWS_SUPPORT
			featureToggles[6]	= true;		// SHADOW_MASK_SUPPORT
			featureToggles[7]	= true;		// FORWARD_PLUS_SUPPORT_UNITY6
			featureToggles[8]	= false;	// REFLECTIONS_PROBES_SUPPORT_UNITY6
			featureToggles[9]	= true;     // ALLIN1_ADAPTATIVE_PROBE_VOLUMES_UNITY6 
			featureToggles[10]	= true;		// SSO_SUPPORT
			featureToggles[11]	= false;	// LIGHT_LAYERS_SUPPORT
		}

		public override string GetTabName()
		{
			return TAB_NAME;
		}
	}
}