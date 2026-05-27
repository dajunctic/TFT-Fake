using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ExtractGameplay
{
    [MenuItem("Tools/Fix Gameplay Sync")]
    public static void Fix()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.name != "HomeScene")
        {
            Debug.LogError("Vui long mo HomeScene truoc khi chay tool nay!");
            return;
        }

        var rootObjs = scene.GetRootGameObjects();
        GameObject gameplayObj = null;
        foreach (var obj in rootObjs)
        {
            if (obj.name == "Gameplay")
            {
                gameplayObj = obj;
                break;
            }
        }

        GameObject gameplayInstance = null;
        GameObject prefab = null;
        if (gameplayObj != null)
        {
            gameplayInstance = gameplayObj;
        }
        else
        {
            Debug.LogWarning("Khong tim thay Gameplay trong Scene. Dang tai tu Prefab...");
            string path = "Assets/_TFT/Prefabs/Systems/Gameplay.prefab";
            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) {
                Debug.LogError("Khong tim thay Gameplay.prefab!");
                return;
            }
            gameplayInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            PrefabUtility.UnpackPrefabInstance(gameplayInstance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }

        if (!System.IO.Directory.Exists("Assets/_TFT/Prefabs/Systems"))
        {
            System.IO.Directory.CreateDirectory("Assets/_TFT/Prefabs/Systems");
        }

        var amComp = gameplayInstance.GetComponent<Dajunctic.ArenaManager>();
        GameObject amPrefab = null;
        if (amComp != null)
        {
            GameObject amObj = new GameObject("ArenaManager");
            var newAm = amObj.AddComponent<Dajunctic.ArenaManager>();
            EditorUtility.CopySerialized(amComp, newAm);
            amObj.AddComponent<FishNet.Object.NetworkObject>();
            string amPath = "Assets/_TFT/Prefabs/Systems/ArenaManager.prefab";
            amPrefab = PrefabUtility.SaveAsPrefabAsset(amObj, amPath);
            GameObject.DestroyImmediate(amObj);
            GameObject.DestroyImmediate(amComp, true);
            Debug.Log("Da tach rieng ArenaManager thanh Prefab doc lap!");
        }
        else
        {
            amPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_TFT/Prefabs/Systems/ArenaManager.prefab");
        }

        string gpPath = "Assets/_TFT/Prefabs/Systems/Gameplay.prefab";
        prefab = PrefabUtility.SaveAsPrefabAsset(gameplayInstance, gpPath);
        GameObject.DestroyImmediate(gameplayInstance);

        var existingSpawner = Object.FindFirstObjectByType<Dajunctic.HomeSceneSpawner>();
        if (existingSpawner != null) {
            GameObject.DestroyImmediate(existingSpawner.gameObject);
        }

        GameObject spawnerObj = new GameObject("HomeSceneSpawner");
        var spawner = spawnerObj.AddComponent<Dajunctic.HomeSceneSpawner>();
        var serializedObject = new SerializedObject(spawner);
        var prop = serializedObject.FindProperty("systemPrefabs");
        if (prop != null)
        {
            prop.arraySize = 2;
            prop.GetArrayElementAtIndex(0).objectReferenceValue = prefab;
            prop.GetArrayElementAtIndex(1).objectReferenceValue = amPrefab;
            serializedObject.ApplyModifiedProperties();
            Debug.Log("Da tu dong tao HomeSceneSpawner va gán 2 Prefab rieng biet!");
        }

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Da cap nhat Scene thanh cong!");
        EditorApplication.ExecuteMenuItem("Fish-Networking/Window/Default Prefabs");
        Debug.Log("Da mo cua so FishNet Default Prefabs (Hay bam Refresh/Populate neu can).");
    }
}
