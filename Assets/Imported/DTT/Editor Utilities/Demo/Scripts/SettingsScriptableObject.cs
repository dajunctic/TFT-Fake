using System;
using UnityEngine;

public class SettingsScriptableObject : ScriptableObject
{
    [Serializable]
    private struct SubSetting
    {
        public string name;

        public int value;
    }
    
    [SerializeField]
    private string _setting;

    [SerializeField]
    private int _anotherSetting;

    [SerializeField]
    private float _andAnotherSetting;

    [SerializeField]
    private SubSetting _subSetting;

    public void UpdateSetting(string newSetting) => _setting = newSetting;
}
