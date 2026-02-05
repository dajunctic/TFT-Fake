using Dajunctic;
using UnityEngine;

public class AutoReleaseParticle : PoolableObject
{
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        var main = _ps.main;
        
        main.stopAction = ParticleSystemStopAction.Callback;

        var forwarder = _ps.gameObject.AddComponent<ParticleCallbackForwarder>();
        
        forwarder.OnStopped = Despawn;
    }
}