using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dajunctic
{
    // Class gọn nhẹ chỉ có HP Bar và Level Text cho Player
    public class PlayerHpView : BaseView
    {
        [Header("References")]
        [SerializeField] private Image progressBar; // Thanh máu Player (Image type Filled)
        [SerializeField] private TMP_Text levelText;  // Hiển thị Level
        [SerializeField] private Vector3 offset = new Vector3(0, 2.2f, 0);

        private CombatActor _owner;
        private Camera _mainCamera;
        private Transform _cachedTransform;

        public void Initialize(CombatActor owner, int level)
        {
            _owner = owner;
            _mainCamera = Camera.main;
            _cachedTransform = transform;

            if (levelText != null) 
                levelText.text = level.ToString();

            if (_owner != null)
            {
                _owner.OnHpChanged += UpdateHp;
                UpdateHp(_owner.MaxHp > 0 ? _owner.Hp / _owner.MaxHp : 1f);
            }
        }

        private void UpdateHp(float ratio)
        {
            if (progressBar != null)
            {
                progressBar.fillAmount = ratio;
            }
        }

        public void LateUpdate()
        {
            if (_owner == null) return;

            _cachedTransform.position = _owner.HeadPoint + offset;
            // Debug.LogError(_cachedTransform.position);

            if (_mainCamera != null)
            {
                _cachedTransform.rotation = _mainCamera.transform.rotation;
            }
        }

        private void OnDisable()
        {
            if (_owner != null)
            {
                _owner.OnHpChanged -= UpdateHp;
            }
        }
    }
}
