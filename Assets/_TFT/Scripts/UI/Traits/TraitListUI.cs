using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dajunctic
{
    public class TraitListUI : BaseView
    {
        [SerializeField] private GameObject traitItemPrefab;
        [SerializeField] private Transform container;

        private List<TraitItemUI> _pool = new List<TraitItemUI>();

        public override void Initialize()
        {
            base.Initialize();
            for (var i = 0; i < container.childCount; i++)
            {
                var child = container.GetChild(i);

                Destroy(child.gameObject);
            }

        }

        public override void DoEnable()
        {
            base.DoEnable();
            var traitSystem = GameSystemManager.Instance?.Traits;
            if (traitSystem != null)
            {
                UpdateTraits(traitSystem.ActiveTraitCounts);
            }
        }

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<TraitCountsChangedEvent>(OnTraitCountsChanged);
        }

        public override void StopListenEvents()
        {
            base.StopListenEvents();
            this.RemoveListener<TraitCountsChangedEvent>(OnTraitCountsChanged);
        }

        private void OnTraitCountsChanged(TraitCountsChangedEvent evt)
        {
            UpdateTraits(evt.ActiveTraitCounts);
        }

        private void UpdateTraits(Dictionary<ITrait, int> traitCounts)
        {

            var sortedTraits = traitCounts
                .Select(kvp => new { Trait = kvp.Key as TraitData, Count = kvp.Value })
                .Where(x => x.Trait != null)
                .Select(x => new
                {
                    x.Trait,
                    x.Count,
                    ActiveTier = x.Trait.Tiers
                        .Where(t => x.Count >= t.RequiredCount)
                        .OrderByDescending(t => t.RequiredCount)
                        .FirstOrDefault()
                })
                .OrderByDescending(x => x.ActiveTier != null)
                .ThenByDescending(x => x.ActiveTier?.VisualTier ?? TraitTierType.None)
                .ThenByDescending(x => x.Count)
                .ToList();

            int i = 0;
            foreach (var item in sortedTraits)
            {
                if (item.ActiveTier == null) continue;

                var ui = GetOrCreateItem(i++);
                ui.gameObject.SetActive(true);
                ui.Setup(item.Trait, item.Count);
            }

            for (; i < _pool.Count; i++)
            {
                _pool[i].gameObject.SetActive(false);
            }
        }

        private TraitItemUI GetOrCreateItem(int index)
        {
            if (index < _pool.Count) return _pool[index];

            var go = Instantiate(traitItemPrefab, container);
            var ui = go.GetComponent<TraitItemUI>();
            _pool.Add(ui);
            return ui;
        }
    }
}
