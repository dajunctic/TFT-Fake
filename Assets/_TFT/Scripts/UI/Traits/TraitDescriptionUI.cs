using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dajunctic
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TraitDescriptionUI : BaseView, IPointerExitHandler
    {
        [Header("UI References")]
        [SerializeField] private Image traitIcon;
        [SerializeField] private TMP_Text traitNameText;
        [SerializeField] private TMP_Text traitDescriptionText;
        [SerializeField] private TMP_Text tierDescriptionText;
        [SerializeField] private Transform championIconContainer;
        [SerializeField] private GameObject championIconPrefab;

        [Header("Follow Settings")]
        [SerializeField] private Vector2 offset = new Vector2(20f, 0f);

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Canvas _parentCanvas;
        private RectTransform _canvasRectTransform;
        private bool _isShowing;
        private RectTransform _currentTriggerRect;

        public override void Initialize()
        {
            base.Initialize();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            _rectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>().rootCanvas;
            _canvasRectTransform = _parentCanvas.GetComponent<RectTransform>();

            Hide();
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<TraitHoverEvent>(OnTraitHover);
            this.RegisterListener<TraitHoverExitEvent>(OnTraitHoverExit);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<TraitHoverEvent>(OnTraitHover);
            this.RemoveListener<TraitHoverExitEvent>(OnTraitHoverExit);
        }

        private void Update()
        {
            if (!_isShowing) return;

            // Check if mouse is over the tooltip OR the triggering icon
            bool overTooltip = RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition, 
                _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera);
            
            bool overTrigger = _currentTriggerRect != null && RectTransformUtility.RectangleContainsScreenPoint(_currentTriggerRect, Input.mousePosition, 
                _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera);

            if (!overTooltip && !overTrigger)
            {
                Hide();
            }
        }

        private void OnTraitHover(TraitHoverEvent evt)
        {
            _currentTriggerRect = evt.Trigger;
            Show(evt.Trait, evt.Count);
        }

        private void OnTraitHoverExit(TraitHoverExitEvent evt)
        {
            Hide();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        private void UpdatePosition(Vector2 screenPos)
        {
            if (_parentCanvas == null || _rectTransform == null || _rectTransform.parent == null) return;

            RectTransform parentRect = _rectTransform.parent as RectTransform;
            if (parentRect == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPos,
                _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera,
                out var cursorLocalPoint
            );

            Vector2 panelSize = _rectTransform.rect.size;
            Vector2 canvasSize = _canvasRectTransform.rect.size;
            Vector2 pivot = _rectTransform.pivot;
            
            // Goal: Place the RIGHT edge of the tooltip to the LEFT of the cursor
            // targetPos is the anchoredPosition (the pivot point)
            // pivot.x = 0 means targetPos is the left edge
            // pivot.x = 1 means targetPos is the right edge
            // pivot.x = 0.5 means targetPos is the center
            
            // To put the right edge at (cursor.x - offset.x):
            // RightEdge = targetPos.x + (1 - pivot.x) * panelSize.x
            // targetPos.x = (cursor.x - offset.x) - (1 - pivot.x) * panelSize.x
            
            float targetX = (cursorLocalPoint.x - offset.x) - (1 - pivot.x) * panelSize.x;
            
            // Center the tooltip vertically relative to cursor, accounting for pivot
            // CenterY = targetPos.y + (0.5 - pivot.y) * panelSize.y
            // targetPos.y = cursor.y - (0.5 - pivot.y) * panelSize.y
            float targetY = cursorLocalPoint.y - (0.5f - pivot.y) * panelSize.y;

            Vector2 targetPos = new Vector2(targetX, targetY);

            // Check boundaries (clamping)
            float leftEdge = targetPos.x - pivot.x * panelSize.x;
            float canvasLeft = -canvasSize.x * 0.5f;
            
            // If it goes off the left edge, flip to the right side
            if (leftEdge < canvasLeft) 
            {
                // LeftEdge of tooltip = cursor.x + offset.x
                // targetPos.x - pivot.x * panelSize.x = cursor.x + offset.x
                targetPos.x = cursorLocalPoint.x + offset.x + pivot.x * panelSize.x;
            }

            // Vertical clamping
            float halfCanvasHeight = canvasSize.y * 0.5f;
            float topEdge = targetPos.y + (1 - pivot.y) * panelSize.y;
            float bottomEdge = targetPos.y - pivot.y * panelSize.y;

            if (topEdge > halfCanvasHeight)
                targetPos.y -= (topEdge - halfCanvasHeight + 10f);
            if (bottomEdge < -halfCanvasHeight)
                targetPos.y += (-halfCanvasHeight - bottomEdge + 10f);
            
            // Re-check left/right flip with more margin to avoid overlap
            float currentLeftEdge = targetPos.x - pivot.x * panelSize.x;
            if (currentLeftEdge < -canvasSize.x * 0.5f)
            {
                // Move it sufficiently to the right of the trigger icon
                targetPos.x = cursorLocalPoint.x + offset.x + 40f + pivot.x * panelSize.x;
            }

            _rectTransform.anchoredPosition = targetPos;
        }

        public void Show(TraitData trait, int count)
        {
            if (trait == null) return;

            _isShowing = true;
            SetVisible(true);

            // Set icon
            if (traitIcon != null && trait.Icon != null)
                traitIcon.sprite = trait.Icon;

            // Set name
            if (traitNameText != null)
                traitNameText.text = trait.DisplayName;

            // Build description text
            if (traitDescriptionText != null)
            {
                var activeTier = trait.Tiers
                    .Where(t => count >= t.RequiredCount)
                    .OrderByDescending(t => t.RequiredCount)
                    .FirstOrDefault();

                string desc = activeTier?.SpecialEffectDescription ?? "";
                traitDescriptionText.text = desc;
            }

            // Build tier milestones text
            if (tierDescriptionText != null)
            {
                tierDescriptionText.text = BuildTierText(trait, count);
            }

            // Populate champion icons
            PopulateChampionIcons(trait);

            // Immediately position at current mouse
            UpdatePosition(Input.mousePosition);
        }

        public void Hide()
        {
            _isShowing = false;
            SetVisible(false);
        }

        private void SetVisible(bool visible)
        {
            if (_canvasGroup == null) return;
            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.interactable = visible; 
            _canvasGroup.blocksRaycasts = visible; // Enable raycasts when visible to detect mouse exit
        }

        private string BuildTierText(TraitData trait, int currentCount)
        {
            var sb = new StringBuilder();

            foreach (var tier in trait.Tiers.OrderBy(t => t.RequiredCount))
            {
                bool isActive = currentCount >= tier.RequiredCount;
                string color = isActive ? "#FFD700" : "#888888";
                string arrow = isActive ? "▶ " : "   ";

                sb.Append($"<color={color}>{arrow}({tier.RequiredCount})");

                if (!string.IsNullOrEmpty(tier.SpecialEffectDescription))
                {
                    sb.Append($" {tier.SpecialEffectDescription}");
                }
                else if (tier.StatModifiers != null && tier.StatModifiers.Count > 0)
                {
                    var modDesc = string.Join(", ", ((TraitTierData)tier).statModifiers
                        .Select(m => $"{GetStatName(m.statType)} +{m.value}{(m.type == StatModType.PercentAdd ? "%" : "")}"));
                    sb.Append($" {modDesc}");
                }

                sb.AppendLine("</color>");
            }

            return sb.ToString().TrimEnd();
        }

        private void PopulateChampionIcons(TraitData trait)
        {
            if (championIconContainer == null) return;

            foreach (Transform child in championIconContainer)
            {
                child.gameObject.SetActive(false);
            }

            if (championIconPrefab == null) return;

            var fieldSystem = GameSystemManager.Instance?.Field;
            if (fieldSystem == null) return;

            var heroes = fieldSystem.GetAllHeroes();
            var matchingHeroes = heroes
                .Where(h => h.Traits.Any(t => t.TraidID == trait.TraidID))
                .GroupBy(h => h.ChampionId)
                .Select(g => g.First())
                .ToList();

            int iconIndex = 0;
            foreach (var hero in matchingHeroes)
            {
                GameObject iconGO;
                if (iconIndex < championIconContainer.childCount)
                {
                    iconGO = championIconContainer.GetChild(iconIndex).gameObject;
                    iconGO.SetActive(true);
                }
                else
                {
                    iconGO = Instantiate(championIconPrefab, championIconContainer);
                }

                var img = iconGO.GetComponent<Image>();
                if (img != null && hero.CombatActorData is ChampionData championData)
                {
                    img.sprite = championData.shopIcon;
                }

                iconIndex++;
            }
        }

        private string GetStatName(StatType type)
        {
            return type switch
            {
                StatType.Health => "HP",
                StatType.Armor => "Giáp",
                StatType.MagicResist => "Kháng Phép",
                StatType.AttackDamage => "Sát Thương",
                StatType.AbilityPower => "Sức Mạnh",
                StatType.AttackSpeed => "Tốc Đánh",
                StatType.AttackRange => "Tầm Đánh",
                StatType.CriticalStrikeChance => "Tỉ Lệ Chí Mạng",
                StatType.CriticalStrikeDamage => "Sát Thương Chí Mạng",
                StatType.StartingMana => "Năng Lượng",
                StatType.MaxMana => "Năng Lượng Tối Đa",
                _ => type.ToString()
            };
        }
    }

    // Events for trait hover
    public struct TraitHoverEvent : IEvent
    {
        public TraitData Trait;
        public int Count;
        public RectTransform Trigger;
    }

    public struct TraitHoverExitEvent : IEvent { }
}
