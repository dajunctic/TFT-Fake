using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn13DShader
{
	public class EffectsProfileCollection : ScriptableObject
	{
		public static string ASSET_NAME = "EffectsProfileCollection";

		public EffectsProfile generalProfile;
		public List<EffectsProfile> profiles;

		public static EffectsProfileCollection CreateAsset(PropertiesConfigCollection propertiesConfigCollection)
		{
			EffectsProfileCollection res = EffectsProfileCollection.CreateInstance<EffectsProfileCollection>();

			res.profiles = new List<EffectsProfile>();

			EffectsProfile effectsProfile = new EffectsProfile(string.Empty, string.Empty);
			effectsProfile.CreateDefault(propertiesConfigCollection);
			res.generalProfile = effectsProfile;

			string path = Path.Combine(GlobalConfiguration.GetDefaultConfigFolderPath(), ASSET_NAME + ".asset"); 
			AssetDatabase.CreateAsset(res, path);

			return res;
		}

		public EffectsProfile FindEffectProfileByGUID(string guid)
		{
			EffectsProfile res = null;

			for (int i = 0; i < profiles.Count; i++)
			{
				if (profiles[i].shaderGUID == guid)
				{
					res = profiles[i];
					break;
				}
			}

			return res;
		}

		public EffectsProfile FindEffectProfileByShader(Shader shader)
		{
			EffectsProfile res = null;

			string assetPath = AssetDatabase.GetAssetPath(shader);
			string guid = AssetDatabase.AssetPathToGUID(assetPath);

			res = FindEffectProfileByGUID(guid);

			return res;
		}

		public bool RemoveEffectsProfileByShader(Shader shader)
		{
			bool res = false;
			EffectsProfile effectProfile = FindEffectProfileByShader(shader);
			if(effectProfile != null)
			{
				res = profiles.Remove(effectProfile);
			}

			if (res)
			{
				EditorUtility.SetDirty(this);
			}

			return res;
		}

		public void ConfigureEffectProfileByMaterialInfo(EffectsProfile target, EffectsProfile activeEffectsList, AbstractMaterialInfo matInfo, PropertiesConfig propertiesConfig)
		{
			List<AllIn13DEffectConfig> effectConfigs = propertiesConfig.GetAllEffects();
			for(int i = 0; i < effectConfigs.Count; i++)
			{
				bool effectEnabled = AllIn13DEffectConfig.IsEffectEnabled(effectConfigs[i], matInfo) && 
					effectConfigs[i].AreDependenciesMet(propertiesConfig, matInfo) && 
					activeEffectsList.IsEffectEnabled(effectConfigs[i].effectName);

				if (effectEnabled)
				{
					target.EnableEffect(effectConfigs[i], matInfo);
				}
				else
				{
					target.DisableEffect(effectConfigs[i]);
				}
			}
		}

		public EffectsProfile CreateNewProfile(string profileName)
		{
			System.Guid guid = System.Guid.NewGuid();
			string id = $"{guid.ToString()}";
			
			EffectsProfile res = new EffectsProfile(id, profileName);

			profiles.Add(res);

			return res;
		}

		public void BindEffectConfigs(PropertiesConfig propertiesConfig)
		{
			generalProfile.BindEffectConfigs(propertiesConfig);

			for(int i = 0; i < profiles.Count; i++)
			{
				profiles[i].BindEffectConfigs(propertiesConfig);
			}
		}

		public void CleanInvalidProfiles()
		{
			List<EffectsProfile> profilesToRemove = new List<EffectsProfile>();

			for (int i = 0; i < profiles.Count - 1; i++)
			{
				bool removeThisProfile = false;

				if (profiles[i] == null)
				{
					removeThisProfile = true;
				}
				else
				{
					Shader shader = profiles[i].FindShader(); 
					if(shader == null)
					{
						removeThisProfile = true;
					}
				}

				if (removeThisProfile)
				{
					profilesToRemove.Add(profiles[i]);
				}
			}

			for(int i = 0; i < profilesToRemove.Count; i++)
			{
				profiles.Remove(profilesToRemove[i]);
			}
		}

		public void CheckBakedShadersFolder(string folderPath, PropertiesConfig propertiesConfig)
		{
			if (AssetDatabase.IsValidFolder(folderPath))
			{
				string[] files = Directory.GetFiles(folderPath, "*.shader", SearchOption.AllDirectories);

				for (int i = 0; i < files.Length; i++)
				{
					Shader shader = (Shader)AssetDatabase.LoadAssetAtPath(files[i], typeof(Shader));

					EffectsProfile effectProfile = FindEffectProfileByShader(shader);

					if (effectProfile == null)
					{
						EffectProfileCreatorFromShaderVariant.Create(shader, files[i], propertiesConfig, this);
					}
				}
			}
		}

		public void CheckBakedShadersFolder(PropertiesConfig propertiesConfig)
		{
			string shaderVariantsFolderPath = ShaderVariantCreator.GetShaderVariantsFolderPath();
			CheckBakedShadersFolder(shaderVariantsFolderPath, propertiesConfig);
		}

		public void CheckRemovedShader(string removedPath)
		{
			string guid = AssetDatabase.AssetPathToGUID(removedPath);

			EffectsProfile effectsProfileToRemove = FindEffectProfileByGUID(guid);

			if(effectsProfileToRemove != null)
			{
				profiles.Remove(effectsProfileToRemove);
			}

			EditorUtility.SetDirty(this);
		}
	}
}