using UnityEngine;
using UnityEngine.Events;

public class ParticleCallbackForwarder : MonoBehaviour
{
    public UnityAction OnStopped;
    private void OnParticleSystemStopped()
    {
        OnStopped?.Invoke();
    }
}