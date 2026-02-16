using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(fileName = "SettingsData", menuName = "Dajunctic/Settings/SettingsData")]
    public class SettingsData : ScriptableObject
    {
        [Header("Mouse Cursor")]
        public Texture2D cursorNormal;
        public Texture2D cursorHover;
        public Texture2D cursorClick;
        public Vector2 cursorHotspot = new Vector2(0, 0);

        [Header("Default Audio Settings")]
        [Range(0f, 1f)] public float defaultMasterVolume = 1f;
        [Range(0f, 1f)] public float defaultMusicVolume = 0.7f;
        [Range(0f, 1f)] public float defaultSFXVolume = 0.8f;

        [Header("Default Graphics Settings")]
        public int defaultQualityLevel = 2; // Medium
        public int defaultTargetFrameRate = 60;
        public bool defaultVSync = true;

        [Header("Default Gameplay Settings")]
        public bool defaultCameraShake = true;
        public bool defaultShowDamageNumbers = true;
    }
}
