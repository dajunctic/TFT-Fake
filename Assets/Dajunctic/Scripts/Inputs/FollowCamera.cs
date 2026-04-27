using Dajunctic;
using UnityEngine;

namespace Dajunctic
{

    public class FollowCamera : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("If null, will auto-find GameSystemManager's LocalPlayer Tactician")]
        public Transform target;

        [Header("Settings")]
        public Vector3 offset = new Vector3(0f, 9.5f, -6f);
        public Vector3 scoutOffset = new Vector3(0f, 9.5f, 6f);
        public float smoothTime = 0.3f;
        public float rotSmoothTime = 0.15f;

        [Header("Camera Lock")]
        [Tooltip("If true, the camera locks strictly to the center of the active Arena instead of following the target tactician.")]
        public bool lockToArenaCenter = true;

        private Vector3 velocity = Vector3.zero;
        private Quaternion _originalRot;
        private bool _rotInitialized = false;

        private Vector3 GetArenaCenter()
        {
            if (target != null && GameSystemManager.Instance != null && GameSystemManager.Instance.Field != null)
            {
                var tactician = target.GetComponent<TacticianActor>();
                if (tactician != null)
                {
                    Arena targetArena = GameSystemManager.Instance.Field.GetArena(tactician.OwnerID);
                    if (targetArena != null) return targetArena.transform.position;
                }

                var arenas = GameSystemManager.Instance.Field.GetAllArenas();
                if (arenas != null && arenas.Count > 0)
                {
                    Arena closest = arenas[0];
                    float minDist = Vector3.Distance(target.position, closest.transform.position);
                    for (int i = 1; i < arenas.Count; i++)
                    {
                        float d = Vector3.Distance(target.position, arenas[i].transform.position);
                        if (d < minDist)
                        {
                            minDist = d;
                            closest = arenas[i];
                        }
                    }
                    return closest.transform.position;
                }
            }
            return target != null ? target.position : Vector3.zero;
        }

        private bool IsFlipped()
        {
            if (target != null)
            {
                var tactician = target.GetComponent<TacticianActor>();
                if (tactician != null && GameSystemManager.Instance != null && GameSystemManager.Instance.Player != null)
                {
                    var localPlayer = GameSystemManager.Instance.Player.LocalPlayer;
                    // Flip camera ONLY if we are looking at someone else's board AND we want that behavior
                    // But usually in TFT, all boards face the same way so we don't flip. 
                    // Let's just return false for now to keep the camera angle consistent for all players!
                    return false;
                }
            }
            return false;
        }

        void LateUpdate()
        {
            if (!_rotInitialized)
            {
                _originalRot = transform.rotation;
                _rotInitialized = true;
            }

            if (target == null)
            {
                FindLocalTactician();
                return;
            }

            bool flipped = IsFlipped();
            Vector3 focusPoint = lockToArenaCenter ? GetArenaCenter() : target.position;
            
            Vector3 desiredPosition = focusPoint + (flipped ? scoutOffset : offset);

            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
            
            Quaternion targetRot = flipped ? Quaternion.Euler(_originalRot.eulerAngles.x, _originalRot.eulerAngles.y + 180f, _originalRot.eulerAngles.z) : _originalRot;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime / Mathf.Max(0.01f, rotSmoothTime));
        }

        private void FindLocalTactician()
        {
            var tacticians = FindObjectsByType<TacticianActor>(FindObjectsSortMode.None);
            foreach (var t in tacticians)
            {
                if (t.IsLocalPlayer)
                {
                    Debug.Log($"[FollowCamera] Found local tactician: {t.gameObject.name} (OwnerID: {t.OwnerID}). Snapping to it.");
                    target = t.transform;
                    SnapToTarget();
                    return;
                }
            }
        }

        public void SnapToTarget()
        {
            if (target == null) return;
            
            if (!_rotInitialized)
            {
                _originalRot = transform.rotation;
                _rotInitialized = true;
            }

            bool flipped = IsFlipped();
            Vector3 focusPoint = lockToArenaCenter ? GetArenaCenter() : target.position;
            
            Vector3 desiredPosition = focusPoint + (flipped ? scoutOffset : offset);
            
            transform.position = desiredPosition;
            Quaternion targetRot = flipped ? Quaternion.Euler(_originalRot.eulerAngles.x, _originalRot.eulerAngles.y + 180f, _originalRot.eulerAngles.z) : _originalRot;
            transform.rotation = targetRot;
            velocity = Vector3.zero;
        }
    }
}