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

        [Header("Settings")]
        [SerializeField] private SpriteLists starBgSprites;
        [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);

        private CombatActor _owner;
        private Transform _headPoint;
        private Transform _cachedTransform;

        private void Awake()
        {
            _cachedTransform = transform;
        }

        public void Initialize(CombatActor owner, int starLevel)
        {
            _owner = owner;
            _headPoint = owner.HeadPoint.transform;
            
            UpdateStarLevel(starLevel);

            _owner.OnHpChanged += UpdateHp;
            _owner.OnEnergyChanged += UpdateEnergy;

            // Initial sync
            UpdateHp(_owner.MaxHp > 0 ? _owner.Hp / _owner.MaxHp : 1f);
            UpdateEnergy(_owner.MaxEnergy > 0 ? _owner.Energy / _owner.MaxEnergy : 0f);
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
        }

        private void LateUpdate()
        {
            if (_headPoint != null)
            {
                _cachedTransform.position = _headPoint.position + offset;
                
                if (Camera.main != null)
                {
                    _cachedTransform.rotation = Camera.main.transform.rotation;
                }
            }
            else
            {
                // If owner is destroyed but this hasn't been yet
                Destroy(gameObject);
            }
        }

        private void UpdateHp(float ratio)
        {
            if (hpProgress != null)
            {
                // Assuming the progress bar is a sprite that scales on X
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
    }
}
