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

            float targetX = (cursorLocalPoint.x - offset.x) - (1 - pivot.x) * panelSize.x;

            float targetY = cursorLocalPoint.y - (0.5f - pivot.y) * panelSize.y;

            Vector2 targetPos = new Vector2(targetX, targetY);

            float leftEdge = targetPos.x - pivot.x * panelSize.x;
            float canvasLeft = -canvasSize.x * 0.5f;

            if (leftEdge < canvasLeft) 
            {

                targetPos.x = cursorLocalPoint.x + offset.x + pivot.x * panelSize.x;
            }

            float halfCanvasHeight = canvasSize.y * 0.5f;
            float topEdge = targetPos.y + (1 - pivot.y) * panelSize.y;
            float bottomEdge = targetPos.y - pivot.y * panelSize.y;

            if (topEdge > halfCanvasHeight)
                targetPos.y -= (topEdge - halfCanvasHeight + 10f);
            if (bottomEdge < -halfCanvasHeight)
                targetPos.y += (-halfCanvasHeight - bottomEdge + 10f);

            float currentLeftEdge = targetPos.x - pivot.x * panelSize.x;
            if (currentLeftEdge < -canvasSize.x * 0.5f)
            {
                
                targetPos.x = cursorLocalPoint.x + offset.x + 40f + pivot.x * panelSize.x;
            }

            _rectTransform.anchoredPosition = targetPos;
        }

        public void Show(TraitData trait, int count)
        {
            if (trait == null) return;

            _isShowing = true;
            SetVisible(true);

            if (traitIcon != null && trait.Icon != null)
                traitIcon.sprite = trait.Icon;

            if (traitNameText != null)
                traitNameText.text = trait.DisplayName;

            if (traitDescriptionText != null)
            {
                var activeTier = trait.Tiers
                    .Where(t => count >= t.RequiredCount)
                    .OrderByDescending(t => t.RequiredCount)
                    .FirstOrDefault();

                string desc = activeTier?.SpecialEffectDescription ?? "";
                traitDescriptionText.text = desc;
            }

            if (tierDescriptionText != null)
            {
                tierDescriptionText.text = BuildTierText(trait, count);
            }

            PopulateChampionIcons(trait);

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
            _canvasGroup.blocksRaycasts = visible; 
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

    public struct TraitHoverEvent : IEvent
    {
        public TraitData Trait;
        public int Count;
        public RectTransform Trigger;
    }

    public struct TraitHoverExitEvent : IEvent { }
}
