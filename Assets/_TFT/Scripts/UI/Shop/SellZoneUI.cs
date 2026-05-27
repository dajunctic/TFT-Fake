using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Dajunctic
{

    public class SellZoneUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static bool IsPointerOverSellZone { get; private set; }

        [Header("Settings")]
        [SerializeField] private GameObject sellVisual; 
        [SerializeField] private TMP_Text sellPriceText; 

        private ChampionActor _currentDraggedHero;

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
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerOverSellZone = false;
            
            if (sellVisual != null)
                sellVisual.SetActive(false);

        }
    }
}
