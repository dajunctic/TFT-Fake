using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Dajunctic
{
    public class SettingsPopup : BasePopup
    {
        [Header("Audio Sliders")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI masterVolumeText;
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private TextMeshProUGUI sfxVolumeText;

        [Header("Graphics")]
        [SerializeField] private TMP_Dropdown qualityDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;

        [Header("Gameplay")]
        [SerializeField] private Toggle cameraShakeToggle;
        [SerializeField] private Toggle damageNumbersToggle;

        [Header("Buttons")]
        [SerializeField] private Button applyButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button closeButton;

        private SettingsSystem _settingsSystem;
        private Resolution[] _resolutions;

        public override void BeforeShow(object data = null)
        {
            base.BeforeShow(data);
            _settingsSystem = GameSystemManager.Instance?.Settings;
            
            if (_settingsSystem == null)
            {
                Debug.LogWarning("SettingsSystem not found! Settings will not be functional.");
            }

            InitializeUI();
            LoadCurrentSettings();
            RegisterListeners();
        }

        public override void BeforeDismiss()
        {
            base.BeforeDismiss();
            UnregisterListeners();
        }

        private void InitializeUI()
        {
            
            if (qualityDropdown != null)
            {
                qualityDropdown.ClearOptions();
                qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
            }

            if (resolutionDropdown != null)
            {
                _resolutions = Screen.resolutions;
                resolutionDropdown.ClearOptions();
                
                var options = new List<string>();
                int currentResolutionIndex = 0;
                
                for (int i = 0; i < _resolutions.Length; i++)
                {
                    string option = $"{_resolutions[i].width} x {_resolutions[i].height} @ {_resolutions[i].refreshRateRatio}Hz";
                    options.Add(option);
                    
                    if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = i;
                    }
                }
                
                resolutionDropdown.AddOptions(options);
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
            }
        }

        private void LoadCurrentSettings()
        {
            if (_settingsSystem == null) return;

            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = _settingsSystem.MasterVolume;
                UpdateVolumeText(masterVolumeText, _settingsSystem.MasterVolume);
            }
            
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = _settingsSystem.MusicVolume;
                UpdateVolumeText(musicVolumeText, _settingsSystem.MusicVolume);
            }
            
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = _settingsSystem.SFXVolume;
                UpdateVolumeText(sfxVolumeText, _settingsSystem.SFXVolume);
            }

            if (qualityDropdown != null)
                qualityDropdown.value = _settingsSystem.QualityLevel;
            
            if (fullscreenToggle != null)
                fullscreenToggle.isOn = Screen.fullScreen;
            
            if (vSyncToggle != null)
                vSyncToggle.isOn = _settingsSystem.VSync;

            if (cameraShakeToggle != null)
                cameraShakeToggle.isOn = _settingsSystem.CameraShake;
            
            if (damageNumbersToggle != null)
                damageNumbersToggle.isOn = _settingsSystem.ShowDamageNumbers;
        }

        private void RegisterListeners()
        {
            
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            if (qualityDropdown != null)
                qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
            
            if (resolutionDropdown != null)
                resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
            
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
            
            if (vSyncToggle != null)
                vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);

            if (cameraShakeToggle != null)
                cameraShakeToggle.onValueChanged.AddListener(OnCameraShakeChanged);
            
            if (damageNumbersToggle != null)
                damageNumbersToggle.onValueChanged.AddListener(OnDamageNumbersChanged);

            if (applyButton != null)
                applyButton.onClick.AddListener(OnApplyClicked);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(OnResetClicked);
            
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
        }

        #region Audio Callbacks

        private void OnMasterVolumeChanged(float value)
        {
            _settingsSystem?.SetMasterVolume(value);
            UpdateVolumeText(masterVolumeText, value);
        }

        private void OnMusicVolumeChanged(float value)
        {
            _settingsSystem?.SetMusicVolume(value);
            UpdateVolumeText(musicVolumeText, value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            _settingsSystem?.SetSFXVolume(value);
            UpdateVolumeText(sfxVolumeText, value);
        }

        private void UpdateVolumeText(TextMeshProUGUI text, float volume)
        {
            if (text != null)
                text.text = $"{Mathf.RoundToInt(volume * 100)}%";
        }

        #endregion

        #region Graphics Callbacks

        private void OnQualityChanged(int index)
        {
            _settingsSystem?.SetQualityLevel(index);
        }

        private void OnResolutionChanged(int index)
        {
            if (_resolutions != null && index < _resolutions.Length)
            {
                Resolution resolution = _resolutions[index];
                _settingsSystem?.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            }
        }

        private void OnFullscreenChanged(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            _settingsSystem?.SaveSettings();
        }

        private void OnVSyncChanged(bool enabled)
        {
            _settingsSystem?.SetVSync(enabled);
        }

        #endregion

        #region Gameplay Callbacks

        private void OnCameraShakeChanged(bool enabled)
        {
            _settingsSystem?.SetCameraShake(enabled);
        }

        private void OnDamageNumbersChanged(bool enabled)
        {
            _settingsSystem?.SetShowDamageNumbers(enabled);
        }

        #endregion

        #region Button Callbacks

        private void OnApplyClicked()
        {
            _settingsSystem?.SaveSettings();
            Debug.Log("Settings applied and saved!");
        }

        private void OnResetClicked()
        {
            _settingsSystem?.ResetToDefaults();
            LoadCurrentSettings();
            Debug.Log("Settings reset to defaults!");
        }

        private void OnCloseClicked()
        {
            Dismiss();
        }

        #endregion

        private void UnregisterListeners()
        {
            
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);

            if (qualityDropdown != null)
                qualityDropdown.onValueChanged.RemoveListener(OnQualityChanged);
            
            if (resolutionDropdown != null)
                resolutionDropdown.onValueChanged.RemoveListener(OnResolutionChanged);
            
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
            
            if (vSyncToggle != null)
                vSyncToggle.onValueChanged.RemoveListener(OnVSyncChanged);

            if (cameraShakeToggle != null)
                cameraShakeToggle.onValueChanged.RemoveListener(OnCameraShakeChanged);
            
            if (damageNumbersToggle != null)
                damageNumbersToggle.onValueChanged.RemoveListener(OnDamageNumbersChanged);

            if (applyButton != null)
                applyButton.onClick.RemoveListener(OnApplyClicked);
            
            if (resetButton != null)
                resetButton.onClick.RemoveListener(OnResetClicked);
            
            if (closeButton != null)
                closeButton.onClick.RemoveListener(OnCloseClicked);
        }
    }
}
