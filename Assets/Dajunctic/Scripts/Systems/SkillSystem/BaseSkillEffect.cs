using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class BaseSkillEffect : PoolableObject
    {
        [Header("References")] 
        [SerializeField] private GameObject visualModel;
        [SerializeField] private ParticleSystem[] bodyFx;
        [SerializeField] private ParticleSystem[] trailFx;
        [SerializeField] private ParticleSystem[] hybridFx;
        [SerializeField] private bool spawnVfxOnGround = false;
        
        private ParticleSystemRenderMode[] _initialRenderModes;
        private ParticleSystemRenderer[] _hybridRenderers;
        
        protected virtual void Awake()
        {
            if (hybridFx != null && hybridFx.Length > 0)
            {
                _initialRenderModes = new ParticleSystemRenderMode[hybridFx.Length];
                _hybridRenderers = new ParticleSystemRenderer[hybridFx.Length];

                for (int i = 0; i < hybridFx.Length; i++)
                {
                    var rend = hybridFx[i].GetComponent<ParticleSystemRenderer>();
                    
                    if (rend != null)
                    {
                        _hybridRenderers[i] = rend;
                        _initialRenderModes[i] = rend.renderMode;
                    }
                }
            }
        }
        
        protected void ResetVisuals()
        {
            if (visualModel != null) visualModel.SetActive(true);
            if (hybridFx != null)
            {
                for (int i = 0; i < hybridFx.Length; i++)
                {
                    if (_hybridRenderers[i] != null)
                    {
                        _hybridRenderers[i].renderMode = _initialRenderModes[i];
                    }
                }
            }
        }
        
        protected IEnumerator DespawnCoroutine()
        {
            if (visualModel != null) visualModel.SetActive(false);
            
            foreach (var ps in bodyFx) ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            foreach (var ps in trailFx) ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            foreach (var ps in hybridFx) ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            
            foreach (var ps in bodyFx)
            {
                ps.Clear(false); 
            }
            if (hybridFx != null)
            {
                for (int i = 0; i < hybridFx.Length; i++)
                {
                    if (_hybridRenderers[i] != null)
                    {
                        _hybridRenderers[i].renderMode = ParticleSystemRenderMode.None; 
                    }
                }
            }
            
            yield return new WaitWhile(() => 
            {
                foreach (var ps in trailFx)
                {
                    if (ps.IsAlive(true)) return true;
                }
                return false;
            });

            Despawn();
        }
        
        protected void SpawnHitVFX(PoolableObject hitPrefab)
        {
            if (hitPrefab == null) return;

            Vector3 spawnPos = transform.position; 

            if (spawnVfxOnGround)
            {
                spawnPos.y = 0;
            }

            Pool.Spawn(hitPrefab, spawnPos, Quaternion.identity);
        }
    }
}