using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dajunctic.SkillSystem.Graph.Editor
{
    public class SkillSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private SkillGraphView _graphView;
        private EditorWindow _window;
        private Texture2D _indentationIcon;

        public Vector2 GraphMousePosition { get; set; }

        public void Init(SkillGraphView graphView, EditorWindow window)
        {
            _graphView = graphView;
            _window = window;

            // Transparent icon to help with indentation
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
            };

            // Scan for all SkillNode types
            var nodeTypes = TypeCache.GetTypesDerivedFrom<SkillNode>()
                .Where(t => !t.IsAbstract && !t.IsGenericType)
                .OrderBy(t => t.Name)
                .ToList();

            // Grouping logic
            HashSet<string> groups = new HashSet<string>();
            foreach (var type in nodeTypes)
            {
                string ns = type.Namespace;
                if (!string.IsNullOrEmpty(ns) && ns.StartsWith("Dajunctic.SkillSystem.Graph.Nodes"))
                {
                    string subNs = ns.Replace("Dajunctic.SkillSystem.Graph.Nodes", "").Trim('.');
                    if (!string.IsNullOrEmpty(subNs))
                    {
                        if (groups.Add(subNs))
                        {
                            tree.Add(new SearchTreeGroupEntry(new GUIContent(subNs), 1));
                        }
                        tree.Add(new SearchTreeEntry(new GUIContent(type.Name, _indentationIcon))
                        {
                            level = 2,
                            userData = type
                        });
                        continue;
                    }
                }

                tree.Add(new SearchTreeEntry(new GUIContent(type.Name, _indentationIcon))
                {
                    level = 1,
                    userData = type
                });
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = (Type)searchTreeEntry.userData;
            _graphView.CreateNode(type, GraphMousePosition);
            return true;
        }
    }
}
