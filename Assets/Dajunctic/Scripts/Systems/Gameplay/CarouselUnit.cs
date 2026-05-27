using UnityEngine;
using System;

namespace Dajunctic
{
    public class CarouselUnit : MonoBehaviour
    {
        private ChampionActor _champion;
        private ItemData _item;
        private bool _isPicked = false;

        public ChampionActor Champion => _champion;
        public ItemData Item => _item;

        public static event Action<CarouselUnit, TacticianActor> OnUnitPicked;

        public void Initialize(ChampionActor champion, ItemData item)
        {
            _champion = champion;
            _item = item;
            _isPicked = false;

            var col = _champion.GetComponent<CapsuleCollider>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isPicked) return;

            TacticianActor tactician = other.GetComponent<TacticianActor>();
            if (tactician != null)
            {
                _isPicked = true;
                OnUnitPicked?.Invoke(this, tactician);
            }
        }
    }
}
