using UnityEngine;

namespace Dajunctic.SkillSystem.Graph
{
    /// <summary>
    /// Interface tách biệt việc gọi dịch vụ runtime (spawn FX, missile...)
    /// ra khỏi các node, để cả GameManager (runtime) và PreviewProvider (editor) đều implement được.
    /// </summary>
    public interface ISkillServiceProvider
    {
        /// <summary>
        /// Spawn một VFX GameObject tại vị trí cho trước.
        /// Trả về instance đã tạo (có thể null nếu không tìm thấy prefab).
        /// </summary>
        GameObject SpawnFx(string fxId, Vector3 position, Quaternion rotation, float duration = -1f);

        /// <summary>
        /// Spawn một đạn (missile) nhắm vào target.
        /// </summary>
        void SpawnMissile(string missileId, MissileData missileData);
    }
}
