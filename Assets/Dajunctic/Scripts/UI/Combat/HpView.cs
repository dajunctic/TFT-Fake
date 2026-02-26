using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class HpView : MonoBehaviour
    {
        [Header("Renderers")]
        [SerializeField] private Image bgRenderer;
        [SerializeField] private Image hpProgress;
        [SerializeField] private Image energyProgress;
        [SerializeField] private Image[] itemIcons;

        [Header("Settings")]
        [SerializeField] private SpriteLists starBgSprites;
        [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);

        private CombatActor _owner;
        private Transform _cachedTransform;

        private void Awake()
        {
            _cachedTransform = transform;
            foreach (var icon in itemIcons)
            {
                icon.gameObject.SetActive(false);
            }
        }

        public void Initialize(CombatActor owner, int starLevel)
        {
            _owner = owner;            
            UpdateStarLevel(starLevel);

            _owner.OnHpChanged += UpdateHp;
            _owner.OnEnergyChanged += UpdateEnergy;

            UpdateHp(_owner.MaxHp > 0 ? _owner.Hp / _owner.MaxHp : 1f);
            UpdateEnergy(_owner.MaxEnergy > 0 ? _owner.Energy / _owner.MaxEnergy : 0f);

            this.RegisterListener<HeroItemsChangedEvent>(OnItemsChanged);
        }

        public void UpdateStarLevel(int starLevel)
        {
            if (bgRenderer != null && starBgSprites != null)
            {
                bgRenderer.sprite = starBgSprites.GetIndex(starLevel - 1);
            }
        }

        private void OnDestroy()
        {
            if (_owner != null)
            {
                _owner.OnHpChanged -= UpdateHp;
                _owner.OnEnergyChanged -= UpdateEnergy;
            }
            this.RemoveListener<HeroItemsChangedEvent>(OnItemsChanged);
        }

        private void LateUpdate()
        {
            _cachedTransform.position = _owner.HeadPoint + offset;
            
            if (Camera.main != null)
            {
                _cachedTransform.rotation = Camera.main.transform.rotation;
            }
        }

        private void UpdateHp(float ratio)
        {
            if (hpProgress != null)
            {
                Vector3 scale = hpProgress.transform.localScale;
                scale.x = ratio;
                hpProgress.transform.localScale = scale;
            }
        }

        private void UpdateEnergy(float ratio)
        {
            if (energyProgress != null)
            {
                Vector3 scale = energyProgress.transform.localScale;
                scale.x = ratio;
                energyProgress.transform.localScale = scale;
            }
        }

        private void OnItemsChanged(HeroItemsChangedEvent evt)
        {
            if (evt.Hero != _owner) return;

            for (int i = 0; i < itemIcons.Length; i++)
            {
                if (itemIcons[i] == null) continue;

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
