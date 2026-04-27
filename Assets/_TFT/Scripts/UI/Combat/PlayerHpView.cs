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

        [Header("Colors")]
        public Color localPlayerColor = new Color(0.2f, 0.8f, 0.2f); // Green
        public Color enemyPlayerColor = new Color(0.8f, 0.2f, 0.2f); // Red
        [Tooltip("If filled, each OwnerID gets a distinct color from this list.")]
        public Color[] customColorsPerPlayer = new Color[]
        {
            new Color(0.12f, 0.82f, 0.28f), // 0: Player (TFT Green)
            new Color(0.85f, 0.15f, 0.22f), // 1: Bot 1 (TFT Red)
            new Color(0.15f, 0.55f, 0.95f), // 2: Bot 2 (Blue)
            new Color(0.95f, 0.75f, 0.15f), // 3: Bot 3 (Gold)
            new Color(0.65f, 0.25f, 0.85f), // 4: Bot 4 (Purple)
            new Color(0.95f, 0.45f, 0.15f), // 5: Bot 5 (Orange)
            new Color(0.15f, 0.85f, 0.85f), // 6: Bot 6 (Cyan)
            new Color(0.95f, 0.35f, 0.65f)  // 7: Bot 7 (Pink)
        };

        private CombatActor _owner;
        private Camera _mainCamera;
        private Transform _cachedTransform;

        private int _currentOwnerId = -1;

        public void Initialize(CombatActor owner, int level)
        {
            // Reset Canvas worldCamera to prevent Unity UI MissingReferenceException if previous camera was destroyed
            var canvas = GetComponent<Canvas>();
            if (canvas != null) canvas.worldCamera = Camera.main;

            _owner = owner;
            _mainCamera = Camera.main;
            _cachedTransform = transform;

            if (levelText != null) 
                levelText.text = level.ToString();

            UpdateColor();

            if (_owner != null)
            {
                _owner.OnHpChanged += UpdateHp;
                UpdateHp(_owner.MaxHp > 0 ? _owner.Hp / _owner.MaxHp : 1f);
            }
        }

        private void UpdateColor()
        {
            if (progressBar != null && _owner != null)
            {
                _currentOwnerId = _owner.OwnerID;
                if (customColorsPerPlayer != null && customColorsPerPlayer.Length > 0)
                {
                    progressBar.color = customColorsPerPlayer[_currentOwnerId % customColorsPerPlayer.Length];
                }
                else
                {
                    Color[] fallbackPalette = new Color[]
                    {
                        new Color(0.12f, 0.82f, 0.28f), // 0: Player (TFT Green)
                        new Color(0.85f, 0.15f, 0.22f), // 1: Bot 1 (TFT Red)
                        new Color(0.15f, 0.55f, 0.95f), // 2: Bot 2 (Blue)
                        new Color(0.95f, 0.75f, 0.15f), // 3: Bot 3 (Gold)
                        new Color(0.65f, 0.25f, 0.85f), // 4: Bot 4 (Purple)
                        new Color(0.95f, 0.45f, 0.15f), // 5: Bot 5 (Orange)
                        new Color(0.15f, 0.85f, 0.85f), // 6: Bot 6 (Cyan)
                        new Color(0.95f, 0.35f, 0.65f)  // 7: Bot 7 (Pink)
                    };
                    progressBar.color = fallbackPalette[_currentOwnerId % fallbackPalette.Length];
                }
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

            if (_mainCamera != null)
            {
                _cachedTransform.rotation = _mainCamera.transform.rotation;
            }

            if (_owner.OwnerID != _currentOwnerId)
            {
                UpdateColor();
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
