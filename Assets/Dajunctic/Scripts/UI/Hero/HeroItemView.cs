using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class HeroItemView : MonoBehaviour
    {
        [SerializeField] private Image[] itemIcons; // Array of 3 images
        private HeroCombatActor _hero;

        public void Initialize(HeroCombatActor hero)
        {
            _hero = hero;
            foreach (var icon in itemIcons) icon.gameObject.SetActive(false);
            
            this.RegisterListener<HeroItemsChangedEvent>(OnItemsChanged);
        }

        private void OnDestroy()
        {
            this.RemoveListener<HeroItemsChangedEvent>(OnItemsChanged);
        }

        private void OnItemsChanged(HeroItemsChangedEvent evt)
        {
            if (evt.Hero != _hero) return;

            for (int i = 0; i < itemIcons.Length; i++)
            {
                if (i < evt.Items.Count)
                {
                    itemIcons[i].gameObject.SetActive(true);
                    itemIcons[i].sprite = evt.Items[i].icon;
                }
                else
                {
                    itemIcons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
