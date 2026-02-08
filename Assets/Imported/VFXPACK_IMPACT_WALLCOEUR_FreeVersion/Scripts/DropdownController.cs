namespace Assets.VFXPACK_IMPACT_WALLCOEUR.Scripts
{
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    
    public class DropdownController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] VfxController _vfxController;
        [SerializeField] TMP_Dropdown _dropdown;

        void Start()
        {
            GenerateDropdown();
        }

        void GenerateDropdown()
        {
            List<string> options = new();

            _dropdown.ClearOptions();

            foreach (GameObject vfx in _vfxController.VfxList)
            {
                options.Add(vfx.name);
            }

            _dropdown.AddOptions(options);
            _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

        void OnDropdownValueChanged(int index)
        {
            PlayVfx();
        }

        public void PlayVfx()
        {
            _vfxController.Play(_dropdown.value);
        }

        public void StopVfx()
        {
            _vfxController.Stop();
        }

        public void PreviousVfx()
        {
            if (_dropdown.value == 0)
            {
                return;
            }

            _dropdown.value--;
            _dropdown.RefreshShownValue();
        }

        public void NextVfx()
        {
            if (_dropdown.value == _dropdown.options.Count)
            {
                return;
            }

            _dropdown.value++;
            _dropdown.RefreshShownValue();
        }
    }
}