using UnityEditor;
using UnityEngine;
using System.IO;

public static class InspectKimMesh
{
    [InitializeOnLoadMethod]
    public static void Inspect()
    {
        string outputPath = "inspect_mesh_output.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(outputPath, false))
            {
                writer.WriteLine("=== MESH INSPECTION ===");
                
                string fbxPath = "Assets/Art/3D/Champions/KimJongUn/chibi_kimjongun.fbx";
                var objects = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
                writer.WriteLine($"FBX Path: {fbxPath}");
                int totalTris = 0;
                foreach (var obj in objects)
                {
                    if (obj is Mesh mesh)
                    {
                        writer.WriteLine($"Mesh Name: {mesh.name}, Vertices: {mesh.vertexCount}, Triangles: {mesh.triangles.Length / 3}, Submesh Count: {mesh.subMeshCount}");
                        totalTris += mesh.triangles.Length / 3;
                    }
                }
                writer.WriteLine($"Total triangles in FBX meshes: {totalTris}");
                
                string prefabPath = "Assets/Prefabs/Champions/KimJongUn/kim_jong_un.prefab";
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefab != null)
                {
                    writer.WriteLine($"\nPrefab: {prefabPath}");
                    var skinnedRenderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach (var smr in skinnedRenderers)
                    {
                        if (smr.sharedMesh != null)
                        {
                            writer.WriteLine($"SkinnedMeshRenderer: {smr.name}, Mesh: {smr.sharedMesh.name}, Vertices: {smr.sharedMesh.vertexCount}, Triangles: {smr.sharedMesh.triangles.Length / 3}");
                        }
                    }
                    var meshFilters = prefab.GetComponentsInChildren<MeshFilter>(true);
                    foreach (var mf in meshFilters)
                    {
                        if (mf.sharedMesh != null)
                        {
                            writer.WriteLine($"MeshFilter: {mf.name}, Mesh: {mf.sharedMesh.name}, Vertices: {mf.sharedMesh.vertexCount}, Triangles: {mf.sharedMesh.triangles.Length / 3}");
                        }
                    }
                }

                // Inspect Nuke FBX and prefab
                writer.WriteLine("\n=== NUKE INSPECTION ===");
                string nukeFbxPath = "Assets/Art/3D/Champions/KimJongUn/Nuke/source/Nuke.fbx";
                var nukeObjects = AssetDatabase.LoadAllAssetsAtPath(nukeFbxPath);
                writer.WriteLine($"Nuke FBX Path: {nukeFbxPath}");
                int nukeTotalTris = 0;
                foreach (var obj in nukeObjects)
                {
                    if (obj is Mesh mesh)
                    {
                        writer.WriteLine($"Mesh Name: {mesh.name}, Vertices: {mesh.vertexCount}, Triangles: {mesh.triangles.Length / 3}, Submesh Count: {mesh.subMeshCount}");
                        nukeTotalTris += mesh.triangles.Length / 3;
                    }
                }
                writer.WriteLine($"Total triangles in Nuke FBX: {nukeTotalTris}");

                string nukePrefabPath = "Assets/Art/3D/Champions/KimJongUn/Nuke/source/Nuke.prefab";
                GameObject nukePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(nukePrefabPath);
                if (nukePrefab != null)
                {
                    writer.WriteLine($"\nNuke Prefab: {nukePrefabPath}");
                    var skinnedRenderers = nukePrefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    foreach (var smr in skinnedRenderers)
                    {
                        if (smr.sharedMesh != null)
                        {
                            writer.WriteLine($"SkinnedMeshRenderer: {smr.name}, Mesh: {smr.sharedMesh.name}, Vertices: {smr.sharedMesh.vertexCount}, Triangles: {smr.sharedMesh.triangles.Length / 3}");
                        }
                    }
                    var meshFilters = nukePrefab.GetComponentsInChildren<MeshFilter>(true);
                    foreach (var mf in meshFilters)
                    {
                        if (mf.sharedMesh != null)
                        {
                            writer.WriteLine($"MeshFilter: {mf.name}, Mesh: {mf.sharedMesh.name}, Vertices: {mf.sharedMesh.vertexCount}, Triangles: {mf.sharedMesh.triangles.Length / 3}");
                        }
                    }
                }
            }
            Debug.Log("Mesh inspection completed and saved to " + outputPath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error in InspectKimMesh: " + ex.Message);
        }
    }
}
