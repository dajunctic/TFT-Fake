using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class AllIn13DShaderWindow : EditorWindow
	{
		private static AllIn13DShaderWindow instance;

		private Vector2 scrollPosition;

		private AssetWindowTabDrawer currentTabDrawer;

		private int currTab = 0;
		private string[] tabsNames;
		private AssetWindowTabDrawer[] tabDrawers;

		private CommonStyles commonStyles;
		private GlobalConfiguration globalConfiguration;

		private Texture imageInspector;

		private SavePathsTabDrawer savePathsTabDrawer;
		private TextureEditorTabDrawer textureEditorTabDrawer;
		private TextureCreatorTabDrawer textureCreatorTabDrawer;

		private OverrideMaterialsTabDrawer overrideMaterialsTabDrawer;
		private OtherTabDrawer otherTabDrawer;
		private EffectsProfileTabDrawer effectsProfileTabDrawer;
		private URPSettingsDrawer urpSettingsDrawer;


		private bool initialized;


		[MenuItem("Tools/AllIn1/3DShaderWindow")] 
		public static void ShowAllIn13DShaderWindow()
		{
			if(instance == null)
			{
				instance = GetWindow<AllIn13DShaderWindow>("All In 1 3DShader Window");
			}
		} 

		private void Init()
		{
			commonStyles = new CommonStyles();
			globalConfiguration = EditorUtils.FindAssetByName<GlobalConfiguration>("GlobalConfiguration");

			PropertiesConfigCollection propertiesConfigCollection = EditorUtils.FindAsset<ScriptableObject>("PropertiesConfigCollection") as PropertiesConfigCollection;

			scrollPosition = Vector2.zero;

			savePathsTabDrawer = new SavePathsTabDrawer(commonStyles, this);
			textureEditorTabDrawer = new TextureEditorTabDrawer(commonStyles, this);
			textureCreatorTabDrawer = new TextureCreatorTabDrawer(commonStyles, this);
			overrideMaterialsTabDrawer = new OverrideMaterialsTabDrawer(commonStyles, this);
			otherTabDrawer = new OtherTabDrawer(globalConfiguration, commonStyles, this);
			effectsProfileTabDrawer = new EffectsProfileTabDrawer(propertiesConfigCollection.propertiesConfig, globalConfiguration, commonStyles, this);
			urpSettingsDrawer = new URPSettingsDrawer(commonStyles, this);

#if ALLIN13DSHADER_URP
			tabDrawers = new AssetWindowTabDrawer[]
			{
				savePathsTabDrawer,
				textureEditorTabDrawer,
				overrideMaterialsTabDrawer,
				effectsProfileTabDrawer,
				otherTabDrawer,
				urpSettingsDrawer
			};

#else
			tabDrawers = new AssetWindowTabDrawer[]
			{
				savePathsTabDrawer,
				textureEditorTabDrawer,
				overrideMaterialsTabDrawer,
				effectsProfileTabDrawer,
				otherTabDrawer
			};			
#endif

			tabsNames = new string[tabDrawers.Length];
			for(int i = 0; i < tabsNames.Length; i++)
			{
				tabsNames[i] = tabDrawers[i].GetTabName();
			}

			if (imageInspector == null)
			{
				imageInspector = AllIn13DShaderConfig.GetInspectorImage();
			}



			currentTabDrawer = savePathsTabDrawer;

			initialized = true;
		}

		private void OnEnable()
		{
			Init(); 

			EditorApplication.playModeStateChanged += PlayModeStateChanged;
			currentTabDrawer.OnEnable();
		}

		private void OnDisable()
		{
			EditorApplication.playModeStateChanged -= PlayModeStateChanged;
			currentTabDrawer.OnDisable();
		}

		private void OnDestroy()
		{
			WindowClosed();
		}

		private void WindowClosed()
		{
			overrideMaterialsTabDrawer.Close();
			Repaint();
		}

		private void OnGUI()
		{
			if (!initialized)
			{
				Init();
			}

			commonStyles.InitStyles();

#if ALLIN13DSHADER_URP
			bool urpCorrectlyConfigured = URPConfigurator.IsURPCorrectlyConfigured();
			if (!urpCorrectlyConfigured)
			{
				Draw_URPError();
			}
			else
			{
				Draw();
			}
#else
			Draw();
#endif

		}

		private void Draw()
		{
			DrawEditor();
		}

		private void DrawEditor()
		{
			using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height)))
			{
				scrollPosition = scrollView.scrollPosition;

				if (imageInspector)
				{
					Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(50));
					GUI.DrawTexture(rect, imageInspector, ScaleMode.ScaleToFit, true);
				}

				EditorUtils.DrawThinLine();
				
				if (Application.isPlaying) 
				{
					DrawPlayMode();
				}
				else
				{
					int newTab = GUILayout.Toolbar(currTab, tabsNames);
					AssetWindowTabDrawer newTabDrawer = tabDrawers[newTab];

					if (newTabDrawer != currentTabDrawer)
					{
						currentTabDrawer.Hide();
						newTabDrawer.Show();

						this.currTab = newTab;
						this.currentTabDrawer = newTabDrawer;
					}
					
					EditorUtils.DrawThinLine();
					currentTabDrawer.Draw();
				}

				GUILayout.Space(10);
				EditorUtils.DrawThinLine();
				GUILayout.Label("Current asset version is " + Constants.VERSION, EditorStyles.boldLabel);
			}
		}

		private void DrawPlayMode()
		{
			GUILayout.Label("Not available during play mode", commonStyles.bigLabel);
		}

		private void Draw_URPError()
		{
			EditorGUILayout.LabelField(CommonMessages.URP_PIPELINE_NOT_ASSIGNED, commonStyles.warningLabel);
			EditorUtils.DrawButtonLink(CommonMessages.URP_PIPELINE_NOT_ASSIGNED_DOC_LINK);
		}

		private void PlayModeStateChanged(PlayModeStateChange state)
		{
			if(state == PlayModeStateChange.ExitingEditMode)
			{
				currentTabDrawer.ExitingEditMode();
			}
			else if(state == PlayModeStateChange.EnteredPlayMode)
			{
				currentTabDrawer.EnteredPlayMode();
				Repaint();
			}
			else if(state == PlayModeStateChange.ExitingPlayMode)
			{
				Init();
				Repaint();
			}
		}

		public static void AllAssetProcessed()
		{
			CheckURPCorrectlyConfigured();
		}

		private static void CheckURPCorrectlyConfigured()
		{
#if ALLIN13DSHADER_URP
			if (!URPConfigurator.IsURPCorrectlyConfigured())
			{
				ShowAllIn13DShaderWindow();
			}
#endif
		}
	}
}