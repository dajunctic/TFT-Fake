using UnityEditor;
using UnityEngine;

public class FixMeshReadWrite
{
    [MenuItem("Tools/Fix Mesh ReadWrite")]
    public static void Fix()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { "Assets" });
        int count = 0;
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ModelImporter importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
                count++;
            }
        }
        Debug.Log("Fixed " + count + " models!");
        EditorApplication.ExecuteMenuItem("Tools/Fish-Networking/Refresh Default Prefabs");
        Debug.Log("Refreshed FishNet Default Prefabs!");
    }
}
