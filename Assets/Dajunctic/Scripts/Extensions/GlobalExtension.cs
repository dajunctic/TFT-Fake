using System.Collections;

namespace Dajunctic
{
    public static class GlobalExtension
    {
        public static void StartGlobalCoroutine(this object obj, IEnumerator coroutine)
        {
            IApplication.Instance?.StartCoroutine(coroutine);
        }
    
    }
}