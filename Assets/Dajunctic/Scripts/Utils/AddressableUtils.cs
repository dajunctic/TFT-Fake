using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Dajunctic
{
    public static class AddressableUtils
    {
        static AsyncOperationHandle<SceneInstance> currentSceneHandle;

        public static void LoadScene(AssetReference sceneRef, Action onCompleted=null, Action<float> onProgress=null, Action onFailed=null)
        {
            _ = LoadSceneAsync(sceneRef, onCompleted, onProgress, onFailed);
        }


        public static async Task LoadSceneAsync(AssetReference sceneRef, Action onCompleted=null, Action<float> onProgress=null, Action onFailed=null)
        {
            if (currentSceneHandle.IsValid())
            {
                await Addressables.UnloadSceneAsync(currentSceneHandle).Task;
            }

            currentSceneHandle = Addressables.LoadSceneAsync(sceneRef, LoadSceneMode.Additive);

            while (!currentSceneHandle.IsDone)
            {
                var progress = currentSceneHandle.PercentComplete;
                onProgress?.Invoke(progress);
                await Task.Yield();
            }

            if (currentSceneHandle.Status == AsyncOperationStatus.Succeeded)
            {
                onCompleted?.Invoke();
            }
            else
            {
                onFailed?.Invoke();
            }
        }
    }
}