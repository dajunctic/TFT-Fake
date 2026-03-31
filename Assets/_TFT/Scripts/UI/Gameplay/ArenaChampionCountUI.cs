using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

namespace Dajunctic
{
    public class ArenaChampionCountUI : PoolableObject
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _championCountText;
        [SerializeField] private CanvasGroup _canvasGroup; // Optional: Can use this or simply enable/disable the object
        [SerializeField] private Image _iconImage;

        [Header("Colors")]
        [SerializeField] private Color _normalColor = new Color(0.06f, 1f, 0.91f); // Neon Cyan
        [SerializeField] private Color _overLimitColor = Color.red;

        private Arena _parentArena;
        private bool _isDraggingAnyUnit;

        private void Awake()
        {
            if (_championCountText == null)
            {
                // Try to find if not assigned
                var texts = GetComponentsInChildren<TextMeshProUGUI>();
                _championCountText = texts.FirstOrDefault(t => t.name.Contains("num_champion_txt"));
            }

            if (_iconImage == null)
            {
                var images = GetComponentsInChildren<Image>();
                _iconImage = images.FirstOrDefault(i => i.name.Contains("icon_txt"));
            }

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            HideUI();
        }

        public void Initialize(Arena parentArena)
        {
            _parentArena = parentArena;
        }

        private void OnEnable()
        {
            Gameplay.OnPhaseChanged += HandlePhaseChanged;
            DragManager.OnGlobalDragStart += HandleGlobalDragStart;
            DragManager.OnGlobalDragEnd += HandleGlobalDragEnd;
        }

        private void OnDisable()
        {
            Gameplay.OnPhaseChanged -= HandlePhaseChanged;
            DragManager.OnGlobalDragStart -= HandleGlobalDragStart;
            DragManager.OnGlobalDragEnd -= HandleGlobalDragEnd;
        }

        private void Update()
        {
            if (GameSystemManager.Instance == null || Gameplay.Instance == null || _parentArena == null) return;

            // Only update logic in Planning phase
            if (Gameplay.Instance.CurrentPhase != GameplayPhase.Planning)
            {
                if (_canvasGroup.alpha > 0) HideUI();
                return;
            }

            // Get current and max counts
            int ownerId = _parentArena.OwnerID;
            int currentUnits = GameSystemManager.Instance.Field.GetUnitCount(ownerId);
            int maxUnits = GameSystemManager.Instance.Economy != null ? GameSystemManager.Instance.Economy.Level : 0;

            bool shouldShow = _isDraggingAnyUnit || (currentUnits < maxUnits) || (currentUnits > maxUnits);

            if (shouldShow)
            {
                ShowUI();
                UpdateText(currentUnits, maxUnits);
            }
            else
            {
                HideUI();
            }
        }

        private void UpdateText(int currentCount, int maxCount)
        {
            if (_championCountText != null)
            {
                _championCountText.text = $"{currentCount}/{maxCount}";
                
                Color targetColor = currentCount > maxCount ? _overLimitColor : _normalColor;
                _championCountText.color = targetColor;
                
                if (_iconImage != null)
                {
                    _iconImage.color = targetColor;
                }
            }
        }

        private void HandleGlobalDragStart(IDraggable draggable)
        {
            // Optional: you can check if draggable belongs to this owner, but TFT shows it regardless
            _isDraggingAnyUnit = true;
        }

        private void HandleGlobalDragEnd(IDraggable draggable)
        {
            _isDraggingAnyUnit = false;
        }

        private void HandlePhaseChanged(GameplayPhase phase)
        {
            if (phase != GameplayPhase.Planning)
            {
                HideUI();
            }
        }

        private void ShowUI()
        {
            if (_canvasGroup != null) _canvasGroup.alpha = 1f;
        }

        private void HideUI()
        {
            if (_canvasGroup != null) _canvasGroup.alpha = 0f;
        }
    }
}
