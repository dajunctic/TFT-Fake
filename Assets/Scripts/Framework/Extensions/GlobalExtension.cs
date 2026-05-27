using System.Collections;

namespace Dajunctic
{
    public static class GlobalExtension
    {
        public static UnityEngine.Coroutine StartGlobalCoroutine(this object obj, IEnumerator coroutine)
        {
            return IApplication.Instance?.StartCoroutine(coroutine);
        }
    
    }
}
