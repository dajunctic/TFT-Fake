using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Dajunctic;

public class ItemRecipeEditorWindow : EditorWindow
{
    private ItemRecipeDatabase database;
    private List<ItemData> components = new List<ItemData>();
    private Vector2 scrollPosition;

    [MenuItem("Dajunctic/Item Recipe Editor")]
    public static void ShowWindow()
    {
        GetWindow<ItemRecipeEditorWindow>("Item Recipe Editor");
    }

    private void OnEnable()
    {
        LoadDatabase();
        LoadComponents();
    }

    private void LoadDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemRecipeDatabase");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            database = AssetDatabase.LoadAssetAtPath<ItemRecipeDatabase>(path);
        }
    }

    private void LoadComponents()
    {
        components.Clear();
        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Data/items/sub_items" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && item.type == ItemType.Component)
            {
                components.Add(item);
            }
        }
        
        components.Sort((a, b) => string.Compare(a.itemName, b.itemName));
    }

    private void OnGUI()
    {
        if (database == null)
        {
            EditorGUILayout.HelpBox("ItemRecipeDatabase not found!", MessageType.Error);
            if (GUILayout.Button("Refresh")) LoadDatabase();
            return;
        }

        if (components.Count == 0)
        {
            EditorGUILayout.HelpBox("No components found in Assets/Data/items/sub_items!", MessageType.Warning);
            if (GUILayout.Button("Refresh")) LoadComponents();
            return;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Database: ", GUILayout.Width(70));
        database = (ItemRecipeDatabase)EditorGUILayout.ObjectField(database, typeof(ItemRecipeDatabase), false);
        if (GUILayout.Button("Save", GUILayout.Width(60)))
        {
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(60), GUILayout.Height(60)); 
        foreach (var comp in components)
        {
            DrawItemIcon(comp, 60);
        }
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < components.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            DrawItemIcon(components[i], 60);

            for (int j = 0; j < components.Count; j++)
            {

                ItemData result = GetResult(components[i], components[j]);
                ItemData newResult = DrawItemSlot(result, 60);

                if (newResult != result)
                {
                    UpdateRecipe(components[i], components[j], newResult);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawItemIcon(ItemData item, float size)
    {
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(size), GUILayout.Height(size));
        
        GUI.Box(rect, "");

        if (item == null) return;

        Texture2D icon = null;
        if (item.icon != null) icon = AssetPreview.GetAssetPreview(item.icon);

        if (icon != null)
        {
            GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit);
        }
        else
        {
            GUI.Label(rect, item.itemName, EditorStyles.miniLabel);
        }
        
        EditorGUI.LabelField(rect, new GUIContent("", item.itemName));
    }

    private ItemData DrawItemSlot(ItemData item, float size)
    {
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(size), GUILayout.Height(size));

        GUI.Box(rect, "");

        if (item != null)
        {
            Texture2D icon = null;
            if (item.icon != null) icon = AssetPreview.GetAssetPreview(item.icon);
            
            if (icon != null)
            {
                GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit);
            }
            else
            {
                GUI.Label(rect, item.itemName, EditorStyles.miniLabel);
            }

            EditorGUI.LabelField(rect, new GUIContent("", item.itemName));
        }
        else
        {
            GUI.Label(rect, "-", EditorStyles.centeredGreyMiniLabel);
        }

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        Event evt = Event.current;

        switch (evt.type)
        {
            case EventType.MouseDown:
                if (rect.Contains(evt.mousePosition))
                {
                    if (evt.button == 0) 
                    {
                        EditorGUIUtility.ShowObjectPicker<ItemData>(item, false, "", controlID);
                        evt.Use();
                    }
                    else if (evt.button == 1) 
                    {
                        item = null;
                        GUI.changed = true;
                        evt.Use();
                    }
                }
                break;

            case EventType.ExecuteCommand:
                if (evt.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == controlID)
                {
                    item = (ItemData)EditorGUIUtility.GetObjectPickerObject();
                    GUI.changed = true;
                    evt.Use();
                }
                break;

            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (rect.Contains(evt.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (var dragged in DragAndDrop.objectReferences)
                        {
                            if (dragged is ItemData data)
                            {
                                item = data;
                                GUI.changed = true;
                                break;
                            }
                        }
                    }
                    evt.Use();
                }
                break;
        }

        return item;
    }

    private ItemData GetResult(ItemData a, ItemData b)
    {
        foreach (var recipe in database.recipes)
        {
            if ((recipe.componentA == a && recipe.componentB == b) ||
                (recipe.componentA == b && recipe.componentB == a))
            {
                return recipe.result;
            }
        }
        return null;
    }

    private void UpdateRecipe(ItemData a, ItemData b, ItemData result)
    {
        
        database.recipes.RemoveAll(r => (r.componentA == a && r.componentB == b) || (r.componentA == b && r.componentB == a));

        if (result != null)
        {
            ItemRecipeDatabase.Recipe newRecipe = new ItemRecipeDatabase.Recipe
            {
                componentA = a,
                componentB = b,
                result = result
            };
            database.recipes.Add(newRecipe);
        }
        
        EditorUtility.SetDirty(database);
    }
}
