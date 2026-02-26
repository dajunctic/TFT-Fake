using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Dajunctic
{
    /// <summary>
    /// This component should be attached to the UI panel that represents the Shop area.
    /// It detects when a hero is being dragged over it and shows the sell UI.
    /// </summary>
    public class SellZoneUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static bool IsPointerOverSellZone { get; private set; }

        [Header("Settings")]
        [SerializeField] private GameObject sellVisual; // The panel that says "SELL"
        [SerializeField] private TMP_Text sellPriceText; // Text to show refund amount

        private HeroCombatActor _currentDraggedHero;

        private void Awake()
        {
            if (sellVisual != null)
                sellVisual.SetActive(false);
            
            this.RegisterListener<HeroDragStartedEvent>(OnDragStarted);
            this.RegisterListener<HeroDragEndedEvent>(OnDragEnded);
        }

        private void OnDestroy()
        {
            this.RemoveListener<HeroDragStartedEvent>(OnDragStarted);
            this.RemoveListener<HeroDragEndedEvent>(OnDragEnded);
            
            // Clean up static state if this object is destroyed
            IsPointerOverSellZone = false;
        }

        private void OnDragStarted(HeroDragStartedEvent evt)
        {
            _currentDraggedHero = evt.Hero;
        }

        private void OnDragEnded(HeroDragEndedEvent evt)
        {
            _currentDraggedHero = null;
            IsPointerOverSellZone = false;
            
            if (sellVisual != null)
                sellVisual.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_currentDraggedHero == null) return;

            IsPointerOverSellZone = true;
            
            if (sellVisual != null)
                sellVisual.SetActive(true);

            if (sellPriceText != null && _currentDraggedHero != null)
            {
                int gold = _currentDraggedHero.GetSellValue();
                sellPriceText.text = $"+{gold}<sprite=0>";
            }
            
            Debug.Log("<color=orange>Mouse entered Sell Zone while dragging hero</color>");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerOverSellZone = false;
            
            if (sellVisual != null)
                sellVisual.SetActive(false);
            
            // Debug.Log("<color=orange>Mouse left Sell Zone</color>");
        }
    }
}
