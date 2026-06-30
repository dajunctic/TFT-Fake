using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using Dajunctic;
using System.Linq;

[InitializeOnLoad]
public static class InspectPlayMode
{
    static InspectPlayMode()
    {
        if (EditorApplication.isPlaying && !EditorApplication.isPaused)
        {
            EditorApplication.update += SpawnTesterOnUpdate;
        }
    }

    private static void SpawnTesterOnUpdate()
    {
        EditorApplication.update -= SpawnTesterOnUpdate;
        
        // Ensure we don't spawn multiple testers
        if (GameObject.Find("PlayModeTester") == null)
        {
            GameObject testerObj = new GameObject("PlayModeTester");
            testerObj.AddComponent<PlayModeTesterComponent>();
            Object.DontDestroyOnLoad(testerObj);
        }
    }

    [MenuItem("Tools/Inspect Play Mode")]
    public static void StartInspection()
    {
        EditorApplication.isPlaying = true;
    }
}

public class PlayModeTesterComponent : MonoBehaviour
{
    private IEnumerator Start()
    {
        string outputPath = "inspect_playmode_output.txt";
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("=== PLAYMODE RUNTIME INSPECTION ===");
        
        // Wait for the scene to settle and load systems
        yield return new WaitForSeconds(3.0f);
        
        sb.AppendLine($"Initially loaded scene triangles: {GetTotalRenderedTriangles()}");
        
        // Find Kim Jong Un data
        ChampionData kimData = null;
        var shopSystem = FindFirstObjectByType<ShopSystem>();
        if (shopSystem != null && shopSystem.ShopSystemData != null && shopSystem.ShopSystemData.allHeroes != null)
        {
            kimData = shopSystem.ShopSystemData.allHeroes.FirstOrDefault(h => h != null && h.Id == "champion_kim_jong_un");
        }
        
        if (kimData == null)
        {
            sb.AppendLine("Kim Jong Un data not found in ShopSystemData. Searching all ChampionData assets...");
            string[] guids = AssetDatabase.FindAssets("t:ChampionData");
            foreach (var g in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(g);
                var cd = AssetDatabase.LoadAssetAtPath<ChampionData>(path);
                if (cd != null && cd.Id == "champion_kim_jong_un")
                {
                    kimData = cd;
                    break;
                }
            }
        }
        
        if (kimData != null)
        {
            sb.AppendLine($"Found Kim Jong Un data: {kimData.displayName}, Prefab: {(kimData.prefab != null ? kimData.prefab.name : "NULL")}");
            
            if (kimData.prefab != null)
            {
                sb.AppendLine("Instantiating Kim Jong Un...");
                GameObject inst = Instantiate(kimData.prefab, new Vector3(0, 0.5f, 0), Quaternion.identity);
                
                // Wait for any initialization and dynamic spawns
                yield return new WaitForSeconds(3.0f);
                
                sb.AppendLine($"After instantiating Kim Jong Un, scene triangles: {GetTotalRenderedTriangles()}");
                
                sb.AppendLine("\nActive Objects in scene:");
                int totalTris = 0;
                var allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                foreach (var go in allObjects)
                {
                    if (go.activeInHierarchy)
                    {
                        int tris = 0;
                        var mf = go.GetComponent<MeshFilter>();
                        if (mf != null && mf.sharedMesh != null)
                            tris += mf.sharedMesh.triangles.Length / 3;
                        var smr = go.GetComponent<SkinnedMeshRenderer>();
                        if (smr != null && smr.sharedMesh != null)
                            tris += smr.sharedMesh.triangles.Length / 3;
                            
                        if (tris > 0 || go.name.ToLower().Contains("kim") || go.name.ToLower().Contains("nuke"))
                        {
                            sb.AppendLine($"  - {go.name}: {tris} tris");
                        }
                        totalTris += tris;
                    }
                }
                sb.AppendLine($"Total Active Tris: {totalTris}");
            }
            else
            {
                sb.AppendLine("ERROR: Kim Jong Un prefab is null!");
            }
        }
        else
        {
            sb.AppendLine("ERROR: Kim Jong Un data could not be found!");
        }
        
        File.WriteAllText(outputPath, sb.ToString());
        Debug.Log("Playmode inspection completed and saved to " + outputPath);
        
        // Stop play mode
        EditorApplication.isPlaying = false;
    }
    
    private int GetTotalRenderedTriangles()
    {
        int total = 0;
        var renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        foreach (var r in renderers)
        {
            if (r.gameObject.activeInHierarchy && r.enabled)
            {
                if (r is SkinnedMeshRenderer smr && smr.sharedMesh != null)
                {
                    total += smr.sharedMesh.triangles.Length / 3;
                }
                else if (r is MeshRenderer && r.GetComponent<MeshFilter>() != null && r.GetComponent<MeshFilter>().sharedMesh != null)
                {
                    total += r.GetComponent<MeshFilter>().sharedMesh.triangles.Length / 3;
                }
            }
        }
        return total;
    }
}
