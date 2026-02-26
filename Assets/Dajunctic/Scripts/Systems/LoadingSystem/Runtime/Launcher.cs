using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dajunctic
{
    public class Launcher : BaseView
    {
        [SerializeField] AssetReference homeScene;
        [SerializeField] BaseApplication application;

        async void Start()
        {
            // Wait for application initialization
            await Task.Run(() => SpinWait.SpinUntil(() => application.Initialized));

            // Wait for all game systems to be ready (data loaded + initialized)
            await Task.Run(() => SpinWait.SpinUntil(() =>
                GameSystemManager.Instance != null && GameSystemManager.Instance.AllSystemsReady));

            Debug.Log("<color=green>Launcher: All systems ready — loading home scene...</color>");
            AddressableUtils.LoadScene(homeScene);
        }
    }
}