using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ShaderCrew.TheToonShader
{

    // Source: https://discussions.unity.com/t/how-to-get-shader-source-code-from-script/839046/5
    public static class ShaderGraphToShaderExporterUtil
    {
        private static object GetGraphData(string shaderAssetPath)
        {
            var importer = AssetImporter.GetAtPath(shaderAssetPath);

            var textGraph = File.ReadAllText(importer.assetPath, Encoding.UTF8);
            var graphObjectType = Type.GetType("UnityEditor.Graphing.GraphObject, Unity.ShaderGraph.Editor")!;

            // var graphObject = CreateInstance<GraphObject>();
            var graphObject = ScriptableObject.CreateInstance(graphObjectType);

            graphObject.hideFlags = HideFlags.HideAndDontSave;
            bool isSubGraph;
            var extension = Path.GetExtension(importer.assetPath).Replace(".", "");
            switch (extension)
            {
                case "shadergraph":
                    isSubGraph = false;
                    break;
                case "ShaderGraph":
                    isSubGraph = false;
                    break;
                case "shadersubgraph":
                    isSubGraph = true;
                    break;
                default:
                    throw new Exception($"Invalid file extension {extension}");
            }
            var assetGuid = AssetDatabase.AssetPathToGUID(importer.assetPath);

            // graphObject.graph = new GraphData { assetGuid = assetGuid, isSubGraph = isSubGraph, messageManager = null };
            var graphObject_graphProperty = graphObjectType.GetProperty("graph")!;
            var graphDataType = Type.GetType("UnityEditor.ShaderGraph.GraphData, Unity.ShaderGraph.Editor")!;
            var graphDataInstance = Activator.CreateInstance(graphDataType);
            graphDataType.GetProperty("assetGuid")!.SetValue(graphDataInstance, assetGuid);
            graphDataType.GetProperty("isSubGraph")!.SetValue(graphDataInstance, isSubGraph);
            graphDataType.GetProperty("messageManager")!.SetValue(graphDataInstance, null);
            graphObject_graphProperty.SetValue(graphObject, graphDataInstance);

            // MultiJson.Deserialize(graphObject.graph, textGraph);
            // = MultiJson.Deserialize<JsonObject>(graphObject.graph, textGraph, null, false);
            var multiJsonType = Type.GetType("UnityEditor.ShaderGraph.Serialization.MultiJson, Unity.ShaderGraph.Editor")!;
            var deserializeMethod = multiJsonType.GetMethod("Deserialize")!;
            var descrializeGenericMethod = deserializeMethod.MakeGenericMethod(graphDataType);
            descrializeGenericMethod.Invoke(null, new object[] { graphDataInstance, textGraph, null, false });

            // graphObject.graph.OnEnable();
            graphDataType.GetMethod("OnEnable")!.Invoke(graphDataInstance, null);

            // graphObject.graph.ValidateGraph();
            graphDataType.GetMethod("ValidateGraph")!.Invoke(graphDataInstance, null);

            // return graphData.graph
            return graphDataInstance;
        }

        public static string GenerateShaderCode(string shaderAssetPath, string shaderName = null)
        {
            Type generatorType =
                Type.GetType("UnityEditor.ShaderGraph.Generator, Unity.ShaderGraph.Editor")!;
            Type modeType =
                Type.GetType("UnityEditor.ShaderGraph.GenerationMode, Unity.ShaderGraph.Editor")!;

            shaderName ??= Path.GetFileNameWithoutExtension(shaderAssetPath);

            object graphData = GetGraphData(shaderAssetPath);

            // new Generator(graphData, null, GenerationMode.ForReals, assetName, target:null, assetCollection:null, humanReadable: true);
            object forReals = ((FieldInfo)modeType.GetMember("ForReals")[0]).GetValue(null);
            object generator = Activator.CreateInstance(
                generatorType,
                new object[] { graphData, null, forReals, shaderName, null, null, true }
            );
            object shaderCode = generatorType
                .GetProperty("generatedShader", BindingFlags.Public | BindingFlags.Instance)!
                .GetValue(generator);

            return (string)shaderCode;
        }
    }
}