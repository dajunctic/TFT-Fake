using UnityEngine;

namespace Dajunctic
{

    public class DummyActor : CombatActor
    {
        [Header("Dummy Settings")]
        [SerializeField] private bool infiniteHp = true;
        [SerializeField] private bool alwaysTargetable = true;

        public override string DataId => name;
        public override bool CanBeTarget => alwaysTargetable && gameObject.activeInHierarchy && Alive;

        public void SetInfiniteHp(bool value) => infiniteHp = value;

        public override void Initialize()
        {
            if (Initialized) return;
            base.Initialize();

            var hpView = GetComponentInChildren<HpView>(true);
            if (hpView != null)
            {
                hpView.Initialize(this, 1);
                hpView.gameObject.SetActive(true);
            }
            else
            {
                this.Raise(new SpawnHpViewEvent { owner = this, starLevel = 1 });
            }
        }

        protected override void SetupTree()
        {
            
        }

        public override void Tick()
        {
            
            if (CachedTransform != null) Position = CachedTransform.position;

            if (animator != null)
                animator.SetFloat("Speed", 0f);
            
        }

        protected override void SyncEntity()
        {
            
        }

        public override void TakeDamage(CombineDamage combineDamage)
        {
            if (infiniteHp)
            {
                Debug.Log($"<color=orange>[Dummy] {name}</color> hit for " +
                          $"<b>{combineDamage.damage:F1}</b> {combineDamage.damageType}  (HP ∞)");
                base.TakeDamage(combineDamage);
                if (Hp <= 0)
                {
                    ForceSetHp(MaxHp);
                }
                return;
            }

            base.TakeDamage(combineDamage);

            if (!Alive)
            {
                Debug.Log($"<color=red>[Dummy] {name} died!</color>");
                this.Raise(new EnemyDiedEvent { enemy = this });
                gameObject.SetActive(false); 
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

    public struct EnemyDiedEvent : IEvent
    {
        public DummyActor enemy;
    }
}
