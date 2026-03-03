using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TraitDescriptionUI : BaseView
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

        private void LateUpdate()
        {
            if (!_isShowing) return;
            UpdatePosition(Input.mousePosition);
        }

        private void OnTraitHover(TraitHoverEvent evt)
        {
            Show(evt.Trait, evt.Count);
        }

        private void OnTraitHoverExit(TraitHoverExitEvent evt)
        {
            Hide();
        }

        private void UpdatePosition(Vector2 screenPos)
        {
            if (_parentCanvas == null || _rectTransform == null || _rectTransform.parent == null) return;

            // Convert screen position to canvas local position relative to our parent
            RectTransform parentRect = _rectTransform.parent as RectTransform;
            if (parentRect == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPos,
                _parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _parentCanvas.worldCamera,
                out var localPoint
            );

            Vector2 panelSize = _rectTransform.rect.size;
            Vector2 canvasSize = _canvasRectTransform.rect.size;
            
            // Place to the right of cursor by default
            Vector2 targetPos = localPoint + offset;

            // Check boundaries relative to parent/canvas
            // Simplified clamping: just make sure it stays on screen
            float halfW = panelSize.x;
            
            if (targetPos.x + halfW > canvasSize.x * 0.5f) // Rough check against canvas edge
            {
                targetPos.x = localPoint.x - offset.x - panelSize.x;
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
            _canvasGroup.interactable = false; // Tooltips shouldn't be interactable
            _canvasGroup.blocksRaycasts = false; // IMPORTANT: prevent tooltip from blocking the mouse for the trait icons
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
    }

    public struct TraitHoverExitEvent : IEvent { }
}
