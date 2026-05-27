using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class ChampionItemView : MonoBehaviour
    {
        [SerializeField] private Image[] itemIcons; 
        private ChampionActor _hero;

        public void Initialize(ChampionActor hero)
        {
            _hero = hero;
            foreach (var icon in itemIcons) icon.gameObject.SetActive(false);
            
            this.RegisterListener<ChampionItemsChangedEvent>(OnItemsChanged);
        }

        private void OnDestroy()
        {
            this.RemoveListener<ChampionItemsChangedEvent>(OnItemsChanged);
        }

        private void OnItemsChanged(ChampionItemsChangedEvent evt)
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
