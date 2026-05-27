using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public enum CursorState
    {
        Normal,
        Hover,
        Click,
        Disabled
    }

    public class SettingsSystem : MonoBehaviour, IGameSystem
    {
        private SettingsData _settingsData;

        private float _masterVolume = 1f;
        private float _musicVolume = 0.7f;
        private float _sfxVolume = 0.8f;
        private int _qualityLevel = 2;
        private bool _vSync = true;
        private int _targetFrameRate = 60;
        private bool _cameraShake = true;
        private bool _showDamageNumbers = true;

        private CursorState _currentCursorState = CursorState.Normal;

        private const string KEY_MASTER_VOLUME = "Settings.Audio.MasterVolume";
        private const string KEY_MUSIC_VOLUME = "Settings.Audio.MusicVolume";
        private const string KEY_SFX_VOLUME = "Settings.Audio.SFXVolume";
        private const string KEY_QUALITY = "Settings.Graphics.QualityLevel";
        private const string KEY_RESOLUTION_WIDTH = "Settings.Graphics.ResolutionWidth";
        private const string KEY_RESOLUTION_HEIGHT = "Settings.Graphics.ResolutionHeight";
        private const string KEY_FULLSCREEN = "Settings.Graphics.Fullscreen";
        private const string KEY_VSYNC = "Settings.Graphics.VSync";
        private const string KEY_TARGET_FPS = "Settings.Graphics.TargetFPS";
        private const string KEY_CAMERA_SHAKE = "Settings.Gameplay.CameraShake";
        private const string KEY_DAMAGE_NUMBERS = "Settings.Gameplay.DamageNumbers";

        public async Task LoadDataAsync()
        {
            var handle = Addressables.LoadAssetAsync<SettingsData>(GameSystemManager.Instance.Config.settingsData);
            _settingsData = await handle.Task;
            Debug.Log("<color=cyan>SettingsSystem data loaded</color>");
        }

        public void Initialize(GameSystemManager manager)
        {
            Debug.Log("<color=cyan>SettingsSystem initialized</color>");

            LoadSettings();
            ApplyAllSettings();
        }

        public void Shutdown()
        {
            SaveSettings();
            Debug.Log("<color=yellow>SettingsSystem shutdown</color>");
        }

        #region Mouse Cursor

        public void SetCursorState(CursorState state)
        {
            _currentCursorState = state;
            ApplyCursor();
        }

        private void ApplyCursor()
        {
            if (_settingsData == null) return;

            Texture2D cursorTexture = _currentCursorState switch
            {
                CursorState.Normal => _settingsData.cursorNormal,
                CursorState.Hover => _settingsData.cursorHover,
                CursorState.Click => _settingsData.cursorClick,
                CursorState.Disabled => null,
                _ => _settingsData.cursorNormal
            };

            if (_currentCursorState == CursorState.Disabled)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
                if (cursorTexture != null)
                {
                    Cursor.SetCursor(cursorTexture, _settingsData.cursorHotspot, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }

        #endregion

        #region Audio

        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SFXVolume => _sfxVolume;

        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            ApplyMasterVolume();
            SaveSettings();

            this.Raise(new SettingsChangedEvent { SettingType = "MasterVolume", Value = _masterVolume });
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            ApplyMusicVolume();
            SaveSettings();

            this.Raise(new SettingsChangedEvent { SettingType = "MusicVolume", Value = _musicVolume });
        }

        public void SetSFXVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            ApplySFXVolume();
            SaveSettings();

            this.Raise(new SettingsChangedEvent { SettingType = "SFXVolume", Value = _sfxVolume });
        }

        private void ApplyMasterVolume()
        {
            AudioListener.volume = _masterVolume;
        }

        private void ApplyMusicVolume()
        {

            this.Raise(new AudioVolumeChangedEvent { VolumeType = "Music", Volume = _musicVolume });
        }

        private void ApplySFXVolume()
        {
            
            this.Raise(new AudioVolumeChangedEvent { VolumeType = "SFX", Volume = _sfxVolume });
        }

        #endregion

        #region Graphics

        public int QualityLevel => _qualityLevel;
        public bool VSync => _vSync;
        public int TargetFrameRate => _targetFrameRate;

        public void SetQualityLevel(int level)
        {
            _qualityLevel = Mathf.Clamp(level, 0, QualitySettings.names.Length - 1);
            QualitySettings.SetQualityLevel(_qualityLevel, true);
            SaveSettings();

            Debug.Log($"Quality set to: {QualitySettings.names[_qualityLevel]}");
        }

        public void SetResolution(int width, int height, bool fullscreen)
        {
            Screen.SetResolution(width, height, fullscreen);
            SaveSettings();

            Debug.Log($"Resolution set to: {width}x{height} Fullscreen: {fullscreen}");
        }

        public void SetVSync(bool enabled)
        {
            _vSync = enabled;
            QualitySettings.vSyncCount = enabled ? 1 : 0;
            SaveSettings();
        }

        public void SetTargetFrameRate(int fps)
        {
            _targetFrameRate = fps;
            Application.targetFrameRate = fps;
            SaveSettings();
        }

        #endregion

        #region Gameplay

        public bool CameraShake => _cameraShake;
        public bool ShowDamageNumbers => _showDamageNumbers;

        public void SetCameraShake(bool enabled)
        {
            _cameraShake = enabled;
            SaveSettings();

            this.Raise(new SettingsChangedEvent { SettingType = "CameraShake", Value = enabled ? 1f : 0f });
        }

        public void SetShowDamageNumbers(bool enabled)
        {
            _showDamageNumbers = enabled;
            SaveSettings();

            this.Raise(new SettingsChangedEvent { SettingType = "DamageNumbers", Value = enabled ? 1f : 0f });
        }

        #endregion

        #region Persistence

        public void SaveSettings()
        {
            
            PlayerPrefs.SetFloat(KEY_MASTER_VOLUME, _masterVolume);
            PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, _musicVolume);
            PlayerPrefs.SetFloat(KEY_SFX_VOLUME, _sfxVolume);

            PlayerPrefs.SetInt(KEY_QUALITY, _qualityLevel);
            PlayerPrefs.SetInt(KEY_RESOLUTION_WIDTH, Screen.width);
            PlayerPrefs.SetInt(KEY_RESOLUTION_HEIGHT, Screen.height);
            PlayerPrefs.SetInt(KEY_FULLSCREEN, Screen.fullScreen ? 1 : 0);
            PlayerPrefs.SetInt(KEY_VSYNC, _vSync ? 1 : 0);
            PlayerPrefs.SetInt(KEY_TARGET_FPS, _targetFrameRate);

            PlayerPrefs.SetInt(KEY_CAMERA_SHAKE, _cameraShake ? 1 : 0);
            PlayerPrefs.SetInt(KEY_DAMAGE_NUMBERS, _showDamageNumbers ? 1 : 0);

            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            if (_settingsData == null)
            {
                Debug.LogWarning("SettingsData not assigned! Using hardcoded defaults.");
                LoadDefaults();
                return;
            }

            _masterVolume = PlayerPrefs.GetFloat(KEY_MASTER_VOLUME, _settingsData.defaultMasterVolume);
            _musicVolume = PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, _settingsData.defaultMusicVolume);
            _sfxVolume = PlayerPrefs.GetFloat(KEY_SFX_VOLUME, _settingsData.defaultSFXVolume);

            _qualityLevel = PlayerPrefs.GetInt(KEY_QUALITY, _settingsData.defaultQualityLevel);
            _vSync = PlayerPrefs.GetInt(KEY_VSYNC, _settingsData.defaultVSync ? 1 : 0) == 1;
            _targetFrameRate = PlayerPrefs.GetInt(KEY_TARGET_FPS, _settingsData.defaultTargetFrameRate);

            _cameraShake = PlayerPrefs.GetInt(KEY_CAMERA_SHAKE, _settingsData.defaultCameraShake ? 1 : 0) == 1;
            _showDamageNumbers = PlayerPrefs.GetInt(KEY_DAMAGE_NUMBERS, _settingsData.defaultShowDamageNumbers ? 1 : 0) == 1;
        }

        private void LoadDefaults()
        {
            _masterVolume = 1f;
            _musicVolume = 0.7f;
            _sfxVolume = 0.8f;
            _qualityLevel = 2;
            _vSync = true;
            _targetFrameRate = 60;
            _cameraShake = true;
            _showDamageNumbers = true;
        }

        private void ApplyAllSettings()
        {
            
            ApplyMasterVolume();
            ApplyMusicVolume();
            ApplySFXVolume();

            QualitySettings.SetQualityLevel(_qualityLevel, true);
            QualitySettings.vSyncCount = _vSync ? 1 : 0;
            Application.targetFrameRate = _targetFrameRate;

            ApplyCursor();
        }

        public void ResetToDefaults()
        {
            if (_settingsData != null)
            {
                _masterVolume = _settingsData.defaultMasterVolume;
                _musicVolume = _settingsData.defaultMusicVolume;
                _sfxVolume = _settingsData.defaultSFXVolume;
                _qualityLevel = _settingsData.defaultQualityLevel;
                _vSync = _settingsData.defaultVSync;
                _targetFrameRate = _settingsData.defaultTargetFrameRate;
                _cameraShake = _settingsData.defaultCameraShake;
                _showDamageNumbers = _settingsData.defaultShowDamageNumbers;
            }
            else
            {
                LoadDefaults();
            }

            ApplyAllSettings();
            SaveSettings();

            Debug.Log("Settings reset to defaults");
        }

        #endregion
    }

    public struct SettingsChangedEvent : IEvent
    {
        public string SettingType;
        public float Value;
    }

    public struct AudioVolumeChangedEvent : IEvent
    {
        public string VolumeType;
        public float Volume;
    }
}
