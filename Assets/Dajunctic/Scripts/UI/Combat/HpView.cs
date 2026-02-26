using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dajunctic
{
    public class HpView : BaseView
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
        private Camera _mainCamera;

        public override void Initialize()
        {
            base.Initialize();
            _cachedTransform = transform;
            _mainCamera = Camera.main;
            foreach (var icon in itemIcons)
            {
                if (icon != null) icon.gameObject.SetActive(false);
            }
        }

        public void Initialize(CombatActor owner, int starLevel)
        {
            // Unsubscribe from previous owner if exists
            if (_owner != null && _owner != owner)
            {
                UnsubscribeFromOwner();
            }

            _owner = owner;            
            UpdateStarLevel(starLevel);

            if (_owner != null)
            {
                _owner.OnHpChanged += UpdateHp;
                _owner.OnEnergyChanged += UpdateEnergy;

                UpdateHp(_owner.MaxHp > 0 ? _owner.Hp / _owner.MaxHp : 1f);
                UpdateEnergy(_owner.MaxEnergy > 0 ? _owner.Energy / _owner.MaxEnergy : 0f);
            }
        }

        public override void ListenEvents()
        {
            this.RegisterListener<HeroItemsChangedEvent>(OnItemsChanged);
            this.RegisterListener<DespawnHpViewEvent>(OnDespawn);
            this.RegisterListener<UpdateStarLevelEvent>(OnUpdateStarLevel);
        }

        public override void StopListenEvents()
        {
            this.RemoveListener<UpdateStarLevelEvent>(OnUpdateStarLevel);
            this.RemoveListener<HeroItemsChangedEvent>(OnItemsChanged);
            this.RemoveListener<DespawnHpViewEvent>(OnDespawn);
            
            // Safety: also unsubscribe from owner when disabled/stopped
            UnsubscribeFromOwner();
        }

        private void UnsubscribeFromOwner()
        {
            if (_owner != null)
            {
                _owner.OnHpChanged -= UpdateHp;
                _owner.OnEnergyChanged -= UpdateEnergy;
            }
        }

        public void OnUpdateStarLevel(UpdateStarLevelEvent param)
        {
            if (_owner != param.owner) return;

            UpdateStarLevel(param.starLevel);
        }

        public void UpdateStarLevel(int starLevel)
        {
            if (bgRenderer != null && starBgSprites != null)
            {
                bgRenderer.sprite = starBgSprites.GetIndex(starLevel - 1);
            }
        }

        public void OnDespawn(DespawnHpViewEvent param)
        {
            if (_owner != param.owner) return;
            ActualDespawn();
        }

        private void ActualDespawn()
        {
            UnsubscribeFromOwner();
            _owner = null;
            Despawn(); // Return to pool using PoolableObject.Despawn()
        }

        public override void LateTick()
        {
            if (_owner == null) return;
            
            // If owner is destroyed or inactive, we should hide/despawn the HP view
            if (_owner.gameObject == null || !_owner.gameObject.activeInHierarchy)
            {
                ActualDespawn();
                return;
            }

            base.LateTick();
            _cachedTransform.position = _owner.HeadPoint + offset;
            
            if (_mainCamera == null) _mainCamera = Camera.main;
            if (_mainCamera != null)
            {
                _cachedTransform.rotation = _mainCamera.transform.rotation;
            }
        }

        private void UpdateHp(float ratio)
        {
            if (float.IsNaN(ratio)) ratio = 0;
            if (hpProgress != null)
            {
                Vector3 scale = hpProgress.transform.localScale;
                scale.x = Mathf.Clamp01(ratio);
                hpProgress.transform.localScale = scale;
            }
        }

        private void UpdateEnergy(float ratio)
        {
            if (float.IsNaN(ratio)) ratio = 0;
            if (energyProgress != null)
            {
                Vector3 scale = energyProgress.transform.localScale;
                scale.x = Mathf.Clamp01(ratio);
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

    public class SpawnHpViewEvent: IEvent
    {
        public CombatActor owner;
        public int starLevel;
    }

    public class DespawnHpViewEvent: IEvent
    {
        public CombatActor owner;
    }

    public class UpdateStarLevelEvent: IEvent
    {
        public CombatActor owner;
        public int starLevel;
    }
}
