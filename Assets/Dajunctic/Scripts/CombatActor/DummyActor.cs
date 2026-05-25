using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Actor giả dùng để làm mục tiêu test skill trong scene hoặc trong Editor Preview.
    /// Không cần NavMesh, không có AI, không di chuyển.
    /// Chỉ đứng im, nhận damage và trả về đúng Team/IsTargetable.
    /// Khi dùng làm quái PvE: gọi SetInfiniteHp(false) trước khi Initialize().
    /// </summary>
    public class DummyActor : CombatActor
    {
        [Header("Dummy Settings")]
        [SerializeField] private bool infiniteHp = true;
        [SerializeField] private bool alwaysTargetable = true;

        // ─── ICombatActor overrides ──────────────────────────────────────────

        public override string DataId => name;
        public override bool CanBeTarget => alwaysTargetable && gameObject.activeInHierarchy && Alive;

        // ─── Runtime control (dùng khi spawn làm quái PvE) ─────────────────

        /// <summary>Gọi trước Initialize() để quái có máu giới hạn.</summary>
        public void SetInfiniteHp(bool value) => infiniteHp = value;

        // ─── No behavior tree ────────────────────────────────────────────────

        protected override void SetupTree()
        {
            // Intentionally empty – Dummy has no AI
        }

        // ─── Tick: chỉ update animator idle, không chạy BT hay NavMesh ──────

        public override void Tick()
        {
            // Cập nhật Position để Gambit distance check hoạt động đúng (base.Tick() bị skip)
            if (CachedTransform != null) Position = CachedTransform.position;

            if (animator != null)
                animator.SetFloat("Speed", 0f);
            // Skip base.Tick() to avoid running BT and NavMesh SyncTransform
        }

        // ─── SyncEntity: skip vì không có NavMesh ─────────────────────────────

        protected override void SyncEntity()
        {
            // DummyActor doesn't move — no sync needed
        }

        // ─── TakeDamage: override để handle infiniteHp và fire event ─────────

        public new void TakeDamage(CombineDamage combineDamage)
        {
            if (infiniteHp)
            {
                Debug.Log($"<color=orange>[Dummy] {name}</color> hit for " +
                          $"<b>{combineDamage.damage:F1}</b> {combineDamage.damageType}  (HP ∞)");
                return;
            }

            // Delegate to base for actual HP reduction
            base.TakeDamage(combineDamage);

            // Check if died after taking damage
            if (!Alive)
            {
                Debug.Log($"<color=red>[Dummy] {name} died!</color>");
                this.Raise(new EnemyDiedEvent { enemy = this });
                gameObject.SetActive(false); // Deactivate instead of destroy to let wave spawner track
            }
        }

        // ─── OnDestroy: fire event nếu bị destroy khi còn alive (wave cleanup) ─

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }

    // ─── Event ───────────────────────────────────────────────────────────────────
    public struct EnemyDiedEvent : IEvent
    {
        public DummyActor enemy;
    }
}
