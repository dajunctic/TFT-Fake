using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    public class TickerView: Singleton<TickerView>
    {
        [SerializeField] public Ticker ticker;

        public void SetTicker(Ticker t)
        {
            ticker = t;
        }

        private void Update()
        {
            ticker?.EarlyTick();
            ticker?.Tick();
        }

        private void FixedUpdate() => ticker?.FixedTick();

        private void LateUpdate() => ticker?.LateTick();

        public void StartGlobalCoroutine(IEnumerator coroutine)
        {
            ticker.StartCoroutine(coroutine);
        }
    }

    public static class TickerExtension
    {
        public static void StartGlobalCoroutine(this MonoBehaviour ticker, IEnumerator coroutine)
        {
            TickerView.Instance.StartGlobalCoroutine(coroutine);
        }
    }
}