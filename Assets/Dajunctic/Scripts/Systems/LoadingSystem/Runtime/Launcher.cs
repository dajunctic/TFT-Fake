using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class Launcher : BaseView
    {
        [SerializeField] AssetReference lobbyScene;
        [SerializeField] BaseApplication application;

        async void Start()
        {
            await Task.Run(() => SpinWait.SpinUntil(() => application.Initialized));
            await Task.Run(() => SpinWait.SpinUntil(() =>
                GameSystemManager.Instance != null && GameSystemManager.Instance.AllSystemsReady));

            AddressableUtils.LoadScene(lobbyScene);
        }
    }
}