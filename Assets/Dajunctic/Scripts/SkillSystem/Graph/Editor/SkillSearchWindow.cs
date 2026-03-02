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
            string nodesNs = "Dajunctic.SkillSystem.Graph.Nodes";
            string actionNs = "Dajunctic.SkillSystem.Graph.ActionNodes";

            foreach (var type in nodeTypes)
            {
                string ns = type.Namespace;
                if (string.IsNullOrEmpty(ns))
                {
                    AddEntry(tree, type, 1);
                    continue;
                }

                string groupPath = null;
                if (ns.StartsWith(nodesNs))
                {
                    groupPath = ns.Replace(nodesNs, "").Trim('.');
                }
                else if (ns.StartsWith(actionNs))
                {
                    groupPath = "Actions";
                    string sub = ns.Replace(actionNs, "").Trim('.');
                    if (!string.IsNullOrEmpty(sub)) groupPath += "/" + sub;
                }

                if (!string.IsNullOrEmpty(groupPath))
                {
                    // Split path for potential nested groups (though currently handled as single string)
                    if (groups.Add(groupPath))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(groupPath), 1));
                    }
                    AddEntry(tree, type, 2);
                }
                else
                {
                    AddEntry(tree, type, 1);
                }
            }

            return tree;
        }

        private void AddEntry(List<SearchTreeEntry> tree, Type type, int level)
        {
            tree.Add(new SearchTreeEntry(new GUIContent(type.Name, _indentationIcon))
            {
                level = level,
                userData = type
            });
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = (Type)searchTreeEntry.userData;
            _graphView.CreateNode(type, GraphMousePosition);
            return true;
        }
    }
}
