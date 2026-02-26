using UnityEngine;

namespace Dajunctic
{
    /// <summary>
    /// Actor giả dùng để làm mục tiêu test skill trong scene hoặc trong Editor Preview.
    /// Không cần NavMesh, không có AI, không di chuyển.
    /// Chỉ đứng im, nhận damage và trả về đúng Team/IsTargetable.
    /// </summary>
    public class DummyActor : CombatActor
    {
        [Header("Dummy Settings")]
        [SerializeField] private bool infiniteHp = true;
        [SerializeField] private bool alwaysTargetable = true;

        // ─── ICombatActor overrides ──────────────────────────────────────────

        public override string DataId => name;
        public override bool CanBeTarget => alwaysTargetable && gameObject.activeInHierarchy;

        // Dummy không cần Movement — override để skip NavMesh init
        public override bool CanMove => false;

        // ─── No behavior tree ────────────────────────────────────────────────

        protected override void SetupTree()
        {
            // Intentionally empty – Dummy has no AI
        }

        // ─── Tick: chỉ update animator idle, không chạy BT hay NavMesh ──────

        public override void Tick()
        {
            if (animator != null)
                animator.SetFloat("Speed", 0f);
            // Skip base.Tick() to avoid running BT and NavMesh SyncTransform
        }

        // ─── SyncEntity: sync từ Transform trực tiếp (không có NavMesh) ─────

        protected override void SyncEntity()
        {
            // Write transform → Position/Forward (reversed from base which goes Position → transform)
            // Since Dummy doesn't move, position stays fixed at spawn point
        }

        // ─── TakeDamage: infinite HP variant ─────────────────────────────────

        public new void TakeDamage(CombineDamage combineDamage)
        {
            if (infiniteHp)
            {
                Debug.Log($"<color=orange>[Dummy] {name}</color> hit for " +
                          $"<b>{combineDamage.damage:F1}</b> {combineDamage.damageType}  (HP ∞)");
                return;
            }
            base.TakeDamage(combineDamage);
        }
    }
}
