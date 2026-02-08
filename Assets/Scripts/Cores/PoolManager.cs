using UnityEngine;

namespace Dajunctic
{
    public class PoolManager: BaseView
    {
        [SerializeField] private PoolSO poolSO;

        public override void ListenEvents()
        {
            base.ListenEvents();
            this.RegisterListener<PlayFxEvent>(PlayFx);
        }

        public void PlayFx(PlayFxEvent playFxEvent)
        {
            var position = playFxEvent.Position;
            var fxId = playFxEvent.Id;
            var fxViewPrefab = poolSO.fxLists.Find(f => f.Id == fxId).fxViewPrefab;

            var fxView = Instantiate(fxViewPrefab, position, Quaternion.identity);
            fxView.Play(playFxEvent);
        }

    }
}