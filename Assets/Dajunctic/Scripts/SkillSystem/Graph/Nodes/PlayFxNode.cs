using UnityEngine;

namespace Dajunctic.SkillSystem.Graph.Nodes
{
    public class PlayFxNode : SkillNode
    {
        public string fxId;            // ID tra cứu qua ISkillServiceProvider
        public GameObject vfxPrefab;   // Fallback: dùng trực tiếp khi không có services
        public AnchorType spawnAnchor = AnchorType.FootPoint;
        public float duration = 2f;

        public override void Execute()
        {
            Vector3 spawnPos = _context.actor.GetAnchorPosition(spawnAnchor);
            Quaternion spawnRot = Quaternion.LookRotation(_context.actor.Forward);

            if (_context.Services != null && !string.IsNullOrEmpty(fxId))
            {
                // Dùng service (cả runtime lẫn editor preview)
                _context.Services.SpawnFx(fxId, spawnPos, spawnRot, duration);
            }
            else if (vfxPrefab != null)
            {
                // Fallback: Instantiate trực tiếp từ prefab (khi không có fxId)
                var vfx = Object.Instantiate(vfxPrefab, spawnPos, spawnRot);

                if (Application.isPlaying)
                {
                    Object.Destroy(vfx, duration);
                }
#if UNITY_EDITOR
                else
                {
                    // Editor: tự dọn sau thời gian duration
                    float startTime = (float)UnityEditor.EditorApplication.timeSinceStartup;
                    UnityEditor.EditorApplication.CallbackFunction update = null;
                    update = () =>
                    {
                        if (vfx == null) { UnityEditor.EditorApplication.update -= update; return; }
                        if ((float)UnityEditor.EditorApplication.timeSinceStartup - startTime >= duration)
                        {
                            UnityEditor.EditorApplication.update -= update;
                            Object.DestroyImmediate(vfx);
                        }
                    };
                    UnityEditor.EditorApplication.update += update;
                }
#endif
            }

            Complete();
        }
    }
}
