#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dajunctic
{
    public static class SceneMenu
    {
        const string PrefabScene = "PrefabScene";
        const string InitializeScene = "InitializeScene";
        const string LauncherScene = "LauncherScene";
        const string DummyScene = "Dummy";
        const string HomeScene = "HomeScene";
        const string GameScene = "GameScene";
        const string FadingScene = "FadingScene";


        static void ChangeScene(string name)
        {
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene(Application.dataPath + "/Panthera/Scenes/" + name + ".unity");
        }

        static bool CanChangeScene(string name)
        {
            return HasScene(name) && SceneManager.GetActiveScene().name != name;
        }

        static bool HasScene(string name)
        {
            return File.Exists(Application.dataPath + "/Panthera/Scenes/" + name + ".unity");
        }

        [MenuItem("Scenes/Prefab Scene", false, 11)]
        static void OpenPrefabScene()
        {
            ChangeScene(PrefabScene);
        }

        [MenuItem("Scenes/Prefab Scene", true, 11)]
        static bool CanOpenPrefabScene()
        {
            return CanChangeScene(PrefabScene);
        }

        [MenuItem("Scenes/Dummy Scene", false, 22)]
        static void OpenDummyScene()
        {
            ChangeScene(DummyScene);
        }

        [MenuItem("Scenes/Dummy Scene", true, 22)]
        static bool CanOpenDummyScene()
        {
            return CanChangeScene(DummyScene);
        }
        
        // [MenuItem("Scenes/Initialize Scene", false, 22)]
        // static void OpenInitializeScene()
        // {
        //     ChangeScene(InitializeScene);
        // }

        // [MenuItem("Scenes/Initialize Scene", true, 22)]
        // static bool CanOpenInitializeScene()
        // {
        //     return CanChangeScene(InitializeScene);
        // }
        
        // [MenuItem("Scenes/Launcher Scene", false, 22)]
        // static void OpenLauncherScene()
        // {
        //     ChangeScene(LauncherScene);
        // }

        // [MenuItem("Scenes/Launcher Scene", true, 22)]
        // static bool CanOpenLauncherScene()
        // {
        //     return CanChangeScene(LauncherScene);
        // }
        
        // [MenuItem("Scenes/Home Scene", false, 22)]
        // static void OpenHomeScene()
        // {
        //     ChangeScene(HomeScene);
        // }

        // [MenuItem("Scenes/Home Scene", true, 22)]
        // static bool CanOpenHomeScene()
        // {
        //     return CanChangeScene(HomeScene);
        // }

        // [MenuItem("Scenes/Fading Scene", false, 22)]
        // static void OpenFadingScene()
        // {
        //     ChangeScene(FadingScene);
        // }

        // [MenuItem("Scenes/Fading Scene", true, 22)]
        // static bool CanOpenFadingScene()
        // {
        //     return CanChangeScene(FadingScene);
        // }


        [MenuItem("Scenes/Game Scene", false, 22)]
        static void OpenGameScene()
        {
            ChangeScene(GameScene);
        }

        [MenuItem("Scenes/Game Scene", true, 22)]
        static bool CanOpenGameScene()
        {
            return CanChangeScene(GameScene);
        }

        [MenuItem("Scenes/Play", false, 44)]
        public static void Play()
        {
            if (HasScene(InitializeScene))
            {
                EditorSceneManager.SaveOpenScenes();
                ChangeScene(InitializeScene);
                EditorApplication.isPlaying = true;
            }
        }

        [MenuItem("Scenes/Play", true, 44)]
        static bool CanPlay()
        {
            return HasScene(InitializeScene) && !Application.isPlaying;
        }
    }
}
#endif

