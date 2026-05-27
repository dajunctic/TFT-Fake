using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Dajunctic;

public class RecipeImporter : EditorWindow
{
    [MenuItem("Dajunctic/Import Recipes")]
    public static void Execute()
    {
        string jsonPath = "Assets/Art/TFT/set7/items.json";
        string databasePath = "Assets/Data/items/item_recipe_database.asset";
        string subItemsPath = "Assets/Data/items/sub_items";
        string largeItemsPath = "Assets/Data/items/large_items";

        if (!File.Exists(jsonPath))
        {
            Debug.LogError("items.json not found!");
            return;
        }

        string[] lines = File.ReadAllLines(jsonPath);
        List<ItemJson> items = ParseJson(lines);
        Debug.Log($"Parsed {items.Count} items.");

        ItemRecipeDatabase database = AssetDatabase.LoadAssetAtPath<ItemRecipeDatabase>(databasePath);
        if (database == null)
        {
            Debug.LogError("ItemRecipeDatabase not found!");
            return;
        }

        database.recipes.Clear();

        Dictionary<int, ItemData> itemMap = new Dictionary<int, ItemData>();

        foreach (var itemJson in items)
        {
            string fileName = Path.GetFileNameWithoutExtension(itemJson.imageUrl);
            string assetPath = FindAssetPath(fileName, subItemsPath, largeItemsPath);
            
            if (string.IsNullOrEmpty(assetPath))
            {
                string[] guids = AssetDatabase.FindAssets(fileName);
                if (guids.Length > 0)
                {
                    assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                }
            }

            if (!string.IsNullOrEmpty(assetPath))
            {
                ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
                if (itemData != null)
                {
                    itemMap[itemJson.id] = itemData;
                }
            }
        }

        foreach (var itemJson in items)
        {
            if (itemJson.fromItemIngameIds != null && itemJson.fromItemIngameIds.Count == 2)
            {
                int idA = itemJson.fromItemIngameIds[0];
                int idB = itemJson.fromItemIngameIds[1];

                if (itemMap.ContainsKey(idA) && itemMap.ContainsKey(idB) && itemMap.ContainsKey(itemJson.id))
                {
                    ItemData itemA = itemMap[idA];
                    ItemData itemB = itemMap[idB];
                    ItemData result = itemMap[itemJson.id];

                    ItemRecipeDatabase.Recipe recipe = new ItemRecipeDatabase.Recipe
                    {
                        componentA = itemA,
                        componentB = itemB,
                        result = result
                    };

                    database.recipes.Add(recipe);
                }
            }
        }

        EditorUtility.SetDirty(database);
        AssetDatabase.SaveAssets();
        Debug.Log($"Imported {database.recipes.Count} recipes.");
    }

    private static List<ItemJson> ParseJson(string[] lines)
    {
        List<ItemJson> items = new List<ItemJson>();
        ItemJson currentItem = new ItemJson();
        bool insideIds = false;

        foreach (string line in lines)
        {
            string trimmed = line.Trim();
            if (trimmed.StartsWith("\"id\":"))
            {
                currentItem.id = int.Parse(Regex.Match(trimmed, @"\d+").Value);
            }
            else if (trimmed.StartsWith("\"imageUrl\":"))
            {
                currentItem.imageUrl = Regex.Match(trimmed, "\"(.*?)\"").Groups[1].Value;
                
                if (currentItem.imageUrl.EndsWith(",")) currentItem.imageUrl = currentItem.imageUrl.TrimEnd(',');

                Match match = Regex.Match(trimmed, "\"imageUrl\":\\s*\"(.*?)\"");
                if (match.Success)
                {
                    currentItem.imageUrl = match.Groups[1].Value;
                }
            }
            else if (trimmed.StartsWith("\"fromItemIngameIds\":"))
            {
                currentItem.fromItemIngameIds = new List<int>();
                if (trimmed.Contains("]"))
                {

                    MatchCollection matches = Regex.Matches(trimmed, @"\d+");
                    foreach (Match m in matches)
                    {
                        currentItem.fromItemIngameIds.Add(int.Parse(m.Value));
                    }
                }
                else
                {
                    insideIds = true;
                }
            }
            else if (insideIds)
            {
                if (trimmed.StartsWith("]"))
                {
                    insideIds = false;
                }
                else
                {
                    Match match = Regex.Match(trimmed, @"\d+");
                    if (match.Success)
                    {
                        currentItem.fromItemIngameIds.Add(int.Parse(match.Value));
                    }
                }
            }
            else if (trimmed.StartsWith("},"))
            {
                items.Add(currentItem);
                currentItem = new ItemJson();
            }
        }
        
        if (currentItem.id != 0)
        {
            items.Add(currentItem);
        }

        return items;
    }

    private static string FindAssetPath(string fileName, string subItemsPath, string largeItemsPath)
    {
        string path = Path.Combine(subItemsPath, fileName + ".asset");
        if (File.Exists(path)) return path;

        path = Path.Combine(largeItemsPath, fileName + ".asset");
        if (File.Exists(path)) return path;

        return null;
    }

    private class ItemJson
    {
        public int id;
        public string imageUrl;
        public List<int> fromItemIngameIds = new List<int>();
    }
}
