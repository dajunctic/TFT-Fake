
using System;
using System.Collections;
using UnityEngine;

namespace Dajunctic
{
    [CreateAssetMenu(menuName = "Dajunctic/Ticker", fileName = "Ticker")]
    public class Ticker: ScriptableObject
    {
        [SerializeField] private TickerView tickerMonoPrefab;
        private TickerView tickerMono;

        public void Initialize() => Validate();

        private event Action OnEarlyTickEvent;
        private event Action OnTickEvent;
        private event Action OnFixedTickEvent;
        private event Action OnLateTickEvent;

        public void SubEarlyTick(IEntity entity) => Validate(() => OnEarlyTickEvent += entity.Tick);
        public void UnsubEarlyTick(IEntity entity) => OnEarlyTickEvent -= entity.Tick;

        public void SubTick(IEntity entity) => Validate(() => OnTickEvent += entity.Tick);
        public void UnsubTick(IEntity entity) => OnTickEvent -= entity.Tick;

        public void SubFixedTick(IEntity entity) => Validate(() => OnFixedTickEvent += entity.Tick);
        public void UnsubFixedTick(IEntity entity) => OnFixedTickEvent -= entity.Tick;

        public void SubLateTick(IEntity entity) => Validate(() => OnLateTickEvent += entity.Tick);
        public void UnsubLateTick(IEntity entity) => OnLateTickEvent -= entity.Tick;

        public void EarlyTick() => OnEarlyTickEvent?.Invoke();
        public void Tick() => OnTickEvent?.Invoke();
        public void FixedTick() => OnFixedTickEvent?.Invoke();
        public void LateTick() => OnLateTickEvent?.Invoke();

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            Validate();
            return tickerMono.StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Validate();
                tickerMono.StopCoroutine(coroutine);
            }
        }

        private void Validate(Action onValidateCallback = null)
        {
            if (tickerMono == null)
            {
                tickerMono = Instantiate(tickerMonoPrefab);
                tickerMono.name = "Ticker";
                tickerMono.SetTicker(this);
            }
            onValidateCallback?.Invoke();
        }
    }    

    [Flags]
    public enum TickType
    {
        None        = 0,
        EarlyTick   = 1 << 0,
        Tick        = 1 << 1,
        FixedTick   = 1 << 2,
        LateTick    = 1 << 3
    }
}